<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { EnemyConditionViewModel } from "@/viewmodels/enemy/enemy-condition.viewmodel";

const props = defineProps<{
  condition: EnemyConditionViewModel;
}>();

const emit = defineEmits<{
  showTooltip: [event: MouseEvent];
  moveTooltip: [event: MouseEvent];
  hideTooltip: [];
}>();

const { t } = useI18n();

const isBooleanCondition = computed(() => {
  return !("value" in props.condition);
});

const displayValue = computed(() => {
  if (isBooleanCondition.value) {
    return props.condition.can ? t("conditions.yes") : t("conditions.no");
  }

  return props.condition.value && Number(props.condition.value) >= 0
    ? `${props.condition.value}%`
    : t("conditions.no");
});

const valueColorClass = computed(() => {
  if (isBooleanCondition.value) {
    return props.condition.can ? "text-green-400" : "text-red-400";
  }

  return props.condition.can ? "text-white" : "text-red-400";
});
</script>

<template>
  <div class="flex items-center gap-1.5 min-w-0">
    <div
      class="flex items-center w-5 shrink-0 justify-center select-none cursor-help"
      @mouseenter="emit('showTooltip', $event)"
      @mousemove="emit('moveTooltip', $event)"
      @mouseleave="emit('hideTooltip')"
    >
      <span
        class="text-sm 2xl:text-base font-emoji drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-1"
        >{{ condition.icon }}</span
      >
    </div>
    <div
      class="font-bold tracking-wide flex items-center min-w-0 text-[10px] 2xl:text-base"
      :class="valueColorClass"
    >
      <span class="shadow-text cursor-default">{{ displayValue }}</span>
    </div>
  </div>
</template>
