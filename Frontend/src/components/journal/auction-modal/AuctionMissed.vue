<script setup lang="ts">
import type { AuctionViewModel } from "@/viewmodels/auction/auction.viewmodel";

defineProps<{
  auction: AuctionViewModel;
}>();
</script>

<template>
  <div class="p-3 rounded border relative overflow-hidden border-rose-900/40 bg-[#1a0a0f] opacity-70">
    <div class="flex items-center justify-between mb-1 relative z-10">
      <span class="font-bold text-xs truncate text-rose-300/80">
        {{ $t(`equipments.${auction.equipmentId}.name`) }}
      </span>

      <span class="text-rose-400/80 text-xs shrink-0 ml-2">✕</span>
    </div>

    <p class="text-[10px] leading-tight relative z-10 text-rose-300/70">
      {{ $t("auction.historyTiming.alreadyEnded") }}
    </p>

    <p class="text-[10px] leading-tight mt-1 min-h-[12px] relative z-10 text-rose-300/70">
      {{ $t("auction.historyParticipation.missed") }}
    </p>
  </div>
</template>
