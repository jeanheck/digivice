import { computed } from "vue";
import { useGameStore } from "@/stores/use-game-store";
import { HealthStatus } from "@/models/health-status";
import type { AppHealthyScreenViewModel } from "@/models/app-healthy-screen";
import { EmulatorConnectionErrorHelper } from "@/events/helpers/emulator-connection-error.helper";

export function useAppHealthyScreen() {
  const store = useGameStore();

  return computed((): AppHealthyScreenViewModel | null => {
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

    if (store.healthStatus === HealthStatus.Loading) {
      return {
        kind: "loading",
        titleKey: "errors.loading.title",
        hintKey: "errors.loading.hint",
      };
    }

    if (store.healthStatus === HealthStatus.Error) {
      const { titleKey, hintKey } = EmulatorConnectionErrorHelper.resolveErrorKeys(
        store.lastErrorCode,
      );

      return {
        kind: "operational-error",
        titleKey,
        hintKey,
        detail: store.lastErrorDetail ?? undefined,
      };
    }

    return null;
  });
}
