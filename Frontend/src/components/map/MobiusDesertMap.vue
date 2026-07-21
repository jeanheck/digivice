<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import Location from "./Location.vue";
import Enemies from "./Enemies.vue";
import DesertExitWest from "./desert/DesertExitWest.vue";
import DesertExitNorth from "./desert/DesertExitNorth.vue";
import DesertExitEast from "./desert/DesertExitEast.vue";
import DesertExitSouth from "./desert/DesertExitSouth.vue";
import { MapPresenter } from "@/presenters/map/map.presenter.ts";
import { DesertNeighborHelper } from "@/presenters/helper/desert-neighbor.helper";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

const props = defineProps<{
  location: LocationViewModel | null;
  locationId: string | null;
  mapVariant: number;
}>();

const emit = defineEmits<{
  (e: "open-enemy-modal", enemyId: string): void;
}>();

const { t } = useI18n();

const mobiusDesertCell = computed(() => {
  if (props.locationId === null) {
    return null;
  }

  return MapPresenter.getCell(props.locationId, props.mapVariant);
});

const locationTitleOverride = computed(() => {
  if (mobiusDesertCell.value === null || props.locationId === null) {
    return null;
  }

  return `${t(`location.${props.locationId}`)} (${mobiusDesertCell.value.label})`;
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
      <Location :location="location" :title-override="locationTitleOverride" />
      <Enemies :location="location" @open-enemy-modal="emit('open-enemy-modal', $event)" />
    </div>
  </div>
</template>
