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
        kind: "sidecar-crashed",
        titleKey: "errors.sidecarCrashed.title",
        hintKey: "errors.sidecarCrashed.hint",
      };
    }

    if (!store.isConnectedWithBackend) {
      return {
        kind: "hub-unreachable",
        titleKey: "errors.hubUnreachable.title",
        hintKey: "errors.hubUnreachable.hint",
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
