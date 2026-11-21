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
    class="p-2 rounded border cursor-pointer transition-all duration-200 group relative overflow-hidden border-cyan-400/80 bg-[#001a2a] hover:bg-[#002a3a] hover:border-cyan-300 auction-card-active in-[[data-theme=seabed]]:border-yellow-500/80 in-[[data-theme=seabed]]:bg-yellow-50 in-[[data-theme=seabed]]:hover:bg-yellow-100 in-[[data-theme=seabed]]:hover:border-yellow-400"
  >
    <div
      class="absolute inset-0 bg-cyan-500/10 in-[[data-theme=seabed]]:bg-yellow-400/15 pointer-events-none"
    />

    <div class="flex items-center justify-between mb-1 relative z-10">
      <span
        class="font-bold truncate transition-colors text-xs text-cyan-300 group-hover:text-cyan-200 in-[[data-theme=seabed]]:text-yellow-700 in-[[data-theme=seabed]]:group-hover:text-yellow-600"
      >
        {{ t("auction.cardActiveTitle") }}
      </span>

      <span
        class="text-xs shrink-0 ml-2 text-cyan-300 in-[[data-theme=seabed]]:text-yellow-600 animate-auction-pulse"
        >◆</span
      >
    </div>

    <p
      class="text-[10px] leading-tight line-clamp-2 relative z-10 text-cyan-200/90 in-[[data-theme=seabed]]:text-yellow-700/85"
    >
      {{ t("auction.cardActiveSubtitle") }}
    </p>
  </div>
</template>
