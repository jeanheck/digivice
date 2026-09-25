<script setup lang="ts">
import { computed } from "vue";
import MapFrame from "@/components/map-frame/MapFrame.vue";
import { MAP_FRAME_WIDTH_PX } from "@/constants/map-display.constant";
import { MobiusDesertAreaDetailsPresenter } from "@/presenters/mobius-desert-modal/mobius-desert-area-details.presenter";
import type { DesertAreaViewModel } from "@/viewmodels/desert/desert-area.viewmodel";
import type { MapFrameSlideViewModel } from "@/viewmodels/map-frame/map-frame-slide.viewmodel";

const props = defineProps<{
  selectedArea: DesertAreaViewModel | null;
}>();

const areaDetails = computed(() => {
  return MobiusDesertAreaDetailsPresenter.getAreaDetails(props.selectedArea);
});

const selectedImageUrl = computed(() => {
  return areaDetails.value?.imageUrl ?? null;
});

const coordinates = computed(() => {
  return areaDetails.value?.coordinates ?? null;
});

const areaSlides = computed((): MapFrameSlideViewModel[] => {
  const imageUrl = selectedImageUrl.value;
  if (imageUrl === null) {
    return [];
  }

  if (coordinates.value === null) {
    return [
      {
        imageUrl,
        pins: [],
      },
    ];
  }

  const pinLabel = props.selectedArea?.note ?? null;

  return [
    {
      imageUrl,
      pins: [
        {
          coordinates: coordinates.value,
          label: pinLabel,
        },
      ],
    },
  ];
});
</script>

<template>
  <div class="flex h-full w-full items-center justify-center">
    <MapFrame v-if="areaSlides.length > 0" :slides="areaSlides" />
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
