<script setup lang="ts">
const { title, toneClass = "text-foreground" } = defineProps<{
  title: string;
  toneClass?: string;
}>();
</script>

<template>
  <h4
    class="text-center text-[20px] font-bold leading-none uppercase tracking-widest text-outline-black-glow"
    :class="toneClass"
  >
    {{ title }}
  </h4>
</template>
