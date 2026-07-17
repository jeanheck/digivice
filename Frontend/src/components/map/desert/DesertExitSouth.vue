<script setup lang="ts">
defineProps<{
  name: string | null;
}>();
</script>

<template>
  <div v-if="name" class="absolute bottom-2 left-2 z-20">
    <span class="inline-flex leading-none text-[1.2rem] filter-[sepia(1)_saturate(6)_hue-rotate(5deg)_brightness(1.05)]">↙️</span>
    <span class="text-[10px] text-yellow-300 text-left leading-tight">{{ name }}</span>
  </div>
</template>
