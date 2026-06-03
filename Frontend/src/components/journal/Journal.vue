<script setup lang="ts">
import { computed, ref } from "vue";
import JournalQuestsSection from "@/components/journal/JournalQuestsSection.vue";
import JournalQuestCard from "@/components/journal/JournalQuestCard.vue";
import QuestModal from "@/components/journal/quest-modal/QuestModal.vue";
import { useGameStore } from "@/stores/use-game-store";
import { JournalPresenter } from "@/presenters/journal/journal.presenter";

const store = useGameStore();

const journalViewModel = computed(() => {
  const journal = store.currentState?.journal;
  if (journal === null || journal === undefined) {
    return null;
  }
  return JournalPresenter.getJournalViewModel(journal);
});

const activeQuestId = ref<string | null>(null);
const isQuestModalOpen = ref(false);

const openQuestModal = (questId: string) => {
  activeQuestId.value = questId;
  isQuestModalOpen.value = true;
};

const closeQuestModal = () => {
  isQuestModalOpen.value = false;
  setTimeout(() => {
    activeQuestId.value = null;
  }, 300);
};
</script>

<template>
  <aside
    v-if="journalViewModel"
    class="dw3-aside flex-1 min-h-0"
  >
    <div class="dw3-aside-header">
      <h3 class="dw3-aside-title shadow-text">{{ $t("journal.title") }}</h3>
    </div>

    <div class="flex-1 overflow-y-auto mt-2 pr-1 custom-scroll space-y-4">
      <section>
        <h4 class="text-xs text-yellow-400 font-bold mb-2 uppercase tracking-wide border-b border-yellow-700/60 pb-1">
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
        accent-color="cyan"
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
    :quest-id="activeQuestId"
    @close="closeQuestModal"
  />
</template>
