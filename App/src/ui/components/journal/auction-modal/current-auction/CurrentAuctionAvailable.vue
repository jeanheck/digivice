<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { useGameState } from "@/ui/composables/use-game-state";
import { CurrentAuctionPresenter } from "@/presentation/presenters/auction/current-auction.presenter";
import type { AuctionViewModel } from "@/presentation/viewmodels/auction/auction.viewmodel";

const props = defineProps<{
  auction: AuctionViewModel;
}>();

const { t } = useI18n();
const gameState = useGameState();

const playerBits = computed(() => {
  return gameState.value.player.bits;
});

const showNotEnoughBitsWarning = computed(() => {
  return CurrentAuctionPresenter.hasNotEnoughBits(props.auction, playerBits.value);
});

const equipmentName = computed(() => {
  return t(`equipments.${props.auction.equipmentId}.name`);
});

const closesWhenText = computed(() => {
  return t(`auction.closesWhen.${props.auction.id}`);
});
</script>

<template>
  <div
    class="p-4 rounded border relative overflow-hidden shrink-0 border-accent/80 bg-panel auction-card-active in-[[data-theme=seabed]]:border-yellow-500/80 in-[[data-theme=seabed]]:bg-yellow-50"
  >
    <div
      class="absolute inset-0 bg-accent/15 in-[[data-theme=seabed]]:bg-yellow-400/15 pointer-events-none"
    />

    <div class="relative z-10 space-y-2">
      <div class="flex items-start justify-between gap-2">
        <h3
          class="font-bold text-sm leading-snug text-accent-bright in-[[data-theme=seabed]]:text-yellow-700"
        >
          {{ $t("auction.currentActiveTitle") }}
        </h3>

        <span
          class="text-xs shrink-0 text-accent-bright in-[[data-theme=seabed]]:text-yellow-600 animate-auction-pulse"
        >
          ◆
        </span>
      </div>

      <p class="text-[11px] leading-relaxed">
        <span class="text-ink">{{
          $t("auction.currentActiveAcquirePrefix")
        }}</span>
        <span class="text-accent-bright in-[[data-theme=seabed]]:text-yellow-700"
          >&nbsp;{{ equipmentName }}</span
        >
        <span class="text-ink">!</span>
      </p>

      <div class="flex flex-wrap gap-x-4 gap-y-1 pt-1">
        <p class="text-[10px]">
          <span class="text-ink">{{
            $t("auction.bidLabel")
          }}</span>
          <span class="text-accent-bright in-[[data-theme=seabed]]:text-yellow-700"
            >&nbsp;{{ auction.bid }}&nbsp;</span
          >
          <span class="text-ink">
            {{ $t("auction.bitsLabel") }}&nbsp;-</span
          >
        </p>
        <p class="text-[10px]">
          <span class="text-ink">{{
            $t("auction.resalePriceLabel")
          }}</span>
          <span class="text-accent-bright in-[[data-theme=seabed]]:text-yellow-700"
            >&nbsp;{{ auction.resale }}&nbsp;</span
          >
          <span class="text-ink">
            {{ $t("auction.bitsLabel") }}</span
          >
        </p>
      </div>

      <div class="pt-2 border-t border-accent/20 in-[[data-theme=seabed]]:border-yellow-500/30">
        <p class="text-[10px] leading-relaxed">
          <span class="text-ink">{{
            $t("auction.closesWhenLabel")
          }}</span>
          <span class="text-ink/80 in-[[data-theme=seabed]]:text-yellow-700/80"
            >&nbsp;{{ closesWhenText }}</span
          >
        </p>
      </div>

      <div
        v-if="showNotEnoughBitsWarning"
        class="pt-2 border-t border-accent/20 in-[[data-theme=seabed]]:border-yellow-500/30"
      >
        <p
          class="text-[10px] leading-relaxed text-red-400 in-[[data-theme=seabed]]:text-red-600"
        >
          {{ $t("auction.notEnoughBitsWarning", { bits: playerBits }) }}
        </p>
      </div>
    </div>
  </div>
</template>
