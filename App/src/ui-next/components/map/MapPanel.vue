<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import MapLocationTitle from "@/ui-next/components/map/MapLocationTitle.vue";
import MapEncounters from "@/ui-next/components/map/MapEncounters.vue";
import { MapPresenter } from "@/presentation/presenters/map/map.presenter";
import { MapEnemiesPresenter } from "@/presentation/presenters/map/map-enemies.presenter";
import { MapMock } from "@/ui-next/mocks/map.mock";

const { t } = useI18n();

const mapName = computed(() => {
  return t(`map.${MapMock.mapId}`);
});

const backgroundImageUrl = computed(() => {
  return MapPresenter.getByLocationId(MapMock.mapId).locationImageUrl;
});

const enemies = computed(() => {
  return MapEnemiesPresenter.getResumedEnemiesByEncounterSources(
    MapMock.walking,
    [],
    MapMock.fishing,
    MapMock.kickingTree,
  );
});
</script>

<template>
  <div class="relative flex flex-col text-foreground dw3-beveled">
    <div class="absolute inset-0 bg-progress-frame dw3-beveled pointer-events-none"></div>
    <div class="absolute inset-[1.5px] bg-black dw3-beveled overflow-hidden pointer-events-none">
      <div
        v-if="backgroundImageUrl"
        class="absolute inset-0 bg-cover bg-center opacity-60 mix-blend-lighten"
        :style="{ backgroundImage: `url(${backgroundImageUrl})` }"
      ></div>
    </div>

    <div class="relative z-10 flex-1 px-2 py-2 flex flex-col items-center gap-2">
      <MapLocationTitle :title="mapName" />
      <MapEncounters :enemies="enemies" :npcs="MapMock.npcs" />
    </div>
  </div>
</template>
