import { computed } from "vue";
import { useGameStore } from "@/stores/use-game-store";
import { HealthStatus } from "@/models";
import type { AppHealthyScreenViewModel } from "@/viewmodels/app-healthy/app-healthy-screen.viewmodel";
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

    if (store.currentState === null) {
      return {
        kind: "loading",
        titleKey: "errors.loading.title",
        hintKey: "errors.loading.hint",
      };
    }

    return null;
  });
}
