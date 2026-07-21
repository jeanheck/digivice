<script setup lang="ts">
import { computed } from "vue";
import MapDetailsFrame from "@/components/map-details-frame/MapDetailsFrame.vue";
import { MobiusDesertPresenter } from "@/presenters/map/mobius-desert.presenter";
import type { DesertAreaViewModel } from "@/viewmodels/desert/desert-area.viewmodel";
import { MAP_FRAME_WIDTH_PX } from "@/components/map-details-frame/map-details-frame";

const props = defineProps<{
  selectedArea: DesertAreaViewModel | null;
}>();

const areaDetails = computed(() => {
  return MobiusDesertPresenter.getAreaDetails(props.selectedArea);
});

const selectedImageUrl = computed(() => {
  return areaDetails.value?.imageUrl ?? null;
});

const coordinates = computed(() => {
  return areaDetails.value?.coordinates ?? null;
});
</script>

<template>
  <div class="flex h-full w-full items-center justify-center">
    <MapDetailsFrame v-if="selectedImageUrl" :image-url="selectedImageUrl" :coordinates="coordinates" />
    <div
      v-else
      class="flex flex-col items-center justify-center gap-3 px-8"
      :style="{ width: `${MAP_FRAME_WIDTH_PX}px` }"
    >
      <span class="text-cyan-500/50 text-sm tracking-widest text-center animate-pulse">
        {{ $t("map.mobiusDesertHint") }}
      </span>
    </div>
  </div>
</template>
