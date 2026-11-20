<script setup lang="ts">
import { computed } from "vue";
import type { Digimon, Party } from "@/state/models";
import DebugDigimon from "@/ui/components/debug-monitor/DebugDigimon.vue";

interface OccupiedSlot {
  index: number;
  digimonId: number;
  digimon: Digimon;
}

const props = defineProps<{
  party: Party;
  mapId: string;
}>();

const occupiedSlots = computed((): OccupiedSlot[] =>
  props.party.slots.flatMap((slot) => {
    if (slot.digimonId === null || slot.digimon === null) {
      return [];
    }

    return [{ index: slot.index, digimonId: slot.digimonId, digimon: slot.digimon }];
  }),
);
</script>

<template>
  <section class="flex flex-col gap-2">
    <div class="text-cyan-300 font-bold">PARTY</div>
    <div v-if="occupiedSlots.length === 0" class="text-gray-500">(No Digimons in party slots)</div>
    <DebugDigimon
      v-for="slot in occupiedSlots"
      :key="slot.index"
      :slot-index="slot.index"
      :digimon-id="slot.digimonId"
      :digimon="slot.digimon"
      :map-id="props.mapId"
    />
  </section>
</template>
