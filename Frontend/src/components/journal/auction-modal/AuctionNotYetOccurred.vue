<script setup lang="ts">
import type { AuctionViewModel } from "@/viewmodels/auction/auction.viewmodel";

defineProps<{
  auction: AuctionViewModel;
}>();
</script>

<template>
  <div class="p-3 rounded border relative overflow-hidden border-gray-700/40 bg-[#0a0a1a] opacity-50">
    <div class="flex items-center justify-between mb-1 relative z-10">
      <span class="font-bold text-xs truncate text-gray-500">
        {{ $t(`equipments.${auction.equipmentId}.name`) }}
      </span>

      <span class="text-xs shrink-0 ml-2">🔒</span>
    </div>

    <p class="text-[10px] leading-tight relative z-10 text-gray-400">
      {{ $t("auction.historyTiming.upcoming") }}
    </p>

    <p class="text-[10px] leading-tight mt-1 min-h-[12px] relative z-10 invisible">
      &nbsp;
    </p>
  </div>
</template>
