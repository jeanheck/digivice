<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { AuctionCardViewModel } from "@/viewmodels/auction/auction-card.viewmodel";

const props = defineProps<{
  auctionCard: AuctionCardViewModel;
}>();

const emit = defineEmits<{
  click: [];
}>();

const { t } = useI18n();

const isActive = computed(() => {
  return props.auctionCard.isActive;
});

const cardClass = computed(() => {
  if (isActive.value) {
    return "border-amber-400/80 bg-[#2a1a00] hover:bg-[#3a2800] hover:border-amber-300 auction-card-active";
  }

  return "border-gray-700/40 bg-[#0a0a1a] opacity-50 hover:opacity-70";
});

const titleClass = computed(() => {
  if (isActive.value) {
    return "text-dw3-gold group-hover:text-yellow-300";
  }

  return "text-gray-500";
});

const titleText = computed(() => {
  if (isActive.value && props.auctionCard.activeEquipmentId !== null) {
    return t(`equipments.${props.auctionCard.activeEquipmentId}.name`);
  }

  return t("auction.cardIdleTitle");
});

const subtitleText = computed(() => {
  if (isActive.value) {
    return t("auction.cardActiveSubtitle");
  }

  return t("auction.cardIdleSubtitle");
});

const onClick = () => {
  emit("click");
};
</script>

<template>
  <div
    class="p-2 rounded border cursor-pointer transition-all duration-200 group relative overflow-hidden"
    :class="cardClass"
    @click="onClick"
  >
    <div
      v-if="isActive"
      class="absolute inset-0 bg-amber-500/10 pointer-events-none"
    />

    <div class="flex items-center justify-between mb-1 relative z-10">
      <span
        class="font-bold truncate transition-colors text-xs"
        :class="titleClass"
      >
        {{ titleText }}
      </span>

      <span v-if="isActive" class="text-xs shrink-0 ml-2 text-amber-300 animate-auction-pulse">◆</span>
      <span v-else class="text-xs shrink-0 ml-2">🔒</span>
    </div>

    <p
      class="text-gray-400 text-[10px] leading-tight line-clamp-1 relative z-10"
      :class="{ 'opacity-50': !isActive }"
    >
      {{ subtitleText }}
    </p>
  </div>
</template>
