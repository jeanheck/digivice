<script setup lang="ts">
import { computed } from "vue";
import { DigimonStatusConstant } from "@/constants/digimon-status.constant";
import type { Vital } from "@/models/party/digimon/vital";
import { DigimonBattleEnemyStatusPresenter } from "@/presenters/map/digimon-battle-enemy-status.presenter";

const props = defineProps<{
  condition: number;
  hp: Vital;
}>();

const emit = defineEmits<{
  showTooltip: [event: MouseEvent];
  moveTooltip: [event: MouseEvent];
  hideTooltip: [];
}>();

const statusColorByState: Record<DigimonStatusConstant, string> = {
  [DigimonStatusConstant.healthy]: "#00B6BF",
  [DigimonStatusConstant.injured]: "#A3D956",
  [DigimonStatusConstant.debuffed]: "#CB9200",
  [DigimonStatusConstant.knockedOut]: "#760F08",
};

const backgroundColor = computed(() => {
  const status = DigimonBattleEnemyStatusPresenter.getStatus(props.condition, props.hp);
  return statusColorByState[status];
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
