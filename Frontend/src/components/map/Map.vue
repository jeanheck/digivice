<script setup lang="ts">
import Location from "./Location.vue";
import Enemies from "./Enemies.vue";
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { useGameStore } from "@/stores/use-game-store";
import { MapPresenter } from "@/presenters/map/map.presenter.ts";

const store = useGameStore();
const { t } = useI18n();

const locationViewModel = computed(() => {
  const locationId = store.currentState?.player?.location ?? null;
  if (locationId === null) {
    return null;
  }
  const mainQuest = store.currentState?.journal?.mainQuest ?? null;
  return MapPresenter.getLocationById(locationId, mainQuest);
});

const locationName = computed(() => {
  return locationViewModel.value?.id
    ? t(`location.${locationViewModel.value.id}`)
    : t("map.unknownZone");
});
</script>

<template>
  <aside class="dw3-aside flex-1 min-h-0">
    <div class="dw3-aside-header">
      <h3 class="dw3-aside-title shadow-text truncate max-w-full px-2">
        {{ locationName }}
      </h3>
    </div>

    <div class="flex-1 min-h-0 flex flex-col mt-2 overflow-hidden">
      <Location :location="locationViewModel" />
      <Enemies :location="locationViewModel" />
    </div>
  </aside>
</template>
