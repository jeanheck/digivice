<script setup lang="ts">
import { computed, useSlots } from "vue";
import type {
  TooltipHorizontalAlign,
  TooltipPlacement,
} from "@/ui/composables/use-tooltip-position";

export type TooltipSize = "default" | "tiny";

const props = withDefaults(
  defineProps<{
    show: boolean;
    x: number;
    y: number;
    title: string;
    size?: TooltipSize;
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
        class="fixed z-9999 pointer-events-none bg-[#001133ee] border-2 border-[#0066cc] rounded-sm shadow-[0_4px_12px_rgba(0,0,0,0.8)] backdrop-blur-sm w-max"
        :class="
          isTiny
            ? ['p-1.5', { 'flex flex-col gap-0.5': hasSlotContent }]
            : 'p-3 flex flex-col gap-1'
        "
        :style="{
          top: `${y}px`,
          left: `${x}px`,
          maxWidth: isTiny ? 'min(90vw, 200px)' : 'min(90vw, 520px)',
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
                  { 'border-b border-[#0066cc]/50 pb-0.5 mb-0.5': hasSlotContent },
                ]
              : [
                  'text-sm',
                  { 'border-b border-[#0066cc]/50 pb-1 mb-1': hasSlotContent },
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
