<script setup lang="ts">
import { computed } from "vue";
import type { CoordinatesViewModel } from "@/viewmodels/quest/coordinates.viewmodel";
import {
  MAP_FRAME_MAX_HEIGHT_PX,
  MAP_FRAME_WIDTH_PX,
} from "@/components/map-details-frame/map-details-frame";
import { useMapFrame } from "@/composables/use-map-frame";

const props = defineProps<{
  imageUrl: string;
  coordinates: CoordinatesViewModel | null;
  pinLabel?: string | null;
}>();

const imageUrlSource = computed((): string | null => {
  return props.imageUrl;
});

const { displayHeight, mapImageFrameStyle, onImageLoad } = useMapFrame(imageUrlSource);

const mapFrameStyle = computed(() => {
  return {
    width: `${MAP_FRAME_WIDTH_PX}px`,
    minHeight: `${displayHeight.value}px`,
    maxHeight: `${MAP_FRAME_MAX_HEIGHT_PX}px`,
  };
});

const MAP_LABEL_LEFT_EDGE_THRESHOLD_PERCENT = 25;
const MAP_LABEL_RIGHT_EDGE_THRESHOLD_PERCENT = 75;

const showPinLabel = computed(() => {
  return props.pinLabel != null && props.pinLabel !== "";
});

const mapLabelHorizontalAnchorClass = computed(() => {
  const coordinateX = props.coordinates?.x;

  if (coordinateX === undefined) {
    return "left-1/2 -translate-x-1/2";
  }

  if (coordinateX <= MAP_LABEL_LEFT_EDGE_THRESHOLD_PERCENT) {
    return "left-1/2 translate-x-0";
  }

  if (coordinateX >= MAP_LABEL_RIGHT_EDGE_THRESHOLD_PERCENT) {
    return "left-1/2 -translate-x-full";
  }

  return "left-1/2 -translate-x-1/2";
});

const mapLabelVerticalAnchorClass = computed(() => {
  const coordinateY = props.coordinates?.y;

  if (coordinateY !== undefined && coordinateY < 25) {
    return "top-6.25";
  }

  return "bottom-6.25";
});
</script>

<template>
  <div
    class="relative shrink-0 min-h-0 overflow-y-auto overflow-x-hidden custom-scroll bg-[#00051a] border border-cyan-800/50 rounded shadow-[0_0_15px_rgba(0,170,255,0.1)]"
    :style="mapFrameStyle"
  >
    <div
      class="relative overflow-hidden flex items-center justify-center"
      :style="mapImageFrameStyle"
    >
      <img
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
        <div
          v-if="showPinLabel"
          class="absolute w-max whitespace-nowrap text-cyan-100 drop-shadow bg-cyan-950/95 rounded border border-cyan-700/80 text-center z-20 shadow-[0_0_10px_rgba(0,0,0,0.5)] leading-tight text-[9px] px-2 py-0.5"
          :class="[mapLabelHorizontalAnchorClass, mapLabelVerticalAnchorClass]"
        >
          {{ pinLabel }}
        </div>
      </div>
    </div>
  </div>
</template>
