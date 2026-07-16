<script setup lang="ts">
import DesertArea from "@/components/desert/DesertArea.vue";

const GRID_SIZE = 6;
const DESERT_AREA_COUNT = GRID_SIZE * GRID_SIZE;

function hasRightConnection(areaIndex: number): boolean {
  return (areaIndex + 1) % GRID_SIZE !== 0;
}

function hasBottomConnection(areaIndex: number): boolean {
  return areaIndex < DESERT_AREA_COUNT - GRID_SIZE;
}
</script>

<template>
  <div class="flex h-full w-full items-center justify-center">
    <div class="grid grid-cols-6 gap-8.75">
      <DesertArea
        v-for="areaIndex in DESERT_AREA_COUNT"
        :key="areaIndex"
        :has-right-connection="hasRightConnection(areaIndex - 1)"
        :has-bottom-connection="hasBottomConnection(areaIndex - 1)"
      />
    </div>
  </div>
</template>
