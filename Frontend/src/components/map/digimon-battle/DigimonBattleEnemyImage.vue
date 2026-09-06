<script setup lang="ts">
defineProps<{
  imageUrl: string;
  clickable: boolean;
}>();

const emit = defineEmits<{
  click: [];
}>();
</script>

<template>
  <img
    :src="imageUrl"
    class="absolute top-2 left-2 z-1 w-[50%] max-h-[65%] object-contain drop-shadow-[0_6px_14px_rgba(0,0,0,1)]"
    :class="clickable ? 'cursor-pointer' : 'pointer-events-none'"
    @click="emit('click')"
  />
</template>
