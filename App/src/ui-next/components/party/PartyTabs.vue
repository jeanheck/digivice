<script setup lang="ts">
import PartyTab from "@/ui-next/components/party/PartyTab.vue";
import SectionArrowButton from "@/ui-next/components/main-panel/SectionArrowButton.vue";
import { useMainPanelSection } from "@/ui-next/composables/use-main-panel-section";
import type { PartyDigimonMock } from "@/ui-next/mocks/party.mock";

const { slots, selectedDigimonId } = defineProps<{
  slots: PartyDigimonMock[];
  selectedDigimonId: number | null;
}>();

const emit = defineEmits<{
  select: [digimonId: number];
}>();

const { showPrevious, showNext } = useMainPanelSection();
</script>

<template>
  <div class="flex items-end gap-1 px-1 border-b-2 border-frame">
    <SectionArrowButton class="self-center" direction="previous" @click="showPrevious" />

    <div class="flex-1 min-w-0 flex items-end">
      <PartyTab
        v-for="slot in slots"
        :key="slot.digimonId"
        :name="slot.name"
        :level="slot.level"
        :is-active="slot.digimonId === selectedDigimonId"
        @click="emit('select', slot.digimonId)"
      />
    </div>

    <SectionArrowButton class="self-center" direction="next" @click="showNext" />
  </div>
</template>
