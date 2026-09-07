<script setup lang="ts">
import { computed } from "vue";
import AuctionCurrentAvailable from "@/components/journal/auction-modal/auction-current/AuctionCurrentAvailable.vue";
import AuctionCurrentUnavailable from "@/components/journal/auction-modal/auction-current/AuctionCurrentUnavailable.vue";
import { AuctionCurrentPresenter } from "@/presenters/auction/auction-current.presenter";
import { useGameStore } from "@/stores/use-game-store";

const store = useGameStore();

const currentAuction = computed(() => {
  const auctions = store.currentState?.auctions ?? null;
  const mainQuest = store.currentState?.journal?.mainQuest ?? null;
  return AuctionCurrentPresenter.getAuctionCurrent(auctions, mainQuest);
});
</script>

<template>
  <AuctionCurrentAvailable v-if="currentAuction?.isActive" :current-auction="currentAuction" />
  <AuctionCurrentUnavailable v-else />
</template>
