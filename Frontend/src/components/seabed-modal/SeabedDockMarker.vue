<script setup lang="ts">
import type { SeabedDirectionDockViewModel } from "@/viewmodels/seabed-modal/seabed-route-dock.viewmodel";

const props = defineProps<{
  dock: SeabedDirectionDockViewModel;
  routeId: string;
  isHovered: boolean;
}>();

const emit = defineEmits<{
  "select-dock": [locationId: string];
  "route-enter": [routeId: string];
  "route-leave": [];
}>();

function onDockClick(): void {
  if (props.dock.type !== "normal") {
    return;
  }

  emit("select-dock", props.dock.location);
}
</script>

<template>
  <div
    class="absolute z-10 -translate-x-1/2 -translate-y-1/2 flex items-center justify-center"
    :class="dock.type === 'dead-end' ? 'w-3 h-3' : 'w-7 h-7'"
    :style="{ left: dock.x + '%', top: dock.y + '%' }"
    @mouseenter="emit('route-enter', routeId)"
    @mouseleave="emit('route-leave')"
    @click="onDockClick"
  >
    <div
      class="rounded-full w-full h-full transition-all"
      :class="[
        dock.type === 'dead-end' ? 'cursor-help' : 'cursor-pointer',
        isHovered ? 'bg-cyan-300' : 'bg-cyan-500/50',
      ]"
    />
  </div>
</template>
