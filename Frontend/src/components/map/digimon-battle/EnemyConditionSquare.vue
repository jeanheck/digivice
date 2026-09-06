<script setup lang="ts">
import { computed } from "vue";
import { DigimonConditionConstant } from "@/constants/digimon-condition.constant";
import { ProfilePresenter } from "@/presenters/party/digimon/profile.presenter";
import type { Vital } from "@/models/party/digimon/vital";

const props = defineProps<{
  condition: number;
  hp: Vital;
}>();

const emit = defineEmits<{
  showTooltip: [event: MouseEvent];
  moveTooltip: [event: MouseEvent];
  hideTooltip: [];
}>();

const conditionColorByState: Record<DigimonConditionConstant, string> = {
  [DigimonConditionConstant.healthy]: "#00B6BF",
  [DigimonConditionConstant.injured]: "#A3D956",
  [DigimonConditionConstant.condition]: "#CB9200",
  [DigimonConditionConstant.ko]: "#760F08",
};

const backgroundColor = computed(() => {
  const calculatedCondition = ProfilePresenter.getCalculatedCondition(props.condition, props.hp);
  return conditionColorByState[calculatedCondition];
});
</script>

<template>
  <div
    class="h-6 w-6 shrink-0 rounded border-2 border-[#00154a] cursor-help"
    :style="{ backgroundColor }"
    aria-hidden="true"
    @mouseenter="emit('showTooltip', $event)"
    @mousemove="emit('moveTooltip', $event)"
    @mouseleave="emit('hideTooltip')"
  />
</template>
