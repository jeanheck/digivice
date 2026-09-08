<script setup lang="ts">
import { computed } from "vue";
import WikiCardBoosters from "@/components/wiki-modal/wiki-card-panel/WikiCardBoosters.vue";
import WikiCardDetails from "@/components/wiki-modal/wiki-card-panel/WikiCardDetails.vue";
import WikiCardShops from "@/components/wiki-modal/wiki-card-panel/WikiCardShops.vue";
import { WikiCardPanelPresenter } from "@/presenters/map/wiki-modal/wiki-card-panel.presenter";
import { useGameStore } from "@/stores/use-game-store";
import type { DropType } from "@/repositories/tables/raws/drop/drop-type";

const props = defineProps<{
  cardId: string;
}>();

const emit = defineEmits<{
  (e: "open-drop", payload: { dropId: string; dropType: DropType }): void;
  (e: "open-store", storeId: string): void;
}>();

const store = useGameStore();

const mainQuest = computed(() => {
  return store.currentState?.journal?.mainQuest ?? null;
});

const viewModel = computed(() => {
  return WikiCardPanelPresenter.getViewModel(props.cardId);
});

const handleOpenDrop = (dropKey: string): void => {
  emit("open-drop", { dropId: dropKey, dropType: "booster" });
};

const handleOpenStore = (storeId: string): void => {
  emit("open-store", storeId);
};
</script>

<template>
  <div class="p-4 flex flex-col gap-4 h-full min-h-0 overflow-hidden">
    <WikiCardDetails
      v-if="viewModel !== null"
      :card="viewModel.card"
    />
    <div
      v-if="viewModel !== null"
      class="flex gap-4 shrink-0 w-full"
    >
      <WikiCardBoosters
        :booster-ids="viewModel.boosters"
        @open-drop="handleOpenDrop"
      />
      <WikiCardShops
        :card-shops="viewModel.cardShops"
        :main-quest="mainQuest"
        @open-store="handleOpenStore"
      />
    </div>
  </div>
</template>
