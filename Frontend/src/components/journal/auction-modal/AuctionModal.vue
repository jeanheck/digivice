<script setup lang="ts">
import { computed } from "vue";
import Modal from "@/components/modal/Modal.vue";
import AuctionListItem from "@/components/journal/auction-modal/AuctionListItem.vue";
import { AuctionPresenter } from "@/presenters/auction/auction.presenter";

defineProps<{
  isOpen: boolean;
}>();

const emit = defineEmits<{
  (e: "close"): void;
}>();

const auctionList = computed(() => {
  return AuctionPresenter.getAuctionListViewModels();
});

const activeAuction = computed(() => {
  return AuctionPresenter.getActiveAuction();
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
      <div
        v-if="activeAuction"
        class="p-3 rounded border border-amber-400/80 bg-[#2a1a00] auction-card-active shrink-0"
      >
        <p class="text-[10px] uppercase tracking-widest text-amber-300/80 mb-1">
          {{ $t("auction.activeBanner") }}
        </p>
        <p class="text-dw3-gold font-bold text-sm shadow-text-dark">
          {{ $t(`equipments.${activeAuction.equipmentId}.name`) }}
        </p>
      </div>

      <div class="space-y-2">
        <AuctionListItem
          v-for="auction in auctionList"
          :key="auction.id"
          :auction="auction"
        />
      </div>
    </div>
  </Modal>
</template>
