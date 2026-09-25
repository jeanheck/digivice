<script setup lang="ts">
import { computed, ref } from "vue";
import MobiusDesertModal from "@/components/mobius-desert-modal/MobiusDesertModal.vue";
import { MobiusDesertButtonPresenter } from "@/presenters/footer/mobius-desert-button.presenter";
import { useGameState } from "@/composables/use-game-state";

const emit = defineEmits<{
  (e: "show-tooltip", event: MouseEvent): void;
  (e: "move-tooltip", event: MouseEvent): void;
  (e: "hide-tooltip"): void;
}>();

const gameState = useGameState();
const isMobiusDesertModalOpen = ref(false);

const locationViewModel = computed(() => {
  return MobiusDesertButtonPresenter.getLocation(
    gameState.value.player.mapId,
    gameState.value.journal.mainQuest,
  );
});

const mapVariant = computed(() => {
  return gameState.value.player.mapVariant;
});

function onClick(): void {
  isMobiusDesertModalOpen.value = true;
}

function closeMobiusDesertModal(): void {
  isMobiusDesertModalOpen.value = false;
}

function onMouseEnter(event: MouseEvent): void {
  emit("show-tooltip", event);
}

function onMouseMove(event: MouseEvent): void {
  emit("move-tooltip", event);
}

function onMouseLeave(): void {
  emit("hide-tooltip");
}
</script>

<template>
  <button
    type="button"
    class="inline-flex items-center justify-center leading-none text-[1.2rem] -translate-y-px cursor-pointer hover:opacity-100 opacity-90 transition-opacity"
    @click="onClick"
    @mouseenter="onMouseEnter"
    @mousemove="onMouseMove"
    @mouseleave="onMouseLeave"
  >
    🌵
  </button>

  <MobiusDesertModal
    :is-open="isMobiusDesertModalOpen"
    :location="locationViewModel"
    :map-variant="mapVariant"
    @close="closeMobiusDesertModal"
  />
</template>
