<script setup lang="ts">
const { icon, name, isRightAligned = false } = defineProps<{
  icon: string;
  name: string;
  isRightAligned?: boolean;
}>();
</script>

<template>
  <div
    class="flex items-center gap-1 min-w-0 text-[12px] font-bold leading-tight text-yellow-300"
    :class="isRightAligned ? 'flex-row-reverse text-right' : 'text-left'"
  >
    <span class="shrink-0 text-[16px] leading-none font-emoji text-yellow-300 text-outline-black">{{ icon }}</span>
    <span class="min-w-0 text-outline-black">{{ name }}</span>
  </div>
</template>
