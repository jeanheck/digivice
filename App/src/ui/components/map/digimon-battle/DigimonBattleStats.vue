<script setup lang="ts">
import { computed, ref } from "vue";
import { useI18n } from "vue-i18n";
import DigimonBattleCondition from "@/ui/components/map/digimon-battle/DigimonBattleCondition.vue";
import DigimonBattleStat from "@/ui/components/map/digimon-battle/DigimonBattleStat.vue";
import type { TooltipHorizontalAlign } from "@/ui/composables/use-tooltip-position";
import type { EnemyConditionViewModel } from "@/presentation/viewmodels/enemy/enemy-condition.viewmodel";
import type { EnemyStatViewModel } from "@/presentation/viewmodels/enemy/enemy-stat.viewmodel";

const props = defineProps<{
  attributes: EnemyStatViewModel[];
  elements: EnemyStatViewModel[];
  conditions: EnemyConditionViewModel[];
}>();

const emit = defineEmits<{
  showTooltip: [event: MouseEvent, stat: EnemyStatViewModel];
  moveTooltip: [event: MouseEvent];
  hideTooltip: [];
  showBuffTooltip: [event: MouseEvent, stat: EnemyStatViewModel];
  moveBuffTooltip: [event: MouseEvent];
  hideBuffTooltip: [];
  showConditionTooltip: [
    event: MouseEvent,
    condition: EnemyConditionViewModel,
    align?: TooltipHorizontalAlign,
  ];
}>();

const { t } = useI18n();
const isStatsOpen = ref(false);

const firstHalfConditions = computed(() => {
  const mid = Math.ceil(props.conditions.length / 2);
  return props.conditions.slice(0, mid);
});

const secondHalfConditions = computed(() => {
  const mid = Math.ceil(props.conditions.length / 2);
  return props.conditions.slice(mid);
});

function toggleStatsPanel(): void {
  isStatsOpen.value = !isStatsOpen.value;
}
</script>

<template>
  <button
    type="button"
    class="absolute bottom-2 right-2 z-20 cursor-pointer rounded bg-black/80 border border-gray-300 px-2 py-1 flex items-center justify-center text-gray-300 hover:bg-white/10 hover:border-white hover:text-white transition-all font-bold text-[9px] tracking-wide shadow-[0_0_10px_rgba(0,170,255,0.2)]"
    @click="toggleStatsPanel"
  >
    {{ isStatsOpen ? t("digimonBattle.hideDetails") : t("digimonBattle.showDetails") }}
  </button>

  <Transition name="fade">
    <div
      v-if="isStatsOpen"
      class="map-info-panel absolute inset-0 z-10 max-w-none! w-full border-0! rounded-none! backdrop-blur-none! pb-8 text-white text-xs"
    >
      <div class="grid grid-cols-4 w-full">
        <div class="flex flex-col gap-1 min-w-0">
          <DigimonBattleStat
            v-for="stat in attributes"
            :key="stat.statKey"
            :stat="stat"
            @show-tooltip="emit('showTooltip', $event, stat)"
            @move-tooltip="emit('moveTooltip', $event)"
            @hide-tooltip="emit('hideTooltip')"
            @show-buff-tooltip="emit('showBuffTooltip', $event, stat)"
            @move-buff-tooltip="emit('moveBuffTooltip', $event)"
            @hide-buff-tooltip="emit('hideBuffTooltip')"
          />
        </div>

        <div class="flex flex-col gap-1 min-w-0">
          <DigimonBattleStat
            v-for="stat in elements"
            :key="stat.statKey"
            :stat="stat"
            @show-tooltip="emit('showTooltip', $event, stat)"
            @move-tooltip="emit('moveTooltip', $event)"
            @hide-tooltip="emit('hideTooltip')"
            @show-buff-tooltip="emit('showBuffTooltip', $event, stat)"
            @move-buff-tooltip="emit('moveBuffTooltip', $event)"
            @hide-buff-tooltip="emit('hideBuffTooltip')"
          />
        </div>

        <div class="flex flex-col gap-1 min-w-0">
          <DigimonBattleCondition
            v-for="condition in firstHalfConditions"
            :key="condition.conditionKey"
            :condition="condition"
            @show-tooltip="emit('showConditionTooltip', $event, condition)"
            @move-tooltip="emit('moveTooltip', $event)"
            @hide-tooltip="emit('hideTooltip')"
          />
        </div>

        <div class="flex flex-col gap-1 min-w-0">
          <DigimonBattleCondition
            v-for="condition in secondHalfConditions"
            :key="condition.conditionKey"
            :condition="condition"
            @show-tooltip="emit('showConditionTooltip', $event, condition, 'left')"
            @move-tooltip="emit('moveTooltip', $event)"
            @hide-tooltip="emit('hideTooltip')"
          />
        </div>
      </div>
    </div>
  </Transition>
</template>
