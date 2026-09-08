<script setup lang="ts">
import { computed } from "vue";
import WikiCardShopInventory from "@/components/wiki-modal/wiki-card-shop-panel/WikiCardShopInventory.vue";
import WikiCardShopLocatedIn from "@/components/wiki-modal/wiki-card-shop-panel/WikiCardShopLocatedIn.vue";
import { WikiCardShopPanelPresenter } from "@/presenters/map/wiki-modal/wiki-card-shop-panel.presenter";
import { useGameStore } from "@/stores/use-game-store";

const props = defineProps<{
  cardShopId: string;
}>();

const emit = defineEmits<{
  (e: "open-card", cardId: string): void;
  (e: "open-location", locationId: string): void;
}>();

const store = useGameStore();

const mainQuest = computed(() => {
  return store.currentState?.journal?.mainQuest ?? null;
});

const cardShopViewModel = computed(() => {
  return WikiCardShopPanelPresenter.getViewModel(props.cardShopId, mainQuest.value);
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
      <WikiCardShopInventory :cards="cardShopViewModel.cards" @open-card="handleOpenCard" />
    </section>

    <WikiCardShopLocatedIn
      v-if="cardShopViewModel.locationId !== null"
      :location-id="cardShopViewModel.locationId"
      @open-location="handleOpenLocation"
    />
  </div>
</template>
