<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { EnemyResumedViewModel } from "@/presentation/viewmodels/enemy/enemy-resumed.viewmodel";
import type { MapNpcViewModel } from "@/presentation/viewmodels/map/map-npc.viewmodel";
import type { EnemySource } from "@/shared/types/enemy-source.type";
import type { NpcBattleKind } from "@/shared/types/npc-battle-kind.type";

const { enemies, npcs = [] } = defineProps<{
  enemies: EnemyResumedViewModel[];
  npcs?: MapNpcViewModel[];
}>();

// Code points without U+FE0F, otherwise WebView2 falls back to the colored Segoe UI Emoji.
const EnemySourceIcons = {
  walking: "\u{1F3C3}",
  boss: "\u{2620}",
  fishing: "\u{1F3A3}",
  kickingTree: "\u{1F334}",
} as const satisfies Record<EnemySource, string>;

const NpcBattleKindIcons = {
  card: "\u{1F3B4}",
  digimon: "\u{2694}",
} as const satisfies Record<NpcBattleKind, string>;

const NpcBattleKindIconClasses = {
  card: "text-cyan-300 drop-shadow-[0_0_3px_rgba(34,211,238,0.7)]",
  digimon: "text-foreground",
} as const satisfies Record<NpcBattleKind, string>;

const { t } = useI18n();

const hasEncounters = computed(() => {
  return enemies.length > 0 || npcs.length > 0;
});

function getSourceIcons(enemy: EnemyResumedViewModel): string[] {
  if (enemy.boss) {
    return [EnemySourceIcons.boss];
  }

  const icons: string[] = [];

  if (enemy.walking) {
    icons.push(EnemySourceIcons.walking);
  }

  if (enemy.fishing) {
    icons.push(EnemySourceIcons.fishing);
  }

  if (enemy.kickingTree) {
    icons.push(EnemySourceIcons.kickingTree);
  }

  return icons;
}
</script>

<template>
  <div v-if="hasEncounters" class="map-info-panel border-progress-frame! flex flex-col items-center gap-1">
    <div v-if="enemies.length > 0" class="flex flex-wrap items-center justify-center gap-x-3 gap-y-1">
      <span
        v-for="enemy in enemies"
        :key="enemy.id"
        class="px-1 flex items-center text-[12px] font-bold tracking-wide"
        :class="
          enemy.boss
            ? 'text-amber-400 drop-shadow-[0_0_5px_rgba(255,191,0,0.8)]'
            : 'text-red-400 drop-shadow-[0_0_2px_rgba(158,55,55,0.8)]'
        "
      >
        <span>{{ enemy.name }}</span>
        <span
          v-for="icon in getSourceIcons(enemy)"
          :key="icon"
          class="ml-0.5 text-[12px] 2xl:text-[16px] leading-none -translate-y-0.5 font-emoji text-foreground"
        >
          {{ icon }}
        </span>
      </span>
    </div>

    <div v-if="npcs.length > 0" class="flex flex-wrap items-center justify-center gap-x-3 gap-y-1">
      <span
        v-for="npc in npcs"
        :key="npc.id"
        class="px-1 flex items-center text-[12px] font-bold tracking-wide"
        :class="npc.hasAvailableBattle ? 'text-sky-300' : 'text-gray-400'"
      >
        <span>{{ t(npc.nameKey) }}</span>
        <span
          v-if="npc.availableBattleKind"
          class="ml-0.5 text-[12px] 2xl:text-[16px] leading-none -translate-y-0.5 font-emoji"
          :class="NpcBattleKindIconClasses[npc.availableBattleKind]"
        >
          {{ NpcBattleKindIcons[npc.availableBattleKind] }}
        </span>
      </span>
    </div>
  </div>
</template>
