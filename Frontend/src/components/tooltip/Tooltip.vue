<script setup lang="ts">
import { computed } from "vue";
import type { TooltipHorizontalAlign, TooltipPlacement } from "@/composables/use-tooltip-position";

const props = withDefaults(
    defineProps<{
        show: boolean;
        x: number;
        y: number;
        title: string;
        maxWidth?: number;
        placement?: TooltipPlacement;
        horizontalAlign?: TooltipHorizontalAlign;
    }>(),
    {
        maxWidth: 250,
        placement: "below",
        horizontalAlign: "right"
    }
);

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
        class="fixed z-9999 pointer-events-none p-3 bg-[#001133ee] border-2 border-[#0066cc] rounded-sm shadow-[0_4px_12px_rgba(0,0,0,0.8)] flex flex-col gap-1 backdrop-blur-sm w-max"
        :style="{
          top: `${y}px`,
          left: `${x}px`,
          maxWidth: `${maxWidth}px`,
          transform: tooltipTransform
        }"
      >
        <div
          v-if="title"
          class="font-bold text-yellow-300 text-sm shadow-black shadow-text uppercase tracking-wider text-center whitespace-nowrap"
          :class="{ 'border-b border-[#0066cc]/50 pb-1 mb-1': !!$slots.default }"
        >
          {{ title }}
        </div>

        <slot />
      </div>
    </Transition>
  </Teleport>
</template>
