<script setup lang="ts">
import DockMapFrame from "@/components/map/map-modal/DockMapFrame.vue";
import { ImageCatalog } from "@/catalogs/image.catalog.ts";

const mockBoxNames = [
  "Power Charge",
  "Buckler",
  "Bandanna",
];

const centralParkMapUrl = ImageCatalog.getMapImageUrl("Central Park");
</script>

<template>
  <div class="flex flex-1 min-h-0 w-full gap-4 p-4 overflow-auto">
    <div class="flex flex-1 min-w-0 items-center justify-center">
      <ul class="flex flex-col items-center gap-3">
        <li
          v-for="boxName in mockBoxNames"
          :key="boxName"
          class="text-sm font-bold tracking-widest text-yellow-500"
        >
          {{ boxName }}
        </li>
      </ul>
    </div>

    <div class="flex flex-1 min-w-0 items-center justify-center">
      <DockMapFrame :image-url="centralParkMapUrl" />
    </div>
  </div>
</template>
