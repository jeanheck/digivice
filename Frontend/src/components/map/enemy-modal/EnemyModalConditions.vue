<script setup lang="ts">
import { computed } from "vue";
import {
  BOOLEAN_CONDITION_KEYS,
  RESISTANCE_TOOLTIP_KEYS,
} from "@/constants/stat/condition-definition";
import { EnemyConditionsPresenter } from "@/presenters/map/enemy-modal/enemy-conditions.presenter";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";
import type { EnemyConditionViewModel } from "@/viewmodels/enemy/enemy-condition.viewmodel";

const props = defineProps<{
  conditions: EnemyViewModel["conditions"];
}>();

const emit = defineEmits<{
  (e: "show-condition-tooltip", event: MouseEvent, tooltipKey: string): void;
  (e: "move-stat-tooltip", event: MouseEvent): void;
  (e: "hide-stat-tooltip"): void;
}>();

const conditions = computed(() => {
  return EnemyConditionsPresenter.getConditions(props.conditions);
});

function isBooleanCondition(conditionKey: string): boolean {
  return BOOLEAN_CONDITION_KEYS.has(conditionKey);
}

function getDisplayValue(condition: EnemyConditionViewModel): string {
  if (condition.value !== null) {
    return condition.value;
  }

  if (isBooleanCondition(condition.conditionKey)) {
    return condition.can ? "Yes" : "No";
  }

  return "No";
}

function getValueColorClass(condition: EnemyConditionViewModel): string {
  if (isBooleanCondition(condition.conditionKey)) {
    return condition.can ? "text-green-400" : "text-red-400";
  }

  return condition.can ? "text-white" : "text-red-400";
}

function getTooltipKey(condition: EnemyConditionViewModel): string {
  if (isBooleanCondition(condition.conditionKey)) {
    return `conditions.${condition.conditionKey}`;
  }

  const tooltipKeys = RESISTANCE_TOOLTIP_KEYS[condition.conditionKey]!;

  return condition.can ? tooltipKeys.resistance : tooltipKeys.canSuffer;
}
</script>

<template>
  <div class="flex-1 flex flex-col items-center gap-1.5 border-l border-blue-900/10 pl-3">
    <h4 class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-2 border-b border-blue-900/30 pb-1 w-full text-center">{{ $t("enemy.status") }}</h4>
    <div v-for="condition in conditions" :key="condition.conditionKey" class="flex items-center justify-center gap-2 w-full">
      <div
        class="flex items-center w-5 justify-center cursor-help select-none z-20"
        @mouseenter="emit('show-condition-tooltip', $event, getTooltipKey(condition))"
        @mousemove="emit('move-stat-tooltip', $event)"
        @mouseleave="emit('hide-stat-tooltip')"
      >
        <span class="text-[16px] font-emoji opacity-90 drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-[2.5px]">{{ condition.icon }}</span>
      </div>
      <div class="font-bold tracking-widest text-shadow text-sm w-12 text-center" :class="getValueColorClass(condition)">
        {{ getDisplayValue(condition) }}
      </div>
    </div>
  </div>
</template>
