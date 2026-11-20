import { AppConfig } from "@/core/config";
import { memoryLogger } from "@/core/logger/logger";
import { GameLoop } from "@/memory/loop/game.loop";
import { TauriSnapshotProvider } from "@/memory/snapshot/tauri-snapshot.provider";
import { StateComposer } from "@/memory/state.composer";
import { useGameStore } from "@/state/stores/use-game-store";

export function initializeMemoryLoop(): void {
  if (!AppConfig.isTauri) {
    memoryLogger.warn("The memory loop needs the Tauri window. Memory loop not started.");
    return;
  }

  const snapshotProvider = new TauriSnapshotProvider();

  const gameStore = useGameStore();

  function logHealthChange(previousStatus: string, previousErrorCode: string | null): void {
    if (
      previousStatus === gameStore.healthStatus &&
      previousErrorCode === gameStore.lastErrorCode
    ) {
      return;
    }

    if (gameStore.healthStatus === "Error") {
      memoryLogger.info(
        `Health: Error (${gameStore.lastErrorCode})`,
        gameStore.lastErrorDetail ?? "",
      );
      return;
    }

    memoryLogger.info(`Health: ${gameStore.healthStatus}`);
  }

  const gameLoop = new GameLoop({
    snapshotProvider,
    compose: (snapshot) => StateComposer.compose(snapshot),
    intervalMs: AppConfig.memory.pollingIntervalMs,
    onSuccess: (state) => {
      const previousStatus = gameStore.healthStatus;
      const previousErrorCode = gameStore.lastErrorCode;
      gameStore.applyState(state);
      logHealthChange(previousStatus, previousErrorCode);
    },
    onError: (code, detail) => {
      const previousStatus = gameStore.healthStatus;
      const previousErrorCode = gameStore.lastErrorCode;
      gameStore.applyError(code, detail);
      logHealthChange(previousStatus, previousErrorCode);
    },
  });

  memoryLogger.info("Memory loop started.");
  gameLoop.start();

  if (import.meta.hot) {
    import.meta.hot.dispose(() => gameLoop.stop());
  }
}
