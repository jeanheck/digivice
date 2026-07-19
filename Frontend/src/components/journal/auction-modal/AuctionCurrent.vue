<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { AuctionCurrentViewModel } from "@/viewmodels/auction/auction-current.viewmodel";

const props = defineProps<{
  currentAuction: AuctionCurrentViewModel;
}>();

const { t } = useI18n();

const panelClass = computed(() => {
  if (props.currentAuction.isActive) {
    return "border-cyan-400/80 bg-[#001a2a] auction-card-active";
  }

  return "border-gray-700/40 bg-[#0a0a1a] opacity-70";
});

const titleClass = computed(() => {
  if (props.currentAuction.isActive) {
    return "text-cyan-300";
  }

  return "text-gray-500";
});

const titleText = computed(() => {
  if (props.currentAuction.isActive) {
    return t("auction.currentActiveTitle");
  }

  return t("auction.currentIdleTitle");
});

const equipmentName = computed(() => {
  if (props.currentAuction.equipmentId === null) {
    return null;
  }

  return t(`equipments.${props.currentAuction.equipmentId}.name`);
});

const closesWhenText = computed(() => {
  if (props.currentAuction.closesWhenKey === null) {
    return null;
  }

  return t(props.currentAuction.closesWhenKey);
});
</script>

<template>
  <div
    class="p-4 rounded border relative overflow-hidden shrink-0"
    :class="panelClass"
  >
    <div
      v-if="currentAuction.isActive"
      class="absolute inset-0 bg-cyan-500/10 pointer-events-none"
    />

    <div class="relative z-10 space-y-2">
      <div class="flex items-start justify-between gap-2">
        <h3 class="font-bold text-sm leading-snug" :class="titleClass">
          {{ titleText }}
        </h3>

        <span
          v-if="currentAuction.isActive"
          class="text-xs shrink-0 text-cyan-300 animate-auction-pulse"
        >
          ◆
        </span>
        <span v-else class="text-xs shrink-0">🔒</span>
      </div>

      <p
        v-if="currentAuction.isActive && equipmentName !== null"
        class="text-[11px] leading-relaxed"
      >
        <span class="text-white">{{ $t("auction.currentActiveAcquirePrefix") }}</span>
        <span class="text-cyan-300">&nbsp;{{ equipmentName }}</span>
        <span class="text-white">!</span>
      </p>
      <p
        v-else
        class="text-[11px] leading-relaxed text-gray-400"
      >
        {{ $t("auction.currentIdleSubtitle") }}
      </p>

      <div
        v-if="currentAuction.isActive && currentAuction.purchasePrice !== null && currentAuction.resalePrice !== null"
        class="flex flex-wrap gap-x-4 gap-y-1 pt-1"
      >
        <p class="text-[10px]">
          <span class="text-white">{{ $t("auction.purchasePriceLabel") }} </span>
          <span class="text-cyan-300">&nbsp;{{ currentAuction.purchasePrice }}&nbsp;</span>
          <span class="text-white"> {{ $t("auction.bitsLabel") }}&nbsp;-</span>
        </p>
        <p class="text-[10px]">
          <span class="text-white">{{ $t("auction.resalePriceLabel") }} </span>
          <span class="text-cyan-300">&nbsp;{{ currentAuction.resalePrice }}&nbsp;</span>
          <span class="text-white"> {{ $t("auction.bitsLabel") }}</span>
        </p>
      </div>

      <div
        v-if="currentAuction.isActive && closesWhenText !== null"
        class="pt-2 border-t border-cyan-400/20"
      >
        <p class="text-[10px] leading-relaxed">
          <span class="text-white">{{ $t("auction.closesWhenLabel") }}</span>
          <span class="text-cyan-200/80">&nbsp;{{ closesWhenText }}</span>
        </p>
      </div>
    </div>
  </div>
</template>
