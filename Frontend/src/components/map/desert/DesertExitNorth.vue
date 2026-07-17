<script setup lang="ts">
defineProps<{
  name: string | null;
}>();
</script>

<template>
  <div v-if="name" class="absolute top-2 right-2 z-20 flex w-[45%] items-start justify-end gap-1.5 text-right">
    <span class="min-w-0 text-balance text-[10px] text-yellow-300 leading-tight">{{ name }}</span>
    <span class="inline-flex shrink-0 leading-none text-[1.2rem] filter-[sepia(1)_saturate(6)_hue-rotate(5deg)_brightness(1.05)]">↗️</span>
  </div>
</template>
