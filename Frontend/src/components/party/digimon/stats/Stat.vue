<script setup lang="ts">
import { computed } from "vue";
import { DigimonStatIcon } from "@/constants/icon.constant";
import { useI18n } from "vue-i18n";
import type { DigimonStat } from "@/types/digimon-stat.type";
import type { StatViewModel } from "@/viewmodels/digimon/stat.viewmodel";

const props = defineProps<{
  statViewModel: StatViewModel;
  stat: DigimonStat;
}>();

const emit = defineEmits<{
  (e: "showIconTooltip", event: MouseEvent, title: string, propertyKey: DigimonStat): void;
  (
    e: "showMathTooltip",
    event: MouseEvent,
    title: string,
    base: number,
    equip: number,
    total: number,
    battleDelta: number,
  ): void;
  (e: "showTitleTooltip", event: MouseEvent, title: string): void;
  (e: "moveTooltip", event: MouseEvent): void;
  (e: "hideTooltip"): void;
}>();

const digievolutionBonusLabel = computed(() => {
  return t("digimon.digievolutionBonus");
});

const { t } = useI18n();

const label = computed(() => {
  return t(`stat.${props.stat}`);
});

const icon = computed(() => {
  return DigimonStatIcon[props.stat];
});

const battleDelta = computed(() => {
  return props.statViewModel.fromBattle ?? 0;
});

const displayValue = computed(() => {
  return props.statViewModel.sumBetweenDigimonAndEquipaments + battleDelta.value;
});

const buffOrDebuffClass = computed(() => {
  if (battleDelta.value > 0) {
    return "text-green-400";
  }

  if (battleDelta.value < 0) {
    return "text-red-400";
  }

  return "";
});
</script>

<template>
  <div class="flex items-center gap-1.5 min-w-0">
    <div
      class="flex items-center w-5 shrink-0 justify-center cursor-help select-none z-20 tooltip-anchor relative"
      @mouseenter="(event) => emit('showIconTooltip', event, label, stat)"
      @mousemove="(event) => emit('moveTooltip', event)"
      @mouseleave="emit('hideTooltip')"
    >
      <span
        class="text-sm 2xl:text-base font-emoji drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-1"
        >{{ icon }}</span
      >
    </div>

    <div class="flex items-center gap-1 min-w-0 font-bold tracking-wide text-xs 2xl:text-base">
      <span
        class="min-w-[3ch] text-right tabular-nums shadow-text cursor-help"
        :class="buffOrDebuffClass"
        @mouseenter="
          (event) =>
            emit(
              'showMathTooltip',
              event,
              label,
              statViewModel.fromDigimon,
              statViewModel.fromEquipaments,
              displayValue,
              battleDelta,
            )
        "
        @mousemove="(event) => emit('moveTooltip', event)"
        @mouseleave="emit('hideTooltip')"
        >{{ displayValue }}</span
      >
      <span
        v-if="statViewModel.fromDigievolution > 0"
        class="min-w-[3ch] text-left tabular-nums font-bold text-dw3-gold shadow-text-dark tracking-normal shrink-0 cursor-help"
        @mouseenter="(event) => emit('showTitleTooltip', event, digievolutionBonusLabel)"
        @mousemove="(event) => emit('moveTooltip', event)"
        @mouseleave="emit('hideTooltip')"
        >+{{ statViewModel.fromDigievolution }}</span
      >
    </div>
  </div>
</template>
