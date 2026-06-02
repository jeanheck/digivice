<script setup lang="ts">
import type { TooltipPlacement } from "@/composables/use-tooltip-position";

withDefaults(
    defineProps<{
        show: boolean;
        x: number;
        y: number;
        title: string;
        maxWidth?: number;
        placement?: TooltipPlacement;
    }>(),
    {
        maxWidth: 250,
        placement: "below"
    }
);
</script>

<template>
  <Teleport to="body">
    <Transition name="fade">
      <div
        v-if="show"
        class="fixed z-9999 pointer-events-none p-3 bg-[#001133ee] border-2 border-[#0066cc] rounded-sm shadow-[0_4px_12px_rgba(0,0,0,0.8)] flex flex-col gap-1 backdrop-blur-sm"
        :style="{
          top: `${y}px`,
          left: `${x}px`,
          maxWidth: `${maxWidth}px`,
          transform: placement === 'above' ? 'translateY(-100%)' : undefined
        }"
      >
        <div
          v-if="title"
          class="font-bold text-yellow-300 text-sm border-b border-[#0066cc]/50 pb-1 mb-1 shadow-black shadow-text uppercase tracking-wider"
        >
          {{ title }}
        </div>

        <slot />
      </div>
    </Transition>
  </Teleport>
</template>
