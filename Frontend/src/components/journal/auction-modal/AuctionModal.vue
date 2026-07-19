<script setup lang="ts">
import { computed } from "vue";
import Modal from "@/components/modal/Modal.vue";
import type { Journal } from "@/models";
import { AuctionModalPresenter } from "@/presenters/auction/auction-modal.presenter";
import Auction from "./Auction.vue";
import AuctionCurrent from "./AuctionCurrent.vue";

const props = defineProps<{
  isOpen: boolean;
  journal: Journal | null;
}>();

const emit = defineEmits<{
  (e: "close"): void;
}>();

const currentAuction = computed(() => {
  return AuctionModalPresenter.getAuctionCurrentViewModel(props.journal);
});

const auctions = computed(() => {
  return AuctionModalPresenter.getAuctions(props.journal);
});

const closeModal = () => {
  emit("close");
};
</script>

<template>
  <Modal
    :is-open="isOpen"
    max-width="max-w-lg"
    max-height="h-[85vh] max-h-200"
    @close="closeModal"
  >
    <template #header>
      <h2 class="text-white font-bold tracking-widest drop-shadow">
        {{ $t("auction.modalTitle") }}
      </h2>
    </template>

    <div class="flex min-h-0 flex-1 flex-col gap-4 overflow-y-auto p-4 custom-scroll">
      <AuctionCurrent :current-auction="currentAuction" />

      <section class="space-y-2">
        <h3 class="text-xs font-bold uppercase tracking-wide text-gray-400 border-b border-gray-700/40 pb-1">
          {{ $t("auction.historySubtitle") }}
        </h3>

        <Auction
          v-for="auction in auctions"
          :key="auction.id"
          :auction="auction"
        />
      </section>
    </div>
  </Modal>
</template>
