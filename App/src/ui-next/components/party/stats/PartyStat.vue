<script setup lang="ts">
import { computed } from "vue";
import type { PartyStatMock } from "@/ui-next/mocks/party.mock";

const { imageUrl, label, stat } = defineProps<{
  imageUrl: string | null;
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
    <img v-if="imageUrl" :src="imageUrl" :alt="label" class="w-5 h-5 shrink-0 object-fill" />
    <span
      class="min-w-[3ch] text-right tabular-nums text-[14px] font-bold shadow-text"
      :class="valueColorClass"
    >
      {{ displayValue }}
    </span>
    <span
      v-if="stat.digievolutionBonus > 0"
      class="text-[14px] font-bold text-highlight shadow-text"
    >
      +{{ stat.digievolutionBonus }}
    </span>
  </div>
</template>
