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
    class="w-20 h-6 bg-[#000e3f] border-2 border-[#00154a] rounded flex items-center justify-center gap-1.25 cursor-help"
    @mouseenter="emit('showTooltip', $event)"
    @mousemove="emit('moveTooltip', $event)"
    @mouseleave="emit('hideTooltip')"
  >
    <span class="leading-none text-[1rem] -translate-y-1">💪</span>
    <span class="text-[0.8rem] font-bold text-white leading-none">{{ tp }}</span>
  </div>
</template>
