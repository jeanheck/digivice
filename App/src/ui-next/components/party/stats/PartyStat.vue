<script setup lang="ts">
import { computed } from "vue";
import type { PartyStatMock } from "@/ui-next/mocks/party.mock";

const {
  icon,
  iconClass = "text-foreground",
  label,
  stat,
} = defineProps<{
  icon: string;
  iconClass?: string;
  label: string;
  stat: PartyStatMock;
}>();

const displayValue = computed(() => {
  return stat.value + stat.battleDelta;
});

const valueColorClass = computed(() => {
  if (stat.battleDelta > 0) {
    return "text-green-400";
  }

  if (stat.battleDelta < 0) {
    return "text-red-400";
  }

  return "text-foreground";
});
</script>

<template>
  <div class="flex items-center gap-1.5 min-w-0 cursor-help" :title="label">
    <span class="w-5 shrink-0 flex justify-center text-[15px] leading-none font-emoji">
      <span :class="iconClass">{{ icon }}</span>
    </span>
    <span
      class="min-w-[3ch] text-right tabular-nums text-[12px] font-bold shadow-text"
      :class="valueColorClass"
    >
      {{ displayValue }}
    </span>
    <span
      v-if="stat.digievolutionBonus > 0"
      class="text-[12px] font-bold text-dw3-gold shadow-text-dark"
    >
      +{{ stat.digievolutionBonus }}
    </span>
  </div>
</template>
