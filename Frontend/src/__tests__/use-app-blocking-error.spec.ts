import { describe, it, expect, beforeEach } from "vitest";
import { createPinia, setActivePinia } from "pinia";
import { useGameStore } from "@/stores/use-game-store";
import { useAppBlockingError } from "@/composables/use-app-blocking-error";

describe("useAppBlockingError", () => {
    beforeEach(() => {
        setActivePinia(createPinia());
    });

    it("returns backend error when hub is disconnected", () => {
        const store = useGameStore();
        store.syncHubConnectionStatus({
            isConnected: false,
            errorMessage: "Failed to fetch",
        });

        const blockingError = useAppBlockingError();

        expect(blockingError.value).toEqual({
            kind: "backend-unreachable",
            titleKey: "errors.backend.title",
            hintKey: "errors.backend.hint",
            detail: "Failed to fetch",
        });
    });

    it("returns emulator error when backend is connected but emulator is not", () => {
        const store = useGameStore();
        store.syncHubConnectionStatus({ isConnected: true });
        store.syncEmulatorConnectionStatus({ isConnected: false });

        const blockingError = useAppBlockingError();

        expect(blockingError.value).toEqual({
            kind: "emulator-not-found",
            titleKey: "errors.emulator.title",
            hintKey: "errors.emulator.hint",
        });
    });

    it("prioritizes backend error over emulator error", () => {
        const store = useGameStore();
        store.syncHubConnectionStatus({
            isConnected: false,
            errorMessage: "Connection closed.",
        });
        store.syncEmulatorConnectionStatus({ isConnected: false });

        const blockingError = useAppBlockingError();

        expect(blockingError.value?.kind).toBe("backend-unreachable");
    });

    it("returns null when both connections are ready", () => {
        const store = useGameStore();
        store.syncHubConnectionStatus({ isConnected: true });
        store.syncEmulatorConnectionStatus({ isConnected: true });

        const blockingError = useAppBlockingError();

        expect(blockingError.value).toBeNull();
    });
});
