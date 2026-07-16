<script setup lang="ts">
import DesertArea from "@/components/desert/DesertArea.vue";
import { DESERT_AREAS, DESERT_GRID_SIZE } from "@/constants/desert.constant";

const desertAreas = DESERT_AREAS.flat();

function hasRightConnection(areaIndex: number): boolean {
  return (areaIndex + 1) % DESERT_GRID_SIZE !== 0;
}

function hasBottomConnection(areaIndex: number): boolean {
  return areaIndex < desertAreas.length - DESERT_GRID_SIZE;
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
      />
    </div>
  </div>
</template>
