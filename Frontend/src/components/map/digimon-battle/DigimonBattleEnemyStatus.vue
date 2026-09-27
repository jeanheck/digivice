<script setup lang="ts">
import { computed } from "vue";
import type { Vital } from "@/models";
import { DigimonBattleEnemyStatusPresenter } from "@/presenters/map/digimon-battle-enemy-status.presenter";
import type { DigimonStatus } from "@/types/digimon-status.type";

const props = defineProps<{
  condition: number;
  hp: Vital;
}>();

const emit = defineEmits<{
  showTooltip: [event: MouseEvent];
  moveTooltip: [event: MouseEvent];
  hideTooltip: [];
}>();

const statusColorByState: Record<DigimonStatus, string> = {
  healthy: "#00B6BF",
  injured: "#A3D956",
  debuffed: "#CB9200",
  ko: "#760F08",
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
