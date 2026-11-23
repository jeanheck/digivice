<script setup lang="ts">
import { useI18n } from "vue-i18n";

const { t } = useI18n();

defineProps<{
  tp: number;
}>();

const emit = defineEmits<{
  showTooltip: [event: MouseEvent];
  moveTooltip: [event: MouseEvent];
  hideTooltip: [];
}>();
</script>

<template>
  <div
    class="w-20 h-6 bg-surface-deep border-2 border-frame rounded flex items-center justify-center gap-1.25 cursor-help"
    @mouseenter="emit('showTooltip', $event)"
    @mousemove="emit('moveTooltip', $event)"
    @mouseleave="emit('hideTooltip')"
  >
    <span
      class="text-[0.8rem] font-bold text-white leading-none text-outline-black px-px py-px"
      >{{ t("digimon.tp") }}</span
    >
    <span
      class="text-[0.8rem] font-bold text-white leading-none text-outline-black px-px py-px"
      >{{ tp }}</span
    >
  </div>
</template>
