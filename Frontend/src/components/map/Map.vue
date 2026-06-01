<script setup lang="ts">
import MapLocation from "./MapLocation.vue";
import MapEnemies from "./MapEnemies.vue";
import { computed } from "vue";
import { useGameStore } from "@/stores/use-game-store";
import { MapPresenter } from "@/presenters/map/map.presenter.ts";

const store = useGameStore();

const locationId = computed(() => {
  return store.currentState?.player?.location ?? null;
});

const locationViewModel = computed(() => {
  if (!locationId.value) {
    return null;
  }
  return MapPresenter.getLocationById(locationId.value);
});
</script>

<template>
  <aside class="w-full h-full bg-[#000e3f] rounded-md shadow-lg border-2 border-[#0033aa] p-3 flex flex-col pt-0 mb-1 overflow-hidden">
    <!-- Header banner -->
    <div class="w-full flex items-center justify-center border-b border-[#0033aa]/50 bg-[#000e3f] sticky top-0 py-2 z-10">
      <h3 class="font-bold tracking-widest text-[#0077ff] text-shadow-sm uppercase text-sm">{{ $t("map.map") }}</h3>
    </div>

    <div class="flex-1 flex flex-col mt-2 h-full">
      <MapLocation :location-id="locationId" :location="locationViewModel" />
      <MapEnemies :location="locationViewModel" />
    </div>
  </aside>
</template>
