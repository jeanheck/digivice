<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import MapEnemyGroup from "@/ui-next/components/map/MapEnemyGroup.vue";
import { MapPresenter } from "@/presentation/presenters/map/map.presenter";
import { MapEnemiesPresenter } from "@/presentation/presenters/map/map-enemies.presenter";
import { MapMock } from "@/ui-next/mocks/map.mock";

// Code points without U+FE0F, otherwise WebView2 falls back to the colored Segoe UI Emoji.
const WalkingIcon = "\u{1F3C3}";
const FishingIcon = "\u{1F3A3}";
const KickingTreeIcon = "\u{1F334}";

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

const enemyGroups = computed(() => {
  const groups = [
    { icon: WalkingIcon, enemies: enemies.value.filter((enemy) => enemy.walking) },
    { icon: FishingIcon, enemies: enemies.value.filter((enemy) => enemy.fishing) },
    { icon: KickingTreeIcon, enemies: enemies.value.filter((enemy) => enemy.kickingTree) },
  ];

  return groups.filter((group) => group.enemies.length > 0);
});
</script>

<template>
  <div class="relative flex flex-col text-white dw3-beveled">
    <div class="absolute inset-0 bg-progress-frame dw3-beveled pointer-events-none"></div>
    <div class="absolute inset-[1.5px] bg-black dw3-beveled overflow-hidden pointer-events-none">
      <div
        v-if="backgroundImageUrl"
        class="absolute inset-0 bg-cover bg-center opacity-60 mix-blend-lighten"
        :style="{ backgroundImage: `url(${backgroundImageUrl})` }"
      ></div>
      <div
        class="absolute inset-x-0 top-0 h-24 bg-linear-to-b from-black/85 via-black/50 to-transparent"
      ></div>
    </div>

    <div class="relative z-10 flex-1 px-2 py-2 flex flex-col gap-2">
      <h4
        class="self-center text-[12px] leading-none uppercase tracking-widest text-gray-200 drop-shadow-[0_0_5px_rgba(0,170,255,0.8)]"
      >
        {{ mapName }}
      </h4>
      <div class="mt-auto flex flex-col items-start gap-1.5">
        <MapEnemyGroup
          v-for="group in enemyGroups"
          :key="group.icon"
          :icon="group.icon"
          :enemies="group.enemies"
        />
      </div>
    </div>
  </div>
</template>
