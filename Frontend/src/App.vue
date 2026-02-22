<script setup lang="ts">
import { ref } from 'vue'
import DigimonCard from './components/digimon/DigimonCard.vue'
import GeneralInfoPanel from './components/layout/GeneralInfoPanel.vue'
import PlayerFooter from './components/layout/PlayerFooter.vue'

// Mock Data for testing the UI
const dummyDigimons = ref([
  { id: 1, name: 'Agumon', level: 5, currentExp: 156, expForNextLevel: 250, currentHp: 200, maxHp: 250, currentMp: 45, maxMp: 100 },
  { id: 2, name: 'Gabumon', level: 6, currentExp: 50, expForNextLevel: 300, currentHp: 180, maxHp: 180, currentMp: 110, maxMp: 120 },
  { id: 3, name: 'Patamon', level: 4, currentExp: 90, expForNextLevel: 150, currentHp: 120, maxHp: 130, currentMp: 200, maxMp: 250 }
])
</script>

<template>
  <main class="min-h-screen bg-gray-100 p-4 flex flex-col gap-4 max-w-[1400px] mx-auto">
    <!-- Top Section: Core Game Data -->
    <div class="flex-1 flex gap-4 min-h-[600px]">
      
      <!-- Digimon Slots (3 Columns) -->
      <div class="flex-[3] grid grid-cols-3 gap-4">
        <DigimonCard 
          v-for="digimon in dummyDigimons" 
          :key="digimon.id" 
          :digimon="digimon" 
        />
      </div>

      <!-- General Info Panel (Right Sidebar) -->
      <div class="flex-1 min-w-[300px]">
        <GeneralInfoPanel />
      </div>

    </div>

    <!-- Bottom Section: Player Info -->
    <PlayerFooter player-name="Jean" :bits="15420" />
  </main>
</template>
