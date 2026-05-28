<script setup lang="ts">
import { computed, ref } from "vue";
import Digimon from "@/components/digimon/Digimon.vue";
import DigimonDigievolutionsSlots from "@/components/digimon/DigimonDigievolutionsSlots.vue";
import DigievolutionGridModal from "@/components/digimon/digievolution/DigievolutionGridModal.vue";
import DigimonAttributesResistances from "@/components/digimon/DigimonAttributesResistances.vue";
import DigimonEquipments from "@/components/digimon/DigimonEquipments.vue";

const props = defineProps<{
  slot: import("@/models/digimon-slot").DigimonSlot;
}>();
const digimon = computed(() => {
  return props.slot.digimon!;
});
const digimonId = computed(() => {
  return props.slot.digimonId!;
});

const isGridModalOpen = ref(false);

function openDigievolutionGrid(): void {
  isGridModalOpen.value = true;
}

function closeDigievolutionGrid(): void {
  isGridModalOpen.value = false;
}
</script>

<template>
  <div class="flex flex-col h-full w-full bg-[#000e3f] p-4 rounded-md shadow-lg border-2 border-[#0033aa] gap-4">
    <Digimon :digimon="digimon" :digimon-id="digimonId" />
    <DigimonDigievolutionsSlots
      :slots="digimon.digievolutions"
      :active-digievolution-id="digimon.activeDigievolutionId"
    />
    <DigimonAttributesResistances 
      :attributes="digimon.attributes" 
      :resistances="digimon.resistances"
      :equipments="digimon.equipments" 
      :active-digievolution-id="digimon.activeDigievolutionId"
    />
    <DigimonEquipments :equipments="digimon.equipments" />

    <div 
      class="flex items-center justify-center bg-[#000a2b] border-2 border-[#00154a] rounded shadow-inner py-1.5 mt-auto cursor-pointer hover:bg-[#001233] transition-colors"
      @click="openDigievolutionGrid"
    >
      <span class="text-[0.65rem] font-bold text-gray-400 tracking-widest uppercase">{{ $t('digimon.digievolutions') }}</span>
    </div>

    <DigievolutionGridModal 
      :is-open="isGridModalOpen" 
      :digimon="digimon"
      :digimon-id="digimonId"
      @close="closeDigievolutionGrid"
    />
  </div>
</template>
