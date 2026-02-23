<script setup lang="ts">
import { computed } from 'vue'
import { useGameStore } from './stores/useGameStore'
import DigimonCard from './components/digimon/DigimonCard.vue'
import GeneralInfoPanel from './components/layout/GeneralInfoPanel.vue'
import PlayerFooter from './components/layout/PlayerFooter.vue'

const store = useGameStore()
const partySlots = computed(() => store.gameState?.party?.slots ?? [null, null, null])
</script>

<template>
  <main class="min-h-screen bg-gray-100 p-4 flex flex-col gap-4 max-w-[1800px] mx-auto text-black">
    <!-- Top Section: Core Game Data -->
    <div class="flex-1 flex gap-4 min-h-[600px]">
      
      <!-- Digimon Slots (3 Columns) -->
      <div class="flex-[3] grid grid-cols-3 gap-4">
        <!-- Loop exactly 3 times -->
        <template v-for="(digimon, index) in partySlots" :key="index">
          <DigimonCard 
            v-if="digimon" 
            :digimon="digimon" 
          />
          
          <!-- Empty Slot Placeholder -->
          <div v-else class="flex flex-col h-full w-full p-4 rounded-md shadow-lg border-2 border-dashed border-gray-400 bg-gray-200/50 flex items-center justify-center">
            <span class="text-gray-500 font-medium opacity-70">Slot Empty</span>
          </div>
        </template>
      </div>

      <!-- General Info Panel (Right Sidebar) -->
      <div class="flex-1 min-w-[300px]">
        <GeneralInfoPanel />
      </div>

    </div>

    <!-- Bottom Section: Player Info -->
    <PlayerFooter />
  </main>
</template>
