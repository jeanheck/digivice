<script setup lang="ts">
import BestiaryProfile from "@/components/map/bestiary-modal/BestiaryProfile.vue";
import BestiaryAttributes from "@/components/map/bestiary-modal/BestiaryAttributes.vue";
import BestiaryElements from "@/components/map/bestiary-modal/BestiaryElements.vue";
import BestiaryConditions from "@/components/map/bestiary-modal/BestiaryConditions.vue";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

defineProps<{
  enemy: EnemyViewModel;
  enemyImageUrl: string | null;
}>();

const emit = defineEmits<{
  (e: "open-drops"): void;
  (e: "show-stat-key-tooltip", event: MouseEvent, statKey: string): void;
  (e: "show-condition-tooltip", event: MouseEvent, tooltipKey: string): void;
  (e: "move-stat-tooltip", event: MouseEvent): void;
  (e: "hide-stat-tooltip"): void;
}>();

const forwardStatKeyTooltip = (event: MouseEvent, statKey: string): void => {
  emit("show-stat-key-tooltip", event, statKey);
};

const forwardConditionTooltip = (event: MouseEvent, tooltipKey: string): void => {
  emit("show-condition-tooltip", event, tooltipKey);
};

const forwardMoveStatTooltip = (event: MouseEvent): void => {
  emit("move-stat-tooltip", event);
};

const forwardHideStatTooltip = (): void => {
  emit("hide-stat-tooltip");
};
</script>

<template>
  <div class="p-4 flex flex-col sm:flex-row gap-4 max-h-[70vh] overflow-y-auto custom-scroll">
    <BestiaryProfile
      :enemy="enemy"
      :enemy-image-url="enemyImageUrl"
      @open-drops="emit('open-drops')"
    />

    <div class="flex-1">
      <div
        class="bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-row justify-around gap-6 h-full items-start"
      >
        <BestiaryAttributes
          :attributes="enemy.attributes"
          @show-stat-key-tooltip="forwardStatKeyTooltip"
          @move-stat-tooltip="forwardMoveStatTooltip"
          @hide-stat-tooltip="forwardHideStatTooltip"
        />
        <BestiaryElements
          :elements="enemy.elements"
          @show-stat-key-tooltip="forwardStatKeyTooltip"
          @move-stat-tooltip="forwardMoveStatTooltip"
          @hide-stat-tooltip="forwardHideStatTooltip"
        />
        <BestiaryConditions
          :conditions="enemy.conditions"
          @show-condition-tooltip="forwardConditionTooltip"
          @move-stat-tooltip="forwardMoveStatTooltip"
          @hide-stat-tooltip="forwardHideStatTooltip"
        />
      </div>
    </div>
  </div>
</template>
