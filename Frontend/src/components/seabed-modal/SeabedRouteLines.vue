<script setup lang="ts">
import type { SeabedDirectionViewModel } from "@/viewmodels/seabed-modal/seabed-direction.viewmodel";

defineProps<{
  routes: SeabedDirectionViewModel[];
  hoveredRouteId: string | null;
}>();

const emit = defineEmits<{
  "route-enter": [routeId: string];
  "route-leave": [];
}>();

function isRouteHovered(hoveredRouteId: string | null, routeId: string): boolean {
  return hoveredRouteId === routeId;
}
</script>

<template>
  <svg
    class="absolute inset-0 w-full h-full z-9"
    viewBox="0 0 100 100"
    preserveAspectRatio="none"
  >
    <template
      v-for="route in routes"
      :key="route.id"
    >
      <template
        v-for="(segment, segmentIndex) in route.segments"
        :key="`${route.id}-${segmentIndex}`"
      >
        <line
          :x1="segment.from.x"
          :y1="segment.from.y"
          :x2="segment.to.x"
          :y2="segment.to.y"
          stroke="transparent"
          stroke-width="3"
          pointer-events="stroke"
          cursor="help"
          @mouseenter="emit('route-enter', route.id)"
          @mouseleave="emit('route-leave')"
        />
        <line
          :x1="segment.from.x"
          :y1="segment.from.y"
          :x2="segment.to.x"
          :y2="segment.to.y"
          :stroke="isRouteHovered(hoveredRouteId, route.id) ? '#67e8f9' : '#06b6d4'"
          :stroke-opacity="isRouteHovered(hoveredRouteId, route.id) ? 1 : 0.5"
          :stroke-width="isRouteHovered(hoveredRouteId, route.id) ? 1.3 : 1.1"
          pointer-events="none"
        />
      </template>
    </template>
  </svg>
</template>
