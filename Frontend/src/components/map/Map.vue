<script setup lang="ts">
import AsukaServerMap from "./asuka-server-map/AsukaServerMap.vue";
import BattleMap from "./battle-map/BattleMap.vue";
import CardBattleMap from "./card-battle-map/CardBattleMap.vue";
import SeabedMap from "./seabed-map/SeabedMap.vue";
import MobiusDesertMap from "./mobius-desert-map/MobiusDesertMap.vue";
import WikiModal from "@/components/wiki-modal/WikiModal.vue";
import { computed, ref } from "vue";
import { LocationRegionConstant } from "@/constants/location-region.constant";
import { useGameStore } from "@/stores/use-game-store";
import { BattleMapPresenter } from "@/presenters/map/battle-map.presenter";
import { CardBattleMapPresenter } from "@/presenters/map/card-battle-map.presenter";
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

const isInCardBattle = computed(() => {
  return CardBattleMapPresenter.isInCardBattle(locationId.value);
});

const backgroundImageUrl = computed(() => {
  if (isInBattle.value || isInCardBattle.value) {
    return null;
  }

  return mapViewModel.value.locationImageUrl;
});

const isWikiModalOpen = ref(false);
const selectedEnemyId = ref<string | null>(null);
const selectedWikiLocationId = ref<string | null>(null);
const selectedNpcId = ref<string | null>(null);

const openWikiModal = (enemyId: string) => {
  selectedWikiLocationId.value = null;
  selectedNpcId.value = null;
  selectedEnemyId.value = enemyId;
  isWikiModalOpen.value = true;
};

const openWikiModalForLocation = (locationId: string) => {
  selectedEnemyId.value = null;
  selectedNpcId.value = null;
  selectedWikiLocationId.value = locationId;
  isWikiModalOpen.value = true;
};

const openWikiModalForNpc = (npcId: string) => {
  selectedEnemyId.value = null;
  selectedWikiLocationId.value = null;
  selectedNpcId.value = npcId;
  isWikiModalOpen.value = true;
};

const closeWikiModal = () => {
  isWikiModalOpen.value = false;
  selectedEnemyId.value = null;
  selectedWikiLocationId.value = null;
  selectedNpcId.value = null;
};
</script>

<template>
  <aside class="dw3-aside flex-1 min-h-0 pt-1.5! pb-1.5! relative overflow-hidden">
    <div
      class="absolute inset-0 bg-black bg-opacity-60"
      :class="{ 'bg-grid-pattern': !backgroundImageUrl && !isInBattle && !isInCardBattle }"
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

    <BattleMap v-if="isInBattle" @open-enemy-modal="openWikiModal" />
    <CardBattleMap
      v-else-if="isInCardBattle"
      @open-npc-modal="openWikiModalForNpc"
    />
    <SeabedMap
      v-else-if="mapViewModel.locationRegion === LocationRegionConstant.seabed"
      @open-enemy-modal="openWikiModal"
      @open-location-wiki="openWikiModalForLocation"
    />
    <MobiusDesertMap
      v-else-if="mapViewModel.locationRegion === LocationRegionConstant.mobiusDesert"
      @open-enemy-modal="openWikiModal"
      @open-location-wiki="openWikiModalForLocation"
    />
    <AsukaServerMap
      v-else
      @open-enemy-modal="openWikiModal"
      @open-location-wiki="openWikiModalForLocation"
      @open-npc-modal="openWikiModalForNpc"
    />

    <WikiModal
      :is-open="isWikiModalOpen"
      :enemy-id="selectedEnemyId"
      :location-id="selectedWikiLocationId"
      :npc-id="selectedNpcId"
      @close="closeWikiModal"
    />
  </aside>
</template>
