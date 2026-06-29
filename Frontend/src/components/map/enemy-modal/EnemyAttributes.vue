<script setup lang="ts">
import { computed } from "vue";
import { EnemyAttributesPresenter } from "@/presenters/map/enemy-modal/enemy-attributes.presenter";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

const props = defineProps<{
  attributes: EnemyViewModel["attributes"];
}>();

const emit = defineEmits<{
  (e: "show-stat-key-tooltip", event: MouseEvent, statKey: string): void;
  (e: "move-stat-tooltip", event: MouseEvent): void;
  (e: "hide-stat-tooltip"): void;
}>();

const stats = computed(() => {
  return EnemyAttributesPresenter.getStats(props.attributes);
});
</script>

<template>
  <div class="flex-1 flex flex-col items-center gap-1.5">
    <h4 class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-2 border-b border-blue-900/30 pb-1 w-full text-center">{{ $t("enemy.attr") }}</h4>
    <div v-for="stat in stats" :key="stat.statKey" class="flex items-center justify-center gap-2 w-full">
      <div
        class="flex items-center w-5 justify-center cursor-help select-none z-20"
        @mouseenter="emit('show-stat-key-tooltip', $event, stat.statKey)"
        @mousemove="emit('move-stat-tooltip', $event)"
        @mouseleave="emit('hide-stat-tooltip')"
      >
        <span class="text-[16px] font-emoji opacity-90 drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-[2.5px]">{{ stat.icon }}</span>
      </div>
      <div class="font-bold tracking-widest text-shadow text-white text-sm w-12 text-center">
        {{ stat.value }}
      </div>
    </div>
  </div>
</template>
