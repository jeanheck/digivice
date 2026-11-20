import { beforeEach, describe, expect, it } from "vitest";
import { createPinia, setActivePinia } from "pinia";
import type { State } from "@/state/models";
import { useGameStore } from "@/state/stores/use-game-store";
import { FieldCentralParkExpected } from "../../fixtures/snapshots/field-central-park.expected";

function centralParkState(): State {
  return structuredClone(FieldCentralParkExpected);
}

describe("useGameStore", () => {
  beforeEach(() => {
    setActivePinia(createPinia());
  });

  describe("applyState", () => {
    it("should assign the whole state and become healthy on the first tick", () => {
      const store = useGameStore();

      store.applyState(centralParkState());

      expect(store.currentState).toEqual(FieldCentralParkExpected);
      expect(store.healthStatus).toBe("Healthy");
    });

    it("should patch the current state in place on the next ticks", () => {
      const store = useGameStore();
      store.applyState(centralParkState());
      const player = store.currentState?.player;
      const source = centralParkState();
      source.player.bits = 1;

      store.applyState(source);

      expect(store.currentState?.player).toBe(player);
      expect(store.currentState?.player.bits).toBe(1);
    });

    it("should assign the whole state again when recovering from an error", () => {
      const store = useGameStore();
      store.applyState(centralParkState());
      store.applyError("process_not_found", null);
      const source = centralParkState();

      store.applyState(source);

      expect(store.currentState).toEqual(source);
      expect(store.healthStatus).toBe("Healthy");
      expect(store.lastErrorCode).toBeNull();
    });
  });

  describe("applyError", () => {
    it("should clear the state and keep the error code and detail", () => {
      const store = useGameStore();
      store.applyState(centralParkState());

      store.applyError("memory_read_failed", "outside the snapshot");

      expect(store.currentState).toBeNull();
      expect(store.healthStatus).toBe("Error");
      expect(store.lastErrorCode).toBe("memory_read_failed");
      expect(store.lastErrorDetail).toBe("outside the snapshot");
    });
  });
});
