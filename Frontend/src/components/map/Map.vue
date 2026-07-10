<script setup lang="ts">
import Location from "./Location.vue";
import Enemies from "./Enemies.vue";
import Seabed from "./Seabed.vue";
import DocksButton from "./DocksButton.vue";
import BoxesButton from "./BoxesButton.vue";
import MapModal from "@/components/map/map-modal/MapModal.vue";
import { computed, ref } from "vue";
import { useGameStore } from "@/stores/use-game-store";
import { MapPresenter } from "@/presenters/map/map.presenter.ts";
import { ImageCatalog } from "@/catalogs/image.catalog.ts";

type MapModalSection = "enemies" | "docks" | "boxes";

const store = useGameStore();

const locationViewModel = computed(() => {
  const locationId = store.currentState?.player?.location ?? null;
  if (locationId === null) {
    return null;
  }
  const mainQuest = store.currentState?.journal?.mainQuest ?? null;
  return MapPresenter.getLocationById(locationId, mainQuest);
});

const locationImage = computed(() => {
  return ImageCatalog.getMapImageUrl(locationViewModel.value?.image ?? null);
});

const isMapModalOpen = ref(false);
const initialEnemyId = ref<string | null>(null);
const initialSection = ref<MapModalSection>("enemies");

const openMapModal = (enemyId: string) => {
  initialSection.value = "enemies";
  initialEnemyId.value = enemyId;
  isMapModalOpen.value = true;
};

const closeMapModal = () => {
  isMapModalOpen.value = false;
  initialEnemyId.value = null;
  initialSection.value = "enemies";
};

function onDocksClick(): void {
  initialSection.value = "docks";
  initialEnemyId.value = null;
  isMapModalOpen.value = true;
}

function onBoxesClick(): void {
}
</script>

<template>
  <aside class="dw3-aside flex-1 min-h-0 pt-1.5! pb-1.5! relative overflow-hidden">
    <div
      class="absolute inset-0 bg-black bg-opacity-60"
      :class="{ 'bg-grid-pattern': !locationImage }"
    />

    <div
      v-if="locationImage"
      class="absolute inset-0 bg-cover bg-center opacity-60 mix-blend-lighten pointer-events-none"
      :style="{ backgroundImage: `url(${locationImage})` }"
    />

    <div class="absolute top-1 left-1 w-3 h-3 border-t-2 border-l-2 border-[#00aaff]/60 pointer-events-none" />
    <div class="absolute top-1 right-1 w-3 h-3 border-t-2 border-r-2 border-[#00aaff]/60 pointer-events-none" />
    <div class="absolute bottom-1 left-1 w-3 h-3 border-b-2 border-l-2 border-[#00aaff]/60 pointer-events-none" />
    <div class="absolute bottom-1 right-1 w-3 h-3 border-b-2 border-r-2 border-[#00aaff]/60 pointer-events-none" />

    <div class="relative z-10 flex flex-col flex-1 min-h-0 pt-1">
      <div class="flex flex-col items-center gap-2 shrink-0">
        <Location :location="locationViewModel" />
        <Enemies :location="locationViewModel" @open-map-modal="openMapModal" />
        <!--<Seabed />-->
      </div>

      <div class="flex-1 min-h-0" />

      <div class="w-full flex justify-center shrink-0 px-0.5 pb-1">
        <div class="flex gap-1 w-full max-w-[95%]">
          <DocksButton :dock="locationViewModel?.dock ?? false" @click="onDocksClick" />
          <BoxesButton @click="onBoxesClick" />
        </div>
      </div>
    </div>

    <MapModal
      :is-open="isMapModalOpen"
      :location="locationViewModel"
      :initial-enemy-id="initialEnemyId"
      :initial-section="initialSection"
      @close="closeMapModal"
    />
  </aside>
</template>
