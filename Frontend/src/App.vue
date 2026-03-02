<script setup lang="ts">
import { computed } from 'vue'
import { useGameStore } from './stores/useGameStore'
import DigimonCard from './components/digimon/DigimonCard.vue'
import QuestJournalPanel from './components/layout/QuestJournalPanel.vue'
import AreaInformationPanel from './components/layout/AreaInformationPanel.vue'
import QuestDetailsModal from './components/layout/QuestDetailsModal.vue'
import PlayerFooter from './components/layout/PlayerFooter.vue'
import type { MainQuest, SideQuest } from './types/backend'
import { ref } from 'vue'

const store = useGameStore()
const partySlots = computed(() => store.gameState?.party?.slots ?? [null, null, null])

const activeQuestId = ref<number | null>(null)
const isQuestModalOpen = ref(false)

const activeQuestForModal = computed(() => {
  if (activeQuestId.value === null) return null;
  const journal = store.gameState?.journal;
  if (!journal) return null;
  
  if (journal.mainQuest && journal.mainQuest.id === activeQuestId.value) {
    return journal.mainQuest;
  }
  return journal.sideQuests?.find(q => q.id === activeQuestId.value) || null;
})

const handleQuestClick = (quest: MainQuest | SideQuest) => {
  activeQuestId.value = quest.id
  isQuestModalOpen.value = true
}

const handleCloseQuestModal = () => {
  isQuestModalOpen.value = false
  setTimeout(() => {
     activeQuestId.value = null
  }, 300) // Wait for fade transition
}
</script>

<template>
  <main class="min-h-screen bg-transparent p-4 flex flex-col gap-4 max-w-[1800px] mx-auto text-white">
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

      <!-- Right Sidebar (Journal + Area Info) -->
      <div class="flex-1 min-w-[300px] min-h-0 overflow-hidden flex flex-col gap-4">
        <!-- Quest Journal -->
        <div class="flex-[3] min-h-0 overflow-hidden">
          <QuestJournalPanel @quest-click="handleQuestClick" class="h-full" />
        </div>
        
        <!-- Area Information -->
        <div class="flex-1 min-h-[150px]">
          <AreaInformationPanel class="h-full" />
        </div>
      </div>

    </div>

    <!-- Bottom Section: Player Info -->
    <PlayerFooter />

    <!-- Overlays -->
    <QuestDetailsModal 
      :is-open="isQuestModalOpen" 
      :quest="activeQuestForModal" 
      @close="handleCloseQuestModal" 
    />
  </main>
</template>
