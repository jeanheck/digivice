<script setup lang="ts">
import { computed } from "vue";
import Location from "@/components/map/Location.vue";
import Enemies from "@/components/map/Enemies.vue";
import Seabed from "./Seabed.vue";
import { useGameState } from "@/composables/use-game-state";
import { SeabedMapPresenter } from "@/presenters/map/seabed-map.presenter.ts";

const emit = defineEmits<{
  (e: "open-enemy-modal", enemyId: string): void;
  (e: "open-location-wiki", locationId: string): void;
}>();

const gameState = useGameState();

const locationId = computed(() => {
  return gameState.value.player.mapId;
});

const seabedRoute = computed(() => {
  return gameState.value.player.seabedRoute;
});

const mapVariant = computed(() => {
  return gameState.value.player.mapVariant;
});

const enemyIds = computed(() => {
  return SeabedMapPresenter.getEnemyIds(seabedRoute.value);
});

const isSafeZone = computed(() => {
  return enemyIds.value.length === 0;
});
</script>

<template>
  <div class="relative z-10 flex flex-col flex-1 min-h-0 pt-1">
    <div class="flex flex-col items-center gap-2 shrink-0">
      <Location
        :location-id="locationId"
        :is-safe-zone="isSafeZone"
        @open-location-wiki="emit('open-location-wiki', $event)"
      />
      <Enemies :enemy-ids="enemyIds" @open-enemy-modal="emit('open-enemy-modal', $event)" />
      <Seabed :seabed-route="seabedRoute" :map-variant="mapVariant" :location-id="locationId" />
    </div>

    <div class="flex-1 min-h-0" />
  </div>
</template>
