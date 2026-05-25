<script setup lang="ts">
import { computed, ref } from "vue";
import EnemyDetailsModal from "./EnemyDetailsModal.vue";
import type { Enemy } from "../../models/enemy";

const props = defineProps<{
  locationAlias: string | null;
}>();

const enemies = computed(() => {
  if(!props.locationId){
    return null;
  }
  return MapImagePresenter.getLocationById(props.locationId);
});

const isEnemyModalOpen = ref(false);
const selectedEnemy = ref<Enemy | null>(null);

const openEnemyDetails = (enemy: Enemy) => {
  selectedEnemy.value = enemy;
  isEnemyModalOpen.value = true;
};

const closeEnemyDetails = () => {
  isEnemyModalOpen.value = false;
  selectedEnemy.value = null;
};
</script>

<template>
  <div class="flex-1 w-full mt-2 pt-2 border-t border-[#0033aa]/50 flex flex-col justify-center items-center">
    <h4 class="text-[9px] uppercase font-bold tracking-[0.2em] text-[#00aaff] mb-1">{{ $t("map.enemies") }}</h4>

    <div v-if="areaEnemies.length === 0" class="text-xs text-[#00aaff] opacity-50 italic">
      {{ $t("map.safeZone")}}
    </div>
    <div v-else class="flex flex-wrap items-center justify-center gap-x-3 gap-y-1">
      <button
        v-for="enemy in areaEnemies"
        :key="enemy.id"
        @click="openEnemyDetails(enemy)"
        class="font-bold text-sm tracking-wide transition-all flex items-center justify-center focus:outline-none rounded px-1 cursor-pointer"
        :class="enemy.boss ? 'text-amber-400 drop-shadow-[0_0_5px_rgba(255,191,0,0.8)]' : 'text-[#9e3737] hover:text-[#b24848] drop-shadow-[0_0_2px_rgba(158,55,55,0.8)]'"
      >
        {{ enemy.name }}
      </button>
    </div>

    <EnemyDetailsModal :is-open="isEnemyModalOpen" :enemy="selectedEnemy" @close="closeEnemyDetails" />
  </div>
</template>
