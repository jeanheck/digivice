<script setup lang="ts">
import { computed, ref, watch } from "vue";
import EnemyModal from "./enemy-modal/EnemyModal.vue";
import { MapEnemiesPresenter } from "@/presenters/map/map-enemies.presenter.ts";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

const props = defineProps<{
  location: LocationViewModel | null;
}>();

const resumedEnemies = computed(() => {
  return MapEnemiesPresenter.getResumedEnemiesByIds(props.location?.enemies ?? []);
});

const hasEnemies = computed(() => resumedEnemies.value.length > 0);

const isEnemyModalOpen = ref(false);
const selectedEnemy = ref<string | null>(null);

const openEnemyDetails = (enemyId: string) => {
  selectedEnemy.value = enemyId;
  isEnemyModalOpen.value = true;
};

const closeEnemyDetails = () => {
  isEnemyModalOpen.value = false;
  selectedEnemy.value = null;
};

watch(
  () => props.location,
  () => {
    closeEnemyDetails();
  }
);
</script>

<template>
  <div class="shrink-0 w-full mt-1 py-1 border-t border-[#0033aa]/50 flex flex-col items-center">
    <div v-if="!hasEnemies" class="text-xs text-[#00aaff] opacity-50 italic">
      {{ $t("map.safeZone") }}
    </div>
    <div v-else class="flex flex-wrap items-center justify-center gap-x-3 gap-y-1">
      <button
        v-for="enemy in resumedEnemies"
        :key="enemy.id"
        @click="openEnemyDetails(enemy.id)"
        class="font-bold text-xs tracking-wide transition-all flex items-center justify-center focus:outline-none rounded px-1 cursor-pointer"
        :class="enemy.boss ? 'text-amber-400 drop-shadow-[0_0_5px_rgba(255,191,0,0.8)]' : 'text-[#9e3737] hover:text-[#b24848] drop-shadow-[0_0_2px_rgba(158,55,55,0.8)]'"
      >
        {{ enemy.name }}
      </button>
    </div>

    <EnemyModal :is-open="isEnemyModalOpen" :enemyId="selectedEnemy" @close="closeEnemyDetails" />
  </div>
</template>
