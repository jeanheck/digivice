<script setup lang="ts">
import { computed } from "vue";
import { WikiEnemyElementsPresenter } from "@/presentation/presenters/map/wiki-modal/wiki-enemy-elements.presenter";
import type { EnemyViewModel } from "@/presentation/viewmodels/enemy/enemy.viewmodel";

const props = defineProps<{
  elements: EnemyViewModel["elements"];
}>();

const emit = defineEmits<{
  (e: "show-stat-key-tooltip", event: MouseEvent, statKey: string): void;
  (e: "move-stat-tooltip", event: MouseEvent): void;
  (e: "hide-stat-tooltip"): void;
}>();

const stats = computed(() => {
  return WikiEnemyElementsPresenter.getStats(props.elements);
});
</script>

<template>
  <div
    class="flex-1 flex flex-col items-center gap-1 border-l border-blue-900/10 in-[[data-theme=seabed]]:border-[var(--color-frame)]/20 pl-3"
  >
    <h4
      class="text-[10px] uppercase font-bold tracking-widest text-blue-500 in-[[data-theme=seabed]]:text-[var(--color-accent)] mb-2 border-b border-blue-900/30 in-[[data-theme=seabed]]:border-[var(--color-frame)]/40 pb-1 w-full text-center"
    >
      {{ $t("enemy.elements") }}
    </h4>
    <div
      v-for="stat in stats"
      :key="stat.statKey"
      class="flex items-center justify-center gap-2 w-full"
    >
      <div
        class="flex items-center w-5 justify-center cursor-help select-none z-20"
        @mouseenter="emit('show-stat-key-tooltip', $event, stat.statKey)"
        @mousemove="emit('move-stat-tooltip', $event)"
        @mouseleave="emit('hide-stat-tooltip')"
      >
        <img
          v-if="stat.iconUrl"
          :src="stat.iconUrl"
          :alt="stat.statKey"
          class="w-[25.5px] h-[27px] object-contain rendering-pixelated"
        />
      </div>
      <div
        class="font-bold tracking-widest text-shadow text-white text-sm w-12 text-center in-[[data-theme=seabed]]:[text-shadow:-1px_-1px_0_#000,1px_-1px_0_#000,-1px_1px_0_#000,1px_1px_0_#000]"
      >
        {{ stat.value }}
      </div>
    </div>
  </div>
</template>
