<script setup lang="ts">
import PartyDigievolution from "@/ui-next/components/party/digievolutions/PartyDigievolution.vue";
import type { PartyDigievolutionSlotMock } from "@/ui-next/mocks/party.mock";

const { digievolutions, activeDigievolutionId } = defineProps<{
  digievolutions: PartyDigievolutionSlotMock[];
  activeDigievolutionId: number | null;
}>();

const TierIndicators = ["none", "none", "none"] as const;
</script>

<template>
  <div class="flex flex-col gap-1">
    <PartyDigievolution
      v-for="(digievolution, index) in digievolutions"
      :key="index"
      :digievolution="digievolution"
      :is-active="
        digievolution !== null && digievolution.digievolutionId === activeDigievolutionId
      "
      :tier-indicator="TierIndicators[index] ?? 'none'"
    />
  </div>
</template>
