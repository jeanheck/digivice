<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { ImageCatalog } from "@/catalogs/image.catalog";
import type { WikiStoreCardViewModel } from "@/viewmodels/wiki-modal/wiki-store-card.viewmodel";

const props = defineProps<{
  card: WikiStoreCardViewModel;
}>();

const emit = defineEmits<{
  (e: "select", cardId: string): void;
}>();

const { t } = useI18n();

const cardName = computed(() => {
  return t(props.card.nameKey);
});

const cardImageUrl = computed(() => {
  return ImageCatalog.getCardImageUrl(cardName.value);
});

const handleSelect = (): void => {
  emit("select", props.card.cardId);
};
</script>

<template>
  <button
    type="button"
    class="flex flex-col items-center gap-1.5 w-36 h-full px-1.5 py-2 rounded transition-colors cursor-pointer bg-blue-900/30 hover:bg-blue-900/50 border border-blue-800/60"
    @click="handleSelect"
  >
    <img
      v-if="cardImageUrl"
      :src="cardImageUrl"
      :alt="cardName"
      class="w-12 h-16 shrink-0 object-contain rendering-pixelated"
    />
    <span class="h-[2.5em] w-full text-[8px] font-bold text-blue-200 leading-tight text-center line-clamp-2">
      {{ cardName }}
    </span>
    <span class="w-full shrink-0 text-[8px] font-bold text-amber-400 leading-tight text-center">
      {{ card.price }} {{ $t("enemy.baseBits") }}
    </span>
  </button>
</template>
