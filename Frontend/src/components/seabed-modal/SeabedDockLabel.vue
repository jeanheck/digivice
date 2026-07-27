<script setup lang="ts">
import type { DockLabelPosition } from "@/repositories/tables/raws/seabed/seabed-direction-dock.raw";
import type { SeabedDirectionDockViewModel } from "@/viewmodels/seabed-modal/seabed-route-dock.viewmodel";

defineProps<{
  dock: SeabedDirectionDockViewModel;
}>();

function getLabelPlacementClasses(labelPlacement: DockLabelPosition): string {
  if (labelPlacement === "below") {
    return "top-full mt-1 left-1/2 -translate-x-1/2";
  }

  if (labelPlacement === "left") {
    return "right-full mr-1 top-1/2 -translate-y-1/2";
  }

  if (labelPlacement === "right") {
    return "left-full ml-1 top-1/2 -translate-y-1/2";
  }

  return "bottom-full mb-1 left-1/2 -translate-x-1/2";
}
</script>

<template>
  <div
    class="absolute -translate-x-1/2 -translate-y-1/2 flex items-center justify-center"
    :class="dock.type === 'dead-end' ? 'w-3 h-3' : 'w-7 h-7'"
    :style="{ left: dock.x + '%', top: dock.y + '%' }"
  >
    <div
      class="absolute w-max whitespace-nowrap text-cyan-100 drop-shadow bg-cyan-950/95 rounded border border-cyan-700/80 text-center shadow-[0_0_10px_rgba(0,0,0,0.5)] leading-tight text-[10px] px-2 py-0.5"
      :class="getLabelPlacementClasses(dock.labelPlacement)"
    >
      {{ $t(`location.${dock.location}`) }}
    </div>
  </div>
</template>
