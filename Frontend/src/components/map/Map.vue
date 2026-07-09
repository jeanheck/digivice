<script setup lang="ts">
import Location from "./Location.vue";
import Enemies from "./Enemies.vue";
import { computed } from "vue";
import { useGameStore } from "@/stores/use-game-store";
import { MapPresenter } from "@/presenters/map/map.presenter.ts";

const store = useGameStore();

const locationViewModel = computed(() => {
  const locationId = store.currentState?.player?.location ?? null;
  if (locationId === null) {
    return null;
  }
  const mainQuest = store.currentState?.journal?.mainQuest ?? null;
  return MapPresenter.getLocationById(locationId, mainQuest);
});
</script>

<template>
  <aside class="dw3-aside flex-1 min-h-0 !pt-3">
    <div class="flex-1 min-h-0 flex flex-col overflow-hidden">
      <Location :location="locationViewModel" />
      <Enemies :location="locationViewModel" />
    </div>
  </aside>
</template>
