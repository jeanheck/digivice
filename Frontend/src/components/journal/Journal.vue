<script setup lang="ts">
import { computed, ref } from 'vue'
import { useGameStore } from '../../stores/use-game-store'
import { JournalPresenter } from '@/presenters/journal.presenter';
import type { QuestViewModel } from '@/viewmodels/quest/quest.viewmodel';

const store = useGameStore()

const journalViewModel = computed(() => {
  return JournalPresenter.getJournalViewModel(store.currentState?.journal!);
});

const currentMainQuestStep = computed(() => {
  if (!journalViewModel.value.mainQuest || !journalViewModel.value.mainQuest.steps) return null
  return journalViewModel.value.mainQuest.steps.find((s: any) => !s.isDone) || null
})

const isSideQuestsExpanded = ref(true)

const emit = defineEmits(['quest-click'])

const toggleSideQuests = () => {
  isSideQuestsExpanded.value = !isSideQuestsExpanded.value
}

const onQuestClick = (questViewModel: QuestViewModel) => {
  emit('quest-click', questViewModel)
}

// Calculate visually if a quest is done based on its steps
const isQuestDone = (quest: QuestViewModel) => {
  if (!quest || !quest.steps || quest.steps.length === 0) return false;
  return quest.steps.every((s: any) => s.isDone);
}

const isQuestLocked = (quest: QuestViewModel) => {
  if (!quest || !quest.requisites || quest.requisites.length === 0) return false;
  return !quest.requisites.every((p: any) => p.isDone);
}

const isQuestNew = (quest: QuestViewModel) => {
  if (isQuestLocked(quest)) return false;
  if (isQuestDone(quest)) return false;
  // New = all requisites met AND first step not yet completed
  return quest.steps["1"]?.isDone === false;
}
</script>

<template>
  <aside class="w-full h-full bg-[#000e3f] rounded-md shadow-lg border-2 border-[#0033aa] p-3 flex flex-col pt-0 overflow-hidden mb-1">
    <div class="w-full flex items-center justify-center border-b border-[#0033aa]/50 bg-[#000e3f] sticky top-0 py-2 z-10">
      <h3 class="font-bold tracking-widest text-[#0077ff] text-shadow-sm uppercase text-sm">{{ $t('journal.title') }}</h3>
    </div>
    
    <div class="flex-1 overflow-y-auto mt-2 pr-1 custom-scroll space-y-4">
      
      <!-- MAIN QUEST SECTION -->
      <section>
        <h4 class="text-xs text-orange-400 font-bold mb-2 uppercase tracking-wide border-b border-orange-900 pb-1">{{ $t('journal.mainQuest') }}</h4>
        
        <div v-if="journalViewModel.mainQuest" 
             @click="onQuestClick(journalViewModel.mainQuest)"
             class="p-2 rounded border border-gray-600 bg-gray-800/50 hover:bg-gray-700/60 cursor-pointer transition-colors group">
          <div class="flex items-center justify-between mb-1">
            <span class="text-white font-bold text-sm truncate group-hover:text-orange-300 transition-colors">{{ $t('mainQuest.title') }}</span>
            <span v-if="isQuestDone(journalViewModel.mainQuest)" class="text-green-400 text-xs">✔</span>
          </div>
          <p class="text-gray-400 text-xs line-clamp-2 leading-tight">
             <span v-if="currentMainQuestStep" class="text-orange-300 font-bold mr-1">[{{ currentMainQuestStep.number }}]</span>
             {{ currentMainQuestStep ? $t(`mainQuest.steps.${currentMainQuestStep.number}.description`) : $t('mainQuest.description') }}
          </p>
        </div>
        
        <div v-else class="p-2 rounded border border-gray-700 border-dashed text-center opacity-60">
          <span class="text-gray-500 text-xs italic">{{ $t('journal.awaitingDestination') }}</span>
        </div>
      </section>

      <!-- SIDE QUESTS SECTION -->
      <section>
        <div 
          @click="toggleSideQuests"
          class="flex items-center justify-between mb-2 border-b border-blue-900 pb-1 cursor-pointer hover:bg-blue-900/30 transition-colors p-1 -mx-1 rounded"
        >
          <h4 class="text-xs text-blue-400 font-bold uppercase tracking-wide">{{ $t('journal.sideQuests') }}</h4>
          <span class="text-blue-500 text-xs transform transition-transform duration-300" :class="{ 'rotate-180': isSideQuestsExpanded }">▼</span>
        </div>
        
        <!-- Accordion Content -->
        <div v-show="isSideQuestsExpanded" class="space-y-2">
          
          <div v-for="sideQuest in journalViewModel.sideQuests" :key="sideQuest.id"
            @click="onQuestClick(sideQuest)"
            class="p-2 rounded border cursor-pointer transition-all duration-200 group relative overflow-hidden"
            :class="
              isQuestLocked(sideQuest)
                ? 'border-gray-700/40 bg-[#0a0a1a] opacity-50 hover:opacity-70'
                : isQuestDone(sideQuest)
                  ? 'border-green-800/50 bg-green-900/20 hover:bg-green-900/40'
                  : isQuestNew(sideQuest)
                    ? 'border-cyan-300/60 bg-[#001a2a] hover:bg-[#002a3a] hover:border-cyan-300'
                    : 'border-[#0033aa]/60 bg-[#001122] hover:bg-[#002244] hover:border-[#0055ff]'
            ">
            <div v-if="isQuestDone(sideQuest)" class="absolute inset-0 bg-green-500/5 pointer-events-none"></div>

            <div class="flex items-center justify-between mb-1 relative z-10">
              <span class="text-white font-bold text-xs truncate transition-colors"
                :class="{
                   'text-gray-500': isQuestLocked(sideQuest),
                   'text-gray-400 line-through': isQuestDone(sideQuest),
                   'group-hover:text-cyan-300': !isQuestLocked(sideQuest) && !isQuestDone(sideQuest)
                }">
                {{ $t(`${sideQuest.id}.title`) }}
              </span>
              <span v-if="isQuestDone(sideQuest)" class="text-green-500 text-xs drop-shadow shrink-0 ml-2">✔</span>
              <span v-else-if="isQuestLocked(sideQuest)" class="text-xs shrink-0 ml-2">🔒</span>
              <span v-else-if="isQuestNew(sideQuest)" class="text-cyan-300 text-xs shrink-0 ml-2">❕</span>
            </div>
            <p class="text-gray-400 text-[10px] leading-tight line-clamp-1 relative z-10" :class="{ 'opacity-50': isQuestDone(sideQuest) || isQuestLocked(sideQuest) }">
              {{ $t(`${sideQuest.id}.description`) }}
            </p>
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
