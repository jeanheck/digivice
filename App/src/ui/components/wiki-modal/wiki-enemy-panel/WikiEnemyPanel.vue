<script setup lang="ts">
import WikiEnemyImage from "@/ui/components/wiki-modal/wiki-enemy-panel/WikiEnemyImage.vue";
import WikiEnemyResume from "@/ui/components/wiki-modal/wiki-enemy-panel/WikiEnemyResume.vue";
import WikiEnemyTechniques from "@/ui/components/wiki-modal/wiki-enemy-panel/WikiEnemyTechniques.vue";
import WikiEnemyAttributes from "@/ui/components/wiki-modal/wiki-enemy-panel/WikiEnemyAttributes.vue";
import WikiEnemyElements from "@/ui/components/wiki-modal/wiki-enemy-panel/WikiEnemyElements.vue";
import WikiEnemyConditions from "@/ui/components/wiki-modal/wiki-enemy-panel/WikiEnemyConditions.vue";
import WikiEnemyDrops from "@/ui/components/wiki-modal/wiki-enemy-panel/WikiEnemyDrops.vue";
import type { DropType } from "@/presentation/repositories/tables/raws/drop/drop-type";
import type { EnemyViewModel } from "@/presentation/viewmodels/enemy/enemy.viewmodel";
import type { TooltipHorizontalAlign } from "@/ui/composables/use-tooltip-position";

defineProps<{
  enemy: EnemyViewModel;
  enemyImageUrl: string | null;
}>();

const emit = defineEmits<{
  (e: "open-drops", payload: { dropId: string; dropType: DropType }): void;
  (e: "open-locations", locationId: string): void;
  (e: "show-stat-key-tooltip", event: MouseEvent, statKey: string): void;
  (
    e: "show-condition-tooltip",
    event: MouseEvent,
    tooltipKey: string,
    align: TooltipHorizontalAlign,
  ): void;
  (e: "move-stat-tooltip", event: MouseEvent): void;
  (e: "hide-stat-tooltip"): void;
}>();

const forwardStatKeyTooltip = (event: MouseEvent, statKey: string): void => {
  emit("show-stat-key-tooltip", event, statKey);
};

const forwardConditionTooltip = (
  event: MouseEvent,
  tooltipKey: string,
  align: TooltipHorizontalAlign,
): void => {
  emit("show-condition-tooltip", event, tooltipKey, align);
};

const forwardMoveStatTooltip = (event: MouseEvent): void => {
  emit("move-stat-tooltip", event);
};

const forwardHideStatTooltip = (): void => {
  emit("hide-stat-tooltip");
};
</script>

<template>
  <div
    class="p-4 grid grid-cols-2 grid-rows-[24rem_1fr] gap-4 h-full min-h-0 overflow-y-auto custom-scroll"
  >
    <div class="flex gap-4 h-full min-h-0">
      <WikiEnemyImage
        class="w-1/2 shrink-0"
        :enemy-image-url="enemyImageUrl"
        :enemy-name="enemy.name"
      />
      <WikiEnemyResume
        class="w-1/2"
        :enemy="enemy"
        @open-locations="emit('open-locations', $event)"
      />
    </div>

    <div
      class="h-full min-h-0 overflow-y-auto custom-scroll bg-panel border border-frame/50 rounded p-4 shadow-inner flex flex-row justify-around gap-6 items-start"
    >
      <WikiEnemyAttributes
        :attributes="enemy.attributes"
        @show-stat-key-tooltip="forwardStatKeyTooltip"
        @move-stat-tooltip="forwardMoveStatTooltip"
        @hide-stat-tooltip="forwardHideStatTooltip"
      />
      <WikiEnemyElements
        :elements="enemy.elements"
        @show-stat-key-tooltip="forwardStatKeyTooltip"
        @move-stat-tooltip="forwardMoveStatTooltip"
        @hide-stat-tooltip="forwardHideStatTooltip"
      />
      <WikiEnemyConditions
        :conditions="enemy.conditions"
        @show-condition-tooltip="forwardConditionTooltip"
        @move-stat-tooltip="forwardMoveStatTooltip"
        @hide-stat-tooltip="forwardHideStatTooltip"
      />
    </div>

    <WikiEnemyTechniques :enemy="enemy" />
    <WikiEnemyDrops :drops="enemy.drops" @open-drops="emit('open-drops', $event)" />
  </div>
</template>
