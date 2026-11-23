<script setup lang="ts">
import { useI18n } from "vue-i18n";
import type { AuctionViewModel } from "@/presentation/viewmodels/auction/auction.viewmodel";

defineProps<{
  auction: AuctionViewModel;
}>();

const { t } = useI18n();
</script>

<template>
  <div
    class="p-2 rounded border cursor-pointer transition-all duration-200 group relative overflow-hidden border-highlight-border bg-highlight-surface hover:border-highlight auction-card-active"
  >
    <div class="absolute inset-0 bg-highlight-surface pointer-events-none" />

    <div class="flex items-center justify-between mb-1 relative z-10">
      <span
        class="font-bold truncate transition-colors text-xs text-highlight group-hover:text-highlight-bright"
      >
        {{ t("auction.cardActiveTitle") }}
      </span>

      <span class="text-xs shrink-0 ml-2 text-highlight-bright animate-auction-pulse">◆</span>
    </div>

    <p class="text-[10px] leading-tight line-clamp-2 relative z-10 text-highlight/85">
      {{ t("auction.cardActiveSubtitle") }}
    </p>
  </div>
</template>
