<script setup lang="ts">
import AsukaServerMap from "./asuka-server-map/AsukaServerMap.vue";
import BattleMap from "./battle-map/BattleMap.vue";
import SeabedMap from "./seabed-map/SeabedMap.vue";
import MobiusDesertMap from "./mobius-desert-map/MobiusDesertMap.vue";
import WikiModal from "@/components/wiki-modal/WikiModal.vue";
import { computed, ref } from "vue";
import { LocationRegionConstant } from "@/constants/location-region.constant";
import { useGameStore } from "@/stores/use-game-store";
import { BattleMapPresenter } from "@/presenters/map/battle-map.presenter";
import { MapPresenter } from "@/presenters/map/map.presenter.ts";

const store = useGameStore();

const locationId = computed(() => {
  return store.currentState?.player?.location ?? null;
});

const mapViewModel = computed(() => {
  return MapPresenter.getByLocationId(locationId.value);
});

const isInBattle = computed(() => {
  return BattleMapPresenter.isInBattle(locationId.value);
});

const backgroundImageUrl = computed(() => {
  if (isInBattle.value) {
    return null;
  }

  return mapViewModel.value.locationImageUrl;
});

const isWikiModalOpen = ref(false);
const selectedEnemyId = ref<string | null>(null);

const openWikiModal = (enemyId: string) => {
  selectedEnemyId.value = enemyId;
  isWikiModalOpen.value = true;
};

const closeWikiModal = () => {
  isWikiModalOpen.value = false;
  selectedEnemyId.value = null;
};
</script>

<template>
  <aside class="dw3-aside flex-1 min-h-0 pt-1.5! pb-1.5! relative overflow-hidden">
    <div
      class="absolute inset-0 bg-black bg-opacity-60"
      :class="{ 'bg-grid-pattern': !backgroundImageUrl && !isInBattle }"
    />

    <div
      v-if="backgroundImageUrl"
      class="absolute inset-0 bg-cover bg-center opacity-60 mix-blend-lighten pointer-events-none"
      :style="{ backgroundImage: `url(${backgroundImageUrl})` }"
    />

    <div class="dw3-scan-corner top-left" />
    <div class="dw3-scan-corner top-right" />
    <div class="dw3-scan-corner bottom-left" />
    <div class="dw3-scan-corner bottom-right" />

    <BattleMap v-if="isInBattle" />
    <SeabedMap
      v-else-if="mapViewModel.locationRegion === LocationRegionConstant.seabed"
      @open-enemy-modal="openWikiModal"
    />
    <MobiusDesertMap
      v-else-if="mapViewModel.locationRegion === LocationRegionConstant.mobiusDesert"
      @open-enemy-modal="openWikiModal"
    />
    <AsukaServerMap v-else @open-enemy-modal="openWikiModal" />

    <WikiModal
      :is-open="isWikiModalOpen"
      :enemy-id="selectedEnemyId"
      @close="closeWikiModal"
    />
  </aside>
</template>
