<script setup lang="ts">
const emit = defineEmits<{
  openDigievolutions: [];
  showTooltip: [event: MouseEvent];
  moveTooltip: [event: MouseEvent];
  hideTooltip: [];
}>();

function onClick(): void {
  emit("openDigievolutions");
}
</script>

<template>
  <div
    class="w-20 h-6 bg-[#000e3f] border-2 border-[#00154a] rounded flex items-center justify-center cursor-pointer hover:bg-[#001233] transition-colors"
    @click="onClick"
    @mouseenter="emit('showTooltip', $event)"
    @mousemove="emit('moveTooltip', $event)"
    @mouseleave="emit('hideTooltip')"
  >
    <span class="inline-flex items-center justify-center leading-none text-[1.2rem] translate-y-[-1px]">🧬</span>
  </div>
</template>
