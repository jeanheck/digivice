<script setup lang="ts">
import { computed, ref, watch } from "vue";
import { DocksPresenter } from "@/presenters/map/docks.presenter";
import type { SeabedRouteDockLabelPlacement } from "@/repositories/tables/raws/dock/seabed-route-dock.raw";

const MAP_FRAME_WIDTH_PX = 600;

const props = defineProps<{
  imageUrl: string | null;
}>();

const emit = defineEmits<{
  "select-dock": [locationId: string];
}>();

const routes = DocksPresenter.getRoutes();

const hoveredRouteId = ref<string | null>(null);

const imageNaturalSize = ref<{ width: number; height: number } | null>(null);

const mapImageFrameStyle = computed(() => {
  if (imageNaturalSize.value === null) {
    return {
      width: `${MAP_FRAME_WIDTH_PX}px`,
      minHeight: `${Math.round(MAP_FRAME_WIDTH_PX * 0.75)}px`,
    };
  }

  const displayHeight = Math.round(
    MAP_FRAME_WIDTH_PX * (imageNaturalSize.value.height / imageNaturalSize.value.width)
  );

  return {
    width: `${MAP_FRAME_WIDTH_PX}px`,
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

function getLabelPlacementClasses(labelPlacement: SeabedRouteDockLabelPlacement): string {
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
  () => props.imageUrl,
  () => {
    imageNaturalSize.value = null;
  }
);
</script>

<template>
  <div
    class="relative shrink-0 max-h-full min-h-0 overflow-visible bg-[#00051a] border border-cyan-800/50 rounded shadow-[0_0_15px_rgba(0,170,255,0.1)]"
    :style="{ width: `${MAP_FRAME_WIDTH_PX}px` }"
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

          <div
            v-if="isRouteHovered(route.id)"
            class="absolute w-max whitespace-nowrap text-cyan-100 drop-shadow bg-cyan-950/95 rounded border border-cyan-700/80 text-center z-20 shadow-[0_0_10px_rgba(0,0,0,0.5)] leading-tight text-[10px] px-2 py-0.5 pointer-events-none"
            :class="getLabelPlacementClasses(dock.labelPlacement)"
          >
            {{ $t(`location.${dock.location}`) }}
          </div>
        </div>
      </template>
    </div>
  </div>
</template>
