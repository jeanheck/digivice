<script setup lang="ts">
import { computed, ref } from "vue";
import EnemyData from "@/components/map/map-modal/EnemyData.vue";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

const mockEnemyNames = [
  "Musyamon",
  "DemiDevimon",
  "Bakemon",
  "Coelamon",
  "Gekomon",
  "Yanmamon(Green)",
];

const selectedEnemyName = ref<string | null>(null);

const selectEnemy = (enemyName: string) => {
  selectedEnemyName.value = enemyName;
};

const mockEnemy = computed((): EnemyViewModel => {
  return {
    name: selectedEnemyName.value ?? "",
    level: 24,
    hp: 1850,
    mp: 420,
    species: "virus",
    rate: 12,
    attributes: {
      strength: 180,
      defense: 140,
      spirit: 95,
      wisdom: 110,
      speed: 160,
    },
    elements: {
      fire: 20,
      water: -10,
      ice: 0,
      wind: 15,
      thunder: -5,
      machine: 0,
      dark: 25,
    },
    conditions: {
      poison: {
        can: true,
        value: 40,
      },
      paralyze: {
        can: true,
        value: 30,
      },
      confuse: {
        can: false,
        value: 0,
      },
      sleep: {
        can: true,
        value: 50,
      },
      ko: {
        can: false,
        value: 0,
      },
      drain: {
        can: true,
      },
      steal: {
        can: false,
      },
      escape: {
        can: true,
      },
    },
    strDown: "medium",
    defDown: "low",
    spdDown: "high",
    dvxp: 8,
    exp: 320,
    bits: 150,
    dropId: null,
    regularAttackId: null,
    techniqueId: null,
    boss: false,
  };
});
</script>

<template>
  <div class="flex flex-1 min-h-0 w-full flex-col p-4 gap-4">
    <div class="flex flex-wrap items-center justify-center gap-x-5 gap-y-1 w-full shrink-0">
      <button
        v-for="enemyName in mockEnemyNames"
        :key="enemyName"
        type="button"
        class="font-bold tracking-wide transition-all flex items-center justify-center focus:outline-none rounded px-1 cursor-pointer drop-shadow-[0_0_2px_rgba(158,55,55,0.8)]"
        :class="selectedEnemyName === enemyName
          ? 'text-sm text-[#ffe4e4]'
          : 'text-sm text-[#bc3737] hover:text-[#ffe4e4]'"
        @click="selectEnemy(enemyName)"
      >
        {{ enemyName }}
      </button>
    </div>

    <EnemyData
      v-if="selectedEnemyName"
      :enemy="mockEnemy"
      :enemy-image-url="null"
      class="flex-1 min-h-0"
    />
  </div>
</template>
