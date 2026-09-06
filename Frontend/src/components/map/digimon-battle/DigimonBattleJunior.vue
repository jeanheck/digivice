<script setup lang="ts">
import { ImageCatalog } from "@/catalogs/image.catalog";

const juniorImageUrl = ImageCatalog.getJuniorImageUrl();
</script>

<template>
  <div
    v-if="juniorImageUrl"
    class="absolute -bottom-12 -right-6 z-2 w-full h-[110%] overflow-hidden pointer-events-none"
  >
    <img
      :src="juniorImageUrl"
      alt=""
      class="absolute top-0 right-0 w-full max-w-none h-[260%] object-contain object-top drop-shadow-[0_6px_14px_rgba(0,0,0,1)]"
    />
  </div>
</template>
