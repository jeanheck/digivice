<script setup lang="ts">
import { computed } from "vue";
import type { Vital } from "@/state/models";

type DebugBarKind = "hp" | "mp";

const SegmentCount = 10;

const props = defineProps<{
  label: string;
  vital: Vital;
  kind: DebugBarKind;
}>();

const percentage = computed(() => Math.calculatePercentage(props.vital.current, props.vital.max));

const filledSegments = computed(() => Math.floor((percentage.value / 100) * SegmentCount));

const colorClass = computed(() => {
  if (props.kind === "mp") {
    return "text-blue-400";
  }

  if (props.vital.max === 0 || percentage.value <= 20) {
    return "text-red-400";
  }

  if (percentage.value <= 50) {
    return "text-yellow-300";
  }

  return "text-green-400";
});
</script>

<template>
  <div class="whitespace-pre">
    <span class="text-gray-400">{{ props.label }}: </span>
    <span>[</span>
    <span :class="colorClass">{{ "█".repeat(filledSegments) }}</span>
    <span class="text-gray-600">{{ "-".repeat(SegmentCount - filledSegments) }}</span>
    <span>] {{ props.vital.current }}/{{ props.vital.max }}</span>
  </div>
</template>
