<script setup lang="ts">
import { computed } from "vue";
import CurrentAuctionAvailable from "@/components/journal/auction-modal/current-auction/CurrentAuctionAvailable.vue";
import CurrentAuctionUnavailable from "@/components/journal/auction-modal/current-auction/CurrentAuctionUnavailable.vue";
import { CurrentAuctionPresenter } from "@/presenters/auction/current-auction.presenter";
import { useGameState } from "@/composables/use-game-state";

const gameState = useGameState();

const auctionAvailable = computed(() => {
  return CurrentAuctionPresenter.getAvailableAuction(
    gameState.value.auctions,
    gameState.value.journal.mainQuest,
  );
});
</script>

<template>
  <CurrentAuctionAvailable v-if="auctionAvailable !== null" :auction="auctionAvailable" />
  <CurrentAuctionUnavailable v-else />
</template>
