<script setup lang="ts">
import type { AuctionViewModel } from "@/viewmodels/auction/auction.viewmodel";
import AuctionAvailable from "./AuctionAvailable.vue";
import AuctionMissed from "./AuctionMissed.vue";
import AuctionNotYetOccurred from "./AuctionNotYetOccurred.vue";
import AuctionParticipated from "./AuctionParticipated.vue";

defineProps<{
  auction: AuctionViewModel;
}>();
</script>

<template>
  <AuctionAvailable v-if="auction.status === 'available'" :auction="auction" />
  <AuctionNotYetOccurred
    v-else-if="auction.status === 'notYetOccurred'"
    :auction="auction"
  />
  <AuctionParticipated
    v-else-if="auction.status === 'participated'"
    :auction="auction"
  />
  <AuctionMissed v-else-if="auction.status === 'missed'" :auction="auction" />
</template>
