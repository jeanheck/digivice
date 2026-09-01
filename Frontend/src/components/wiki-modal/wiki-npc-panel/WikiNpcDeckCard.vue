<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { ImageCatalog } from "@/catalogs/image.catalog";
import type { WikiNpcDeckCardViewModel } from "@/viewmodels/wiki-modal/wiki-npc-deck-card.viewmodel";

const props = defineProps<{
  card: WikiNpcDeckCardViewModel;
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
    class="flex flex-col items-center gap-1.5 w-28 px-1.5 py-2 rounded transition-colors cursor-pointer bg-blue-900/30 hover:bg-blue-900/50 border border-blue-800/60"
    @click="handleSelect"
  >
    <div class="relative w-12 h-16 shrink-0">
      <img
        v-if="cardImageUrl"
        :src="cardImageUrl"
        :alt="cardName"
        class="w-full h-full object-contain rendering-pixelated"
      />
      <div
        v-else
        class="w-full h-full flex items-center justify-center text-lg opacity-30 select-none"
      >
        ❓
      </div>
      <span
        class="absolute bottom-[-3px] left-full ml-[-6px] px-0.5 leading-none text-[10px] font-bold text-blue-300 bg-black/85 border border-cyan-800/80 rounded-sm"
      >
        ×{{ card.quantity }}
      </span>
    </div>
    <span class="w-full text-[8px] font-bold text-blue-200 leading-tight text-center line-clamp-2">
      {{ cardName }}
    </span>
  </button>
</template>
