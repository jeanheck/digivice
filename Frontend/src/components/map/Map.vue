<script setup lang="ts">
import AsukaServerMap from "./asuka-server-map/AsukaServerMap.vue";
import SeabedMap from "./seabed-map/SeabedMap.vue";
import MobiusDesertMap from "./mobius-desert-map/MobiusDesertMap.vue";
import BestiaryModal from "@/components/map/bestiary-modal/BestiaryModal.vue";
import { computed, ref } from "vue";
import { LocationRegionConstant } from "@/constants/location-region.constant";
import { useGameStore } from "@/stores/use-game-store";
import { MapPresenter } from "@/presenters/map/map.presenter.ts";

const store = useGameStore();

const locationId = computed(() => {
  return store.currentState?.player?.location ?? null;
});

const mapViewModel = computed(() => {
  return MapPresenter.getByLocationId(locationId.value);
});

const isBestiaryModalOpen = ref(false);
const selectedEnemyId = ref<string | null>(null);

const openBestiaryModal = (enemyId: string) => {
  selectedEnemyId.value = enemyId;
  isBestiaryModalOpen.value = true;
};

const closeBestiaryModal = () => {
  isBestiaryModalOpen.value = false;
  selectedEnemyId.value = null;
};
</script>

<template>
  <aside class="dw3-aside flex-1 min-h-0 pt-1.5! pb-1.5! relative overflow-hidden">
    <div
      class="absolute inset-0 bg-black bg-opacity-60"
      :class="{ 'bg-grid-pattern': !mapViewModel.locationImageUrl }"
    />

    <div
      v-if="mapViewModel.locationImageUrl"
      class="absolute inset-0 bg-cover bg-center opacity-60 mix-blend-lighten pointer-events-none"
      :style="{ backgroundImage: `url(${mapViewModel.locationImageUrl})` }"
    />

    <div
      class="absolute top-1 left-1 w-3 h-3 border-t-2 border-l-2 border-[#00aaff]/60 pointer-events-none"
    />
    <div
      class="absolute top-1 right-1 w-3 h-3 border-t-2 border-r-2 border-[#00aaff]/60 pointer-events-none"
    />
    <div
      class="absolute bottom-1 left-1 w-3 h-3 border-b-2 border-l-2 border-[#00aaff]/60 pointer-events-none"
    />
    <div
      class="absolute bottom-1 right-1 w-3 h-3 border-b-2 border-r-2 border-[#00aaff]/60 pointer-events-none"
    />

    <SeabedMap
      v-if="mapViewModel.locationRegion === LocationRegionConstant.seabed"
      @open-enemy-modal="openBestiaryModal"
    />
    <MobiusDesertMap
      v-else-if="mapViewModel.locationRegion === LocationRegionConstant.mobiusDesert"
      @open-enemy-modal="openBestiaryModal"
    />
    <AsukaServerMap
      v-else
      @open-enemy-modal="openBestiaryModal"
    />

    <BestiaryModal :is-open="isBestiaryModalOpen" :enemy-id="selectedEnemyId" @close="closeBestiaryModal" />
  </aside>
</template>
