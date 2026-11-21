import { computed, ref, watchEffect } from "vue";
import { MapService } from "@/presentation/services/map.service";
import { useGameStore } from "@/state/stores/use-game-store";
import type { MapTheme } from "@/shared/types/map-theme.type";

export const MapThemeOptions = [
  { theme: "asuka", emoji: "\u{1F535}", label: "Asuka" },
  { theme: "amaterasu", emoji: "\u{1F338}", label: "Amaterasu" },
  { theme: "seabed", emoji: "\u{1F30A}", label: "Seabed" },
  { theme: "circuitBoard", emoji: "\u{1F50C}", label: "Circuit Board" },
  { theme: "undefined", emoji: "\u{26AA}", label: "Undefined" },
] as const satisfies ReadonlyArray<{ theme: MapTheme; emoji: string; label: string }>;

const devThemeOverride = ref<MapTheme | null>(null);

function resolveThemeFromMapId(mapId: string): MapTheme {
  return MapService.getThemeByMapId(mapId);
}

export function resolveEffectiveTheme(
  mapId: string | null,
  previousMapId: string | null,
  override: MapTheme | null = devThemeOverride.value,
): MapTheme {
  if (override !== null) {
    return override;
  }

  if (mapId === null) {
    return "asuka";
  }

  const currentTheme = resolveThemeFromMapId(mapId);
  if (currentTheme !== "undefined") {
    return currentTheme;
  }

  if (previousMapId !== null) {
    const previousTheme = resolveThemeFromMapId(previousMapId);
    if (previousTheme !== "undefined") {
      return previousTheme;
    }
  }

  return "asuka";
}

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

    const theme = resolveEffectiveTheme(
      currentState.player.mapId,
      currentState.player.previousMapId,
      devThemeOverride.value,
    );
    applyThemeToDocument(theme);
  });
}

export function useMapThemeDevOverride() {
  const store = useGameStore();

  const effectiveTheme = computed((): MapTheme => {
    const currentState = store.currentState;
    if (currentState === null) {
      return resolveEffectiveTheme(null, null, devThemeOverride.value);
    }

    return resolveEffectiveTheme(
      currentState.player.mapId,
      currentState.player.previousMapId,
      devThemeOverride.value,
    );
  });

  function setTheme(theme: MapTheme): void {
    devThemeOverride.value = theme;
  }

  return {
    effectiveTheme,
    setTheme,
    options: MapThemeOptions,
  };
}
