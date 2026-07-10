<script setup lang="ts">
import { computed, ref, watch } from "vue";

const MAP_FRAME_WIDTH_PX = 600;

const props = defineProps<{
  imageUrl: string | null;
}>();

const dock = {
  name: "Divermon's Lake",
  x: 70,
  y: 70,
};

const imageNaturalSize = ref<{ width: number; height: number } | null>(null);

const mapImageFrameStyle = computed(() => {
  if (imageNaturalSize.value === null) {
    return {
      width: `${MAP_FRAME_WIDTH_PX}px`,
      minHeight: `${Math.round(MAP_FRAME_WIDTH_PX * 0.75)}px`,
    };
  }

  const displayHeight = Math.round(
    MAP_FRAME_WIDTH_PX * (imageNaturalSize.value.height / imageNaturalSize.value.width)
  );

  return {
    width: `${MAP_FRAME_WIDTH_PX}px`,
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
    class="relative shrink-0 max-h-full min-h-0 overflow-y-auto overflow-x-hidden custom-scroll bg-[#00051a] border border-cyan-800/50 rounded shadow-[0_0_15px_rgba(0,170,255,0.1)]"
    :style="{ width: `${MAP_FRAME_WIDTH_PX}px` }"
  >
    <div class="relative overflow-hidden" :style="mapImageFrameStyle">
      <img
        v-if="imageUrl"
        :key="imageUrl"
        :src="imageUrl"
        class="block w-full h-full"
        @load="onImageLoad"
      />

      <div
        class="absolute z-10 w-3.5 h-3.5 rounded-full bg-cyan-500/50 -translate-x-1/2 -translate-y-1/2 pointer-events-none"
        :style="{ left: dock.x + '%', top: dock.y + '%' }"
      />
    </div>
  </div>
</template>
