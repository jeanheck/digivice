<script setup lang="ts">
import type { AuctionViewModel } from "@/presentation/viewmodels/auction/auction.viewmodel";

defineProps<{
  auction: AuctionViewModel;
}>();
</script>

<template>
  <div
    class="p-3 rounded border relative overflow-hidden border-green-800/50 bg-green-900/20 in-[[data-theme=seabed]]:border-emerald-700/40 in-[[data-theme=seabed]]:bg-emerald-800/10"
  >
    <div
      class="absolute inset-0 bg-green-500/10 in-[[data-theme=seabed]]:bg-emerald-600/5 pointer-events-none"
    />

    <div class="flex items-center justify-between mb-1 relative z-10">
      <span
        class="font-bold text-xs truncate text-green-400 in-[[data-theme=seabed]]:text-emerald-700"
      >
        {{ $t(`equipments.${auction.equipmentId}.name`) }}
      </span>

      <span class="text-green-400 in-[[data-theme=seabed]]:text-emerald-700 text-xs shrink-0 ml-2"
        >✔</span
      >
    </div>

    <p
      class="text-[10px] leading-tight relative z-10 text-ink-muted/70"
    >
      {{ $t("auction.historyTiming.alreadyEnded") }}
    </p>

    <p
      class="text-[10px] leading-tight mt-1 min-h-3 relative z-10 text-green-400/90 in-[[data-theme=seabed]]:text-emerald-700/85"
    >
      {{ $t("auction.historyParticipation.participated") }}
    </p>
  </div>
</template>
