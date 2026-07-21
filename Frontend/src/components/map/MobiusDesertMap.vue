<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import Location from "./Location.vue";
import Enemies from "./Enemies.vue";
import DesertExitWest from "./desert/DesertExitWest.vue";
import DesertExitNorth from "./desert/DesertExitNorth.vue";
import DesertExitEast from "./desert/DesertExitEast.vue";
import DesertExitSouth from "./desert/DesertExitSouth.vue";
import { useGameStore } from "@/stores/use-game-store";
import { MapPresenter } from "@/presenters/map/map.presenter.ts";
import { DesertNeighborHelper } from "@/presenters/helper/desert-neighbor.helper";

const emit = defineEmits<{
  (e: "open-enemy-modal", enemyId: string): void;
}>();

const store = useGameStore();
const { t } = useI18n();

const locationId = computed(() => {
  return store.currentState?.player?.location ?? null;
});

const mapVariant = computed(() => {
  return store.currentState?.player?.mapVariant ?? 0;
});

const locationViewModel = computed(() => {
  if (locationId.value === null) {
    return null;
  }

  const mainQuest = store.currentState?.journal?.mainQuest ?? null;
  const seabedRoute = store.currentState?.player?.seabedRoute ?? 0;
  const previousMapId = store.currentState?.player?.previousMapId ?? "";

  return MapPresenter.getLocation(locationId.value, mainQuest, seabedRoute, previousMapId);
});

const mobiusDesertCell = computed(() => {
  if (locationId.value === null) {
    return null;
  }

  return MapPresenter.getCell(locationId.value, mapVariant.value);
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
</script>

<template>
  <DesertExitWest :name="westExitName" />
  <DesertExitNorth :name="northExitName" />
  <DesertExitEast :name="eastExitName" />
  <DesertExitSouth :name="southExitName" />

  <div class="relative z-10 flex flex-col flex-1 min-h-0 pt-1 justify-center">
    <div class="flex flex-col items-center gap-2 shrink-0">
      <Location :location="locationViewModel" :title-override="locationTitleOverride" />
      <Enemies :location="locationViewModel" @open-enemy-modal="emit('open-enemy-modal', $event)" />
    </div>
  </div>
</template>
