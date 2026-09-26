<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import Location from "@/components/map/Location.vue";
import Enemies from "@/components/map/Enemies.vue";
import DesertExitWest from "./DesertExitWest.vue";
import DesertExitNorth from "./DesertExitNorth.vue";
import DesertExitEast from "./DesertExitEast.vue";
import DesertExitSouth from "./DesertExitSouth.vue";
import { useGameState } from "@/composables/use-game-state";
import { MobiusDesertMapPresenter } from "@/presenters/map/mobius-desert-map.presenter";
import { DesertNeighborHelper } from "@/presenters/helper/desert-neighbor.helper";

const emit = defineEmits<{
  (e: "open-enemy-modal", enemyId: string): void;
  (e: "open-location-wiki", locationId: string): void;
}>();

const gameState = useGameState();
const { t } = useI18n();

const locationId = computed(() => {
  return gameState.value.player.mapId;
});

const mapVariant = computed(() => {
  return gameState.value.player.mapVariant;
});

const enemyIds = computed(() => {
  return MobiusDesertMapPresenter.getEnemyIds(locationId.value, gameState.value.journal.mainQuest);
});

const isSafeZone = computed(() => {
  return enemyIds.value.length === 0;
});

const mobiusDesertArea = computed(() => {
  return MobiusDesertMapPresenter.getMobiusDesertArea(locationId.value, mapVariant.value);
});

const locationTitleOverride = computed(() => {
  if (mobiusDesertArea.value === null) {
    return null;
  }

  return `${t(`location.${locationId.value}`)} (${mobiusDesertArea.value.label})`;
});

function resolveNeighborDisplayName(neighbor: string): string {
  const neighborName = DesertNeighborHelper.resolveNeighborName(neighbor);

  if (neighborName.kind === "i18n") {
    return t(neighborName.key);
  }

  return neighborName.value;
}

const westExitName = computed(() => {
  return mobiusDesertArea.value ? resolveNeighborDisplayName(mobiusDesertArea.value.west) : null;
});

const northExitName = computed(() => {
  return mobiusDesertArea.value ? resolveNeighborDisplayName(mobiusDesertArea.value.north) : null;
});

const eastExitName = computed(() => {
  return mobiusDesertArea.value ? resolveNeighborDisplayName(mobiusDesertArea.value.east) : null;
});

const southExitName = computed(() => {
  return mobiusDesertArea.value ? resolveNeighborDisplayName(mobiusDesertArea.value.south) : null;
});
</script>

<template>
  <DesertExitWest :name="westExitName" />
  <DesertExitNorth :name="northExitName" />
  <DesertExitEast :name="eastExitName" />
  <DesertExitSouth :name="southExitName" />

  <div class="relative z-10 flex flex-col flex-1 min-h-0 pt-1 justify-center">
    <div class="flex flex-col items-center gap-2 shrink-0">
      <Location
        :location-id="locationId"
        :title-override="locationTitleOverride"
        :is-safe-zone="isSafeZone"
        @open-location-wiki="emit('open-location-wiki', $event)"
      />
      <Enemies :enemy-ids="enemyIds" @open-enemy-modal="emit('open-enemy-modal', $event)" />
    </div>
  </div>
</template>
