<script setup lang="ts">
import { computed, ref, watch } from "vue";
import { SeabedDocksPresenter } from "@/presenters/map/seabed-docks.presenter";
import type { DockLabelPosition } from "@/repositories/tables/raws/seabed/seabed-direction-dock.raw";
import {
  SEABED_MAP_FRAME_MAX_HEIGHT_PX,
  SEABED_MAP_FRAME_WIDTH_PX,
} from "@/components/seabed/seabed-map-frame";

const emit = defineEmits<{
  "select-dock": [locationId: string];
}>();

const imageUrl = computed(() => {
  return SeabedDocksPresenter.getAsukaImageUrl();
});

const routes = SeabedDocksPresenter.getRoutes();

const hoveredRouteId = ref<string | null>(null);

const imageNaturalSize = ref<{ width: number; height: number } | null>(null);

const mapImageFrameStyle = computed(() => {
  if (imageNaturalSize.value === null) {
    return {
      width: `${SEABED_MAP_FRAME_WIDTH_PX}px`,
      minHeight: `${Math.round(SEABED_MAP_FRAME_WIDTH_PX * 0.75)}px`,
    };
  }

  const displayHeight = Math.round(
    SEABED_MAP_FRAME_WIDTH_PX * (imageNaturalSize.value.height / imageNaturalSize.value.width)
  );

  return {
    width: `${SEABED_MAP_FRAME_WIDTH_PX}px`,
    height: `${displayHeight}px`,
  };
});

function onDockClick(locationId: string, dockType: string): void {
  if (dockType !== "normal") {
    return;
  }

  emit("select-dock", locationId);
}

function onRouteEnter(routeId: string): void {
  hoveredRouteId.value = routeId;
}

function onRouteLeave(): void {
  hoveredRouteId.value = null;
}

function isRouteHovered(routeId: string): boolean {
  return hoveredRouteId.value === routeId;
}

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

const onImageLoad = (event: Event) => {
  const imageElement = event.target as HTMLImageElement;

  if (imageElement.naturalWidth === 0) {
    return;
  }

  imageNaturalSize.value = {
    width: imageElement.naturalWidth,
    height: imageElement.naturalHeight,
  };
};

watch(
  imageUrl,
  () => {
    imageNaturalSize.value = null;
  }
);
</script>

<template>
  <div
    class="relative shrink-0 min-h-0 overflow-visible bg-[#00051a] border border-cyan-800/50 rounded shadow-[0_0_15px_rgba(0,170,255,0.1)]"
    :style="{
      width: `${SEABED_MAP_FRAME_WIDTH_PX}px`,
      maxHeight: `${SEABED_MAP_FRAME_MAX_HEIGHT_PX}px`,
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
            v-for="segmentIndex in route.docks.length - 1"
            :key="`${route.id}-${segmentIndex}`"
          >
            <line
              :x1="route.docks[segmentIndex - 1].x"
              :y1="route.docks[segmentIndex - 1].y"
              :x2="route.docks[segmentIndex].x"
              :y2="route.docks[segmentIndex].y"
              stroke="transparent"
              stroke-width="3"
              pointer-events="stroke"
              cursor="help"
              @mouseenter="onRouteEnter(route.id)"
              @mouseleave="onRouteLeave"
            />
            <line
              :x1="route.docks[segmentIndex - 1].x"
              :y1="route.docks[segmentIndex - 1].y"
              :x2="route.docks[segmentIndex].x"
              :y2="route.docks[segmentIndex].y"
              :stroke="isRouteHovered(route.id) ? '#67e8f9' : '#06b6d4'"
              :stroke-opacity="isRouteHovered(route.id) ? 1 : 0.5"
              :stroke-width="isRouteHovered(route.id) ? 1 : 0.9"
              pointer-events="none"
            />
          </template>
        </template>
      </svg>

      <template
        v-for="route in routes"
        :key="route.id"
      >
        <div
          v-for="dock in route.docks"
          :key="dock.location"
          class="absolute z-10 -translate-x-1/2 -translate-y-1/2 flex items-center justify-center"
          :class="dock.type === 'dead-end' ? 'w-3 h-3' : 'w-7 h-7'"
          :style="{ left: dock.x + '%', top: dock.y + '%' }"
          @mouseenter="onRouteEnter(route.id)"
          @mouseleave="onRouteLeave"
          @click="onDockClick(dock.location, dock.type)"
        >
          <div
            class="rounded-full w-full h-full transition-all"
            :class="[
              dock.type === 'dead-end' ? 'cursor-help' : 'cursor-pointer',
              isRouteHovered(route.id) ? 'bg-cyan-300' : 'bg-cyan-500/50',
            ]"
          />
        </div>
      </template>

      <div class="absolute inset-0 z-20 pointer-events-none">
        <template
          v-for="route in routes"
          :key="`label-${route.id}`"
        >
          <template v-if="isRouteHovered(route.id)">
            <div
              v-for="dock in route.docks"
              :key="`label-${dock.location}`"
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
        </template>
      </div>
    </div>
  </div>
</template>
