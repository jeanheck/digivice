<script setup lang="ts">
defineProps<{
  name: string | null;
}>();
</script>

<template>
  <div v-if="name" class="absolute bottom-2 left-2 z-20 flex items-center gap-1.5 max-w-[45%]">
    <span class="inline-flex leading-none text-[1.2rem] -translate-y-0.5">↙️</span>
    <span class="text-[9px] text-yellow-300 text-left leading-tight">{{ name }}</span>
  </div>
</template>
