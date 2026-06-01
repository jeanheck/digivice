<script setup lang="ts">
import { computed, ref } from "vue";
import JournalQuestsSection from "@/components/journal/JournalQuestsSection.vue";
import JournalQuestCard from "@/components/journal/JournalQuestCard.vue";
import QuestModal from "@/components/journal/quest-modal/QuestModal.vue";
import { useGameStore } from "@/stores/use-game-store";
import { JournalPresenter } from "@/presenters/journal.presenter";
import type { QuestViewModel } from "@/viewmodels/quest/quest.viewmodel";

const store = useGameStore();

const journalViewModel = computed(() => {
  const journal = store.currentState?.journal;
  if (journal === null || journal === undefined) {
    return null;
  }
  return JournalPresenter.getJournalViewModel(journal);
});

const activeQuest = ref<QuestViewModel | null>(null);
const isQuestModalOpen = ref(false);

const openQuestModal = (questViewModel: QuestViewModel) => {
  activeQuest.value = questViewModel;
  isQuestModalOpen.value = true;
};

const closeQuestModal = () => {
  isQuestModalOpen.value = false;
  setTimeout(() => {
    activeQuest.value = null;
  }, 300);
};
</script>

<template>
  <aside
    v-if="journalViewModel"
    class="w-full h-full bg-[#000e3f] rounded-md shadow-lg border-2 border-[#0033aa] p-3 flex flex-col pt-0 overflow-hidden mb-1"
  >
    <div class="w-full flex items-center justify-center border-b border-[#0033aa]/50 bg-[#000e3f] sticky top-0 py-2 z-10">
      <h3 class="font-bold tracking-widest text-[#0077ff] shadow-text uppercase text-sm">{{ $t("journal.title") }}</h3>
    </div>

    <div class="flex-1 overflow-y-auto mt-2 pr-1 custom-scroll space-y-4">
      <section>
        <h4 class="text-xs text-orange-400 font-bold mb-2 uppercase tracking-wide border-b border-orange-900 pb-1">
          {{ $t("journal.mainQuest") }}
        </h4>

        <JournalQuestCard
          v-if="journalViewModel.mainQuest"
          :quest="journalViewModel.mainQuest"
          display-mode="main"
          @click="openQuestModal"
        />
      </section>

      <JournalQuestsSection
        :title="$t('journal.sideQuests')"
        title-class="text-blue-400"
        border-class="border-blue-900"
        chevron-class="text-blue-500"
        header-hover-class="hover:bg-blue-900/30"
      >
        <JournalQuestCard
          v-for="sideQuest in journalViewModel.sideQuests"
          :key="sideQuest.id"
          :quest="sideQuest"
          display-mode="side"
          @click="openQuestModal"
        />
      </JournalQuestsSection>
    </div>
  </aside>

  <QuestModal
    :is-open="isQuestModalOpen"
    :quest-view-model="activeQuest"
    @close="closeQuestModal"
  />
</template>
