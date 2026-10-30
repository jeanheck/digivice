<script setup lang="ts">
const { tp } = defineProps<{
  tp: number;
}>();
</script>

<template>
  <div
    class="h-6 bg-track border-2 border-track-border rounded flex items-center justify-center gap-1.5 cursor-help"
    :title="$t('digimon.tp')"
  >
    <span class="leading-none text-[16px] font-emoji text-red-500">&#x1F94A;</span>
    <span class="leading-none text-[12px] font-bold font-chakra-petch text-foreground">{{ tp }}</span>
  </div>
</template>
