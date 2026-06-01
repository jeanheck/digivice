<script setup lang="ts">
import { computed, ref } from "vue";
import Profile from "@/components/party/digimon/profile/Profile.vue";
import DigimonDigievolutions from "@/components/party/digimon/digimon-digievolutions/DigimonDigievolutions.vue";
import DigievolutionsModal from "@/components/party/digimon/digievolutions-modal/DigievolutionsModal.vue";
import DigimonStats from "@/components/party/digimon/digimon-stats/DigimonStats.vue";
import DigimonEquipments from "@/components/party/digimon/digimon-equipments/DigimonEquipments.vue";
import type { DigimonSlot } from "@/models";

const props = defineProps<{
  slot: DigimonSlot;
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
    <Profile :digimon="digimon" :digimon-id="digimonId" />
    <DigimonDigievolutions
      :slots="digimon.digievolutions"
      :active-digievolution-id="digimon.activeDigievolutionId"
    />
    <DigimonStats :digimon="digimon" />
    <DigimonEquipments :equipments="digimon.equipments" />

    <div 
      class="flex items-center justify-center bg-[#000a2b] border-2 border-[#00154a] rounded shadow-inner py-1.5 mt-auto cursor-pointer hover:bg-[#001233] transition-colors"
      @click="openDigievolutionGrid"
    >
      <span class="text-[0.65rem] font-bold text-gray-400 tracking-widest uppercase">{{ $t('digimon.digievolutions') }}</span>
    </div>

    <DigievolutionsModal
      :is-open="isGridModalOpen"
      :digimon="digimon"
      :digimon-id="digimonId"
      @close="closeDigievolutionGrid"
    />
  </div>
</template>
