<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { MapPresenter } from "@/presentation/presenters/map/map.presenter";
import { MapEnemiesPresenter } from "@/presentation/presenters/map/map-enemies.presenter";
import type { EnemyResumedViewModel } from "@/presentation/viewmodels/enemy/enemy-resumed.viewmodel";
import { MapMock } from "@/ui-next/mocks/map.mock";

const EncounterSourceColors = [
  { color: "oklch(92.4% 0.12 95.746)", matches: (enemy: EnemyResumedViewModel) => enemy.walking },
  { color: "oklch(88.2% 0.059 254.128)", matches: (enemy: EnemyResumedViewModel) => enemy.fishing },
  { color: "oklch(92.5% 0.084 155.995)", matches: (enemy: EnemyResumedViewModel) => enemy.kickingTree },
];

// Code point without U+FE0F, otherwise WebView2 falls back to the colored Segoe UI Emoji.
const TamerIcon = "\u{2694}";

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

// An enemy found in more than one source is listed once, with the color of the first source.
const enemyNames = computed(() => {
  const listedIds = new Set<string>();
  const names: { id: string; name: string; color: string }[] = [];

  for (const source of EncounterSourceColors) {
    for (const enemy of enemies.value) {
      if (listedIds.has(enemy.id) || !source.matches(enemy)) {
        continue;
      }

      listedIds.add(enemy.id);
      names.push({ id: enemy.id, name: enemy.name, color: source.color });
    }
  }

  return names;
});
</script>

<template>
  <div class="relative flex flex-col text-white dw3-beveled">
    <div class="absolute inset-0 bg-progress-frame dw3-beveled pointer-events-none"></div>
    <div class="absolute inset-[1.5px] bg-black dw3-beveled overflow-hidden pointer-events-none">
      <div
        v-if="backgroundImageUrl"
        class="absolute inset-0 bg-cover bg-center"
        :style="{ backgroundImage: `url(${backgroundImageUrl})` }"
      ></div>
    </div>

    <div class="relative z-10 flex-1 px-2 py-2 flex flex-col gap-2">
      <h4
        class="self-center text-[14px] leading-none uppercase tracking-widest text-white text-outline-black-glow"
      >
        {{ mapName }}
      </h4>
      <div
        class="mt-auto flex items-end justify-between gap-2 text-[10px] leading-none font-bold tracking-wide"
      >
        <div class="min-w-0 flex flex-col items-start gap-1.5 text-left">
          <span
            v-for="enemy in enemyNames"
            :key="enemy.id"
            class="text-outline-black-glow"
            :style="{ color: enemy.color }"
          >
            {{ enemy.name }}
          </span>
        </div>
        <div class="shrink-0 flex flex-col items-end gap-1.5 text-right">
          <span
            v-for="tamerId in MapMock.tamers"
            :key="tamerId"
            class="flex items-center gap-1 text-white text-outline-black-glow"
          >
            <span class="text-[14px] leading-none font-emoji text-white">{{ TamerIcon }}</span>
            {{ t(`tamers.${tamerId}.name`) }}
          </span>
        </div>
      </div>
    </div>
  </div>
</template>
