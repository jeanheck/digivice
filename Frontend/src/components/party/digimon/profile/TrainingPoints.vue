<script setup lang="ts">
defineProps<{
  tp: number;
}>();

const emit = defineEmits<{
  showTooltip: [event: MouseEvent];
  moveTooltip: [event: MouseEvent];
  hideTooltip: [];
}>();
</script>

<template>
  <div
    class="w-16 h-16 bg-[#000e3f] border-2 border-[#00154a] rounded flex flex-col items-center justify-center gap-2 cursor-help"
    @mouseenter="emit('showTooltip', $event)"
    @mousemove="emit('moveTooltip', $event)"
    @mouseleave="emit('hideTooltip')"
  >
    <span class="text-[2rem] leading-none">🏋️‍♂️</span>
    <span class="text-[1rem] font-bold text-white">{{ tp }}</span>
  </div>
</template>
