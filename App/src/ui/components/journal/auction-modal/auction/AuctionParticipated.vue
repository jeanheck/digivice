<script setup lang="ts">
import type { AuctionViewModel } from "@/presentation/viewmodels/auction/auction.viewmodel";

defineProps<{
  auction: AuctionViewModel;
}>();
</script>

<template>
  <div
    class="p-3 rounded border relative overflow-hidden border-success-muted/50 bg-success-surface"
  >
    <div class="absolute inset-0 bg-success-surface pointer-events-none" />

    <div class="flex items-center justify-between mb-1 relative z-10">
      <span class="font-bold text-xs truncate text-success">
        {{ $t(`equipments.${auction.equipmentId}.name`) }}
      </span>

      <span class="text-success text-xs shrink-0 ml-2">✔</span>
    </div>

    <p class="text-[10px] leading-tight relative z-10 text-ink-muted/70">
      {{ $t("auction.historyTiming.alreadyEnded") }}
    </p>

    <p class="text-[10px] leading-tight mt-1 min-h-3 relative z-10 text-success/90">
      {{ $t("auction.historyParticipation.participated") }}
    </p>
  </div>
</template>
