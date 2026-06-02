import { describe, it, expect, beforeEach } from "vitest";
import { createPinia, setActivePinia } from "pinia";
import { useGameStore } from "@/stores/use-game-store";

describe("useGameStore", () => {
    beforeEach(() => {
        setActivePinia(createPinia());
    });

    it("clears currentState when emulator disconnects", () => {
        const store = useGameStore();

        store.setInitialState({
            player: { name: "Test", bits: 0, location: "0001" },
            party: null,
            journal: null,
        });

        expect(store.currentState).not.toBeNull();

        store.syncEmulatorConnectionStatus({ isConnected: false });

        expect(store.isConnectedWithEmulator).toBe(false);
        expect(store.currentState).toBeNull();
    });

    it("stores hub error message when backend disconnects", () => {
        const store = useGameStore();

        store.syncHubConnectionStatus({
            isConnected: false,
            errorMessage: "Connection refused",
        });

        expect(store.isConnectedWithBackend).toBe(false);
        expect(store.lastHubConnectionError).toBe("Connection refused");
    });

    it("clears hub error message when backend reconnects", () => {
        const store = useGameStore();

        store.syncHubConnectionStatus({
            isConnected: false,
            errorMessage: "Connection refused",
        });
        store.syncHubConnectionStatus({ isConnected: true });

        expect(store.lastHubConnectionError).toBeNull();
    });

    it("keeps currentState when emulator reconnects", () => {
        const store = useGameStore();

        store.setInitialState({
            player: { name: "Test", bits: 0, location: "0001" },
            party: null,
            journal: null,
        });

        store.syncEmulatorConnectionStatus({ isConnected: true });

        expect(store.currentState).not.toBeNull();
    });
});
