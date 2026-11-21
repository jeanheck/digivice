<script setup lang="ts">
import type { AuctionViewModel } from "@/presentation/viewmodels/auction/auction.viewmodel";

defineProps<{
  auction: AuctionViewModel;
}>();
</script>

<template>
  <div
    class="p-3 rounded border relative overflow-hidden border-rose-900/40 bg-[#1a0a0f] opacity-70 in-[[data-theme=seabed]]:border-red-800/50 in-[[data-theme=seabed]]:bg-red-950/15 in-[[data-theme=seabed]]:opacity-100"
  >
    <div class="flex items-center justify-between mb-1 relative z-10">
      <span
        class="font-bold text-xs truncate text-rose-300/80 in-[[data-theme=seabed]]:text-red-700"
      >
        {{ $t(`equipments.${auction.equipmentId}.name`) }}
      </span>

      <span class="text-rose-400/80 in-[[data-theme=seabed]]:text-red-600 text-xs shrink-0 ml-2"
        >✕</span
      >
    </div>

    <p
      class="text-[10px] leading-tight relative z-10 text-rose-300/70 in-[[data-theme=seabed]]:text-red-700/80"
    >
      {{ $t("auction.historyTiming.alreadyEnded") }}
    </p>

    <p
      class="text-[10px] leading-tight mt-1 min-h-3 relative z-10 text-rose-300/70 in-[[data-theme=seabed]]:text-red-700/80"
    >
      {{ $t("auction.historyParticipation.missed") }}
    </p>
  </div>
</template>
