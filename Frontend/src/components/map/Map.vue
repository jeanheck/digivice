<script setup lang="ts">
import Location from "./Location.vue";
import Enemies from "./Enemies.vue";
import Seabed from "./Seabed.vue";
import DesertExitWest from "./desert/DesertExitWest.vue";
import DesertExitNorth from "./desert/DesertExitNorth.vue";
import DesertExitEast from "./desert/DesertExitEast.vue";
import DesertExitSouth from "./desert/DesertExitSouth.vue";
import EnemyModal from "@/components/map/enemy-modal/EnemyModal.vue";
import { computed, ref } from "vue";
import { useI18n } from "vue-i18n";
import { useGameStore } from "@/stores/use-game-store";
import { MapPresenter } from "@/presenters/map/map.presenter.ts";
import { MobiusDesertPresenter } from "@/presenters/map/mobius-desert.presenter";
import { DesertNeighborHelper } from "@/presenters/helper/desert-neighbor.helper";
import { ImageCatalog } from "@/catalogs/image.catalog.ts";

const store = useGameStore();
const { t } = useI18n();

const locationViewModel = computed(() => {
  const locationId = store.currentState?.player?.location ?? null;
  if (locationId === null) {
    return null;
  }

  const mainQuest = store.currentState?.journal?.mainQuest ?? null;
  const seabedRoute = store.currentState?.player?.seabedRoute ?? 0;
  const previousMapId = store.currentState?.player?.previousMapId ?? "";

  return MapPresenter.getLocation(locationId, mainQuest, seabedRoute, previousMapId);
});

const locationImage = computed(() => {
  return ImageCatalog.getMapImageUrl(locationViewModel.value?.image ?? null);
});

const seabedRoute = computed(() => {
  return store.currentState?.player?.seabedRoute ?? 0;
});

const mapVariant = computed(() => {
  return store.currentState?.player?.mapVariant ?? 0;
});

const locationId = computed(() => {
  return store.currentState?.player?.location ?? null;
});

const isSeabed = computed(() => {
  return MapPresenter.isSeabedLocation(locationId.value);
});

const isMobiusDesert = computed(() => {
  return MapPresenter.isMobiusDesertLocation(locationId.value);
});

const mobiusDesertCell = computed(() => {
  if (!isMobiusDesert.value || locationId.value === null) {
    return null;
  }

  return MobiusDesertPresenter.getCell(locationId.value, mapVariant.value);
});

const locationTitleOverride = computed(() => {
  if (mobiusDesertCell.value === null || locationId.value === null) {
    return null;
  }

  return `${t(`location.${locationId.value}`)} (${mobiusDesertCell.value.label})`;
});

function resolveNeighborDisplayName(neighbor: string): string {
  const neighborName = DesertNeighborHelper.resolveNeighborName(neighbor);

  if (neighborName.kind === "i18n") {
    return t(neighborName.key);
  }

  return neighborName.value;
}

const westExitName = computed(() => {
  return mobiusDesertCell.value ? resolveNeighborDisplayName(mobiusDesertCell.value.west) : null;
});

const northExitName = computed(() => {
  return mobiusDesertCell.value ? resolveNeighborDisplayName(mobiusDesertCell.value.north) : null;
});

const eastExitName = computed(() => {
  return mobiusDesertCell.value ? resolveNeighborDisplayName(mobiusDesertCell.value.east) : null;
});

const southExitName = computed(() => {
  return mobiusDesertCell.value ? resolveNeighborDisplayName(mobiusDesertCell.value.south) : null;
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

    <template v-if="isMobiusDesert">
      <DesertExitWest :name="westExitName" />
      <DesertExitNorth :name="northExitName" />
      <DesertExitEast :name="eastExitName" />
      <DesertExitSouth :name="southExitName" />
    </template>

    <div
      class="relative z-10 flex flex-col flex-1 min-h-0 pt-1"
      :class="{ 'justify-center': isMobiusDesert }"
    >
      <div class="flex flex-col items-center gap-2 shrink-0">
        <Location :location="locationViewModel" :title-override="locationTitleOverride" />
        <Enemies :location="locationViewModel" @open-enemy-modal="openEnemyModal" />
        <Seabed
          v-if="isSeabed"
          :seabed-route="seabedRoute"
          :map-variant="mapVariant"
          :location-id="locationId"
        />
      </div>

      <div v-if="!isMobiusDesert" class="flex-1 min-h-0" />
    </div>

    <EnemyModal :is-open="isEnemyModalOpen" :enemy-id="selectedEnemyId" @close="closeEnemyModal" />
  </aside>
</template>
