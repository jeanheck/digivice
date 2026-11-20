<script setup lang="ts">
defineProps<{
  speciesEmoji: string | null;
}>();

const emit = defineEmits<{
  showTooltip: [event: MouseEvent];
  moveTooltip: [event: MouseEvent];
  hideTooltip: [];
}>();
</script>

<template>
  <span
    class="font-emoji-color text-[16px] shrink-0 justify-self-center translate-x-0.5 cursor-help"
    @mouseenter="emit('showTooltip', $event)"
    @mousemove="emit('moveTooltip', $event)"
    @mouseleave="emit('hideTooltip')"
    >{{ speciesEmoji }}</span
  >
</template>
