<script setup lang="ts">
import DesertArea from "@/components/desert/DesertArea.vue";
import { DESERT_AREAS, DESERT_GRID_SIZE } from "@/constants/desert.constant";
import type { DesertAreaType } from "@/constants/desert.constant";

const emit = defineEmits<{
  "select-area": [areaType: DesertAreaType];
}>();

const desertAreas = DESERT_AREAS.flat();

function isClickable(areaType: DesertAreaType): boolean {
  return areaType === "noiseDesert" || areaType === "mirageTower";
}

function onAreaClick(areaType: DesertAreaType): void {
  if (!isClickable(areaType)) {
    return;
  }

  emit("select-area", areaType);
}

function hasRightConnection(areaIndex: number): boolean {
  return (areaIndex + 1) % DESERT_GRID_SIZE !== 0;
}

function hasBottomConnection(areaIndex: number): boolean {
  return areaIndex < desertAreas.length - DESERT_GRID_SIZE;
}

function getRightNeighborType(areaIndex: number): DesertAreaType | null {
  if (!hasRightConnection(areaIndex)) {
    return null;
  }

  return desertAreas[areaIndex + 1]?.type ?? null;
}

function getBottomNeighborType(areaIndex: number): DesertAreaType | null {
  if (!hasBottomConnection(areaIndex)) {
    return null;
  }

  return desertAreas[areaIndex + DESERT_GRID_SIZE]?.type ?? null;
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
