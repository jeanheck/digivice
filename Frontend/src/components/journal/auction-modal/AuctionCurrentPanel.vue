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
    return "border-amber-400/80 bg-[#2a1a00] auction-card-active";
  }

  return "border-gray-700/40 bg-[#0a0a1a] opacity-70";
});

const titleClass = computed(() => {
  if (props.currentAuction.isActive) {
    return "text-dw3-gold shadow-text-dark";
  }

  return "text-gray-500";
});

const titleText = computed(() => {
  if (props.currentAuction.isActive) {
    return t("auction.currentActiveTitle");
  }

  return t("auction.currentIdleTitle");
});

const descriptionText = computed(() => {
  if (props.currentAuction.isActive && props.currentAuction.equipmentId !== null) {
    return t("auction.currentActiveAcquire", {
      equipmentName: t(`equipments.${props.currentAuction.equipmentId}.name`),
    });
  }

  return t("auction.currentIdleSubtitle");
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
      class="absolute inset-0 bg-amber-500/10 pointer-events-none"
    />

    <div class="relative z-10 space-y-2">
      <div class="flex items-start justify-between gap-2">
        <h3 class="font-bold text-sm leading-snug" :class="titleClass">
          {{ titleText }}
        </h3>

        <span
          v-if="currentAuction.isActive"
          class="text-xs shrink-0 text-amber-300 animate-auction-pulse"
        >
          ◆
        </span>
        <span v-else class="text-xs shrink-0">🔒</span>
      </div>

      <p
        class="text-[11px] leading-relaxed"
        :class="currentAuction.isActive ? 'text-amber-200/90' : 'text-gray-400'"
      >
        {{ descriptionText }}
      </p>

      <div
        v-if="currentAuction.isActive && currentAuction.purchasePrice !== null && currentAuction.resalePrice !== null"
        class="flex flex-wrap gap-x-4 gap-y-1 pt-1"
      >
        <p class="text-[10px] text-dw3-gold shadow-text-dark">
          {{ $t("auction.purchasePrice", { price: currentAuction.purchasePrice }) }}
        </p>
        <p class="text-[10px] text-gray-300">
          {{ $t("auction.resalePrice", { price: currentAuction.resalePrice }) }}
        </p>
      </div>

      <div
        v-if="currentAuction.isActive && closesWhenText !== null"
        class="pt-2 border-t border-amber-400/20"
      >
        <p class="text-[10px] text-amber-300/80 leading-relaxed">
          <span class="font-bold">{{ $t("auction.closesWhenLabel") }}</span>
          {{ closesWhenText }}
        </p>
      </div>
    </div>
  </div>
</template>
