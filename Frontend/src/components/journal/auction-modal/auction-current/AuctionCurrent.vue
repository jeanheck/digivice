<script setup lang="ts">
import { computed } from "vue";
import AuctionCurrentAvailable from "@/components/journal/auction-modal/auction-current/AuctionCurrentAvailable.vue";
import AuctionCurrentUnavailable from "@/components/journal/auction-modal/auction-current/AuctionCurrentUnavailable.vue";
import { AuctionCardPresenter } from "@/presenters/auction/auction-card.presenter";
import { useGameStore } from "@/stores/use-game-store";

const store = useGameStore();

const auctionAvailable = computed(() => {
  const auctions = store.currentState?.auctions ?? null;
  const mainQuest = store.currentState?.journal?.mainQuest ?? null;
  return AuctionCardPresenter.getAuctionAvailable(auctions, mainQuest);
});
</script>

<template>
  <AuctionCurrentAvailable v-if="auctionAvailable !== null" :auction="auctionAvailable" />
  <AuctionCurrentUnavailable v-else />
</template>
