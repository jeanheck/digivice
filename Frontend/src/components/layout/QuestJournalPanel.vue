<script setup lang="ts">
import { useGameStore } from '../../stores/useGameStore'
import { computed, ref } from 'vue'

const store = useGameStore()
const mainQuest = computed(() => store.gameState?.journal?.mainQuest)
const sideQuests = computed(() => store.gameState?.journal?.sideQuests || [])

const isSideQuestsExpanded = ref(true)

const emit = defineEmits(['quest-click'])

const toggleSideQuests = () => {
  isSideQuestsExpanded.value = !isSideQuestsExpanded.value
}

const onQuestClick = (quest: any) => {
  emit('quest-click', quest)
}

// Temporary test mocks to fill out the sidequests UI until backend provides more
const displaySideQuests = computed(() => {
  const quests = [...sideQuests.value]
  
  if (quests.length === 1) { // We only have kicking boots right now
    quests.push(
      { 
        id: 'mock_1', title: 'The Lost Card', description: 'Help the old man find his rare Digimon Card', done: false, available: true, requirements: [],
        steps: [{ number: 1, description: 'Find card', isCompleted: false}] 
      },
      { 
        id: 'mock_2', title: 'Epic Weapon', description: 'Forge the ultimate elemental weapon', done: true, available: true, requirements: [],
        steps: [{ number: 1, description: 'Forge it', isCompleted: true}] 
      }
    )
  }
  return quests
})
</script>

<template>
  <aside class="w-full h-full bg-[#000e3f] rounded-md shadow-lg border-2 border-[#0033aa] p-3 flex flex-col pt-0 mt-4 overflow-hidden">
    <div class="w-full flex items-center justify-center border-b border-[#0033aa]/50 bg-[#000e3f] sticky top-0 py-2 z-10">
      <h3 class="font-bold tracking-widest text-[#0077ff] text-shadow-sm uppercase text-sm">Journal</h3>
    </div>
    
    <div class="flex-1 overflow-y-auto mt-2 pr-1 custom-scroll space-y-4">
      
      <!-- MAIN QUEST SECTION -->
      <section>
        <h4 class="text-xs text-orange-400 font-bold mb-2 uppercase tracking-wide border-b border-orange-900 pb-1">Main Quest</h4>
        
        <div v-if="mainQuest" 
             @click="onQuestClick(mainQuest)"
             class="p-2 rounded border border-gray-600 bg-gray-800/50 hover:bg-gray-700/60 cursor-pointer transition-colors group">
          <div class="flex items-center justify-between mb-1">
            <span class="text-white font-bold text-sm truncate group-hover:text-orange-300 transition-colors">{{ mainQuest.title }}</span>
            <span v-if="mainQuest.done" class="text-green-400 text-xs">✔</span>
          </div>
          <p class="text-gray-400 text-xs line-clamp-2 leading-tight">{{ mainQuest.description }}</p>
        </div>
        
        <div v-else class="p-2 rounded border border-gray-700 border-dashed text-center opacity-60">
          <span class="text-gray-500 text-xs italic">Awaiting Destination...</span>
        </div>
      </section>

      <!-- SIDE QUESTS SECTION -->
      <section>
        <div 
          @click="toggleSideQuests"
          class="flex items-center justify-between mb-2 border-b border-blue-900 pb-1 cursor-pointer hover:bg-blue-900/30 transition-colors p-1 -mx-1 rounded"
        >
          <h4 class="text-xs text-blue-400 font-bold uppercase tracking-wide">Side Quests</h4>
          <span class="text-blue-500 text-xs transform transition-transform duration-300" :class="{ 'rotate-180': isSideQuestsExpanded }">▼</span>
        </div>
        
        <!-- Accordion Content -->
        <div v-show="isSideQuestsExpanded" class="space-y-2">
          
          <div v-for="quest in displaySideQuests" :key="quest.id"
               @click="onQuestClick(quest)"
               class="p-2 rounded border cursor-pointer transition-all duration-200 group relative overflow-hidden"
               :class="quest.done ? 'border-green-800/50 bg-green-900/20 hover:bg-green-900/40' : 'border-[#0033aa]/60 bg-[#001122] hover:bg-[#002244] hover:border-[#0055ff]'">
            
            <div v-if="quest.done" class="absolute inset-0 bg-green-500/5 pointer-events-none"></div>

            <div class="flex items-center justify-between mb-1 relative z-10">
              <span class="text-white font-bold text-xs truncate group-hover:text-cyan-300 transition-colors"
                :class="{ 'text-gray-400 line-through': quest.done }">
                {{ quest.title }}
              </span>
              <span v-if="quest.done" class="text-green-500 text-xs drop-shadow">✔</span>
            </div>
            <p class="text-gray-400 text-[10px] leading-tight line-clamp-1 relative z-10" :class="{ 'opacity-50': quest.done }">
              {{ quest.description }}
            </p>
          </div>

          <div v-if="displaySideQuests.length === 0" class="p-2 text-center opacity-50">
             <span class="text-gray-500 text-xs italic">No Side Quests Active</span>
          </div>
          
        </div>
      </section>

    </div>
  </aside>
</template>

<style scoped>
.text-shadow-sm {
  text-shadow: 1px 1px 0 #000;
}

/* Custom Scrollbar for the journal log */
.custom-scroll::-webkit-scrollbar {
  width: 4px;
}
.custom-scroll::-webkit-scrollbar-track {
  background: rgba(0, 0, 0, 0.2);
  border-radius: 4px;
}
.custom-scroll::-webkit-scrollbar-thumb {
  background: #0033aa;
  border-radius: 4px;
}
.custom-scroll::-webkit-scrollbar-thumb:hover {
  background: #0077ff;
}
</style>
