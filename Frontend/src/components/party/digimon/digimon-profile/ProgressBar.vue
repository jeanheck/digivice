<script setup lang="ts">
import { computed } from "vue";
import { ProgressBarVariant } from "@/constants/progress-bar-variant";

const props = defineProps<{
  variant: string;
  currentValue: number;
  maxValue: number;
  percentage: number;
}>();

const barColorClass = computed(() => {
  if (props.variant === ProgressBarVariant.HP) {
    return props.percentage <= 30 ? "bg-red-500" : "bg-green-500";
  }

  if (props.variant === ProgressBarVariant.MP) {
    return props.percentage <= 30 ? "bg-yellow-400" : "bg-blue-600";
  }

  return "bg-linear-to-r from-orange-600 to-yellow-500";
});

const transitionDurationClass = computed(() => {
  if (props.variant === ProgressBarVariant.Experience) {
    return "duration-500";
  }

  return "duration-300";
});
</script>

<template>
  <div class="relative w-full h-6 bg-[#000e3f] rounded overflow-hidden shadow-inner flex items-center justify-center border-2 border-[#00154a]">
    <div
      class="absolute left-0 top-0 h-full transition-all bg-opacity-90"
      :class="[barColorClass, transitionDurationClass]"
      :style="{ width: `${percentage}%` }"
    ></div>

    <span class="relative z-10 text-[0.6rem] font-bold text-white drop-shadow-md px-1 tracking-wider">
      {{ `${currentValue} / ${maxValue}` }}
    </span>
  </div>
</template>
