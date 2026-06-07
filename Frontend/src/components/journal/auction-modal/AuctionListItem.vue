<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { AuctionListItemViewModel } from "@/viewmodels/auction/auction-list-item.viewmodel";

const props = defineProps<{
  auction: AuctionListItemViewModel;
}>();

const { t } = useI18n();

const isAvailableNow = computed(() => {
  return props.auction.status === "availableNow";
});

const isNotYetOccurred = computed(() => {
  return props.auction.status === "notYetOccurred";
});

const isBought = computed(() => {
  return props.auction.status === "bought";
});

const isParticipatedWithoutPurchase = computed(() => {
  return props.auction.status === "participatedWithoutPurchase";
});

const isMissed = computed(() => {
  return props.auction.status === "missed";
});

const cardClass = computed(() => {
  if (isAvailableNow.value) {
    return "border-amber-400/80 bg-[#2a1a00] auction-card-active";
  }

  if (isBought.value) {
    return "border-green-800/50 bg-green-900/20";
  }

  if (isParticipatedWithoutPurchase.value) {
    return "border-cyan-800/50 bg-cyan-900/15";
  }

  if (isMissed.value) {
    return "border-rose-900/40 bg-[#1a0a0f] opacity-70";
  }

  return "border-gray-700/40 bg-[#0a0a1a] opacity-50";
});

const titleClass = computed(() => {
  if (isAvailableNow.value) {
    return "text-dw3-gold";
  }

  if (isBought.value) {
    return "text-green-400";
  }

  if (isParticipatedWithoutPurchase.value) {
    return "text-cyan-400";
  }

  if (isMissed.value) {
    return "text-rose-300/80";
  }

  return "text-gray-500";
});

const statusTextClass = computed(() => {
  if (isAvailableNow.value) {
    return "text-amber-200/90";
  }

  if (isParticipatedWithoutPurchase.value) {
    return "text-cyan-300/80";
  }

  if (isMissed.value) {
    return "text-rose-300/70";
  }

  return "text-gray-400";
});

const statusLabel = computed(() => {
  return t(`auction.status.${props.auction.status}`);
});

const stepRangeLabel = computed(() => {
  return t("auction.stepRange", {
    openStep: props.auction.openStep,
    closeStep: props.auction.closeStep,
  });
});
</script>

<template>
  <div
    class="p-3 rounded border relative overflow-hidden"
    :class="cardClass"
  >
    <div
      v-if="isAvailableNow"
      class="absolute inset-0 bg-amber-500/10 pointer-events-none"
    />

    <div
      v-if="isBought"
      class="absolute inset-0 bg-green-500/10 pointer-events-none"
    />

    <div
      v-if="isParticipatedWithoutPurchase"
      class="absolute inset-0 bg-cyan-500/8 pointer-events-none"
    />

    <div class="flex items-center justify-between mb-1 relative z-10">
      <span class="font-bold text-xs truncate" :class="titleClass">
        {{ $t(`equipments.${auction.equipmentId}.name`) }}
      </span>

      <span v-if="isAvailableNow" class="text-xs shrink-0 ml-2 text-amber-300 animate-auction-pulse">◆</span>
      <span v-else-if="isBought" class="text-green-400 text-xs shrink-0 ml-2">✔</span>
      <span v-else-if="isParticipatedWithoutPurchase" class="text-cyan-400 text-xs shrink-0 ml-2">○</span>
      <span v-else-if="isNotYetOccurred" class="text-xs shrink-0 ml-2">🔒</span>
      <span v-else-if="isMissed" class="text-rose-400/80 text-xs shrink-0 ml-2">✕</span>
    </div>

    <p class="text-[10px] leading-tight relative z-10" :class="statusTextClass">
      {{ statusLabel }}
    </p>

    <p class="text-[10px] text-gray-500 leading-tight mt-1 relative z-10">
      {{ stepRangeLabel }}
    </p>
  </div>
</template>
