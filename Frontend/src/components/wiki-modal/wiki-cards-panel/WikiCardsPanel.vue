<script setup lang="ts">
import { computed } from "vue";
import WikiCardBoosters from "@/components/wiki-modal/wiki-cards-panel/WikiCardBoosters.vue";
import WikiCardDetails from "@/components/wiki-modal/wiki-cards-panel/WikiCardDetails.vue";
import WikiCardStores from "@/components/wiki-modal/wiki-cards-panel/WikiCardStores.vue";
import { WikiCardsPanelPresenter } from "@/presenters/map/wiki-modal/wiki-cards-panel.presenter";
import { useGameStore } from "@/stores/use-game-store";

const props = defineProps<{
  cardId: string;
}>();

const emit = defineEmits<{
  (e: "open-drop", dropKey: string): void;
}>();

const store = useGameStore();

const mainQuest = computed(() => {
  return store.currentState?.journal?.mainQuest ?? null;
});

const cardsViewModel = computed(() => {
  return WikiCardsPanelPresenter.getViewModel(props.cardId, mainQuest.value);
});

const handleOpenDrop = (dropKey: string): void => {
  emit("open-drop", dropKey);
};
</script>

<template>
  <div class="p-4 flex flex-col gap-4 h-full min-h-0 overflow-hidden">
    <WikiCardDetails
      v-if="cardsViewModel.card !== null"
      :card="cardsViewModel.card"
    />
    <div class="flex gap-4 shrink-0 w-full">
      <WikiCardBoosters :sources="cardsViewModel.sources" @open-drop="handleOpenDrop" />
      <WikiCardStores :stores="cardsViewModel.stores" />
    </div>
  </div>
</template>
