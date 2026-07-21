<script setup lang="ts">
import { computed, ref } from "vue";
import SeabedDockLabel from "@/components/seabed-modal/SeabedDockLabel.vue";
import SeabedDockMarker from "@/components/seabed-modal/SeabedDockMarker.vue";
import SeabedRouteLines from "@/components/seabed-modal/SeabedRouteLines.vue";
import {
  MAP_FRAME_MAX_HEIGHT_PX,
  MAP_FRAME_WIDTH_PX,
} from "@/components/map-details-frame/map-details-frame";
import { useMapFrame } from "@/composables/use-map-frame";
import { SeabedDocksPresenter } from "@/presenters/map/seabed-docks.presenter";

const emit = defineEmits<{
  "select-dock": [locationId: string];
}>();

const imageUrl = computed(() => {
  return SeabedDocksPresenter.getAsukaImageUrl();
});

const routes = SeabedDocksPresenter.getRoutes();

const hoveredRouteId = ref<string | null>(null);

const { mapImageFrameStyle, onImageLoad } = useMapFrame(imageUrl);

function onRouteEnter(routeId: string): void {
  hoveredRouteId.value = routeId;
}

function onRouteLeave(): void {
  hoveredRouteId.value = null;
}

function isRouteHovered(routeId: string): boolean {
  return hoveredRouteId.value === routeId;
}

function onSelectDock(locationId: string): void {
  emit("select-dock", locationId);
}
</script>

<template>
  <div
    class="relative shrink-0 min-h-0 overflow-visible bg-[#00051a] border border-cyan-800/50 rounded shadow-[0_0_15px_rgba(0,170,255,0.1)]"
    :style="{
      width: `${MAP_FRAME_WIDTH_PX}px`,
      maxHeight: `${MAP_FRAME_MAX_HEIGHT_PX}px`,
    }"
  >
    <div class="relative overflow-visible" :style="mapImageFrameStyle">
      <img
        v-if="imageUrl"
        :key="imageUrl"
        :src="imageUrl"
        class="block w-full h-full"
        @load="onImageLoad"
      />

      <SeabedRouteLines
        :routes="routes"
        :hovered-route-id="hoveredRouteId"
        @route-enter="onRouteEnter"
        @route-leave="onRouteLeave"
      />

      <template
        v-for="route in routes"
        :key="route.id"
      >
        <SeabedDockMarker
          v-for="dock in route.docks"
          :key="dock.location"
          :dock="dock"
          :route-id="route.id"
          :is-hovered="isRouteHovered(route.id)"
          @select-dock="onSelectDock"
          @route-enter="onRouteEnter"
          @route-leave="onRouteLeave"
        />
      </template>

      <div class="absolute inset-0 z-20 pointer-events-none">
        <template
          v-for="route in routes"
          :key="`label-${route.id}`"
        >
          <template v-if="isRouteHovered(route.id)">
            <SeabedDockLabel
              v-for="dock in route.docks"
              :key="`label-${dock.location}`"
              :dock="dock"
            />
          </template>
        </template>
      </div>
    </div>
  </div>
</template>
