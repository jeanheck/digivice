<script setup lang="ts">
import { computed, ref } from "vue";
import { ImageCatalog } from "@/catalogs/image.catalog.ts";

const props = defineProps<{
  digimonName: string;
}>();

const hasError = ref(false);

const digimonIconUrl = computed(() => {
  return ImageCatalog.getDigimonIconUrl(props.digimonName);
});

const handleImageError = () => {
  hasError.value = true;
};
</script>

<template>
  <div class="bg-[#000e3f] rounded overflow-hidden shadow shrink-0 flex items-center justify-center border-2 border-[#00154a] relative">
    <img 
      v-if="digimonIconUrl && !hasError"
      :src="digimonIconUrl" 
      :alt="digimonName"
      class="w-full h-full object-cover rendering-pixelated"
      @error="handleImageError"
    />
    <div 
      v-else 
      class="absolute inset-0 flex items-center justify-center bg-[#001233] text-yellow-500 font-bold text-lg select-none">?
    </div>
  </div>
</template>
