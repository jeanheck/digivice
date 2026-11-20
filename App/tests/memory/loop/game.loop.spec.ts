import { afterEach, beforeEach, describe, expect, it, vi } from "vitest";
import { EmulatorConnectionErrorCodes } from "@/shared/constants/emulator-connection-error-code.constant";
import { GameLoop, type GameLoopOptions } from "@/memory/loop/game.loop";
import type { State } from "@/state/models";
import { EmulatorConnectionError } from "@/memory/snapshot/emulator-connection.error";
import { MemoryReadError } from "@/memory/snapshot/memory-read.error";
import { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { SaveNotLoadedError } from "@/memory/validators/save-not-loaded.error";
import { FieldCentralParkExpected } from "../../fixtures/snapshots/field-central-park.expected";
import { FakeSnapshotBuilder } from "../../helpers/fake-snapshot.builder";

const IntervalMs = 500;

const StateSample: State = FieldCentralParkExpected;

function createSnapshot(): MemorySnapshot {
  return new MemorySnapshot(new FakeSnapshotBuilder().build());
}

function createOptions(overrides: Partial<GameLoopOptions> = {}) {
  const read = vi.fn<() => Promise<MemorySnapshot>>().mockResolvedValue(createSnapshot());
  const compose = vi.fn<GameLoopOptions["compose"]>().mockReturnValue(StateSample);
  const onSuccess = vi.fn<GameLoopOptions["onSuccess"]>();
  const onError = vi.fn<GameLoopOptions["onError"]>();

  const options: GameLoopOptions = {
    snapshotProvider: { read },
    compose,
    intervalMs: IntervalMs,
    onSuccess,
    onError,
    ...overrides,
  };

  return { options, read, compose, onSuccess, onError };
}

describe("GameLoop", () => {
  beforeEach(() => {
    vi.useFakeTimers();
  });

  afterEach(() => {
    vi.useRealTimers();
  });

  describe("start", () => {
    it("should compose the snapshot and report success on the first tick", async () => {
      const snapshot = createSnapshot();
      const { options, read, compose, onSuccess, onError } = createOptions();
      read.mockResolvedValue(snapshot);
      const gameLoop = new GameLoop(options);

      gameLoop.start();
      await vi.advanceTimersByTimeAsync(0);

      expect(compose).toHaveBeenCalledWith(snapshot);
      expect(onSuccess).toHaveBeenCalledExactlyOnceWith(StateSample);
      expect(onError).not.toHaveBeenCalled();
      gameLoop.stop();
    });

    it("should read again after each interval", async () => {
      const { options, read } = createOptions();
      const gameLoop = new GameLoop(options);

      gameLoop.start();
      await vi.advanceTimersByTimeAsync(0);
      await vi.advanceTimersByTimeAsync(IntervalMs * 2);

      expect(read).toHaveBeenCalledTimes(3);
      gameLoop.stop();
    });

    it("should forward the code and detail when the provider throws EmulatorConnectionError", async () => {
      const { options, read, onSuccess, onError } = createOptions();
      read.mockRejectedValue(
        new EmulatorConnectionError(EmulatorConnectionErrorCodes.ProcessNotFound, null),
      );
      const gameLoop = new GameLoop(options);

      gameLoop.start();
      await vi.advanceTimersByTimeAsync(0);

      expect(onError).toHaveBeenCalledExactlyOnceWith(
        EmulatorConnectionErrorCodes.ProcessNotFound,
        null,
      );
      expect(onSuccess).not.toHaveBeenCalled();
      gameLoop.stop();
    });

    it("should report memory_read_failed when composing throws MemoryReadError", async () => {
      const { options, compose, onError } = createOptions();
      compose.mockImplementation(() => {
        throw new MemoryReadError(0x200000, "outside the snapshot");
      });
      const gameLoop = new GameLoop(options);

      gameLoop.start();
      await vi.advanceTimersByTimeAsync(0);

      expect(onError).toHaveBeenCalledExactlyOnceWith(
        EmulatorConnectionErrorCodes.MemoryReadFailed,
        "outside the snapshot",
      );
      gameLoop.stop();
    });

    it("should report save_not_loaded when composing throws SaveNotLoadedError", async () => {
      const { options, compose, onError } = createOptions();
      compose.mockImplementation(() => {
        throw new SaveNotLoadedError("No occupied party slot.");
      });
      const gameLoop = new GameLoop(options);

      gameLoop.start();
      await vi.advanceTimersByTimeAsync(0);

      expect(onError).toHaveBeenCalledExactlyOnceWith(
        EmulatorConnectionErrorCodes.SaveNotLoaded,
        "No occupied party slot.",
      );
      gameLoop.stop();
    });

    it("should report state_compose_failed when composing throws any other error", async () => {
      const { options, compose, onError } = createOptions();
      compose.mockImplementation(() => {
        throw new Error("Invalid definitions");
      });
      const gameLoop = new GameLoop(options);

      gameLoop.start();
      await vi.advanceTimersByTimeAsync(0);

      expect(onError).toHaveBeenCalledExactlyOnceWith(
        EmulatorConnectionErrorCodes.StateComposeFailed,
        "Invalid definitions",
      );
      gameLoop.stop();
    });

    it("should report success again when the provider recovers after an error", async () => {
      const { options, read, onSuccess, onError } = createOptions();
      read.mockRejectedValueOnce(
        new EmulatorConnectionError(EmulatorConnectionErrorCodes.MappingNotFound, "not found"),
      );
      const gameLoop = new GameLoop(options);

      gameLoop.start();
      await vi.advanceTimersByTimeAsync(0);
      await vi.advanceTimersByTimeAsync(IntervalMs);

      expect(onError).toHaveBeenCalledExactlyOnceWith(
        EmulatorConnectionErrorCodes.MappingNotFound,
        "not found",
      );
      expect(onSuccess).toHaveBeenCalledExactlyOnceWith(StateSample);
      gameLoop.stop();
    });

    it("should never overlap reads when a read takes longer than the interval", async () => {
      const { options, read } = createOptions();
      read.mockImplementation(
        () => new Promise((resolve) => setTimeout(() => resolve(createSnapshot()), 1200)),
      );
      const gameLoop = new GameLoop(options);

      gameLoop.start();
      await vi.advanceTimersByTimeAsync(1600);
      expect(read).toHaveBeenCalledTimes(1);

      await vi.advanceTimersByTimeAsync(100);
      expect(read).toHaveBeenCalledTimes(2);
      gameLoop.stop();
    });

    it("should keep a single chain when start is called twice", async () => {
      const { options, read } = createOptions();
      const gameLoop = new GameLoop(options);

      gameLoop.start();
      gameLoop.start();
      await vi.advanceTimersByTimeAsync(0);

      expect(read).toHaveBeenCalledTimes(1);
      expect(gameLoop.isRunning).toBe(true);
      gameLoop.stop();
    });
  });

  describe("stop", () => {
    it("should not read again when the loop is stopped", async () => {
      const { options, read } = createOptions();
      const gameLoop = new GameLoop(options);

      gameLoop.start();
      await vi.advanceTimersByTimeAsync(0);
      gameLoop.stop();
      await vi.advanceTimersByTimeAsync(IntervalMs * 4);

      expect(read).toHaveBeenCalledTimes(1);
      expect(gameLoop.isRunning).toBe(false);
    });

    it("should discard a read in flight when the loop is stopped", async () => {
      const { options, read, onSuccess, onError } = createOptions();
      read.mockImplementation(
        () => new Promise((resolve) => setTimeout(() => resolve(createSnapshot()), 100)),
      );
      const gameLoop = new GameLoop(options);

      gameLoop.start();
      gameLoop.stop();
      await vi.advanceTimersByTimeAsync(IntervalMs * 4);

      expect(read).toHaveBeenCalledTimes(1);
      expect(onSuccess).not.toHaveBeenCalled();
      expect(onError).not.toHaveBeenCalled();
    });

    it("should run a single chain when restarted while a read is in flight", async () => {
      const { options, read, onSuccess } = createOptions();
      read.mockImplementation(
        () => new Promise((resolve) => setTimeout(() => resolve(createSnapshot()), 100)),
      );
      const gameLoop = new GameLoop(options);

      gameLoop.start();
      gameLoop.stop();
      gameLoop.start();
      await vi.advanceTimersByTimeAsync(100);

      expect(read).toHaveBeenCalledTimes(2);
      expect(onSuccess).toHaveBeenCalledTimes(1);

      await vi.advanceTimersByTimeAsync(IntervalMs + 100);
      expect(read).toHaveBeenCalledTimes(3);
      expect(onSuccess).toHaveBeenCalledTimes(2);
      gameLoop.stop();
    });
  });
});
