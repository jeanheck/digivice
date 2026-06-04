<script setup lang="ts">
import { computed, ref, watch } from "vue";
import { MAP_DISPLAY_WIDTH_PX } from "@/constants/map-display.constant";
import type { ZoomedLocationMapViewModel } from "@/viewmodels/quest/zoomed-location-map.viewmodel";

const props = defineProps<{
  locations: ZoomedLocationMapViewModel[];
  mapVariant: "world" | "local";
}>();

const currentLocationIndex = ref(0);
const imageNaturalSize = ref<{ width: number; height: number } | null>(null);

const activeLocation = computed(() => {
  return props.locations[currentLocationIndex.value] ?? null;
});

const showPagination = computed(() => {
  return props.mapVariant === "local" && props.locations.length > 1;
});

const isWorldMap = computed(() => {
  return props.mapVariant === "world";
});

const mapImageFrameStyle = computed(() => {
  if (imageNaturalSize.value === null) {
    return {
      width: `${MAP_DISPLAY_WIDTH_PX}px`,
      minHeight: `${Math.round(MAP_DISPLAY_WIDTH_PX * 0.75)}px`,
    };
  }

  const displayHeight = Math.round(
    MAP_DISPLAY_WIDTH_PX * (imageNaturalSize.value.height / imageNaturalSize.value.width)
  );

  return {
    width: `${MAP_DISPLAY_WIDTH_PX}px`,
    height: `${displayHeight}px`,
  };
});

watch(
  () => props.locations,
  () => {
    currentLocationIndex.value = 0;
    imageNaturalSize.value = null;
  }
);

watch(
  () => activeLocation.value?.imageUrl,
  () => {
    imageNaturalSize.value = null;
  }
);

const onImageLoad = (event: Event) => {
  const imageElement = event.target as HTMLImageElement;
  imageNaturalSize.value = {
    width: imageElement.naturalWidth,
    height: imageElement.naturalHeight,
  };
};

const showPreviousLocation = () => {
  currentLocationIndex.value = (currentLocationIndex.value - 1 + props.locations.length) % props.locations.length;
};

const showNextLocation = () => {
  currentLocationIndex.value = (currentLocationIndex.value + 1) % props.locations.length;
};

const selectLocation = (locationIndex: number) => {
  currentLocationIndex.value = locationIndex;
};
</script>

<template>
  <div
    v-if="activeLocation"
    class="relative mx-auto shrink-0 bg-[#00051a] border border-cyan-800/50 rounded overflow-hidden shadow-[0_0_15px_rgba(0,170,255,0.1)]"
    :style="{ width: `${MAP_DISPLAY_WIDTH_PX}px` }"
  >
    <div
      v-if="showPagination"
      class="flex items-center justify-center gap-3 px-3 py-2 border-b border-cyan-900/50 bg-black/40 z-20"
    >
      <button
        type="button"
        class="w-7 h-7 rounded bg-black/80 border border-cyan-800 flex items-center justify-center text-cyan-400 hover:bg-cyan-900/80 hover:border-cyan-400 hover:text-white transition-all font-bold text-sm shadow-[0_0_10px_rgba(0,170,255,0.2)]"
        @click.prevent="showPreviousLocation"
      >
        &lt;
      </button>

      <div class="flex gap-2 px-3 py-1.5 bg-black/80 rounded border border-cyan-900/80 shadow-[0_0_10px_rgba(0,170,255,0.2)]">
        <button
          v-for="(_, locationDotIndex) in locations"
          :key="locationDotIndex"
          type="button"
          class="w-2 h-2 rounded-full transition-all cursor-pointer"
          :class="Number(locationDotIndex) === currentLocationIndex ? 'bg-cyan-400 shadow-[0_0_8px_rgba(0,255,255,1)] scale-110' : 'bg-cyan-900 hover:bg-cyan-600'"
          @click.prevent="selectLocation(Number(locationDotIndex))"
        />
      </div>

      <button
        type="button"
        class="w-7 h-7 rounded bg-black/80 border border-cyan-800 flex items-center justify-center text-cyan-400 hover:bg-cyan-900/80 hover:border-cyan-400 hover:text-white transition-all font-bold text-sm shadow-[0_0_10px_rgba(0,170,255,0.2)]"
        @click.prevent="showNextLocation"
      >
        &gt;
      </button>
    </div>

    <div
      class="relative overflow-hidden"
      :style="mapImageFrameStyle"
    >
      <img
        v-if="activeLocation.imageUrl"
        :key="activeLocation.imageUrl"
        :src="activeLocation.imageUrl"
        class="block w-full h-full"
        @load="onImageLoad"
      />

      <div
        v-if="imageNaturalSize"
        class="absolute -translate-x-1/2 -translate-y-1/2 z-10 flex items-center justify-center pointer-events-none"
        :class="[
          isWorldMap ? 'w-8 h-8' : 'w-6 h-6 transition-all duration-300',
        ]"
        :style="{ left: activeLocation.coordinates.x + '%', top: activeLocation.coordinates.y + '%' }"
      >
        <div class="absolute inset-0 rounded-full border border-cyan-400 animate-ping opacity-90" />
        <div
          class="rounded-full bg-cyan-400"
          :class="isWorldMap ? 'w-2.5 h-2.5 shadow-[0_0_10px_rgba(0,255,255,1)]' : 'w-2 h-2 shadow-[0_0_8px_rgba(0,255,255,1)]'"
        />
        <div
          class="absolute left-1/2 -translate-x-1/2 text-cyan-100 drop-shadow bg-cyan-950/95 rounded border border-cyan-700/80 text-center z-20 shadow-[0_0_10px_rgba(0,0,0,0.5)] line-clamp-3 leading-tight"
          :class="
            isWorldMap
              ? [
                  'text-[10px] px-3 py-1 max-w-37.5',
                  activeLocation.coordinates.y < 20 ? 'top-6.5' : 'bottom-6.5',
                ]
              : [
                  'text-[9px] px-2 py-0.5 max-w-30',
                  activeLocation.coordinates.y < 25 ? 'top-6.25' : 'bottom-6.25',
                ]
          "
        >
          {{ $t(activeLocation.labelKey) }}
        </div>
      </div>
    </div>
  </div>
</template>
