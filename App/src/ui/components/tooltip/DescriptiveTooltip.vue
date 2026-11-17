<script setup lang="ts">
import Tooltip from "@/ui/components/tooltip/Tooltip.vue";
import type {
  TooltipHorizontalAlign,
  TooltipPlacement,
} from "@/ui/composables/use-tooltip-position";

withDefaults(
  defineProps<{
    show: boolean;
    x: number;
    y: number;
    title: string;
    text: string;
    placement?: TooltipPlacement;
    horizontalAlign?: TooltipHorizontalAlign;
  }>(),
  {
    placement: "below",
    horizontalAlign: "right",
  },
);
</script>

<template>
  <Tooltip
    :show="show"
    :x="x"
    :y="y"
    :title="title"
    :placement="placement"
    :horizontal-align="horizontalAlign"
  >
    <div v-if="text" class="text-gray-100 text-xs leading-relaxed shadow-black shadow-text">
      {{ text }}
    </div>
  </Tooltip>
</template>
