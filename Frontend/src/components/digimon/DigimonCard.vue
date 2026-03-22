<script setup lang="ts">
import { ref } from 'vue'
import DigimonBasicInfo from './DigimonBasicInfo.vue'
import DigimonDigievolutions from './DigimonDigievolutions.vue'
import DigimonDetails from './DigimonDetails.vue'
import DigimonEquipments from './DigimonEquipments.vue'
import DigievolutionGridModal from './DigievolutionGridModal.vue'
// import DigievolutionTreeModal from './DigievolutionTreeModal.vue' // Legacy Mode (Backup)
import type { Digimon } from '../../types/backend'

defineProps<{
  digimon: Digimon
}>()

const isGridModalOpen = ref(false)
</script>

<template>
  <div class="flex flex-col h-full w-full bg-[#000e3f] p-4 rounded-md shadow-lg border-2 border-[#0033aa] gap-4">
    <DigimonBasicInfo :digimon="digimon" />
    
    <DigimonDigievolutions :digimon="digimon" />

    <DigimonDetails :digimon="digimon" />
    
    <DigimonEquipments :digimon="digimon" />

    <div 
      class="flex items-center justify-center bg-[#000a2b] border-2 border-[#00154a] rounded shadow-inner py-1.5 mt-auto cursor-pointer hover:bg-[#001233] transition-colors"
      @click="isGridModalOpen = true"
    >
      <span class="text-[0.65rem] font-bold text-gray-400 tracking-widest uppercase">Digievolutions</span>
    </div>

    <!-- New Grid Modal -->
    <DigievolutionGridModal 
      :is-open="isGridModalOpen" 
      :digimon="digimon"
      @close="isGridModalOpen = false"
    />

    <!-- Legacy Teleport Modal
    <DigievolutionTreeModal 
      :is-open="isTreeModalOpen" 
      :digimon="digimon"
      @close="isTreeModalOpen = false"
    />
    -->
  </div>
</template>
