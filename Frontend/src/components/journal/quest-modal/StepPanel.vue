<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import MapFrame from "@/components/map-frame/MapFrame.vue";
import {
  MAP_DISPLAY_WIDTH_PX,
  MAP_FRAME_QUEST_LOCAL_PIN_DOT_SIZE_PX,
  MAP_FRAME_QUEST_LOCAL_PIN_LABEL_VERTICAL_OFFSET_PX,
  MAP_FRAME_QUEST_LOCAL_PIN_WRAPPER_SIZE_PX,
  MAP_FRAME_QUEST_WORLD_PIN_DOT_SIZE_PX,
  MAP_FRAME_QUEST_WORLD_PIN_LABEL_VERTICAL_OFFSET_PX,
  MAP_FRAME_QUEST_WORLD_PIN_LABEL_VERTICAL_THRESHOLD_PERCENT,
  MAP_FRAME_QUEST_WORLD_PIN_WRAPPER_SIZE_PX,
  MAP_PANEL_MIN_WIDTH_PX,
} from "@/constants/map-display.constant";
import type { MapFrameSlideViewModel } from "@/viewmodels/map-frame/map-frame-slide.viewmodel";
import type { StepViewModel } from "@/viewmodels/quest/step.viewmodel";

const props = defineProps<{
  selectedStep: StepViewModel | null;
  worldMapLocations: MapFrameSlideViewModel[];
  localMapLocations: MapFrameSlideViewModel[];
}>();

const { t } = useI18n();

function translateSlides(slides: MapFrameSlideViewModel[]): MapFrameSlideViewModel[] {
  return slides.map((slide) => {
    return {
      imageUrl: slide.imageUrl,
      pins: slide.pins.map((pin) => {
        return {
          coordinates: pin.coordinates,
          label: pin.label != null && pin.label !== "" ? t(pin.label) : pin.label,
        };
      }),
    };
  });
}

const worldMapSlides = computed(() => {
  return translateSlides(props.worldMapLocations);
});

const localMapSlides = computed(() => {
  return translateSlides(props.localMapLocations);
});
</script>

<template>
  <div
    class="flex min-h-0 shrink-0 flex-col items-center gap-4 overflow-x-hidden overflow-y-auto custom-scroll [scrollbar-gutter:stable] lg:flex-[0.6] lg:border-l lg:border-[#0055ff]/30 lg:pl-6"
    :style="{ minWidth: `${MAP_PANEL_MIN_WIDTH_PX}px` }"
  >
    <div
      v-if="!selectedStep"
      class="flex-1 flex flex-col items-center justify-center border border-cyan-900/40 bg-[#000a1a] rounded min-h-100"
    >
      <span
        class="text-cyan-500/50 text-sm tracking-widest text-center px-8 animate-pulse whitespace-pre-line"
      >
        {{ $t("journal.clickStep") }}
      </span>
    </div>

    <div
      v-else-if="!selectedStep.location"
      class="flex-1 flex flex-col items-center justify-center border border-red-900/40 bg-[#1a0000] rounded min-h-100"
    >
      <span class="text-red-500/50 text-sm tracking-widest text-center px-8 whitespace-pre-line">
        {{ $t("journal.noSignal") }}
      </span>
    </div>

    <template v-else>
      <MapFrame
        v-if="worldMapSlides.length > 0"
        :slides="worldMapSlides"
        :width="MAP_DISPLAY_WIDTH_PX"
        :max-height="null"
        :pin-wrapper-size-px="MAP_FRAME_QUEST_WORLD_PIN_WRAPPER_SIZE_PX"
        :pin-dot-size-px="MAP_FRAME_QUEST_WORLD_PIN_DOT_SIZE_PX"
        pin-label-class="text-[9px] px-3 py-1"
        :pin-label-vertical-offset-px="MAP_FRAME_QUEST_WORLD_PIN_LABEL_VERTICAL_OFFSET_PX"
        :pin-label-vertical-threshold-percent="
          MAP_FRAME_QUEST_WORLD_PIN_LABEL_VERTICAL_THRESHOLD_PERCENT
        "
      />

      <MapFrame
        v-if="localMapSlides.length > 0"
        :slides="localMapSlides"
        :width="MAP_DISPLAY_WIDTH_PX"
        :max-height="null"
        :pin-wrapper-size-px="MAP_FRAME_QUEST_LOCAL_PIN_WRAPPER_SIZE_PX"
        :pin-dot-size-px="MAP_FRAME_QUEST_LOCAL_PIN_DOT_SIZE_PX"
        :pin-label-vertical-offset-px="MAP_FRAME_QUEST_LOCAL_PIN_LABEL_VERTICAL_OFFSET_PX"
      />
    </template>
  </div>
</template>
