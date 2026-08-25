<script setup lang="ts">
import { computed } from "vue";
import { IconConstant } from "@/constants/icon.constant";
import { EnemySourceConstant } from "@/constants/enemy-source.constant";
import { MapEnemiesPresenter } from "@/presenters/map/map-enemies.presenter.ts";
import type { MapNpcViewModel } from "@/viewmodels/map/map-npc.viewmodel";

const props = withDefaults(
  defineProps<{
    enemyIds: string[];
    bossIds?: string[];
    fishingIds?: string[];
    kickingTreeIds?: string[];
    npcs?: MapNpcViewModel[];
  }>(),
  {
    bossIds: () => [],
    fishingIds: () => [],
    kickingTreeIds: () => [],
    npcs: () => [],
  },
);

const emit = defineEmits<{
  (e: "open-enemy-modal", enemyId: string): void;
}>();

const resumedEnemies = computed(() => {
  return MapEnemiesPresenter.getResumedEnemiesByEncounterSources(
    props.enemyIds,
    props.bossIds,
    props.fishingIds,
    props.kickingTreeIds,
  );
});

const hasMapThreatEnemies = computed(() => {
  return props.enemyIds.length > 0 || props.bossIds.length > 0;
});

const hasNpcs = computed(() => {
  return props.npcs.length > 0;
});

const openWikiModal = (enemyId: string) => {
  emit("open-enemy-modal", enemyId);
};
</script>

<template>
  <div class="w-full flex justify-center shrink-0 px-0.5">
    <div class="map-info-panel flex flex-col justify-center items-center gap-1">
      <div v-if="!hasMapThreatEnemies" class="text-[10px] 2xl:text-xs text-[#00aaff] opacity-50 italic">
        {{ $t("map.safeZone") }}
      </div>
      <div v-else class="flex flex-wrap items-center justify-center gap-x-3 gap-y-1">
        <button
          v-for="enemy in resumedEnemies"
          :key="enemy.id"
          type="button"
          class="font-bold text-[9px] 2xl:text-xs tracking-wide transition-all flex items-center justify-center focus:outline-none rounded px-1 cursor-pointer"
          :class="
            enemy.boss
              ? 'text-amber-400 hover:text-amber-200 drop-shadow-[0_0_5px_rgba(255,191,0,0.8)]'
              : 'text-red-400 hover:text-red-200 drop-shadow-[0_0_2px_rgba(158,55,55,0.8)]'
          "
          @click="openWikiModal(enemy.id)"
        >
          <span>{{ enemy.name }}</span>
          <span v-if="enemy.walking && !enemy.boss" class="ml-0.5 text-[12px] 2xl:text-[16px] -translate-y-0.5" aria-hidden="true">{{ IconConstant[EnemySourceConstant.walking] }}</span>
          <span v-if="enemy.fishing && !enemy.boss" class="ml-0.5 text-[12px] 2xl:text-[16px] -translate-y-0.5" aria-hidden="true">{{ IconConstant[EnemySourceConstant.fishing] }}</span>
          <span v-if="enemy.kickingTree && !enemy.boss" class="ml-0.5 text-[12px] 2xl:text-[16px] -translate-y-0.5" aria-hidden="true">{{ IconConstant[EnemySourceConstant.kickingTree] }}</span>
        </button>
      </div>
      <div
        v-if="hasNpcs"
        class="flex flex-wrap items-center justify-center gap-x-3 gap-y-1"
      >
        <span
          v-for="npc in npcs"
          :key="npc.id"
          class="font-bold text-[9px] 2xl:text-xs tracking-wide"
          :class="npc.hasAvailableBattle ? 'text-sky-300' : 'text-gray-400'"
        >
          {{ npc.name }}
        </span>
      </div>
    </div>
  </div>
</template>
