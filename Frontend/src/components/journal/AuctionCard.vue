<script setup lang="ts">
import { computed, ref } from "vue";
import AuctionModal from "@/components/journal/auction-modal/AuctionModal.vue";
import { useGameStore } from "@/stores/use-game-store";
import { AuctionCardPresenter } from "@/presenters/auction/auction-card.presenter";
import AuctionCardAvailable from "./AuctionCardAvailable.vue";
import AuctionCardUnavailable from "./AuctionCardUnavailable.vue";

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
      <AuctionCardAvailable
        v-if="auctionAvailableNow"
        :auction="auctionAvailableNow"
      />
      <AuctionCardUnavailable v-else />
    </div>

    <AuctionModal
      :is-open="isAuctionModalOpen"
      :journal="journal"
      @close="closeAuctionModal"
    />
  </div>
</template>
