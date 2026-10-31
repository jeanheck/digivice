<script setup lang="ts">
import { computed } from "vue";
import type { Vital } from "@/state/models";
import { DigimonBattleEnemyStatusPresenter } from "@/presentation/presenters/map/digimon-battle-enemy-status.presenter";
import { DigimonStatusColor } from "@/shared/constants/digimon-status-color.constant";

const props = defineProps<{
  condition: number;
  hp: Vital;
}>();

const emit = defineEmits<{
  showTooltip: [event: MouseEvent];
  moveTooltip: [event: MouseEvent];
  hideTooltip: [];
}>();

const backgroundColor = computed(() => {
  const status = DigimonBattleEnemyStatusPresenter.getStatus(props.condition, props.hp);
  return DigimonStatusColor[status];
});
</script>

<template>
  <div
    class="h-6 w-6 shrink-0 rounded border-2 border-[#00154a] cursor-help"
    :style="{ backgroundColor }"
    @mouseenter="emit('showTooltip', $event)"
    @mousemove="emit('moveTooltip', $event)"
    @mouseleave="emit('hideTooltip')"
  />
</template>
