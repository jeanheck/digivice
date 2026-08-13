<script setup lang="ts">
import { computed } from "vue";
import { MapEnemiesPresenter } from "@/presenters/map/map-enemies.presenter.ts";

const props = withDefaults(
  defineProps<{
    enemyIds: string[];
    bossIds?: string[];
    fishingIds?: string[];
    kickingTreeIds?: string[];
  }>(),
  {
    bossIds: () => [],
    fishingIds: () => [],
    kickingTreeIds: () => [],
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

const openBestiaryModal = (enemyId: string) => {
  emit("open-enemy-modal", enemyId);
};
</script>

<template>
  <div class="w-full flex justify-center shrink-0 px-0.5">
    <div class="map-info-panel flex flex-col justify-center items-center">
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
          @click="openBestiaryModal(enemy.id)"
        >
          <span>{{ enemy.name }}</span>
          <span v-if="enemy.walking && !enemy.boss" class="ml-0.5 text-[12px] 2xl:text-[16px] -translate-y-0.5" aria-hidden="true">🏃‍➡️</span>
          <span v-if="enemy.fishing && !enemy.boss" class="ml-0.5 text-[12px] 2xl:text-[16px] -translate-y-0.5" aria-hidden="true">🎣</span>
          <span v-if="enemy.kickingTree && !enemy.boss" class="ml-0.5 text-[12px] 2xl:text-[16px] -translate-y-0.5" aria-hidden="true">🌴</span>
        </button>
      </div>
    </div>
  </div>
</template>
