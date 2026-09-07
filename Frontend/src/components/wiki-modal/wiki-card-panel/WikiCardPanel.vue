<script setup lang="ts">
import { computed } from "vue";
import WikiCardBoosters from "@/components/wiki-modal/wiki-card-panel/WikiCardBoosters.vue";
import WikiCardDetails from "@/components/wiki-modal/wiki-card-panel/WikiCardDetails.vue";
import WikiCardStores from "@/components/wiki-modal/wiki-card-panel/WikiCardStores.vue";
import { WikiCardPanelPresenter } from "@/presenters/map/wiki-modal/wiki-card-panel.presenter";
import { useGameStore } from "@/stores/use-game-store";

const props = defineProps<{
  cardId: string;
}>();

const emit = defineEmits<{
  (e: "open-drop", dropKey: string): void;
  (e: "open-store", storeId: string): void;
}>();

const store = useGameStore();

const mainQuest = computed(() => {
  return store.currentState?.journal?.mainQuest ?? null;
});

const wikiCardPanelViewModel = computed(() => {
  return WikiCardPanelPresenter.getViewModel(props.cardId, mainQuest.value);
});

const handleOpenDrop = (dropKey: string): void => {
  emit("open-drop", dropKey);
};

const handleOpenStore = (storeId: string): void => {
  emit("open-store", storeId);
};
</script>

<template>
  <div class="p-4 flex flex-col gap-4 h-full min-h-0 overflow-hidden">
    <WikiCardDetails
      v-if="wikiCardPanelViewModel.card !== null"
      :card="wikiCardPanelViewModel.card"
    />
    <div class="flex gap-4 shrink-0 w-full">
      <WikiCardBoosters :boosters="wikiCardPanelViewModel.boosters" @open-drop="handleOpenDrop" />
      <WikiCardStores :stores="wikiCardPanelViewModel.stores" @open-store="handleOpenStore" />
    </div>
  </div>
</template>
