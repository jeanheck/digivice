<script setup lang="ts">
const { name, title, isEquipped } = defineProps<{
  name: string;
  title: string;
  isEquipped: boolean;
}>();
</script>

<template>
  <div class="min-w-0 py-0.5 border-b border-separator cursor-help" :title="title">
    <span
      class="block truncate text-center text-[14px] font-bold shadow-text"
      :class="isEquipped ? 'text-foreground' : 'text-foreground/40'"
    >
      {{ name }}
    </span>
  </div>
</template>
