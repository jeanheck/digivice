<script setup lang="ts">
import { computed } from "vue";
import Dock from "@/components/seabed/Dock.vue";
import noiseDesertSMapUrl from "@/assets/maps/Noise Desert S.webp";
import mirageTowerMapUrl from "@/assets/maps/Mirage Tower.webp";
import mobiusDesertMapUrl from "@/assets/maps/Mobius Desert.webp";
import mobiusDesertSecondMapUrl from "@/assets/maps/Mobius Desert 2.webp";
import { MobiusDesertPresenter } from "@/presenters/map/mobius-desert.presenter";
import type { CoordinatesViewModel } from "@/viewmodels/quest/coordinates.viewmodel";
import type { DesertAreaViewModel } from "@/viewmodels/desert/desert-area.viewmodel";
import type { DesertAreaTypeViewModel } from "@/viewmodels/desert/desert-area-type.viewmodel";
import { SEABED_MAP_FRAME_WIDTH_PX } from "@/components/seabed/seabed-map-frame";

const props = defineProps<{
  selectedArea: DesertAreaViewModel | null;
}>();

const imageUrlByAreaType: Partial<Record<DesertAreaTypeViewModel, string>> = {
  noiseDesertS: noiseDesertSMapUrl,
  mirageTower: mirageTowerMapUrl
};

const imageUrlByLocationId: Record<string, string> = {
  "0258": mobiusDesertMapUrl,
  "0259": mobiusDesertSecondMapUrl
};

const selectedAreaDetails = computed(() => {
  if (props.selectedArea?.type !== "normal") {
    return null;
  }

  return MobiusDesertPresenter.getAreaDetails(props.selectedArea.label);
});

const selectedImageUrl = computed(() => {
  if (props.selectedArea === null) {
    return null;
  }

  if (props.selectedArea.type !== "normal") {
    return imageUrlByAreaType[props.selectedArea.type] ?? null;
  }

  const locationId = selectedAreaDetails.value?.locationId;

  if (locationId === undefined) {
    return null;
  }

  return imageUrlByLocationId[locationId] ?? null;
});

const coordinates = computed<CoordinatesViewModel | null>(() => {
  return selectedAreaDetails.value?.coordinates ?? null;
});
</script>

<template>
  <div class="flex h-full w-full items-center justify-center">
    <Dock v-if="selectedImageUrl" :image-url="selectedImageUrl" :coordinates="coordinates" />
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
