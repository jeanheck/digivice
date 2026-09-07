<script setup lang="ts">
import { computed, ref } from "vue";
import AuctionModal from "@/components/journal/auction-modal/AuctionModal.vue";
import { useGameStore } from "@/stores/use-game-store";
import { AuctionCardPresenter } from "@/presenters/auction/auction-card.presenter";
import AuctionCardAvailable from "./AuctionCardAvailable.vue";
import AuctionCardUnavailable from "./AuctionCardUnavailable.vue";

const store = useGameStore();

const auctions = computed(() => {
  return store.currentState?.auctions ?? null;
});

const mainQuest = computed(() => {
  return store.currentState?.journal?.mainQuest ?? null;
});

const auctionAvailable = computed(() => {
  return AuctionCardPresenter.getAuctionAvailable(auctions.value, mainQuest.value);
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
      <AuctionCardAvailable v-if="auctionAvailable" :auction="auctionAvailable" />
      <AuctionCardUnavailable v-else />
    </div>

    <AuctionModal
      :is-open="isAuctionModalOpen"
      :auctions="auctions"
      :main-quest="mainQuest"
      @close="closeAuctionModal"
    />
  </div>
</template>
