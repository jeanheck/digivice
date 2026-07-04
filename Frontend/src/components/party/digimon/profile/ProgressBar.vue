<script setup lang="ts">
import { computed } from "vue";
import { ProgressBarConstant } from "@/constants/progress-bar.constant";

const props = defineProps<{
  variant: string;
  currentValue: number;
  maxValue: number;
  percentage: number;
}>();

const emit = defineEmits<{
  showTooltip: [event: MouseEvent];
  moveTooltip: [event: MouseEvent];
  hideTooltip: [];
}>();

const barColorClass = computed(() => {
  if (props.variant === ProgressBarConstant.hp) {
    return props.percentage <= 30 ? "bg-red-500" : "bg-green-500";
  }
  if (props.variant === ProgressBarConstant.mp) {
    return props.percentage <= 30 ? "bg-yellow-400" : "bg-blue-600";
  }
  if (props.variant === ProgressBarConstant.blast) {
    return props.percentage <= 30 ? "bg-amber-200" : "bg-amber-200";
  }

  return "bg-linear-to-r from-orange-600 to-yellow-500";
});

const transitionDurationClass = computed(() => {
  if (props.variant === ProgressBarConstant.experience) {
    return "duration-500";
  }

  return "duration-300";
});
</script>

<template>
  <div
    class="relative w-full h-6 bg-[#000e3f] rounded overflow-hidden shadow-inner flex items-center justify-center border-2 border-[#00154a] cursor-help"
    @mouseenter="emit('showTooltip', $event)"
    @mousemove="emit('moveTooltip', $event)"
    @mouseleave="emit('hideTooltip')"
  >
    <div
      class="absolute left-0 top-0 h-full transition-all bg-opacity-90"
      :class="[barColorClass, transitionDurationClass]"
      :style="{ width: `${percentage}%` }"
    ></div>

    <span class="relative z-10 text-[0.6rem] font-bold text-white text-outline-black px-1 tracking-wider">
      {{ `${currentValue} / ${maxValue}` }}
    </span>
  </div>
</template>
