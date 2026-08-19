<script setup lang="ts">
import { computed } from "vue";
import WikiCardBoosters from "@/components/wiki-modal/wiki-cards-panel/WikiCardBoosters.vue";
import WikiCardDetails from "@/components/wiki-modal/wiki-cards-panel/WikiCardDetails.vue";
import { WikiCardsPanelPresenter } from "@/presenters/map/wiki-modal/wiki-cards-panel.presenter";

const props = defineProps<{
  cardId: string;
}>();

const emit = defineEmits<{
  (e: "open-drop", dropKey: string): void;
}>();

const cardsViewModel = computed(() => {
  return WikiCardsPanelPresenter.getViewModel(props.cardId);
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
    <WikiCardBoosters :sources="cardsViewModel.sources" @open-drop="handleOpenDrop" />
  </div>
</template>
