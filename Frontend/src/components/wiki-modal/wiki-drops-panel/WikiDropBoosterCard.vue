<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { ImageCatalog } from "@/catalogs/image.catalog";
import type { WikiDropBoosterCardViewModel } from "@/viewmodels/wiki-modal/wiki-drop-booster-card.viewmodel";

const props = defineProps<{
  card: WikiDropBoosterCardViewModel;
}>();

const emit = defineEmits<{
  (e: "select", cardId: string): void;
}>();

const { t } = useI18n();

const cardName = computed(() => {
  return t(props.card.nameKey);
});

const cardImageUrl = computed(() => {
  return ImageCatalog.getCardImageUrl(props.card.imageName);
});

const handleSelect = (): void => {
  emit("select", props.card.cardId);
};
</script>

<template>
  <button
    type="button"
    class="flex flex-col items-center gap-1.5 w-36 px-1.5 py-2 rounded transition-colors cursor-pointer bg-blue-900/30 hover:bg-blue-900/50 border border-blue-800/60"
    @click="handleSelect"
  >
    <img
      v-if="cardImageUrl"
      :src="cardImageUrl"
      :alt="cardName"
      class="w-12 h-16 object-contain rendering-pixelated"
    />
    <span class="w-full text-[8px] font-bold text-blue-200 leading-tight text-center line-clamp-2">
      {{ cardName }}
    </span>
  </button>
</template>
