<script setup lang="ts">
import { computed, useSlots } from "vue";
import type {
  TooltipHorizontalAlign,
  TooltipPlacement,
} from "@/ui/composables/use-tooltip-position";

export type TooltipSize = "default" | "tiny";

/** Title-only / default shell ceiling (test values). */
const TitleMaxWidthPx = 360;
/** Compact battle ceiling. */
const TinyMaxWidthPx = 200;

const props = withDefaults(
  defineProps<{
    show: boolean;
    x: number;
    y: number;
    title: string;
    size?: TooltipSize;
    /** Override shell ceiling in px (e.g. DescriptiveTooltip uses a tighter wrap). */
    maxWidthPx?: number;
    placement?: TooltipPlacement;
    horizontalAlign?: TooltipHorizontalAlign;
  }>(),
  {
    size: "default",
    placement: "below",
    horizontalAlign: "right",
  },
);

const slots = useSlots();

const hasSlotContent = computed(() => {
  return (slots.default?.() ?? []).length > 0;
});

const isTiny = computed(() => {
  return props.size === "tiny";
});

const maxWidthCss = computed(() => {
  const ceilingPx = props.maxWidthPx ?? (isTiny.value ? TinyMaxWidthPx : TitleMaxWidthPx);

  return `min(90vw, ${ceilingPx}px)`;
});

const tooltipTransform = computed(() => {
  const translateX = props.horizontalAlign === "left" ? "-100%" : "0";
  const translateY = props.placement === "above" ? "-100%" : "0";

  if (translateX === "0" && translateY === "0") {
    return undefined;
  }

  return `translate(${translateX}, ${translateY})`;
});
</script>

<template>
  <Teleport to="body">
    <Transition name="fade">
      <div
        v-if="show"
        class="fixed z-[10000] pointer-events-none bg-[#001133ee] border-2 border-[#0066cc] rounded-sm shadow-[0_4px_12px_rgba(0,0,0,0.8)] backdrop-blur-sm w-max in-[[data-theme=seabed]]:bg-teal-900/90 in-[[data-theme=seabed]]:border-teal-700"
        :class="
          isTiny
            ? ['p-1.5', { 'flex flex-col gap-0.5': hasSlotContent }]
            : 'p-3 flex flex-col gap-1'
        "
        :style="{
          top: `${y}px`,
          left: `${x}px`,
          maxWidth: maxWidthCss,
          transform: tooltipTransform,
        }"
      >
        <div
          v-if="title"
          class="font-bold text-yellow-300 shadow-black shadow-text uppercase tracking-wider text-center"
          :class="
            isTiny
              ? [
                  'text-[10px]',
                  {
                    'border-b border-[#0066cc]/50 in-[[data-theme=seabed]]:border-teal-700/50 pb-0.5 mb-0.5':
                      hasSlotContent,
                  },
                ]
              : [
                  'text-sm',
                  {
                    'border-b border-[#0066cc]/50 in-[[data-theme=seabed]]:border-teal-700/50 pb-1 mb-1':
                      hasSlotContent,
                  },
                ]
          "
        >
          {{ title }}
        </div>

        <slot />
      </div>
    </Transition>
  </Teleport>
</template>
