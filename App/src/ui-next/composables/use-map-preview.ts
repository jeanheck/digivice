import { ref } from "vue";
import type { MapPreview } from "@/ui-next/constants/map-preview.constant";

// Temporary manual switch for testing the map styles, until the map follows the game state.
const activeMap = ref<MapPreview>("location");

export function useMapPreview() {
  function show(map: MapPreview): void {
    activeMap.value = map;
  }

  return { activeMap, show };
}
