<script setup lang="ts">
defineProps<{
  currentValue: number;
  maxValue: number;
  progressPercentage: number;
  barColorClass: string;
  transitionDurationClass: string;
}>();

const emit = defineEmits<{
  showTooltip: [event: MouseEvent];
  moveTooltip: [event: MouseEvent];
  hideTooltip: [];
}>();
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
      :style="{ width: `${progressPercentage}%` }"
    ></div>

    <span
      class="relative z-10 text-[0.6rem] font-bold text-white text-outline-black px-1 tracking-wider"
    >
      {{ `${currentValue} / ${maxValue}` }}
    </span>
  </div>
</template>
