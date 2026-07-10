<script setup lang="ts">
import { ref } from "vue";

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
</script>

<template>
  <div class="flex flex-1 min-h-0 w-full items-start p-4">
    <div class="flex flex-wrap items-center justify-center gap-x-5 gap-y-1 w-full">
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
  </div>
</template>
