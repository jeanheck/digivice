<script setup lang="ts">
import { ref } from 'vue'
import DigimonBasicInfo from './DigimonBasicInfo.vue'
import DigimonDigievolutions from './DigimonDigievolutions.vue'
import DigimonDetails from './DigimonDetails.vue'
import DigimonEquipments from './DigimonEquipments.vue'
import DigievolutionGridModal from './DigievolutionGridModal.vue'
import type { Digimon } from '../../models'

defineProps<{
  digimon: Digimon
}>()

const isGridModalOpen = ref(false)
</script>

<template>
  <div class="flex flex-col h-full w-full bg-[#000e3f] p-4 rounded-md shadow-lg border-2 border-[#0033aa] gap-4">
    <DigimonBasicInfo :basic-info="digimon.basicInfo" />
    
    <DigimonDigievolutions 
      :equipped-digievolutions="digimon.equippedDigievolutions"
      :active-digievolution-id="digimon.activeDigievolutionId" 
    />

    <DigimonDetails :digimon="digimon" />
    
    <DigimonEquipments :digimon="digimon" />

    <div 
      class="flex items-center justify-center bg-[#000a2b] border-2 border-[#00154a] rounded shadow-inner py-1.5 mt-auto cursor-pointer hover:bg-[#001233] transition-colors"
      @click="isGridModalOpen = true"
    >
      <span class="text-[0.65rem] font-bold text-gray-400 tracking-widest uppercase">{{ $t('digimon.digievolutions') }}</span>
    </div>

    <!-- New Grid Modal -->
    <DigievolutionGridModal 
      :is-open="isGridModalOpen" 
      :digimon="digimon"
      @close="isGridModalOpen = false"
    />
  </div>
</template>
