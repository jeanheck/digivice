<script setup lang="ts">
import asukaMapUrl from "@/assets/AsukaMap.webp";
import DockMapFrame from "@/components/map/map-modal/DockMapFrame.vue";
import { ImageCatalog } from "@/catalogs/image.catalog.ts";

const plugCapeMapUrl = ImageCatalog.getMapImageUrl("Reliability Spot");
</script>

<template>
  <div class="flex flex-1 min-h-0 w-full gap-4 p-4 overflow-y-auto overflow-x-hidden">
    <div class="flex flex-1 min-w-0 min-h-0 max-h-full items-center justify-center">
      <DockMapFrame :image-url="asukaMapUrl" />
    </div>
    <div class="flex flex-1 min-w-0 min-h-0 max-h-full items-center justify-center">
      <DockMapFrame :image-url="plugCapeMapUrl" />
    </div>
  </div>
</template>
