<script setup lang="ts">
defineProps<{
  icon: string;
  label: string;
}>();
</script>

<template>
  <div class="text-[9px] text-indigo-400/80 mb-2 flex items-center uppercase tracking-widest">
    <span class="text-[11px] mr-2 opacity-80 drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-0.5">{{ icon }}</span>
    {{ label }}
  </div>
</template>
