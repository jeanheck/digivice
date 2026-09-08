<script setup lang="ts">
import { computed } from "vue";
import CurrentAuctionAvailable from "@/components/journal/auction-modal/current-auction/CurrentAuctionAvailable.vue";
import CurrentAuctionUnavailable from "@/components/journal/auction-modal/current-auction/CurrentAuctionUnavailable.vue";
import { CurrentAuctionPresenter } from "@/presenters/auction/current-auction.presenter";
import { useGameStore } from "@/stores/use-game-store";

const store = useGameStore();

const auctionAvailable = computed(() => {
  const auctions = store.currentState?.auctions ?? null;
  const mainQuest = store.currentState?.journal?.mainQuest ?? null;
  return CurrentAuctionPresenter.getAuctionAvailable(auctions, mainQuest);
});
</script>

<template>
  <CurrentAuctionAvailable v-if="auctionAvailable !== null" :auction="auctionAvailable" />
  <CurrentAuctionUnavailable v-else />
</template>
