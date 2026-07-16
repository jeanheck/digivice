<script setup lang="ts">
import DesertArea from "@/components/desert/DesertArea.vue";
import { DesertPresenter } from "@/presenters/desert/desert.presenter";
import type { DesertAreaTypeViewModel } from "@/viewmodels/desert/desert-area-type.viewmodel";

const emit = defineEmits<{
  "select-area": [areaType: DesertAreaTypeViewModel];
}>();

const desertAreasViewModel = DesertPresenter.getAreas();
const desertAreas = desertAreasViewModel.areas.flat();
const desertGridSize = desertAreasViewModel.gridSize;

function isClickable(areaType: DesertAreaTypeViewModel): boolean {
  return areaType === "noiseDesert" || areaType === "mirageTower";
}

function onAreaClick(areaType: DesertAreaTypeViewModel): void {
  if (!isClickable(areaType)) {
    return;
  }

  emit("select-area", areaType);
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
      <DesertArea
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
        @click="onAreaClick(desertArea.type)"
      />
    </div>
  </div>
</template>
