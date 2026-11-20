import { invoke } from "@tauri-apps/api/core";
import { beforeEach, describe, expect, it, vi } from "vitest";
import { EmulatorConnectionErrorCodes } from "@/shared/constants/emulator-connection-error-code.constant";
import { EmulatorConnectionError } from "@/memory/snapshot/emulator-connection.error";
import { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { TauriSnapshotProvider } from "@/memory/snapshot/tauri-snapshot.provider";
import { FakeSnapshotBuilder } from "../../helpers/fake-snapshot.builder";

vi.mock("@tauri-apps/api/core", () => ({ invoke: vi.fn() }));

const invokeMock = vi.mocked(invoke);

describe("TauriSnapshotProvider", () => {
  beforeEach(() => {
    invokeMock.mockReset();
  });

  describe("read", () => {
    it("should return a snapshot over the RAM returned by read_ram", async () => {
      invokeMock.mockResolvedValue(new FakeSnapshotBuilder().writeInt16(0x4b3f8, 0x020a).build());

      const snapshot = await new TauriSnapshotProvider().read();

      expect(invokeMock).toHaveBeenCalledWith("read_ram");
      expect(snapshot).toBeInstanceOf(MemorySnapshot);
      expect(snapshot.readInt16(0x4b3f8)).toBe(0x020a);
    });

    it.each([
      [EmulatorConnectionErrorCodes.ProcessNotFound, null],
      [EmulatorConnectionErrorCodes.MappingNotFound, "duckstation_1234: not found"],
      [EmulatorConnectionErrorCodes.ConnectionFailed, "access denied"],
      [EmulatorConnectionErrorCodes.MemoryReadFailed, "view is smaller than 2 MiB"],
    ])("should forward the Rust error when read_ram fails with %s", async (code, detail) => {
      invokeMock.mockRejectedValue({ code, detail });

      const error = await new TauriSnapshotProvider().read().catch((reason: unknown) => reason);

      expect(error).toBeInstanceOf(EmulatorConnectionError);
      expect(error).toMatchObject({ code, detail });
    });

    it("should report memory_read_failed when read_ram rejects with an unknown shape", async () => {
      invokeMock.mockRejectedValue("command read_ram not found");

      const error = await new TauriSnapshotProvider().read().catch((reason: unknown) => reason);

      expect(error).toBeInstanceOf(EmulatorConnectionError);
      expect(error).toMatchObject({
        code: EmulatorConnectionErrorCodes.MemoryReadFailed,
        detail: "command read_ram not found",
      });
    });

    it("should report memory_read_failed when read_ram returns a buffer that is not 2 MiB", async () => {
      invokeMock.mockResolvedValue(new FakeSnapshotBuilder(16).build());

      const error = await new TauriSnapshotProvider().read().catch((reason: unknown) => reason);

      expect(error).toBeInstanceOf(EmulatorConnectionError);
      expect(error).toMatchObject({ code: EmulatorConnectionErrorCodes.MemoryReadFailed });
    });
  });
});
