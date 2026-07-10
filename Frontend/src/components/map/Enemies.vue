<script setup lang="ts">
import { computed } from "vue";
import { MapEnemiesPresenter } from "@/presenters/map/map-enemies.presenter.ts";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

const props = defineProps<{
  location: LocationViewModel | null;
}>();

const emit = defineEmits<{
  (e: "open-map-modal", enemyId: string): void;
}>();

const resumedEnemies = computed(() => {
  return MapEnemiesPresenter.getResumedEnemiesByIds(props.location?.enemies ?? []);
});

const hasEnemies = computed(() => resumedEnemies.value.length > 0);

const openMapModal = (enemyId: string) => {
  emit("open-map-modal", enemyId);
};
</script>

<template>
  <div class="w-full flex justify-center shrink-0 px-0.5">
    <div class="map-info-panel flex flex-col justify-center items-center">
      <div v-if="!hasEnemies" class="text-[10px] 2xl:text-xs text-[#00aaff] opacity-50 italic">
        {{ $t("map.safeZone") }}
      </div>
      <div v-else class="flex flex-wrap items-center justify-center gap-x-3 gap-y-1">
        <button
          v-for="enemy in resumedEnemies"
          :key="enemy.id"
          type="button"
          class="font-bold text-[10px] 2xl:text-xs tracking-wide transition-all flex items-center justify-center focus:outline-none rounded px-1 cursor-pointer"
          :class="enemy.boss ? 'text-amber-400 drop-shadow-[0_0_5px_rgba(255,191,0,0.8)]' : 'text-[#bc3737] hover:text-[#c76060] drop-shadow-[0_0_2px_rgba(158,55,55,0.8)]'"
          @click="openMapModal(enemy.id)"
        >
          {{ enemy.name }}
        </button>
      </div>
    </div>
  </div>
</template>
