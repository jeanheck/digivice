<script setup lang="ts">
import { computed } from "vue";
import WikiDropBoosterCard from "@/components/wiki-modal/wiki-drops-panel/WikiDropBoosterCard.vue";
import { WikiDropBoosterPresenter } from "@/presenters/map/wiki-modal/wiki-drop-booster.presenter";

const props = defineProps<{
  boosterId: number;
}>();

const emit = defineEmits<{
  (e: "open-card", cardId: string): void;
}>();

const cards = computed(() => {
  return WikiDropBoosterPresenter.getViewModel(props.boosterId);
});

const handleSelect = (cardId: string): void => {
  emit("open-card", cardId);
};
</script>

<template>
  <div class="flex flex-col flex-1 min-h-0 overflow-hidden text-xs text-center gap-4">
    <p class="bg-[#002266]/40 px-2 py-1 rounded-sm text-blue-500 uppercase font-bold shrink-0">
      {{ $t("enemy.boosterCardsLabel") }}
    </p>

    <div class="flex-1 min-h-0 overflow-y-auto overflow-x-hidden custom-scroll">
      <div class="flex flex-wrap content-start justify-center gap-2 w-full">
        <WikiDropBoosterCard
          v-for="card in cards"
          :key="card.cardId"
          :card="card"
          @select="handleSelect"
        />
      </div>
    </div>
  </div>
</template>
