<script setup lang="ts">
import { computed } from "vue";
import AuctionCurrentAvailable from "@/components/journal/auction-modal/AuctionCurrentAvailable.vue";
import AuctionCurrentUnavailable from "@/components/journal/auction-modal/AuctionCurrentUnavailable.vue";
import { AuctionCurrentPresenter } from "@/presenters/auction/auction-current.presenter";
import { useGameStore } from "@/stores/use-game-store";

const store = useGameStore();

const currentAuction = computed(() => {
  const journal = store.currentState?.journal ?? null;
  return AuctionCurrentPresenter.getAuctionCurrent(journal);
});
</script>

<template>
  <AuctionCurrentAvailable
    v-if="currentAuction?.isActive"
    :current-auction="currentAuction"
  />
  <AuctionCurrentUnavailable v-else />
</template>
