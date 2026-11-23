<script setup lang="ts">
import type { AuctionViewModel } from "@/presentation/viewmodels/auction/auction.viewmodel";

defineProps<{
  auction: AuctionViewModel;
}>();
</script>

<template>
  <div
    class="p-3 rounded border relative overflow-hidden border-highlight-border bg-highlight-surface"
  >
    <div class="absolute inset-0 bg-highlight-surface pointer-events-none" />

    <div class="flex items-center justify-between mb-1 relative z-10">
      <span class="font-bold text-xs truncate text-highlight">
        {{ $t(`equipments.${auction.equipmentId}.name`) }}
      </span>

      <span class="text-xs shrink-0 ml-2 text-highlight-bright">◆</span>
    </div>

    <p class="text-[10px] leading-tight relative z-10 text-highlight/85">
      {{ $t("auction.historyActive.timing") }}
    </p>

    <p class="text-[10px] leading-tight mt-1 min-h-3 relative z-10 text-highlight-bright/90">
      {{ $t("auction.historyActive.participation") }}
    </p>
  </div>
</template>
