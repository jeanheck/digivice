<script setup lang="ts">
import { ref } from "vue";
import AuctionAvailableNow from "@/components/journal/AuctionAvailableNow.vue";
import NoAuctionAvailableNow from "@/components/journal/NoAuctionAvailableNow.vue";
import AuctionModal from "@/components/journal/auction-modal/AuctionModal.vue";
import type { Journal } from "@/models";
import type { AuctionListItemViewModel } from "@/viewmodels/auction/auction-list-item.viewmodel";

defineProps<{
  auctionAvailableNow: AuctionListItemViewModel | null;
  journal: Journal | null;
}>();

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
