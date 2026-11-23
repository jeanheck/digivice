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
    class="w-20 h-6 bg-surface-deep border-2 border-frame rounded flex items-center justify-center cursor-pointer hover:bg-panel transition-colors"
    @click="onClick"
    @mouseenter="emit('showTooltip', $event)"
    @mousemove="emit('moveTooltip', $event)"
    @mouseleave="emit('hideTooltip')"
  >
    <span class="font-emoji-color inline-flex leading-none text-base">🧬</span>
  </div>
</template>
