<script setup lang="ts">
defineProps<{
  name: string | null;
}>();
</script>

<template>
  <div v-if="name" class="absolute top-2 right-2 z-20 flex items-center gap-1.5 max-w-[45%]">
    <span class="text-[9px] text-yellow-300 text-right leading-tight">{{ name }}</span>
    <span class="inline-flex leading-none text-[1.2rem] -translate-y-0.5">↗️</span>
  </div>
</template>
