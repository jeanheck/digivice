<script setup lang="ts">
const { tp } = defineProps<{
  tp: number;
}>();
</script>

<template>
  <div
    class="h-6 bg-track border-2 border-progress-frame rounded flex items-center justify-center gap-1.5 cursor-help"
    :title="$t('digimon.tp')"
  >
    <span class="leading-none text-[16px] font-emoji text-red-500">&#x1F94A;</span>
    <span class="leading-none text-[14px] font-bold font-chakra-petch text-white">{{ tp }}</span>
  </div>
</template>
