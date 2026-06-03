import { computed } from "vue";
import { useGameStore } from "@/stores/use-game-store";
import type { AppBlockingErrorViewModel } from "@/models/app-blocking-error";

export function useAppBlockingError() {
    const store = useGameStore();

    return computed((): AppBlockingErrorViewModel | null => {
        if (store.backendProcessFailed) {
            return {
                kind: "backend-crashed",
                titleKey: "errors.backendCrashed.title",
                hintKey: "errors.backendCrashed.hint",
            };
        }

        if (!store.isConnectedWithBackend) {
            return {
                kind: "backend-unreachable",
                titleKey: "errors.backend.title",
                hintKey: "errors.backend.hint",
                detail: store.lastHubConnectionError ?? undefined,
            };
        }

        if (!store.isConnectedWithEmulator) {
            return {
                kind: "emulator-not-found",
                titleKey: "errors.emulator.title",
                hintKey: "errors.emulator.hint",
            };
        }

        return null;
    });
}
