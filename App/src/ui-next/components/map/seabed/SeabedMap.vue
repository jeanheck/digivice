<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import MapFrame from "@/ui-next/components/map/MapFrame.vue";
import MapLocationTitle from "@/ui-next/components/map/MapLocationTitle.vue";
import MapEncounters from "@/ui-next/components/map/MapEncounters.vue";
import { MapPresenter } from "@/presentation/presenters/map/map.presenter";
import { MapEnemiesPresenter } from "@/presentation/presenters/map/map-enemies.presenter";
import { SeabedMapPresenter } from "@/presentation/presenters/map/seabed-map.presenter";
import { SeabedPresenter } from "@/presentation/presenters/map/seabed.presenter";
import type { SeabedDirection } from "@/shared/types/seabed-direction.type";
import type { SeabedMapMock } from "@/ui-next/mocks/map.mock";

const { mock } = defineProps<{
  mock: SeabedMapMock;
}>();

// Code points without U+FE0F, otherwise WebView2 falls back to the colored Segoe UI Emoji.
const LeftIcon = "\u{2B05}";
const RightIcon = "\u{27A1}";
const EmergeIcons = {
  topLeft: "\u{2196}",
  top: "\u{2B06}",
  topRight: "\u{2197}",
} as const satisfies Record<SeabedDirection, string>;

const { t } = useI18n();

const mapName = computed(() => {
  return t(`map.${mock.mapId}`);
});

const backgroundImageUrl = computed(() => {
  return MapPresenter.getByLocationId(mock.mapId).locationImageUrl;
});

const routeLocation = computed(() => {
  if (mock.directions) {
    return mock.directions;
  }

  return SeabedPresenter.getRouteLocation(mock.seabedRoute, mock.mapId, mock.mapVariant);
});

const emerge = computed(() => {
  return routeLocation.value?.emerge ?? [];
});

const leftLocations = computed(() => {
  return (routeLocation.value?.left ?? []).filter(isOutsideSeabed);
});

const rightLocations = computed(() => {
  return (routeLocation.value?.right ?? []).filter(isOutsideSeabed);
});

const enemies = computed(() => {
  return MapEnemiesPresenter.getResumedEnemiesByIds(
    SeabedMapPresenter.getEnemyIds(mock.seabedRoute),
  );
});

function isOutsideSeabed(locationId: string): boolean {
  return MapPresenter.getByLocationId(locationId).locationRegion !== "seabed";
}

function getLocationName(locationId: string): string {
  return t(`map.${locationId}`);
}
</script>

<template>
  <MapFrame>
    <template #background>
      <div
        v-if="backgroundImageUrl"
        class="absolute inset-0 bg-cover bg-center opacity-60 mix-blend-lighten"
        :style="{ backgroundImage: `url(${backgroundImageUrl})` }"
      ></div>
    </template>

    <div class="flex-1 px-2 py-2 flex flex-col items-center gap-2">
      <MapLocationTitle :title="mapName" />
      <MapEncounters :enemies="enemies" />

      <div class="w-full flex-1 flex items-center justify-center">
        <div class="map-info-panel border-progress-frame! flex flex-col gap-4 text-[12px] font-bold leading-tight">
          <div v-if="emerge.length > 0" class="flex items-start justify-center gap-3">
            <div
              v-for="(point, index) in emerge"
              :key="`${point.location}-${index}`"
              class="flex flex-col items-center gap-1 min-w-0"
            >
              <span class="text-[16px] leading-none font-emoji text-foreground">
                {{ EmergeIcons[point.on] }}
              </span>
              <span class="text-center text-highlight shadow-text">
                {{ getLocationName(point.location) }}
              </span>
            </div>
          </div>

          <div class="flex items-center gap-2">
            <div v-if="leftLocations.length > 0" class="flex items-center gap-1.5 min-w-0">
              <span class="shrink-0 text-[16px] leading-none font-emoji text-foreground">
                {{ LeftIcon }}
              </span>
              <div class="flex flex-col gap-1 text-highlight shadow-text">
                <span v-for="locationId in leftLocations" :key="locationId">
                  {{ getLocationName(locationId) }}
                </span>
              </div>
            </div>

            <div v-if="rightLocations.length > 0" class="ml-auto flex items-center gap-1.5 min-w-0">
              <div class="flex flex-col gap-1 text-right text-highlight shadow-text">
                <span v-for="locationId in rightLocations" :key="locationId">
                  {{ getLocationName(locationId) }}
                </span>
              </div>
              <span class="shrink-0 text-[16px] leading-none font-emoji text-foreground">
                {{ RightIcon }}
              </span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </MapFrame>
</template>
