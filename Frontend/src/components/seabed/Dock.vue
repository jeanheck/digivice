<script setup lang="ts">
import { computed, ref, watch } from "vue";
import type { CoordinatesViewModel } from "@/viewmodels/quest/coordinates.viewmodel";

const MAP_FRAME_WIDTH_PX = 600;

const props = defineProps<{
  imageUrl: string | null;
  coordinates: CoordinatesViewModel | null;
}>();

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
    <div
      class="relative overflow-hidden flex items-center justify-center"
      :style="mapImageFrameStyle"
    >
      <p
        v-if="!imageUrl"
        class="text-cyan-500/50 text-sm tracking-widest text-center px-8"
      >
        {{ $t("map.noDock") }}
      </p>

      <img
        v-if="imageUrl"
        :key="imageUrl"
        :src="imageUrl"
        class="block w-full h-full"
        @load="onImageLoad"
      />

      <div
        v-if="coordinates"
        class="absolute z-10 w-12 h-12 -translate-x-1/2 -translate-y-1/2 flex items-center justify-center pointer-events-none"
        :style="{ left: coordinates.x + '%', top: coordinates.y + '%' }"
      >
        <div class="absolute inset-0 rounded-full border border-cyan-400 animate-ping opacity-90" />
        <div class="w-4 h-4 rounded-full bg-cyan-400 shadow-[0_0_8px_rgba(0,255,255,1)]" />
      </div>
    </div>
  </div>
</template>
