<script setup lang="ts">
import { computed, ref, watch } from "vue";
import type { CoordinatesViewModel } from "@/viewmodels/quest/coordinates.viewmodel";

export interface ZoomedLocationMapItem {
  imageUrl: string | null;
  coordinates: CoordinatesViewModel;
  labelKey: string;
}

const props = defineProps<{
  locations: ZoomedLocationMapItem[];
  mapVariant: "world" | "local";
}>();

const currentLocationIndex = ref(0);

const activeLocation = computed(() => {
  return props.locations[currentLocationIndex.value] ?? null;
});

const showPagination = computed(() => {
  return props.mapVariant === "local" && props.locations.length > 1;
});

const isWorldMap = computed(() => {
  return props.mapVariant === "world";
});

watch(
  () => props.locations,
  () => {
    currentLocationIndex.value = 0;
  }
);

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
    class="relative w-full min-h-0 flex-1 aspect-4/3 bg-[#00051a] border border-cyan-800/50 rounded overflow-hidden shadow-[0_0_15px_rgba(0,170,255,0.1)] group"
    :class="{ 'flex flex-col': !isWorldMap }"
  >
    <div
      class="relative w-full overflow-hidden"
      :class="isWorldMap ? 'h-full' : 'flex-1 bg-black/50'"
    >
      <img
        v-if="activeLocation.imageUrl"
        :src="activeLocation.imageUrl"
        class="w-full h-full object-cover transition-all duration-500"
        :class="
          isWorldMap
            ? 'opacity-60 mix-blend-screen saturate-50 group-hover:saturate-100'
            : 'absolute inset-0 opacity-70 group-hover:opacity-100'
        "
      />

      <div
        v-if="isWorldMap"
        class="absolute inset-0 bg-blue-900/10 z-0 pointer-events-none"
      />

      <div
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
          class="absolute left-1/2 -translate-x-1/2 font-cyber text-cyan-100 drop-shadow bg-cyan-950/95 rounded border border-cyan-700/80 text-center z-20 shadow-[0_0_10px_rgba(0,0,0,0.5)] line-clamp-3 leading-tight"
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

    <div
      v-if="showPagination"
      class="absolute bottom-3 left-0 right-0 flex items-center justify-center gap-3 z-20"
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
  </div>
</template>
