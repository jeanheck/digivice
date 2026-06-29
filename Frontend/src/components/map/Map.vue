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
  <aside class="dw3-aside">
    <div class="dw3-aside-header">
      <h3 class="dw3-aside-title shadow-text">{{ $t("map.map") }}</h3>
    </div>

    <div class="flex-1 flex flex-col mt-2 h-full">
      <Location :location="locationViewModel" />
      <Enemies :location="locationViewModel" />
    </div>
  </aside>
</template>
