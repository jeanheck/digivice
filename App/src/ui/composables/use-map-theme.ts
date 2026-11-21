import { watchEffect } from "vue";
import { MapService } from "@/presentation/services/map.service";
import { useGameStore } from "@/state/stores/use-game-store";
import type { MapTheme } from "@/shared/types/map-theme.type";

function applyThemeToDocument(theme: MapTheme): void {
  document.documentElement.dataset.theme = theme;
}

export function useMapTheme(): void {
  const store = useGameStore();

  watchEffect(() => {
    const currentState = store.currentState;
    if (currentState === null) {
      applyThemeToDocument("asuka");
      return;
    }

    const theme = MapService.resolveEffectiveTheme(
      currentState.player.mapId,
      currentState.player.previousMapId,
    );
    applyThemeToDocument(theme);
  });
}
