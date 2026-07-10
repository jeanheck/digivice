<script setup lang="ts">
import { computed, ref, watch } from "vue";
import { MAP_DISPLAY_WIDTH_PX } from "@/constants/map-display.constant";

const props = defineProps<{
  imageUrl: string | null;
}>();

const imageNaturalSize = ref<{ width: number; height: number } | null>(null);

const mapImageFrameStyle = computed(() => {
  if (imageNaturalSize.value === null) {
    return {
      width: `${MAP_DISPLAY_WIDTH_PX}px`,
      minHeight: `${Math.round(MAP_DISPLAY_WIDTH_PX * 0.75)}px`,
    };
  }

  const displayHeight = Math.round(
    MAP_DISPLAY_WIDTH_PX * (imageNaturalSize.value.height / imageNaturalSize.value.width)
  );

  return {
    width: `${MAP_DISPLAY_WIDTH_PX}px`,
    height: `${displayHeight}px`,
  };
});

const onImageLoad = (event: Event) => {
  const imageElement = event.target as HTMLImageElement;

  if (imageElement.naturalWidth === 0) {
    return;
  }

  imageNaturalSize.value = {
    width: imageElement.naturalWidth,
    height: imageElement.naturalHeight,
  };
};

watch(
  () => props.imageUrl,
  () => {
    imageNaturalSize.value = null;
  }
);
</script>

<template>
  <div
    class="relative shrink-0 bg-[#00051a] border border-cyan-800/50 rounded overflow-hidden shadow-[0_0_15px_rgba(0,170,255,0.1)]"
    :style="{ width: `${MAP_DISPLAY_WIDTH_PX}px` }"
  >
    <div class="relative overflow-hidden" :style="mapImageFrameStyle">
      <img
        v-if="imageUrl"
        :key="imageUrl"
        :src="imageUrl"
        class="block w-full h-full"
        @load="onImageLoad"
      />
    </div>
  </div>
</template>
