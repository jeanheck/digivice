<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import Location from "./Location.vue";
import Enemies from "./Enemies.vue";
import DesertExitWest from "./DesertExitWest.vue";
import DesertExitNorth from "./DesertExitNorth.vue";
import DesertExitEast from "./DesertExitEast.vue";
import DesertExitSouth from "./DesertExitSouth.vue";
import { useGameStore } from "@/stores/use-game-store";
import { MobiusDesertMapPresenter } from "@/presenters/map/mobius-desert-map.presenter.ts";
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

const enemyIds = computed(() => {
  if (locationId.value === null) {
    return [];
  }

  const mainQuest = store.currentState?.journal?.mainQuest ?? null;

  return MobiusDesertMapPresenter.getEnemyIds(locationId.value, mainQuest);
});

const mobiusDesertCell = computed(() => {
  if (locationId.value === null) {
    return null;
  }

  return MobiusDesertMapPresenter.getCell(locationId.value, mapVariant.value);
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
      <Location :location-id="locationId" :title-override="locationTitleOverride" />
      <Enemies
        :enemy-ids="enemyIds"
        @open-enemy-modal="emit('open-enemy-modal', $event)"
      />
    </div>
  </div>
</template>
