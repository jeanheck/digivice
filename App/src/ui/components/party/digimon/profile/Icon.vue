<script setup lang="ts">
import { computed } from "vue";
import { ImageCatalog } from "@/ui/catalogs/image.catalog";
import { DigimonStatusColor } from "@/shared/constants/digimon-status-color.constant";
import type { DigimonStatus } from "@/shared/types/digimon-status.type";

const props = defineProps<{
  digimonName: string;
  condition: DigimonStatus;
}>();

const emit = defineEmits<{
  showTooltip: [event: MouseEvent];
  moveTooltip: [event: MouseEvent];
  hideTooltip: [];
}>();

const digimonIconUrl = computed(() => {
  return ImageCatalog.getDigimonImageUrl(props.digimonName);
});

const backgroundColor = computed(() => {
  return DigimonStatusColor[props.condition];
});
</script>

<template>
  <div
    class="rounded overflow-hidden shadow shrink-0 flex items-center justify-center border-2 border-[#00154a] relative cursor-help"
    :style="{ backgroundColor }"
    @mouseenter="emit('showTooltip', $event)"
    @mousemove="emit('moveTooltip', $event)"
    @mouseleave="emit('hideTooltip')"
  >
    <img
      v-if="digimonIconUrl"
      :src="digimonIconUrl"
      :alt="digimonName"
      class="w-full h-full object-contain p-1 rendering-pixelated"
    />
    <div
      v-else
      class="absolute inset-0 flex items-center justify-center bg-[#001233] text-yellow-500 font-bold text-lg select-none"
    >
      ?
    </div>
  </div>
</template>
