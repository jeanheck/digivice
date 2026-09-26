<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import MapFrame from "@/components/map-frame/MapFrame.vue";
import type { MapFrameSlideViewModel } from "@/viewmodels/map-frame/map-frame-slide.viewmodel";
import type { StepViewModel } from "@/viewmodels/quest/step.viewmodel";

/**
 * Canonical width for quest pin / zoomed-location map rendering.
 * Quest JSON coordinates are calibrated against this size (not MapFrameWidthPx = 600).
 */
const MapDisplayWidthPx = 512;

/** Left padding (24px) + stable scrollbar gutter (~16px) around the fixed-width map. */
const MapPanelHorizontalGutterPx = 40;
const MapPanelMinWidthPx = MapDisplayWidthPx + MapPanelHorizontalGutterPx;

const MapFrameQuestWorldPinWrapperSizePx = 32;
const MapFrameQuestWorldPinDotSizePx = 10;
const MapFrameQuestWorldPinLabelVerticalOffsetPx = 26;
const MapFrameQuestWorldPinLabelVerticalThresholdPercent = 20;

const MapFrameQuestLocalPinWrapperSizePx = 24;
const MapFrameQuestLocalPinDotSizePx = 8;
const MapFrameQuestLocalPinLabelVerticalOffsetPx = 25;

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
    :style="{ minWidth: `${MapPanelMinWidthPx}px` }"
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
        :width="MapDisplayWidthPx"
        :max-height="null"
        :pin-wrapper-size-px="MapFrameQuestWorldPinWrapperSizePx"
        :pin-dot-size-px="MapFrameQuestWorldPinDotSizePx"
        pin-label-class="text-[9px] px-3 py-1"
        :pin-label-vertical-offset-px="MapFrameQuestWorldPinLabelVerticalOffsetPx"
        :pin-label-vertical-threshold-percent="
          MapFrameQuestWorldPinLabelVerticalThresholdPercent
        "
      />

      <MapFrame
        v-if="localMapSlides.length > 0"
        :slides="localMapSlides"
        :width="MapDisplayWidthPx"
        :max-height="null"
        :pin-wrapper-size-px="MapFrameQuestLocalPinWrapperSizePx"
        :pin-dot-size-px="MapFrameQuestLocalPinDotSizePx"
        :pin-label-vertical-offset-px="MapFrameQuestLocalPinLabelVerticalOffsetPx"
      />
    </template>
  </div>
</template>
