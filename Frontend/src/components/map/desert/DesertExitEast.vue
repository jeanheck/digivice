<script setup lang="ts">
defineProps<{
  name: string | null;
}>();
</script>

<template>
  <div v-if="name" class="absolute bottom-2 right-2 z-20 flex items-center gap-1.5">
    <span class="text-[10px] text-yellow-300 text-right leading-tight whitespace-nowrap">{{ name }}</span>
    <span class="inline-flex leading-none text-[1.2rem] -translate-y-0.5 filter-[sepia(1)_saturate(6)_hue-rotate(5deg)_brightness(1.05)]">↘️</span>
  </div>
</template>
