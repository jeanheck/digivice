<script setup lang="ts">
import { computed, ref } from 'vue';
import { useGameStore } from './stores/use-game-store';
import Journal from './components/journal/Journal.vue';
import Map from '@/components/map/Map.vue';
import QuestDetailsModal from './components/modal/QuestDetailsModal.vue';
import Footer from './components/footer/Footer.vue';
import DigimonSlot from './components/digimon/DigimonSlot.vue';
import type { QuestViewModel } from './viewmodels/quest/quest.viewmodel.ts';

const store = useGameStore();
const activeQuest = ref<QuestViewModel | null>(null);
const isQuestModalOpen = ref(false);

const slotsWithDigimon = computed(() => {
    return (store.currentState?.party?.slots ?? []).filter((slot: import('./models/digimon-slot.ts').DigimonSlot) => {
        return slot !== null && slot !== undefined && slot.digimon !== null;
    });
});

const handleQuestClick = (questViewModel: QuestViewModel) => {
    activeQuest.value = questViewModel;
    isQuestModalOpen.value = true;
};

const handleCloseQuestModal = () => {
    isQuestModalOpen.value = false;
    setTimeout(() => {
        activeQuest.value = null;
    }, 300);
};
</script>

<template>
  <main class="min-h-screen bg-transparent p-4 flex flex-col gap-4 max-w-450 mx-auto text-white">
    <div class="flex-1 flex gap-4 min-h-150">
      
      <div class="flex-3 grid grid-cols-3 gap-4">
        <DigimonSlot
          v-for="slot in slotsWithDigimon"
          :key="slot.index" 
          :slot="slot"
        />
      </div>

      <div class="flex-1 min-w-75 min-h-0 overflow-hidden flex flex-col gap-4">
        <div class="flex-3 min-h-0 overflow-hidden flex flex-col">
          <Journal v-if="store.currentState?.journal" :journal="store.currentState.journal" class="flex-1" />
        </div>
        
        <div class="flex-2 min-h-50 flex flex-col">
          <Map :location-id="store.currentState?.player?.location ?? null" class="flex-1" />
        </div>
      </div>

    </div>

    <Footer 
      :player-name="store.currentState?.player?.name ?? $t('connection.connecting')"
      :bits="store.currentState?.player?.bits ?? 0"
      :is-connected="store.isConnected"
    />

    <QuestDetailsModal 
      :is-open="isQuestModalOpen" 
      :questViewModel="activeQuest" 
      @close="handleCloseQuestModal" 
    />
  </main>
</template>
