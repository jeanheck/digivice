<script setup lang="ts">
import { computed, ref, watch } from "vue";
import { DocksPresenter } from "@/presenters/map/docks.presenter";

const MAP_FRAME_WIDTH_PX = 600;

const props = defineProps<{
  imageUrl: string | null;
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

function onRouteEnter(routeId: string): void {
  hoveredRouteId.value = routeId;
}

function onRouteLeave(): void {
  hoveredRouteId.value = null;
}

function isRouteHovered(routeId: string): boolean {
  return hoveredRouteId.value === routeId;
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
    class="relative shrink-0 max-h-full min-h-0 overflow-y-auto overflow-x-hidden custom-scroll bg-[#00051a] border border-cyan-800/50 rounded shadow-[0_0_15px_rgba(0,170,255,0.1)]"
    :style="{ width: `${MAP_FRAME_WIDTH_PX}px` }"
  >
    <div class="relative overflow-hidden" :style="mapImageFrameStyle">
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
          class="absolute z-10 rounded-full -translate-x-1/2 -translate-y-1/2 transition-all"
          :class="[
            dock.type === 'dead-end' ? 'w-3 h-3 cursor-help' : 'w-7 h-7 cursor-pointer',
            isRouteHovered(route.id) ? 'bg-cyan-300' : 'bg-cyan-500/50',
          ]"
          :style="{ left: dock.x + '%', top: dock.y + '%' }"
          @mouseenter="onRouteEnter(route.id)"
          @mouseleave="onRouteLeave"
        />
      </template>
    </div>
  </div>
</template>
