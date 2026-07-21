<script setup lang="ts">
import { computed } from "vue";
import MobiusDesertArea from "@/components/mobius-desert-modal/MobiusDesertArea.vue";
import { MobiusDesertAreasPresenter } from "@/presenters/mobius-desert-modal/mobius-desert-areas.presenter";
import type { DesertAreaViewModel } from "@/viewmodels/desert/desert-area.viewmodel";
import type { DesertAreaTypeViewModel } from "@/viewmodels/desert/desert-area-type.viewmodel";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

const props = defineProps<{
  location: LocationViewModel | null;
  mapVariant: number;
}>();

const emit = defineEmits<{
  "select-area": [area: DesertAreaViewModel];
}>();

const desertAreasViewModel = MobiusDesertAreasPresenter.getAreas();
const desertAreas = desertAreasViewModel.areas.flat();
const desertGridSize = desertAreasViewModel.gridSize;

const currentAreaLabel = computed(() => {
  return MobiusDesertAreasPresenter.getCurrentAreaLabel(
    props.location?.id ?? null,
    props.mapVariant,
  );
});

function isClickable(areaType: DesertAreaTypeViewModel): boolean {
  return areaType !== "border";
}

function isPlayerLocation(area: DesertAreaViewModel): boolean {
  if (currentAreaLabel.value === null) {
    return false;
  }

  return area.type === "normal" && area.label === currentAreaLabel.value;
}

function onAreaClick(area: DesertAreaViewModel): void {
  if (!isClickable(area.type)) {
    return;
  }

  emit("select-area", area);
}

function getRightNeighbor(areaIndex: number): DesertAreaViewModel | null {
  if ((areaIndex + 1) % desertGridSize === 0) {
    return null;
  }

  return desertAreas[areaIndex + 1] ?? null;
}

function getBottomNeighbor(areaIndex: number): DesertAreaViewModel | null {
  if (areaIndex >= desertAreas.length - desertGridSize) {
    return null;
  }

  return desertAreas[areaIndex + desertGridSize] ?? null;
}

function hasRightConnection(areaIndex: number): boolean {
  const source = desertAreas[areaIndex];
  if (source === undefined) {
    return false;
  }

  return MobiusDesertAreasPresenter.shouldDrawRightConnection(source, getRightNeighbor(areaIndex));
}

function hasBottomConnection(areaIndex: number): boolean {
  const source = desertAreas[areaIndex];
  if (source === undefined) {
    return false;
  }

  return MobiusDesertAreasPresenter.shouldDrawBottomConnection(source, getBottomNeighbor(areaIndex));
}

function getRightNeighborType(areaIndex: number): DesertAreaTypeViewModel | null {
  return getRightNeighbor(areaIndex)?.type ?? null;
}

function getBottomNeighborType(areaIndex: number): DesertAreaTypeViewModel | null {
  return getBottomNeighbor(areaIndex)?.type ?? null;
}
</script>

<template>
  <div class="flex h-full w-full items-center justify-center">
    <div class="grid grid-cols-6 gap-8.75">
      <MobiusDesertArea
        v-for="(desertArea, areaIndex) in desertAreas"
        :key="areaIndex"
        :has-right-connection="hasRightConnection(areaIndex)"
        :has-bottom-connection="hasBottomConnection(areaIndex)"
        :label="desertArea.label"
        :type="desertArea.type"
        :note="desertArea.note ?? ''"
        :right-neighbor-type="getRightNeighborType(areaIndex)"
        :bottom-neighbor-type="getBottomNeighborType(areaIndex)"
        :clickable="isClickable(desertArea.type)"
        :is-player-location="isPlayerLocation(desertArea)"
        @click="onAreaClick(desertArea)"
      />
    </div>
  </div>
</template>
