<script setup lang="ts">
import { computed, ref } from "vue";
import JournalQuestsSection from "@/components/journal/JournalQuestsSection.vue";
import JournalQuestCard from "@/components/journal/JournalQuestCard.vue";
import AuctionCard from "@/components/journal/AuctionCard.vue";
import QuestModal from "@/components/journal/quest-modal/QuestModal.vue";
import AuctionModal from "@/components/journal/auction-modal/AuctionModal.vue";
import { useGameStore } from "@/stores/use-game-store";
import { JournalPresenter } from "@/presenters/journal/journal.presenter";
import { AuctionPresenter } from "@/presenters/auction/auction.presenter";

const store = useGameStore();

const journalViewModel = computed(() => {
  const journal = store.currentState?.journal;
  if (journal === null || journal === undefined) {
    return null;
  }
  const digimonSlots = store.currentState?.party?.slots ?? [];
  return JournalPresenter.getJournalViewModel(journal, digimonSlots);
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

const auctionCardViewModel = computed(() => {
  const journal = store.currentState?.journal ?? null;
  return AuctionPresenter.getAuctionCardViewModel(journal);
});

const isAuctionModalOpen = ref(false);

const openAuctionModal = () => {
  isAuctionModalOpen.value = true;
};

const closeAuctionModal = () => {
  isAuctionModalOpen.value = false;
};
</script>

<template>
  <aside
    v-if="journalViewModel"
    class="dw3-aside flex-1 min-h-0"
  >
    <div class="flex-1 min-h-0 overflow-y-auto mt-2 pr-1 custom-scroll space-y-4">
      <section>
        <JournalQuestCard
          v-if="journalViewModel.mainQuest"
          :quest="journalViewModel.mainQuest"
          display-mode="main"
          @click="openQuestModal"
        />
      </section>

      <section>
        <AuctionCard
          :auction-card="auctionCardViewModel"
          @click="openAuctionModal"
        />
      </section>

      <JournalQuestsSection
        :title="$t('journal.sideQuests')"
        accent-color="teal"
      >
        <JournalQuestCard
          v-for="sideQuest in journalViewModel.sideQuests"
          :key="sideQuest.id"
          :quest="sideQuest"
          display-mode="side"
          @click="openQuestModal"
        />
      </JournalQuestsSection>

      <JournalQuestsSection
        :title="$t('journal.legendaryWeapons')"
        accent-color="cyan"
      >
        <JournalQuestCard
          v-for="legendaryWeapon in journalViewModel.legendaryWeapons"
          :key="legendaryWeapon.id"
          :quest="legendaryWeapon"
          display-mode="side"
          @click="openQuestModal"
        />
      </JournalQuestsSection>

      <JournalQuestsSection
        :title="$t('journal.driAgents')"
        accent-color="sky"
      >
        <JournalQuestCard
          v-for="driAgent in journalViewModel.driAgents"
          :key="driAgent.id"
          :quest="driAgent"
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

  <AuctionModal
    :is-open="isAuctionModalOpen"
    :journal="store.currentState?.journal ?? null"
    @close="closeAuctionModal"
  />
</template>
