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
    class="font-emoji text-[16px] shrink-0 justify-self-center -translate-y-1 cursor-help"
    aria-hidden="true"
    @mouseenter="emit('showTooltip', $event)"
    @mousemove="emit('moveTooltip', $event)"
    @mouseleave="emit('hideTooltip')"
    >{{ speciesEmoji }}</span
  >
</template>
