import { computed } from "vue";
import { useGameStore } from "@/stores/use-game-store";
import type { AppBlockingErrorViewModel } from "@/models/app-blocking-error";
import { EmulatorConnectionErrorHelper } from "@/events/helpers/emulator-connection-error.helper";

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
            const { titleKey, hintKey } = EmulatorConnectionErrorHelper.resolveErrorKeys(
                store.lastEmulatorConnectionErrorCode
            );

            return {
                kind: "emulator-not-found",
                titleKey,
                hintKey,
                detail: store.lastEmulatorConnectionErrorDetail ?? undefined,
            };
        }

        return null;
    });
}
