<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { WikiLocationMapMarkerViewModel } from "@/viewmodels/wiki-modal/wiki-location-map-marker.viewmodel";

const props = defineProps<{
  marker: WikiLocationMapMarkerViewModel;
}>();

const emit = defineEmits<{
  (e: "select"): void;
}>();

const { t } = useI18n();

const layoutClass = computed(() => {
  switch (props.marker.labelPlacement) {
    case "above":
      return "flex-col-reverse items-center";
    case "left":
      return "flex-row-reverse items-center";
    case "right":
      return "flex-row items-center";
    default:
      return "flex-col items-center";
  }
});

const handleSelect = (): void => {
  emit("select");
};
</script>

<template>
  <button
    type="button"
    class="group absolute z-10 -translate-x-1/2 -translate-y-1/2 flex gap-1 cursor-pointer focus:outline-none"
    :class="layoutClass"
    :style="{
      left: marker.coordinates.x + '%',
      top: marker.coordinates.y + '%',
    }"
    @click="handleSelect"
  >
    <img
      v-if="marker.imageUrl"
      :src="marker.imageUrl"
      :alt="t(marker.nameKey)"
      class="w-8 h-8 object-contain rendering-pixelated shrink-0 transition-all duration-150 drop-shadow-[0_2px_3px_rgba(0,0,0,0.9)] group-hover:drop-shadow-[0_0_8px_rgba(0,255,255,0.85)] group-hover:scale-105"
    />
    <span
      class="w-max whitespace-nowrap text-cyan-100 drop-shadow bg-cyan-950/95 rounded border border-cyan-700/80 text-center text-[9px] px-2 py-0.5 leading-tight shadow-[0_0_10px_rgba(0,0,0,0.5)]"
    >
      {{ t(marker.nameKey) }}
    </span>
  </button>
</template>
