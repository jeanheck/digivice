<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import MapFrame from "@/ui-next/components/map/MapFrame.vue";
import MapTitle from "@/ui-next/components/map/MapTitle.vue";
import MapEnemyGroup from "@/ui-next/components/map/MapEnemyGroup.vue";
import MobiusDesertExit from "@/ui-next/components/map/mobius-desert/MobiusDesertExit.vue";
import { MapPresenter } from "@/presentation/presenters/map/map.presenter";
import { MapEnemiesPresenter } from "@/presentation/presenters/map/map-enemies.presenter";
import { MobiusDesertMapPresenter } from "@/presentation/presenters/map/mobius-desert-map.presenter";
import { DesertNeighborHelper } from "@/presentation/presenters/helper/desert-neighbor.helper";
import { MobiusDesertMapMock } from "@/ui-next/mocks/map.mock";

// Code points without U+FE0F, otherwise WebView2 falls back to the colored Segoe UI Emoji.
const WalkingIcon = "\u{1F3C3}";
const WestIcon = "\u{2196}";
const NorthIcon = "\u{2197}";
const EastIcon = "\u{2198}";
const SouthIcon = "\u{2199}";

const SafeZoneToneClass = "text-sky-300 drop-shadow-[0_0_5px_rgba(0,224,248,0.8)]";

const { t } = useI18n();

const backgroundImageUrl = computed(() => {
  return MapPresenter.getByLocationId(MobiusDesertMapMock.mapId).locationImageUrl;
});

const mobiusDesertArea = computed(() => {
  return MobiusDesertMapPresenter.getMobiusDesertArea(
    MobiusDesertMapMock.mapId,
    MobiusDesertMapMock.mapVariant,
  );
});

const mapName = computed(() => {
  if (mobiusDesertArea.value === null) {
    return t(`map.${MobiusDesertMapMock.mapId}`);
  }

  return mobiusDesertArea.value.label;
});

const enemies = computed(() => {
  return MapEnemiesPresenter.getResumedEnemiesByIds(MobiusDesertMapMock.walking);
});

const isSafeZone = computed(() => {
  return enemies.value.length === 0;
});

function resolveNeighborName(neighbor: string): string {
  const neighborName = DesertNeighborHelper.resolveNeighborName(neighbor);
  if (neighborName.kind === "i18n") {
    return t(neighborName.key);
  }

  return neighborName.value;
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

    <div class="flex-1 px-2 py-2 flex flex-col gap-2">
      <div v-if="mobiusDesertArea" class="flex items-start justify-between gap-2">
        <MobiusDesertExit :icon="WestIcon" :name="resolveNeighborName(mobiusDesertArea.west)" />
        <MobiusDesertExit
          :icon="NorthIcon"
          :name="resolveNeighborName(mobiusDesertArea.north)"
          is-right-aligned
        />
      </div>

      <div class="flex-1 flex flex-col items-center justify-center gap-2">
        <MapTitle :title="mapName" :tone-class="isSafeZone ? SafeZoneToneClass : undefined" />
        <MapEnemyGroup v-if="enemies.length > 0" :icon="WalkingIcon" :enemies="enemies" />
      </div>

      <div v-if="mobiusDesertArea" class="flex items-end justify-between gap-2">
        <MobiusDesertExit :icon="SouthIcon" :name="resolveNeighborName(mobiusDesertArea.south)" />
        <MobiusDesertExit
          :icon="EastIcon"
          :name="resolveNeighborName(mobiusDesertArea.east)"
          is-right-aligned
        />
      </div>
    </div>
  </MapFrame>
</template>
