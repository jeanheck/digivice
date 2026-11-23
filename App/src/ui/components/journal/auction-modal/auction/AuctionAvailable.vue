<script setup lang="ts">
import type { AuctionViewModel } from "@/presentation/viewmodels/auction/auction.viewmodel";

defineProps<{
  auction: AuctionViewModel;
}>();
</script>

<template>
  <div
    class="p-3 rounded border relative overflow-hidden border-accent/80 bg-panel in-[[data-theme=seabed]]:border-yellow-500/80 in-[[data-theme=seabed]]:bg-yellow-50"
  >
    <div
      class="absolute inset-0 bg-accent/15 in-[[data-theme=seabed]]:bg-yellow-400/15 pointer-events-none"
    />

    <div class="flex items-center justify-between mb-1 relative z-10">
      <span
        class="font-bold text-xs truncate text-accent-bright in-[[data-theme=seabed]]:text-yellow-700"
      >
        {{ $t(`equipments.${auction.equipmentId}.name`) }}
      </span>

      <span class="text-xs shrink-0 ml-2 text-accent-bright in-[[data-theme=seabed]]:text-yellow-600"
        >◆</span
      >
    </div>

    <p
      class="text-[10px] leading-tight relative z-10 text-ink/90 in-[[data-theme=seabed]]:text-yellow-700/85"
    >
      {{ $t("auction.historyActive.timing") }}
    </p>

    <p
      class="text-[10px] leading-tight mt-1 min-h-3 relative z-10 text-accent-bright/80 in-[[data-theme=seabed]]:text-yellow-600/90"
    >
      {{ $t("auction.historyActive.participation") }}
    </p>
  </div>
</template>
