<script setup lang="ts">
defineProps<{
  level: number | null;
}>();
</script>

<template>
  <span class="text-[10px] font-bold text-gray-300 shrink-0 justify-self-center cursor-default">
    {{ level }}
  </span>
</template>
