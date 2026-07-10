<script setup lang="ts">
import { ref } from "vue";
import Tooltip from "@/components/tooltip/Tooltip.vue";
import EnemyProfile from "@/components/map/enemy-modal/EnemyProfile.vue";
import EnemyAttributes from "@/components/map/enemy-modal/EnemyAttributes.vue";
import EnemyElements from "@/components/map/enemy-modal/EnemyElements.vue";
import EnemyConditions from "@/components/map/enemy-modal/EnemyConditions.vue";
import { useI18n } from "vue-i18n";
import { useTooltipPosition } from "@/composables/use-tooltip-position";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

defineProps<{
  enemy: EnemyViewModel;
  enemyImageUrl: string | null;
}>();

const { t } = useI18n();

const tooltipPlacement = "below" as const;
const tooltipPosition = useTooltipPosition(250);
const { show: tooltipShow, x: tooltipX, y: tooltipY, showAt, move, hide } = tooltipPosition;
const tooltipTitle = ref("");

const showEnemyStatKeyTooltip = (event: MouseEvent, statKey: string) => {
  tooltipTitle.value = t(`stat.${statKey}`);
  showAt(event, { maxWidth: 250, placement: tooltipPlacement });
};

const showEnemyConditionTooltip = (event: MouseEvent, tooltipKey: string) => {
  tooltipTitle.value = t(tooltipKey);
  showAt(event, { maxWidth: 250, placement: tooltipPlacement });
};

const hideEnemyStatTooltip = () => {
  hide();
};

const moveEnemyStatTooltip = (event: MouseEvent) => {
  move(event, tooltipPlacement);
};
</script>

<template>
  <div class="flex flex-1 min-h-0 flex-col">
    <div class="flex flex-1 min-h-0 flex-col sm:flex-row gap-4 overflow-y-auto custom-scroll">
      <EnemyProfile :enemy="enemy" :enemy-image-url="enemyImageUrl" />

      <div class="flex-1 min-h-0">
        <div class="bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-row justify-around gap-6 h-full items-start">
          <EnemyAttributes
            :attributes="enemy.attributes"
            @show-stat-key-tooltip="showEnemyStatKeyTooltip"
            @move-stat-tooltip="moveEnemyStatTooltip"
            @hide-stat-tooltip="hideEnemyStatTooltip"
          />
          <EnemyElements
            :elements="enemy.elements"
            @show-stat-key-tooltip="showEnemyStatKeyTooltip"
            @move-stat-tooltip="moveEnemyStatTooltip"
            @hide-stat-tooltip="hideEnemyStatTooltip"
          />
          <EnemyConditions
            :conditions="enemy.conditions"
            @show-condition-tooltip="showEnemyConditionTooltip"
            @move-stat-tooltip="moveEnemyStatTooltip"
            @hide-stat-tooltip="hideEnemyStatTooltip"
          />
        </div>
      </div>
    </div>

    <Tooltip
      :show="tooltipShow"
      :x="tooltipX"
      :y="tooltipY"
      :title="tooltipTitle"
      :max-width="400"
      placement="below"
    />
  </div>
</template>
