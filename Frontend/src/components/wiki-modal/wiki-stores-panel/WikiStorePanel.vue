<script setup lang="ts">
import { computed } from "vue";
import WikiStoreInventory from "@/components/wiki-modal/wiki-stores-panel/WikiStoreInventory.vue";
import WikiStoreLocatedIn from "@/components/wiki-modal/wiki-stores-panel/WikiStoreLocatedIn.vue";
import { WikiStorePanelPresenter } from "@/presenters/map/wiki-modal/wiki-store-panel.presenter";
import { useGameStore } from "@/stores/use-game-store";

const props = defineProps<{
  storeId: string;
}>();

const emit = defineEmits<{
  (e: "open-card", cardId: string): void;
  (e: "open-location", locationId: string): void;
}>();

const store = useGameStore();

const mainQuest = computed(() => {
  return store.currentState?.journal?.mainQuest ?? null;
});

const storeViewModel = computed(() => {
  return WikiStorePanelPresenter.getViewModel(props.storeId, mainQuest.value);
});

const handleOpenCard = (cardId: string): void => {
  emit("open-card", cardId);
};

const handleOpenLocation = (locationId: string): void => {
  emit("open-location", locationId);
};
</script>

<template>
  <div class="p-4 flex flex-col gap-4 h-full min-h-0 overflow-hidden">
    <section
      class="flex-1 w-full min-h-0 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-col"
    >
      <WikiStoreInventory :cards="storeViewModel.cards" @open-card="handleOpenCard" />
    </section>

    <WikiStoreLocatedIn
      v-if="storeViewModel.locationId !== null"
      :location-id="storeViewModel.locationId"
      @open-location="handleOpenLocation"
    />
  </div>
</template>
