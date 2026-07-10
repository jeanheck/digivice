<script setup lang="ts">
import { computed } from "vue";

export interface SeabedEmergePoint {
  emoji: string;
  location: string;
}

const props = withDefaults(
  defineProps<{
    emerge?: SeabedEmergePoint[];
    leftLocations?: string[];
    rightLocations?: string[];
  }>(),
  {
    emerge: () => [
      { emoji: "↖️", location: "Asuka City" },
      { emoji: "⬆️", location: "Asuka City" },
    ],
    leftLocations: () => ["Solo Oceânico", "Terreno Baldio (Nordeste)"],
    rightLocations: () => ["Solo Oceânico", "Capa de Plug"],
  }
);

const hasEmerge = computed(() => props.emerge.length > 0);
const hasLeft = computed(() => props.leftLocations.length > 0);
const hasRight = computed(() => props.rightLocations.length > 0);
</script>

<template>
  <div v-if="hasEmerge || hasLeft || hasRight" class="w-full flex justify-center shrink-0 px-0.5">
    <div class="map-info-panel flex flex-col gap-1.5">
      <div v-if="hasEmerge" class="flex items-start justify-center gap-3">
        <div
          v-for="(point, index) in emerge"
          :key="`${point.location}-${index}`"
          class="flex flex-col items-center gap-0.5 min-w-0"
        >
          <span class="text-[15px] leading-none">{{ point.emoji }}</span>
          <span class="text-[9px] text-cyan-300 text-center leading-tight">{{ point.location }}</span>
        </div>
      </div>

      <div v-if="hasLeft" class="flex items-center gap-1.5 w-full">
        <span class="text-[15px] leading-none shrink-0">⬅️</span>
        <span class="text-[9px] text-cyan-300 text-left leading-tight min-w-0">{{ leftLocations.join(", ") }}</span>
      </div>

      <div v-if="hasRight" class="flex items-center justify-end gap-1.5 w-full">
        <span class="text-[9px] text-cyan-300 text-right leading-tight min-w-0">{{ rightLocations.join(", ") }}</span>
        <span class="text-[15px] leading-none shrink-0">➡️</span>
      </div>
    </div>
  </div>
</template>
