<script setup lang="ts">
import { computed, ref, watch } from "vue";
import { MAP_FRAME_MAX_HEIGHT_PX, MAP_FRAME_WIDTH_PX } from "@/constants/map-display.constant";
import { useMapFrame } from "@/composables/use-map-frame";
import type { MapFrameSlideViewModel } from "@/viewmodels/map-frame/map-frame-slide.viewmodel";
import type { MapFramePinViewModel } from "@/viewmodels/map-frame/map-frame-pin.viewmodel";

const MAP_FRAME_DEFAULT_PIN_WRAPPER_SIZE_PX = 48;
const MAP_FRAME_DEFAULT_PIN_DOT_SIZE_PX = 16;
const MAP_FRAME_DEFAULT_PIN_LABEL_VERTICAL_OFFSET_PX = 40;
const MAP_FRAME_DEFAULT_PIN_LABEL_VERTICAL_THRESHOLD_PERCENT = 25;
const MAP_LABEL_LEFT_EDGE_THRESHOLD_PERCENT = 25;
const MAP_LABEL_RIGHT_EDGE_THRESHOLD_PERCENT = 75;

const props = withDefaults(
  defineProps<{
    slides: MapFrameSlideViewModel[];
    width?: number;
    maxHeight?: number | null;
    pinWrapperSizePx?: number;
    pinDotSizePx?: number;
    pinLabelClass?: string;
    pinLabelVerticalOffsetPx?: number;
    pinLabelVerticalThresholdPercent?: number;
  }>(),
  {
    width: MAP_FRAME_WIDTH_PX,
    maxHeight: MAP_FRAME_MAX_HEIGHT_PX,
    pinWrapperSizePx: MAP_FRAME_DEFAULT_PIN_WRAPPER_SIZE_PX,
    pinDotSizePx: MAP_FRAME_DEFAULT_PIN_DOT_SIZE_PX,
    pinLabelClass: "text-[9px] px-2 py-0.5",
    pinLabelVerticalOffsetPx: MAP_FRAME_DEFAULT_PIN_LABEL_VERTICAL_OFFSET_PX,
    pinLabelVerticalThresholdPercent: MAP_FRAME_DEFAULT_PIN_LABEL_VERTICAL_THRESHOLD_PERCENT,
  },
);

const currentSlideIndex = ref(0);

const activeSlide = computed(() => {
  return props.slides[currentSlideIndex.value] ?? null;
});

const showPagination = computed(() => {
  return props.slides.length > 1;
});

const activeImageUrl = computed((): string | null => {
  return activeSlide.value?.imageUrl ?? null;
});

const frameWidth = computed(() => {
  return props.width;
});

const { displayHeight, mapImageFrameStyle, onImageLoad } = useMapFrame(activeImageUrl, frameWidth);

const mapFrameStyle = computed(() => {
  const style: Record<string, string> = {
    width: `${props.width}px`,
  };

  if (props.maxHeight != null) {
    style.maxHeight = `${props.maxHeight}px`;
    style.minHeight = `${Math.min(displayHeight.value, props.maxHeight)}px`;
  }

  return style;
});

const mapFrameOverflowClass = computed(() => {
  if (props.maxHeight != null) {
    return "overflow-y-auto overflow-x-hidden custom-scroll";
  }

  return "overflow-hidden";
});

function getPinLabelHorizontalAnchorClass(coordinateX: number): string {
  if (coordinateX <= MAP_LABEL_LEFT_EDGE_THRESHOLD_PERCENT) {
    return "left-1/2 translate-x-0 ml-3";
  }

  if (coordinateX >= MAP_LABEL_RIGHT_EDGE_THRESHOLD_PERCENT) {
    return "left-1/2 -translate-x-full -mr-3";
  }

  return "left-1/2 -translate-x-1/2";
}

function getPinLabelVerticalAnchorStyle(coordinateY: number): Record<string, string> {
  if (coordinateY < props.pinLabelVerticalThresholdPercent) {
    return { top: `${props.pinLabelVerticalOffsetPx}px` };
  }

  return { bottom: `${props.pinLabelVerticalOffsetPx}px` };
}

function hasPinLabel(pin: MapFramePinViewModel): boolean {
  return pin.label != null && pin.label !== "";
}

function showPreviousSlide(): void {
  currentSlideIndex.value =
    (currentSlideIndex.value - 1 + props.slides.length) % props.slides.length;
}

function showNextSlide(): void {
  currentSlideIndex.value = (currentSlideIndex.value + 1) % props.slides.length;
}

function selectSlide(slideIndex: number): void {
  currentSlideIndex.value = slideIndex;
}

watch(
  () => props.slides,
  () => {
    currentSlideIndex.value = 0;
  },
);
</script>

<template>
  <div
    v-if="activeSlide"
    class="relative shrink-0 min-h-0 bg-[#00051a] border border-cyan-800/50 rounded shadow-[0_0_15px_rgba(0,170,255,0.1)]"
    :class="mapFrameOverflowClass"
    :style="mapFrameStyle"
  >
    <div
      v-if="showPagination"
      class="flex items-center justify-center gap-3 px-3 py-2 border-b border-cyan-900/50 bg-black/40 z-20"
    >
      <button
        type="button"
        class="w-7 h-7 rounded bg-black/80 border border-cyan-800 flex items-center justify-center text-cyan-400 hover:bg-cyan-900/80 hover:border-cyan-400 hover:text-white transition-all font-bold text-sm shadow-[0_0_10px_rgba(0,170,255,0.2)]"
        @click.prevent="showPreviousSlide"
      >
        &lt;
      </button>

      <div
        class="flex gap-2 px-3 py-1.5 bg-black/80 rounded border border-cyan-900/80 shadow-[0_0_10px_rgba(0,170,255,0.2)]"
      >
        <button
          v-for="(_, slideDotIndex) in slides"
          :key="slideDotIndex"
          type="button"
          class="w-2 h-2 rounded-full transition-all cursor-pointer"
          :class="
            Number(slideDotIndex) === currentSlideIndex
              ? 'bg-cyan-400 shadow-[0_0_8px_rgba(0,255,255,1)] scale-110'
              : 'bg-cyan-900 hover:bg-cyan-600'
          "
          @click.prevent="selectSlide(Number(slideDotIndex))"
        />
      </div>

      <button
        type="button"
        class="w-7 h-7 rounded bg-black/80 border border-cyan-800 flex items-center justify-center text-cyan-400 hover:bg-cyan-900/80 hover:border-cyan-400 hover:text-white transition-all font-bold text-sm shadow-[0_0_10px_rgba(0,170,255,0.2)]"
        @click.prevent="showNextSlide"
      >
        &gt;
      </button>
    </div>

    <div
      class="relative overflow-hidden flex items-center justify-center"
      :style="mapImageFrameStyle"
    >
      <img
        v-if="activeSlide.imageUrl"
        :key="activeSlide.imageUrl"
        :src="activeSlide.imageUrl"
        class="block w-full h-full"
        @load="onImageLoad"
      />

      <div
        v-for="(pin, pinIndex) in activeSlide.pins"
        :key="pinIndex"
        class="absolute z-10 -translate-x-1/2 -translate-y-1/2 flex items-center justify-center pointer-events-none"
        :style="{
          left: pin.coordinates.x + '%',
          top: pin.coordinates.y + '%',
          width: pinWrapperSizePx + 'px',
          height: pinWrapperSizePx + 'px',
        }"
      >
        <div class="absolute inset-0 rounded-full border border-cyan-400 animate-ping opacity-90" />
        <div
          class="rounded-full bg-cyan-400 shadow-[0_0_8px_rgba(0,255,255,1)]"
          :style="{
            width: pinDotSizePx + 'px',
            height: pinDotSizePx + 'px',
          }"
        />
        <div
          v-if="hasPinLabel(pin)"
          class="absolute w-max whitespace-nowrap text-cyan-100 drop-shadow bg-cyan-950/95 rounded border border-cyan-700/80 text-center z-20 shadow-[0_0_10px_rgba(0,0,0,0.5)] leading-tight"
          :class="[pinLabelClass, getPinLabelHorizontalAnchorClass(pin.coordinates.x)]"
          :style="getPinLabelVerticalAnchorStyle(pin.coordinates.y)"
        >
          {{ pin.label }}
        </div>
      </div>
    </div>
  </div>
</template>
