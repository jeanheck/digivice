<script setup lang="ts">
import { computed } from 'vue';
import Digimon from "./Digimon.vue";
import DigimonDigievolutionSlots from "./DigimonDigievolutionSlots.vue";
import DigimonAttributesResistances from './DigimonAttributesResistances.vue';
import DigimonEquipments from './DigimonEquipments.vue';
import type { DigimonSlot } from '@/models';
import { DigimonSlotPresenter } from '@/presenters/digimon-slot.presenter';

const props = defineProps<{
  slot: DigimonSlot;
}>();
const digimon = computed(() => {
  return props.slot.digimon!;
});
const digimonId = computed(() => {
  return props.slot.digimonId!;
});

const activeDigievolution = computed(() => {
  if (!digimon.value.activeDigievolutionId) {
    return null;
  }
  return DigimonSlotPresenter.getActiveDigievolutionViewModel(digimon.value.activeDigievolutionId);
});
</script>

<template>
  <div class="flex flex-col h-full w-full bg-[#000e3f] p-4 rounded-md shadow-lg border-2 border-[#0033aa] gap-4">
    <Digimon :digimon="digimon" :digimon-id="digimonId" />
    <DigimonDigievolutionSlots
      :slots="digimon.digievolutions"
      :active-digievolution-id="digimon.activeDigievolutionId"
    />
    <DigimonAttributesResistances 
      :attributes="digimon.attributes" 
      :resistances="digimon.resistances"
      :active-digievolution="activeDigievolution"
      :equipments="digimon.equipments" 
    />
    <DigimonEquipments :equipments="digimon.equipments" />

    <div 
      class="flex items-center justify-center bg-[#000a2b] border-2 border-[#00154a] rounded shadow-inner py-1.5 mt-auto cursor-pointer hover:bg-[#001233] transition-colors"
      @click="() => {}"
    >
      <span class="text-[0.65rem] font-bold text-gray-400 tracking-widest uppercase">{{ $t('digimon.digievolutions') }}</span>
    </div>

    <!-- New Grid Modal
    <DigievolutionGridModal 
      :is-open="isGridModalOpen" 
      :digimon="digimon"
      @close="isGridModalOpen = false"
    />
    -->
  </div>
</template>
