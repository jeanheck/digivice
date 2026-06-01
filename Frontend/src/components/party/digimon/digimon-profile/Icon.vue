<script setup lang="ts">
import { computed } from "vue";
import { ImageCatalog } from "@/catalogs/image.catalog.ts";

const props = defineProps<{
  digimonName: string;
}>();
const digimonIconUrl = computed(() => {
  return ImageCatalog.getDigimonIconUrl(props.digimonName);
});
</script>

<template>
  <div class="bg-[#000e3f] rounded overflow-hidden shadow shrink-0 flex items-center justify-center border-2 border-[#00154a] relative">
    <img v-if="digimonIconUrl" :src="digimonIconUrl" :alt="digimonName" class="w-full h-full object-cover rendering-pixelated" />
    <div v-else class="absolute inset-0 flex items-center justify-center bg-[#001233] text-yellow-500 font-bold text-lg select-none">?</div>
  </div>
</template>
