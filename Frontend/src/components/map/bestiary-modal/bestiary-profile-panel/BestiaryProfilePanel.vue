<script setup lang="ts">
import BestiaryProfileImage from "@/components/map/bestiary-modal/bestiary-profile-panel/BestiaryProfileImage.vue";
import BestiaryProfileResume from "@/components/map/bestiary-modal/bestiary-profile-panel/BestiaryProfileResume.vue";
import BestiaryProfileTechniques from "@/components/map/bestiary-modal/bestiary-profile-panel/BestiaryProfileTechniques.vue";
import BestiaryProfileAttributes from "@/components/map/bestiary-modal/bestiary-profile-panel/BestiaryProfileAttributes.vue";
import BestiaryProfileElements from "@/components/map/bestiary-modal/bestiary-profile-panel/BestiaryProfileElements.vue";
import BestiaryProfileConditions from "@/components/map/bestiary-modal/bestiary-profile-panel/BestiaryProfileConditions.vue";
import BestiaryProfileDrops from "@/components/map/bestiary-modal/bestiary-profile-panel/BestiaryProfileDrops.vue";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

defineProps<{
  enemy: EnemyViewModel;
  enemyImageUrl: string | null;
}>();

const emit = defineEmits<{
  (e: "open-drops", dropId: string): void;
  (e: "open-locations"): void;
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
  <div
    class="p-4 grid grid-cols-2 grid-rows-[24rem_1fr] gap-4 h-full min-h-0 overflow-y-auto custom-scroll"
  >
    <div class="flex gap-4 h-full min-h-0">
      <BestiaryProfileImage
        class="w-1/2 shrink-0"
        :enemy-image-url="enemyImageUrl"
        :enemy-name="enemy.name"
      />
      <BestiaryProfileResume
        class="w-1/2"
        :enemy="enemy"
        @open-locations="emit('open-locations')"
      />
    </div>

    <div
      class="h-full min-h-0 overflow-y-auto custom-scroll bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-row justify-around gap-6 items-start"
    >
      <BestiaryProfileAttributes
        :attributes="enemy.attributes"
        @show-stat-key-tooltip="forwardStatKeyTooltip"
        @move-stat-tooltip="forwardMoveStatTooltip"
        @hide-stat-tooltip="forwardHideStatTooltip"
      />
      <BestiaryProfileElements
        :elements="enemy.elements"
        @show-stat-key-tooltip="forwardStatKeyTooltip"
        @move-stat-tooltip="forwardMoveStatTooltip"
        @hide-stat-tooltip="forwardHideStatTooltip"
      />
      <BestiaryProfileConditions
        :conditions="enemy.conditions"
        @show-condition-tooltip="forwardConditionTooltip"
        @move-stat-tooltip="forwardMoveStatTooltip"
        @hide-stat-tooltip="forwardHideStatTooltip"
      />
    </div>

    <BestiaryProfileTechniques :enemy="enemy" />
    <BestiaryProfileDrops :enemy="enemy" @open-drops="emit('open-drops', $event)" />
  </div>
</template>
