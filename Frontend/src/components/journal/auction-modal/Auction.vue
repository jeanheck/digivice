<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { AuctionViewModel } from "@/viewmodels/auction/auction.viewmodel";

const props = defineProps<{
  auction: AuctionViewModel;
}>();

const { t } = useI18n();

const isAvailable = computed(() => {
  return props.auction.status === "available";
});

const isNotYetOccurred = computed(() => {
  return props.auction.status === "notYetOccurred";
});

const isParticipated = computed(() => {
  return props.auction.status === "participated";
});

const isMissed = computed(() => {
  return props.auction.status === "missed";
});

const isPastAuction = computed(() => {
  return isParticipated.value || isMissed.value;
});

const cardClass = computed(() => {
  if (isAvailable.value) {
    return "border-cyan-400/80 bg-[#001a2a]";
  }

  if (isParticipated.value) {
    return "border-green-800/50 bg-green-900/20";
  }

  if (isMissed.value) {
    return "border-rose-900/40 bg-[#1a0a0f] opacity-70";
  }

  return "border-gray-700/40 bg-[#0a0a1a] opacity-50";
});

const titleClass = computed(() => {
  if (isAvailable.value) {
    return "text-cyan-300";
  }

  if (isParticipated.value) {
    return "text-green-400";
  }

  if (isMissed.value) {
    return "text-rose-300/80";
  }

  return "text-gray-500";
});

const timingDescriptionClass = computed(() => {
  if (isAvailable.value) {
    return "text-cyan-200/90";
  }

  if (isMissed.value) {
    return "text-rose-300/70";
  }

  return "text-gray-400";
});

const participationDescriptionClass = computed(() => {
  if (isAvailable.value) {
    return "text-cyan-300/80";
  }

  if (isParticipated.value) {
    return "text-green-400/90";
  }

  if (isMissed.value) {
    return "text-rose-300/70";
  }

  return "text-gray-400";
});

const timingDescription = computed(() => {
  if (isAvailable.value) {
    return t("auction.historyActive.timing");
  }

  if (isNotYetOccurred.value) {
    return t("auction.historyTiming.upcoming");
  }

  if (isPastAuction.value) {
    return t("auction.historyTiming.alreadyEnded");
  }

  return t("auction.historyTiming.alreadyEnded");
});

const participationDescription = computed(() => {
  if (isAvailable.value) {
    return t("auction.historyActive.participation");
  }

  if (isParticipated.value) {
    return t("auction.historyParticipation.participated");
  }

  if (isMissed.value) {
    return t("auction.historyParticipation.missed");
  }

  return null;
});
</script>

<template>
  <div
    class="p-3 rounded border relative overflow-hidden"
    :class="cardClass"
  >
    <div
      v-if="isAvailable"
      class="absolute inset-0 bg-cyan-500/10 pointer-events-none"
    />

    <div
      v-if="isParticipated"
      class="absolute inset-0 bg-green-500/10 pointer-events-none"
    />

    <div class="flex items-center justify-between mb-1 relative z-10">
      <span class="font-bold text-xs truncate" :class="titleClass">
        {{ $t(`equipments.${auction.equipmentId}.name`) }}
      </span>

      <span v-if="isAvailable" class="text-xs shrink-0 ml-2 text-cyan-300">◆</span>
      <span v-else-if="isParticipated" class="text-green-400 text-xs shrink-0 ml-2">✔</span>
      <span v-else-if="isNotYetOccurred" class="text-xs shrink-0 ml-2">🔒</span>
      <span v-else-if="isMissed" class="text-rose-400/80 text-xs shrink-0 ml-2">✕</span>
    </div>

    <p class="text-[10px] leading-tight relative z-10" :class="timingDescriptionClass">
      {{ timingDescription }}
    </p>

    <p
      class="text-[10px] leading-tight mt-1 min-h-[12px] relative z-10"
      :class="participationDescription !== null ? participationDescriptionClass : 'invisible'"
    >
      {{ participationDescription ?? "\u00A0" }}
    </p>
  </div>
</template>
