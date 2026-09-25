<script setup lang="ts">
import Tooltip from "@/components/tooltip/Tooltip.vue";
import type { TooltipPlacement } from "@/composables/use-tooltip-position";

withDefaults(
  defineProps<{
    show: boolean;
    x: number;
    y: number;
    title: string;
    text: string;
    maxWidth?: number;
    minWidth?: number;
    placement?: TooltipPlacement;
  }>(),
  {
    maxWidth: 300,
    placement: "below",
  },
);
</script>

<template>
  <Tooltip
    :show="show"
    :x="x"
    :y="y"
    :title="title"
    :max-width="maxWidth"
    :min-width="minWidth"
    :placement="placement"
  >
    <div
      v-if="text"
      class="text-gray-100 text-xs leading-relaxed shadow-black shadow-text"
    >
      {{ text }}
    </div>
  </Tooltip>
</template>
