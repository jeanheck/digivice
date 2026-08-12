<script setup lang="ts">
import { computed } from "vue";
import BestiaryProfile from "@/components/map/bestiary-modal/bestiary-profile-panel/BestiaryProfile.vue";
import BestiaryProfileAttributes from "@/components/map/bestiary-modal/bestiary-profile-panel/BestiaryProfileAttributes.vue";
import BestiaryProfileElements from "@/components/map/bestiary-modal/bestiary-profile-panel/BestiaryProfileElements.vue";
import BestiaryProfileConditions from "@/components/map/bestiary-modal/bestiary-profile-panel/BestiaryProfileConditions.vue";
import BestiaryProfileLocations from "@/components/map/bestiary-modal/bestiary-profile-panel/BestiaryProfileLocations.vue";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

const props = defineProps<{
  enemy: EnemyViewModel;
  enemyImageUrl: string | null;
}>();

const emit = defineEmits<{
  (e: "open-drops", dropId: string): void;
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

const showEncounterInfo = computed(() => {
  return (props.enemy.locations?.length ?? 0) > 0;
});
</script>

<template>
  <div class="p-4 flex flex-col sm:flex-row gap-4 h-full min-h-0 overflow-y-auto custom-scroll">
    <BestiaryProfile
      :enemy="enemy"
      :enemy-image-url="enemyImageUrl"
      @open-drops="emit('open-drops', $event)"
    />

    <div class="flex-1 flex flex-col gap-4 min-h-0">
      <div
        class="bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-row justify-around gap-6 items-start"
        :class="showEncounterInfo ? 'shrink-0' : 'flex-1 min-h-0'"
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

      <div
        v-if="showEncounterInfo"
        class="flex-1 min-h-24 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner text-sm"
      >
        <div class="flex flex-row gap-6">
          <BestiaryProfileLocations :enemy="enemy" />
        </div>
      </div>
    </div>
  </div>
</template>
