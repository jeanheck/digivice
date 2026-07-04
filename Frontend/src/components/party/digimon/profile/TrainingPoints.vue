<script setup lang="ts">
defineProps<{
  tp: number;
}>();
</script>

<template>
  <div class="w-16 h-16 bg-[#000e3f] border-2 border-[#00154a] rounded flex flex-col items-center justify-center gap-2">
    <span class="text-[2rem] leading-none">🏋️‍♂️</span>
    <span class="text-[1rem] font-bold text-white">{{ tp }}</span>
  </div>
</template>
