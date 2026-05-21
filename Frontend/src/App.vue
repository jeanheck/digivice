<script setup lang="ts">
import { computed, ref } from 'vue';
import { useGameStore } from './stores/use-game-store';
import { useLocalization } from './composables/useLocalization';
import DigimonCard from './components/digimon/DigimonCard.vue';
import QuestJournalPanel from './components/layout/QuestJournalPanel.vue';
import AreaInformationPanel from './components/layout/AreaInformationPanel.vue';
import QuestDetailsModal from './components/layout/QuestDetailsModal.vue';
import Footer from './components/layout/Footer.vue';

const store = useGameStore();
const { getLocalizedQuest } = useLocalization();

const activeQuestId = ref<string | null>(null);
const isQuestModalOpen = ref(false);

const activePartySlots = computed(() => {
    return (store.currentState?.party?.slots ?? []).filter((slot) => {
        return slot !== null && slot !== undefined && slot.digimon !== null;
    });
});

const activeQuestForModal = computed(() => {
    const questId = activeQuestId.value;
    if (!questId) {
        return null;
    }
    
    const journal = store.currentState?.journal;
    if (!journal) {
        return null;
    }
    
    const getQuestId = (q: any) => {
        return q?.Id || q?.id || q?.QuestId;
    };

    if (journal.mainQuest && getQuestId(journal.mainQuest) === questId) {
        return getLocalizedQuest(journal.mainQuest);
    }
    
    const sideQuest = journal.sideQuests?.find((q) => {
        return getQuestId(q) === questId;
    });
    
    return sideQuest ? getLocalizedQuest(sideQuest) : null;
});

const handleQuestClick = (quest: any) => {
    activeQuestId.value = quest.Id || quest.id || quest.QuestId || null;
    isQuestModalOpen.value = true;
};

const handleCloseQuestModal = () => {
    isQuestModalOpen.value = false;
    setTimeout(() => {
        activeQuestId.value = null;
    }, 300);
};
</script>

<template>
  <main class="min-h-screen bg-transparent p-4 flex flex-col gap-4 max-w-[1800px] mx-auto text-white">
    <div class="flex-1 flex gap-4 min-h-[600px]">
      
      <div class="flex-[3] grid grid-cols-3 gap-4">
        <DigimonCard 
          v-for="slot in activePartySlots" 
          :key="slot.index" 
          :digimon="slot.digimon!" 
        />
      </div>

      <div class="flex-1 min-w-[300px] min-h-0 overflow-hidden flex flex-col gap-4">
        <div class="flex-[3] min-h-0 overflow-hidden flex flex-col">
          <QuestJournalPanel @quest-click="handleQuestClick" class="flex-1" />
        </div>
        
        <div class="flex-[2] min-h-[200px] flex flex-col">
          <AreaInformationPanel :area-info="store.areaInformation ?? null" class="flex-1" />
        </div>
      </div>

    </div>

    <Footer 
      :player-name="store.currentState?.player?.name ?? $t('common.connecting')"
      :bits="store.currentState?.player?.bits ?? 0"
      :group-charisma="store.groupCharisma"
      :is-connected="store.isConnected"
    />

    <QuestDetailsModal 
      :is-open="isQuestModalOpen" 
      :quest="activeQuestForModal" 
      @close="handleCloseQuestModal" 
    />
  </main>
</template>
