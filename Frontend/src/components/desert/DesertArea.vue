<script setup lang="ts">
defineProps<{
  hasRightConnection: boolean;
  hasBottomConnection: boolean;
  label?: string | null;
}>();
</script>

<template>
  <div
    class="relative flex w-16 h-16 items-center justify-center bg-[#e0db8e]"
  >
    <span
      v-if="label"
      class="relative z-10 overflow-hidden px-0.0 text-center font-['Exo_2'] text-[11px] font-bold leading-tight text-blue-800"
    >
      {{ label }}
    </span>
    <span
      v-if="hasRightConnection"
      class="absolute left-full top-1/2 h-0.75 w-8.75 -translate-y-1/2 bg-cyan-500"
    />
    <span
      v-if="hasBottomConnection"
      class="absolute left-1/2 top-full h-8.75 w-0.75 -translate-x-1/2 bg-cyan-500"
    />
  </div>
</template>
