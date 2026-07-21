<script setup lang="ts">
import MobiusDesertArea from "@/components/mobius-desert-modal/MobiusDesertArea.vue";
import { MobiusDesertAreasPresenter } from "@/presenters/mobius-desert-modal/mobius-desert-areas.presenter";
import type { DesertAreaViewModel } from "@/viewmodels/desert/desert-area.viewmodel";
import type { DesertAreaTypeViewModel } from "@/viewmodels/desert/desert-area-type.viewmodel";

const emit = defineEmits<{
  "select-area": [area: DesertAreaViewModel];
}>();

const desertAreasViewModel = MobiusDesertAreasPresenter.getAreas();
const desertAreas = desertAreasViewModel.areas.flat();
const desertGridSize = desertAreasViewModel.gridSize;

function isClickable(areaType: DesertAreaTypeViewModel): boolean {
  return areaType !== "border";
}

function onAreaClick(area: DesertAreaViewModel): void {
  if (!isClickable(area.type)) {
    return;
  }

  emit("select-area", area);
}

function hasRightConnection(areaIndex: number): boolean {
  return (areaIndex + 1) % desertGridSize !== 0;
}

function hasBottomConnection(areaIndex: number): boolean {
  return areaIndex < desertAreas.length - desertGridSize;
}

function getRightNeighborType(areaIndex: number): DesertAreaTypeViewModel | null {
  if (!hasRightConnection(areaIndex)) {
    return null;
  }

  return desertAreas[areaIndex + 1]?.type ?? null;
}

function getBottomNeighborType(areaIndex: number): DesertAreaTypeViewModel | null {
  if (!hasBottomConnection(areaIndex)) {
    return null;
  }

  return desertAreas[areaIndex + desertGridSize]?.type ?? null;
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
        @click="onAreaClick(desertArea)"
      />
    </div>
  </div>
</template>
