<script setup lang="ts">
import { computed, ref } from "vue";
import AuctionAvailableNow from "@/components/journal/AuctionAvailableNow.vue";
import NoAuctionAvailableNow from "@/components/journal/NoAuctionAvailableNow.vue";
import AuctionModal from "@/components/journal/auction-modal/AuctionModal.vue";
import { useGameStore } from "@/stores/use-game-store";
import { AuctionCardPresenter } from "@/presenters/auction/auction-card.presenter";

const store = useGameStore();

const journal = computed(() => {
  return store.currentState?.journal ?? null;
});

const auctionAvailableNow = computed(() => {
  return AuctionCardPresenter.getAuctionAvailableNow(journal.value);
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
  <div>
    <div @click="openAuctionModal">
      <AuctionAvailableNow
        v-if="auctionAvailableNow"
        :auction="auctionAvailableNow"
      />
      <NoAuctionAvailableNow v-else />
    </div>

    <AuctionModal
      :is-open="isAuctionModalOpen"
      :journal="journal"
      @close="closeAuctionModal"
    />
  </div>
</template>
