<script setup lang="ts">
import { computed } from "vue";
import { EnemySourceIcon, NpcBattleKindIcon } from "@/shared/constants/icon.constant";
import { MapEnemiesPresenter } from "@/presentation/presenters/map/map-enemies.presenter";
import type { MapNpcViewModel } from "@/presentation/viewmodels/map/map-npc.viewmodel";

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
  (e: "open-npc-modal", npcId: string): void;
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

const showPanel = computed(() => {
  return hasMapThreatEnemies.value || hasNpcs.value;
});

const openWikiModal = (enemyId: string) => {
  emit("open-enemy-modal", enemyId);
};

const openNpcWikiModal = (npcId: string) => {
  emit("open-npc-modal", npcId);
};
</script>

<template>
  <div v-if="showPanel" class="w-full flex justify-center shrink-0 px-0.5">
    <div class="map-info-panel flex flex-col justify-center items-center gap-1">
      <div
        v-if="hasMapThreatEnemies"
        class="flex flex-wrap items-center justify-center gap-x-3 gap-y-1"
      >
        <button
          v-for="enemy in resumedEnemies"
          :key="enemy.id"
          type="button"
          class="font-bold text-[9px] fhdscaled:text-xs tracking-wide transition-all flex items-center justify-center focus:outline-none rounded px-1 cursor-pointer"
          :class="
            enemy.boss
              ? 'text-amber-400 hover:text-amber-200 drop-shadow-[0_0_5px_rgba(255,191,0,0.8)]'
              : 'text-red-400 hover:text-red-200 drop-shadow-[0_0_2px_rgba(158,55,55,0.8)]'
          "
          @click="openWikiModal(enemy.id)"
        >
          <span>{{ enemy.name }}</span>
          <span v-if="enemy.boss" class="ml-0.5 font-emoji-color text-[12px] fhdscaled:text-[16px]">{{
            EnemySourceIcon.boss
          }}</span>
          <span
            v-if="enemy.walking && !enemy.boss"
            class="ml-0.5 font-emoji-color text-[12px] fhdscaled:text-[16px]"
            >{{ EnemySourceIcon.walking }}</span
          >
          <span
            v-if="enemy.fishing && !enemy.boss"
            class="ml-0.5 font-emoji-color text-[12px] fhdscaled:text-[16px]"
            >{{ EnemySourceIcon.fishing }}</span
          >
          <span
            v-if="enemy.kickingTree && !enemy.boss"
            class="ml-0.5 font-emoji-color text-[12px] fhdscaled:text-[16px]"
            >{{ EnemySourceIcon.kickingTree }}</span
          >
        </button>
      </div>
      <div v-if="hasNpcs" class="flex flex-wrap items-center justify-center gap-x-3 gap-y-1">
        <button
          v-for="npc in npcs"
          :key="npc.id"
          type="button"
          class="font-bold text-[9px] fhdscaled:text-xs tracking-wide focus:outline-none rounded px-1 cursor-pointer transition-colors flex items-center justify-center"
          :class="
            npc.hasAvailableBattle
              ? 'text-sky-300 hover:text-sky-200'
              : 'text-gray-400 hover:text-gray-300'
          "
          @click="openNpcWikiModal(npc.id)"
        >
          <span>{{ $t(npc.nameKey) }}</span>
          <span
            v-if="npc.availableBattleKind === 'card'"
            class="ml-0.5 font-emoji-color text-[12px] fhdscaled:text-[16px] drop-shadow-[0_0_3px_rgba(34,211,238,0.7)]"
            >{{ NpcBattleKindIcon.card }}</span
          >
          <span
            v-if="npc.availableBattleKind === 'digimon'"
            class="ml-0.5 font-emoji-color text-[12px] fhdscaled:text-[16px]"
            >{{ NpcBattleKindIcon.digimon }}</span
          >
        </button>
      </div>
    </div>
  </div>
</template>
