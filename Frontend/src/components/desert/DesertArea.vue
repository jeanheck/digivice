<script setup lang="ts">
defineProps<{
  hasRightConnection: boolean;
  hasBottomConnection: boolean;
}>();
</script>

<template>
  <div class="relative w-15 h-15 bg-amber-600 border border-amber-300">
    <span
      v-if="hasRightConnection"
      class="absolute left-full top-1/2 h-px w-15 -translate-y-1/2 border-amber-300"
    />
    <span
      v-if="hasBottomConnection"
      class="absolute left-1/2 top-full h-15 w-px -translate-x-1/2 border-amber-300"
    />
  </div>
</template>
