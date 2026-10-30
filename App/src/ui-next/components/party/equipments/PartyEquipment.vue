<script setup lang="ts">
const { icon, label, isEquipped } = defineProps<{
  icon: string;
  label: string;
  isEquipped: boolean;
}>();
</script>

<template>
  <span
    class="text-[20px] leading-none font-emoji cursor-help"
    :class="isEquipped ? 'text-foreground' : 'text-foreground/25'"
    :title="label"
  >
    {{ icon }}
  </span>
</template>
