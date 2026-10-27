<script setup lang="ts">
import { computed, ref } from "vue";
import PartyTabs from "@/ui-next/components/party/PartyTabs.vue";
import Digimon from "@/ui-next/components/party/digimon/Digimon.vue";
import { PartySlotsMock, type PartyDigimonMock } from "@/ui-next/mocks/party.mock";

const occupiedSlots = computed<PartyDigimonMock[]>(() => {
  return PartySlotsMock.filter((slot): slot is PartyDigimonMock => {
    return slot !== null;
  });
});

const selectedDigimonId = ref<number | null>(occupiedSlots.value[0]?.digimonId ?? null);

const selectedDigimon = computed(() => {
  return occupiedSlots.value.find((slot) => {
    return slot.digimonId === selectedDigimonId.value;
  });
});

function select(digimonId: number): void {
  selectedDigimonId.value = digimonId;
}
</script>

<template>
  <div class="h-full flex flex-col">
    <PartyTabs
      :slots="occupiedSlots"
      :selected-digimon-id="selectedDigimonId"
      @select="select"
    />
    <Digimon v-if="selectedDigimon" :digimon="selectedDigimon" />
  </div>
</template>
