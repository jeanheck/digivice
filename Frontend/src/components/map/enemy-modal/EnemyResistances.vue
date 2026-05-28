<script setup lang="ts">
export interface EnemyResistanceStatItem {
  label: string;
  val: string;
  icon: string;
  valColor: string;
}

defineProps<{
  items: EnemyResistanceStatItem[];
}>();

const emit = defineEmits<{
  (e: "show-stat-tooltip", event: MouseEvent, label: string): void;
  (e: "move-stat-tooltip", event: MouseEvent): void;
  (e: "hide-stat-tooltip"): void;
}>();
</script>

<template>
  <div class="flex-1 flex flex-col items-center gap-1.5 border-l border-blue-900/10 pl-3">
    <h4 class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-2 border-b border-blue-900/30 pb-1 w-full text-center">{{ $t("enemy.status") }}</h4>
    <div v-for="resistance in items" :key="resistance.label" class="flex items-center justify-center gap-2 w-full">
      <div
        class="flex items-center w-5 justify-center cursor-help select-none z-20"
        @mouseenter="emit('show-stat-tooltip', $event, resistance.label)"
        @mousemove="emit('move-stat-tooltip', $event)"
        @mouseleave="emit('hide-stat-tooltip')"
      >
        <span class="text-[16px] font-emoji opacity-90 drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-[2.5px]">{{ resistance.icon }}</span>
      </div>
      <div class="font-bold tracking-widest text-shadow text-sm w-12 text-center" :class="resistance.valColor">
        {{ resistance.val }}
      </div>
    </div>
  </div>
</template>
