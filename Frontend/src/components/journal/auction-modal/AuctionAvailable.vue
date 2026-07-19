<script setup lang="ts">
import type { AuctionViewModel } from "@/viewmodels/auction/auction.viewmodel";

defineProps<{
  auction: AuctionViewModel;
}>();
</script>

<template>
  <div class="p-3 rounded border relative overflow-hidden border-cyan-400/80 bg-[#001a2a]">
    <div class="absolute inset-0 bg-cyan-500/10 pointer-events-none" />

    <div class="flex items-center justify-between mb-1 relative z-10">
      <span class="font-bold text-xs truncate text-cyan-300">
        {{ $t(`equipments.${auction.equipmentId}.name`) }}
      </span>

      <span class="text-xs shrink-0 ml-2 text-cyan-300">◆</span>
    </div>

    <p class="text-[10px] leading-tight relative z-10 text-cyan-200/90">
      {{ $t("auction.historyActive.timing") }}
    </p>

    <p class="text-[10px] leading-tight mt-1 min-h-[12px] relative z-10 text-cyan-300/80">
      {{ $t("auction.historyActive.participation") }}
    </p>
  </div>
</template>
