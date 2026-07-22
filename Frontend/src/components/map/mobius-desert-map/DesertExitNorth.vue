<script setup lang="ts">
defineProps<{
  name: string | null;
}>();
</script>

<template>
  <div v-if="name" class="absolute top-2 right-2 z-20 flex items-baseline">
    <span class="max-w-40 whitespace-normal text-[10px] text-yellow-300 text-right leading-tight">{{ name }}</span>
    <span class="inline-flex shrink-0 leading-none text-[1.2rem] filter-[sepia(1)_saturate(6)_hue-rotate(5deg)_brightness(1.05)]">↗️</span>
  </div>
</template>
