<script setup lang="ts">
import { useI18n } from "vue-i18n";
import type { AuctionListItemViewModel } from "@/viewmodels/auction/auction-list-item.viewmodel";

defineProps<{
  auction: AuctionListItemViewModel;
}>();

const emit = defineEmits<{
  click: [];
}>();

const { t } = useI18n();

const onClick = () => {
  emit("click");
};
</script>

<template>
  <div
    class="p-2 rounded border cursor-pointer transition-all duration-200 group relative overflow-hidden border-cyan-400/80 bg-[#001a2a] hover:bg-[#002a3a] hover:border-cyan-300 auction-card-active"
    @click="onClick"
  >
    <div class="absolute inset-0 bg-cyan-500/10 pointer-events-none" />

    <div class="flex items-center justify-between mb-1 relative z-10">
      <span class="font-bold truncate transition-colors text-xs text-cyan-300 group-hover:text-cyan-200">
        {{ t("auction.cardActiveTitle") }}
      </span>

      <span class="text-xs shrink-0 ml-2 text-cyan-300 animate-auction-pulse">◆</span>
    </div>

    <p class="text-[10px] leading-tight line-clamp-2 relative z-10 text-cyan-200/90">
      {{ t("auction.cardActiveSubtitle") }}
    </p>
  </div>
</template>
