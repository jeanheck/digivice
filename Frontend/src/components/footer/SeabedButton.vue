<script setup lang="ts">
import { computed, ref } from "vue";
import SeabedModal from "@/components/seabed-modal/SeabedModal.vue";
import { SeabedButtonPresenter } from "@/presenters/footer/seabed-button.presenter";
import { useGameStore } from "@/stores/use-game-store";

const emit = defineEmits<{
  (e: "show-tooltip", event: MouseEvent): void;
  (e: "move-tooltip", event: MouseEvent): void;
  (e: "hide-tooltip"): void;
}>();

const store = useGameStore();
const isSeabedModalOpen = ref(false);

const locationViewModel = computed(() => {
  const locationId = store.currentState?.player?.location ?? null;
  if (locationId === null) {
    return null;
  }

  const mainQuest = store.currentState?.journal?.mainQuest ?? null;
  const seabedRoute = store.currentState?.player?.seabedRoute ?? 0;

  return SeabedButtonPresenter.getLocation(locationId, mainQuest, seabedRoute);
});

function onClick(): void {
  isSeabedModalOpen.value = true;
}

function closeSeabedModal(): void {
  isSeabedModalOpen.value = false;
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
    ⚓
  </button>

  <SeabedModal
    :is-open="isSeabedModalOpen"
    :location="locationViewModel"
    @close="closeSeabedModal"
  />
</template>
