<script setup lang="ts">
import { computed } from "vue";
import Dock from "@/components/seabed/Dock.vue";
import noiseDesertMapUrl from "@/assets/maps/Noise Desert.webp";
import mirageTowerMapUrl from "@/assets/maps/Mirage Tower.webp";
import type { CoordinatesViewModel } from "@/viewmodels/quest/coordinates.viewmodel";
import type { DesertAreaTypeViewModel } from "@/viewmodels/desert/desert-area-type.viewmodel";
import { SEABED_MAP_FRAME_WIDTH_PX } from "@/components/seabed/seabed-map-frame";

const props = defineProps<{
  selectedAreaType: DesertAreaTypeViewModel | null;
}>();

const coordinates: CoordinatesViewModel | null = null;

const imageUrlByAreaType: Partial<Record<DesertAreaTypeViewModel, string>> = {
  noiseDesert: noiseDesertMapUrl,
  mirageTower: mirageTowerMapUrl,
};

const selectedImageUrl = computed(() => {
  if (props.selectedAreaType === null) {
    return null;
  }

  return imageUrlByAreaType[props.selectedAreaType] ?? null;
});
</script>

<template>
  <div class="flex h-full w-full items-center justify-center">
    <Dock
      v-if="selectedImageUrl"
      :image-url="selectedImageUrl"
      :coordinates="coordinates"
    />
    <div
      v-else
      class="flex flex-col items-center justify-center gap-3 px-8"
      :style="{ width: `${SEABED_MAP_FRAME_WIDTH_PX}px` }"
    >
      <span class="text-cyan-500/50 text-sm tracking-widest text-center animate-pulse">
        {{ $t("map.noDesertAreaSelected") }}
      </span>
    </div>
  </div>
</template>
