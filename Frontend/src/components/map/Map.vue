<script setup lang="ts">
import AsukaServerMap from "./AsukaServerMap.vue";
import SeabedMap from "./SeabedMap.vue";
import MobiusDesertMap from "./MobiusDesertMap.vue";
import EnemyModal from "@/components/map/enemy-modal/EnemyModal.vue";
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

const isEnemyModalOpen = ref(false);
const selectedEnemyId = ref<string | null>(null);

const openEnemyModal = (enemyId: string) => {
  selectedEnemyId.value = enemyId;
  isEnemyModalOpen.value = true;
};

const closeEnemyModal = () => {
  isEnemyModalOpen.value = false;
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
      @open-enemy-modal="openEnemyModal"
    />
    <MobiusDesertMap
      v-else-if="mapViewModel.locationRegion === LocationRegionConstant.mobiusDesert"
      @open-enemy-modal="openEnemyModal"
    />
    <AsukaServerMap
      v-else
      @open-enemy-modal="openEnemyModal"
    />

    <EnemyModal :is-open="isEnemyModalOpen" :enemy-id="selectedEnemyId" @close="closeEnemyModal" />
  </aside>
</template>
