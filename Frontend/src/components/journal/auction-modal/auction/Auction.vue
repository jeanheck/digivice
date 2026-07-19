<script setup lang="ts">
import { AuctionStatusConstant } from "@/constants/auction-status.constant";
import type { AuctionViewModel } from "@/viewmodels/auction/auction.viewmodel";
import AuctionAvailable from "./AuctionAvailable.vue";
import AuctionMissed from "./AuctionMissed.vue";
import AuctionNotYetOccurred from "./auction/AuctionNotYetOccurred.vue";
import AuctionParticipated from "./AuctionParticipated.vue";

defineProps<{
  auction: AuctionViewModel;
}>();
</script>

<template>
  <AuctionAvailable
    v-if="auction.status === AuctionStatusConstant.available"
    :auction="auction"
  />
  <AuctionNotYetOccurred
    v-else-if="auction.status === AuctionStatusConstant.notYetOccurred"
    :auction="auction"
  />
  <AuctionParticipated
    v-else-if="auction.status === AuctionStatusConstant.participated"
    :auction="auction"
  />
  <AuctionMissed
    v-else-if="auction.status === AuctionStatusConstant.missed"
    :auction="auction"
  />
</template>
