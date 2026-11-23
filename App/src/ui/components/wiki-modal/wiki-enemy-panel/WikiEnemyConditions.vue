<script setup lang="ts">
import { computed } from "vue";
import { WikiEnemyConditionsPresenter } from "@/presentation/presenters/map/wiki-modal/wiki-enemy-conditions.presenter";
import type { EnemyViewModel } from "@/presentation/viewmodels/enemy/enemy.viewmodel";
import type { EnemyConditionViewModel } from "@/presentation/viewmodels/enemy/enemy-condition.viewmodel";
import type { TooltipHorizontalAlign } from "@/ui/composables/use-tooltip-position";
import { useI18n } from "vue-i18n";

const { t } = useI18n();

const props = defineProps<{
  conditions: EnemyViewModel["conditions"];
}>();

const emit = defineEmits<{
  (
    e: "show-condition-tooltip",
    event: MouseEvent,
    tooltipKey: string,
    align: TooltipHorizontalAlign,
  ): void;
  (e: "move-stat-tooltip", event: MouseEvent): void;
  (e: "hide-stat-tooltip"): void;
}>();

const conditions = computed(() => {
  return WikiEnemyConditionsPresenter.getConditions(props.conditions);
});

function isBooleanCondition(condition: EnemyConditionViewModel): boolean {
  return !("value" in condition);
}

function calculateValue(condition: EnemyConditionViewModel): string {
  if (isBooleanCondition(condition)) {
    return condition.can ? t("conditions.yes") : t("conditions.no");
  }

  return condition.value && Number(condition.value) >= 0
    ? `${condition.value}%`
    : t("conditions.no");
}

function calculateColorClass(condition: EnemyConditionViewModel): string {
  if (isBooleanCondition(condition)) {
    return condition.can ? "text-success" : "text-danger";
  }

  return condition.can ? "text-ink" : "text-danger";
}

function calculateTooltip(condition: EnemyConditionViewModel): string {
  if (isBooleanCondition(condition)) {
    return `conditions.${condition.conditionKey}.canSuffer`;
  }

  const canSufferTooltip = `conditions.${condition.conditionKey}.canSuffer`;
  const vunerableTooltip = `conditions.${condition.conditionKey}.vunerable`;

  return condition.can ? vunerableTooltip : canSufferTooltip;
}
</script>

<template>
  <div
    class="flex-1 flex flex-col items-center gap-1 border-l border-frame/20 pl-3"
  >
    <h4
      class="text-[10px] uppercase font-bold tracking-widest text-accent mb-2 border-b border-frame/40 pb-1 w-full text-center"
    >
      {{ $t("enemy.status") }}
    </h4>
    <div
      v-for="condition in conditions"
      :key="condition.conditionKey"
      class="flex items-center justify-center gap-2 w-full"
    >
      <div
        class="flex items-center w-5 justify-center cursor-help select-none z-20"
        @mouseenter="emit('show-condition-tooltip', $event, calculateTooltip(condition), 'left')"
        @mousemove="emit('move-stat-tooltip', $event)"
        @mouseleave="emit('hide-stat-tooltip')"
      >
        <img
          v-if="condition.iconUrl"
          :src="condition.iconUrl"
          :alt="condition.conditionKey"
          class="w-[25.5px] h-[27px] object-contain rendering-pixelated"
        />
        <span v-else class="text-[16px] font-emoji-color leading-none">{{ condition.icon }}</span>
      </div>
      <div
        class="font-bold tracking-widest text-shadow text-sm w-12 text-center"
        :class="calculateColorClass(condition)"
      >
        {{ calculateValue(condition) }}
      </div>
    </div>
  </div>
</template>
