<script setup lang="ts">
import { computed } from "vue";
import Location from "../Location.vue";
import Enemies from "@/components/map/Enemies.vue";
import Seabed from "./Seabed.vue";
import { useGameStore } from "@/stores/use-game-store";
import { SeabedMapPresenter } from "@/presenters/map/seabed-map.presenter.ts";

const emit = defineEmits<{
  (e: "open-enemy-modal", enemyId: string): void;
}>();

const store = useGameStore();

const locationId = computed(() => {
  return store.currentState?.player?.location ?? null;
});

const seabedRoute = computed(() => {
  return store.currentState?.player?.seabedRoute ?? 0;
});

const mapVariant = computed(() => {
  return store.currentState?.player?.mapVariant ?? 0;
});

const enemyIds = computed(() => {
  return SeabedMapPresenter.getEnemyIds(seabedRoute.value);
});
</script>

<template>
  <div class="relative z-10 flex flex-col flex-1 min-h-0 pt-1">
    <div class="flex flex-col items-center gap-2 shrink-0">
      <Location :location-id="locationId" />
      <Enemies
        :enemy-ids="enemyIds"
        @open-enemy-modal="emit('open-enemy-modal', $event)"
      />
      <Seabed
        :seabed-route="seabedRoute"
        :map-variant="mapVariant"
        :location-id="locationId"
      />
    </div>

    <div class="flex-1 min-h-0" />
  </div>
</template>
