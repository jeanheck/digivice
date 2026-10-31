<script setup lang="ts">
defineProps<{
  label: string;
}>();
</script>

<template>
  <div class="text-[9px] text-indigo-400/80 mb-2 flex items-center uppercase tracking-widest">
    {{ label }}
  </div>
</template>
