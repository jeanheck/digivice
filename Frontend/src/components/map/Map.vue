<script setup lang="ts">
import Location from "./Location.vue";
import Enemies from "./Enemies.vue";
import Seabed from "./Seabed.vue";
import EnemyModal from "@/components/map/enemy-modal/EnemyModal.vue";
import { computed, ref } from "vue";
import { useGameStore } from "@/stores/use-game-store";
import { MapPresenter } from "@/presenters/map/map.presenter.ts";
import { ImageCatalog } from "@/catalogs/image.catalog.ts";

const store = useGameStore();

const locationViewModel = computed(() => {
  const locationId = store.currentState?.player?.location ?? null;
  if (locationId === null) {
    return null;
  }
  const mainQuest = store.currentState?.journal?.mainQuest ?? null;
  const seabedRoute = store.currentState?.player?.seabedRoute ?? 0;
  return MapPresenter.getLocationById(locationId, mainQuest, seabedRoute);
});

const locationImage = computed(() => {
  return ImageCatalog.getMapImageUrl(locationViewModel.value?.image ?? null);
});

const seabedRoute = computed(() => {
  return store.currentState?.player?.seabedRoute ?? 0;
});

const seabedRouteType = computed(() => {
  return store.currentState?.player?.seabedRouteType ?? 0;
});

const locationId = computed(() => {
  return store.currentState?.player?.location ?? null;
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
        <Enemies :location="locationViewModel" @open-enemy-modal="openEnemyModal" />
        <Seabed
          v-if="seabedRoute !== 0"
          :seabed-route="seabedRoute"
          :seabed-route-type="seabedRouteType"
          :location-id="locationId"
        />
      </div>

      <div class="flex-1 min-h-0" />
    </div>

    <EnemyModal
      :is-open="isEnemyModalOpen"
      :enemy-id="selectedEnemyId"
      @close="closeEnemyModal"
    />
  </aside>
</template>
