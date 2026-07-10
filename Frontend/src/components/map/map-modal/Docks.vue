<script setup lang="ts">
import { computed, ref, watch } from "vue";

const MAP_FRAME_WIDTH_PX = 600;

const props = defineProps<{
  imageUrl: string | null;
}>();

const routes = [
  {
    name: "Divermon's Lake - Duel Island",
    docks: [
      { name: "Divermon's Lake", x: 74.5, y: 47, type: "normal" },
      { name: "Duel Island", x: 76.5, y: 24, type: "normal" },
    ],
  },
  {
    name: "Bulk Bridge - Central Park - Kicking Forest",
    docks: [
      { name: "Bulk Bridge", x: 79, y: 72, type: "normal" },
      { name: "Central Park", x: 50.5, y: 50.5, type: "normal" },
      { name: "Kicking Forest", x: 87, y: 47, type: "normal" },
    ],
  },
  {
    name: "Asuka City - Asuka Bridge - Asuka Sewers",
    docks: [
      { name: "Asuka City", x: 46.5, y: 43, type: "dead-end" },
      { name: "Asuka Bridge", x: 51, y: 44, type: "normal" },
      { name: "Asuka Sewers", x: 55, y: 40, type: "dead-end" },
    ],
  },
];

const hoveredRouteName = ref<string | null>(null);

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

function onRouteEnter(routeName: string): void {
  hoveredRouteName.value = routeName;
}

function onRouteLeave(): void {
  hoveredRouteName.value = null;
}

function isRouteHovered(routeName: string): boolean {
  return hoveredRouteName.value === routeName;
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
          :key="route.name"
        >
          <template
            v-for="segmentIndex in route.docks.length - 1"
            :key="`${route.name}-${segmentIndex}`"
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
              @mouseenter="onRouteEnter(route.name)"
              @mouseleave="onRouteLeave"
            />
            <line
              :x1="route.docks[segmentIndex - 1].x"
              :y1="route.docks[segmentIndex - 1].y"
              :x2="route.docks[segmentIndex].x"
              :y2="route.docks[segmentIndex].y"
              :stroke="isRouteHovered(route.name) ? '#67e8f9' : '#06b6d4'"
              :stroke-opacity="isRouteHovered(route.name) ? 1 : 0.5"
              :stroke-width="isRouteHovered(route.name) ? 1 : 1"
              pointer-events="none"
            />
          </template>
        </template>
      </svg>

      <template
        v-for="route in routes"
        :key="route.name"
      >
        <div
          v-for="dock in route.docks"
          :key="dock.name"
          class="absolute z-10 rounded-full -translate-x-1/2 -translate-y-1/2 transition-all"
          :class="[
            dock.type === 'dead-end' ? 'w-3 h-3 cursor-help' : 'w-7 h-7 cursor-pointer',
            isRouteHovered(route.name) ? 'bg-cyan-300' : 'bg-cyan-500/50',
          ]"
          :style="{ left: dock.x + '%', top: dock.y + '%' }"
          @mouseenter="onRouteEnter(route.name)"
          @mouseleave="onRouteLeave"
        />
      </template>
    </div>
  </div>
</template>
