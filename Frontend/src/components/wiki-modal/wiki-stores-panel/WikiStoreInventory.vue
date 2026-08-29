<script setup lang="ts">
import WikiStoreCard from "@/components/wiki-modal/wiki-stores-panel/WikiStoreCard.vue";
import type { WikiStoreCardViewModel } from "@/viewmodels/wiki-modal/wiki-store-card.viewmodel";

defineProps<{
  cards: WikiStoreCardViewModel[];
}>();

const emit = defineEmits<{
  (e: "open-card", cardId: string): void;
}>();

const handleSelect = (cardId: string): void => {
  emit("open-card", cardId);
};
</script>

<template>
  <div class="flex flex-col flex-1 min-h-0 overflow-hidden text-xs text-center gap-4">
    <p class="bg-[#002266]/40 px-2 py-1 rounded-sm text-blue-500 uppercase font-bold shrink-0">
      {{ $t("enemy.storeCardsLabel") }}
    </p>

    <div class="flex-1 min-h-0 overflow-y-auto overflow-x-hidden custom-scroll">
      <div class="flex flex-wrap content-start justify-center gap-2 w-full">
        <WikiStoreCard
          v-for="card in cards"
          :key="card.cardId"
          :card="card"
          @select="handleSelect"
        />
      </div>
    </div>
  </div>
</template>
