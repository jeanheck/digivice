<script setup lang="ts">
import { ref } from 'vue'
import DigimonBasicInfo from './DigimonBasicInfo.vue'
import DigimonDigievolutions from './DigimonDigievolutions.vue'
import DigimonDetails from './DigimonDetails.vue'
import DigimonEquipments from './DigimonEquipments.vue'
import DigievolutionTreeModal from './DigievolutionTreeModal.vue'
import type { Digimon } from '../../types/backend'

defineProps<{
  digimon: Digimon
}>()

const isTreeModalOpen = ref(false)
</script>

<template>
  <div class="flex flex-col h-full w-full bg-[#000e3f] p-4 rounded-md shadow-lg border-2 border-[#0033aa] gap-4">
    <DigimonBasicInfo :digimon="digimon" />
    
    <DigimonDigievolutions :digimon="digimon" />

    <DigimonDetails :digimon="digimon" />
    
    <DigimonEquipments :digimon="digimon" />

    <div 
      class="flex items-center justify-center bg-[#000a2b] border-2 border-[#00154a] rounded shadow-inner py-1.5 mt-auto cursor-pointer hover:bg-[#001233] transition-colors"
      @click="isTreeModalOpen = true"
    >
      <span class="text-[0.65rem] font-bold text-gray-400 tracking-widest uppercase">Digievolution Tree</span>
    </div>

    <!-- Teleport Modal -->
    <DigievolutionTreeModal 
      :is-open="isTreeModalOpen" 
      :digimon="digimon"
      @close="isTreeModalOpen = false"
    />
  </div>
</template>
