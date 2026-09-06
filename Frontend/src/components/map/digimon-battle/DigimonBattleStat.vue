<script setup lang="ts">
import { computed } from "vue";
import type { EnemyStatViewModel } from "@/viewmodels/enemy/enemy-stat.viewmodel";

const BATTLE_DELTA_STAT_KEYS = new Set(["strength", "defense", "speed"]);

const props = defineProps<{
  stat: EnemyStatViewModel;
}>();

const emit = defineEmits<{
  showTooltip: [event: MouseEvent];
  moveTooltip: [event: MouseEvent];
  hideTooltip: [];
  showBuffTooltip: [event: MouseEvent];
  moveBuffTooltip: [event: MouseEvent];
  hideBuffTooltip: [];
}>();

const hasBattleDelta = computed(() => {
  return BATTLE_DELTA_STAT_KEYS.has(props.stat.statKey) && (props.stat.delta ?? 0) !== 0;
});

const valueColorClass = computed(() => {
  const delta = props.stat.delta ?? 0;

  if (delta > 0) {
    return "text-green-400";
  }

  if (delta < 0) {
    return "text-red-400";
  }

  return "";
});

function onValueMouseEnter(event: MouseEvent): void {
  if (!hasBattleDelta.value) {
    return;
  }

  emit("showBuffTooltip", event);
}

function onValueMouseMove(event: MouseEvent): void {
  if (!hasBattleDelta.value) {
    return;
  }

  emit("moveBuffTooltip", event);
}

function onValueMouseLeave(): void {
  if (!hasBattleDelta.value) {
    return;
  }

  emit("hideBuffTooltip");
}
</script>

<template>
  <div class="flex items-center gap-1.5 min-w-0">
    <div
      class="flex items-center w-5 shrink-0 justify-center select-none cursor-help"
      @mouseenter="emit('showTooltip', $event)"
      @mousemove="emit('moveTooltip', $event)"
      @mouseleave="emit('hideTooltip')"
    >
      <span
        class="text-sm 2xl:text-base font-emoji drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-1"
        >{{ stat.icon }}</span
      >
    </div>
    <div class="font-bold tracking-wide flex items-center min-w-0 text-[10px] 2xl:text-base">
      <span
        class="shadow-text tabular-nums"
        :class="[valueColorClass, hasBattleDelta ? 'cursor-help' : 'cursor-default']"
        @mouseenter="onValueMouseEnter($event)"
        @mousemove="onValueMouseMove($event)"
        @mouseleave="onValueMouseLeave()"
        >{{ stat.value }}</span
      >
    </div>
  </div>
</template>
