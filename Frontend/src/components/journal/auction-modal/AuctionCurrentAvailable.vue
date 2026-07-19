<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { AuctionCurrentViewModel } from "@/viewmodels/auction/auction-current.viewmodel";

const props = defineProps<{
  currentAuction: AuctionCurrentViewModel;
}>();

const { t } = useI18n();

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
  <div class="p-4 rounded border relative overflow-hidden shrink-0 border-cyan-400/80 bg-[#001a2a] auction-card-active">
    <div class="absolute inset-0 bg-cyan-500/10 pointer-events-none" />

    <div class="relative z-10 space-y-2">
      <div class="flex items-start justify-between gap-2">
        <h3 class="font-bold text-sm leading-snug text-cyan-300">
          {{ $t("auction.currentActiveTitle") }}
        </h3>

        <span class="text-xs shrink-0 text-cyan-300 animate-auction-pulse">
          ◆
        </span>
      </div>

      <p
        v-if="equipmentName !== null"
        class="text-[11px] leading-relaxed"
      >
        <span class="text-white">{{ $t("auction.currentActiveAcquirePrefix") }}</span>
        <span class="text-cyan-300">&nbsp;{{ equipmentName }}</span>
        <span class="text-white">!</span>
      </p>

      <div
        v-if="currentAuction.purchasePrice !== null && currentAuction.resalePrice !== null"
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
        v-if="closesWhenText !== null"
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
