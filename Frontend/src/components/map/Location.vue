<script setup lang="ts">
import { computed } from "vue";
import { ImageCatalog } from "@/catalogs/image.catalog.ts";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

const props = defineProps<{
  location: LocationViewModel | null;
}>();

const locationImage = computed(() => {
  return ImageCatalog.getMapImageUrl(props.location?.image ?? null);
});
</script>

<template>
  <div
    class="flex-1 min-h-0 relative w-full rounded border-2 border-[#0044aa]/50 bg-black bg-opacity-60 flex items-center justify-center overflow-hidden shadow-inner"
    :class="{ 'bg-grid-pattern': !locationImage }"
  >
    <img
      v-if="locationImage"
      :src="locationImage"
      class="absolute inset-0 w-full h-full object-cover opacity-60 mix-blend-lighten pointer-events-none"
    />

    <div class="absolute top-1 left-1 w-3 h-3 border-t-2 border-l-2 border-[#00aaff]/60"></div>
    <div class="absolute bottom-1 right-1 w-3 h-3 border-b-2 border-r-2 border-[#00aaff]/60"></div>
    <div class="absolute top-1 right-1 w-3 h-3 border-t-2 border-r-2 border-[#00aaff]/60"></div>
    <div class="absolute bottom-1 left-1 w-3 h-3 border-b-2 border-l-2 border-[#00aaff]/60"></div>
  </div>
</template>
