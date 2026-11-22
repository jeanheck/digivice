<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { ImageCatalog } from "@/ui/catalogs/image.catalog";
import { CardRepository } from "@/presentation/repositories/card.repository";
import type { WikiCardShopInventoryCardViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-card-shop-inventory-card.viewmodel";

const props = defineProps<{
  card: WikiCardShopInventoryCardViewModel;
}>();

const emit = defineEmits<{
  (e: "select", cardId: string): void;
}>();

const { t } = useI18n();

const cardName = computed(() => {
  return t(`card.${props.card.cardId}.name`);
});

const cardImageUrl = computed(() => {
  const cardRaw = CardRepository.getCardById(props.card.cardId);
  return ImageCatalog.getCardImageUrl(cardRaw?.imageName ?? null);
});

const handleSelect = (): void => {
  emit("select", props.card.cardId);
};
</script>

<template>
  <button
    type="button"
    class="flex flex-col items-center gap-1.5 w-36 h-full px-1.5 py-2 rounded transition-colors cursor-pointer bg-blue-900/30 in-[[data-theme=seabed]]:bg-[var(--color-accent)]/15 hover:bg-blue-900/50 in-[[data-theme=seabed]]:hover:bg-[var(--color-accent)]/25 border border-blue-800/60 in-[[data-theme=seabed]]:border-[var(--color-frame)]/50"
    @click="handleSelect"
  >
    <img
      v-if="cardImageUrl"
      :src="cardImageUrl"
      :alt="cardName"
      class="w-12 h-16 shrink-0 object-contain rendering-pixelated"
    />
    <span
      class="h-[2.5em] w-full text-[8px] font-bold text-blue-200 in-[[data-theme=seabed]]:text-[var(--color-ink)] leading-tight text-center line-clamp-2"
    >
      {{ cardName }}
    </span>
    <span class="w-full shrink-0 text-[8px] font-bold text-amber-400 in-[[data-theme=seabed]]:text-amber-700 leading-tight text-center">
      {{ card.price }} {{ $t("enemy.bits") }}
    </span>
  </button>
</template>
