<script setup lang="ts">
defineProps<{
  hasRightConnection: boolean;
  hasBottomConnection: boolean;
}>();
</script>

<template>
  <div class="relative w-[10px] h-[10px] bg-white border border-black">
    <span
      v-if="hasRightConnection"
      class="absolute left-full top-1/2 h-px w-[10px] -translate-y-1/2 bg-black"
    />
    <span
      v-if="hasBottomConnection"
      class="absolute left-1/2 top-full h-[10px] w-px -translate-x-1/2 bg-black"
    />
  </div>
</template>
