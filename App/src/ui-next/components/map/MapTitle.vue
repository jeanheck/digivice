<script setup lang="ts">
const { title, toneClass = "text-gray-200 drop-shadow-[0_0_5px_rgba(0,224,248,0.8)]" } =
  defineProps<{
    title: string;
    toneClass?: string;
  }>();
</script>

<template>
  <h4
    class="min-w-0 text-center text-[12px] leading-tight uppercase tracking-widest"
    :class="toneClass"
  >
    {{ title }}
  </h4>
</template>
