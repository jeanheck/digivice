<script setup lang="ts">
import SectionArrowButton from "@/ui-next/components/main-panel/SectionArrowButton.vue";

const { label } = defineProps<{
  label: string;
}>();

const emit = defineEmits<{
  previous: [];
  next: [];
}>();
</script>

<template>
  <nav class="h-12 flex items-center justify-between px-4">
    <SectionArrowButton direction="previous" @click="emit('previous')" />

    <span class="text-[16px] leading-none whitespace-nowrap uppercase tracking-widest">{{ label }}</span>

    <SectionArrowButton direction="next" @click="emit('next')" />
  </nav>
</template>
