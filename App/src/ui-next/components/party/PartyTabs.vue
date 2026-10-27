<script setup lang="ts">
import PartyTab from "@/ui-next/components/party/PartyTab.vue";
import type { PartyDigimonMock } from "@/ui-next/mocks/party.mock";

const { slots, selectedDigimonId } = defineProps<{
  slots: PartyDigimonMock[];
  selectedDigimonId: number | null;
}>();

const emit = defineEmits<{
  select: [digimonId: number];
}>();
</script>

<template>
  <div class="flex items-end border-b-2 border-frame">
    <PartyTab
      v-for="slot in slots"
      :key="slot.digimonId"
      :name="slot.name"
      :level="slot.level"
      :is-active="slot.digimonId === selectedDigimonId"      @click="emit('select', slot.digimonId)"
    />
  </div>
</template>
