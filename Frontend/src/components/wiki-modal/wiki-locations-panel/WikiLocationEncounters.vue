<script setup lang="ts">
import { useI18n } from "vue-i18n";
import { EnemySourceConstant } from "@/constants/enemy-source.constant";
import { IconConstant } from "@/constants/icon.constant";
import type { WikiLocationEncounterLineViewModel } from "@/viewmodels/wiki-modal/wiki-location-encounter-line.viewmodel";

defineProps<{
  lines: WikiLocationEncounterLineViewModel[];
}>();

const emit = defineEmits<{
  (e: "open-enemy", enemyId: string): void;
}>();

const { t } = useI18n();

const getSourceIcon = (source: WikiLocationEncounterLineViewModel["source"]): string => {
  return IconConstant[EnemySourceConstant[source]];
};

const getSourceAriaLabel = (source: WikiLocationEncounterLineViewModel["source"]): string => {
  return t(`enemy.locationSource.${source}`);
};

const handleOpenEnemy = (enemyId: string): void => {
  emit("open-enemy", enemyId);
};
</script>

<template>
  <div class="h-full w-full flex flex-col justify-center gap-1.5">
    <div
      v-for="line in lines"
      :key="line.source"
      class="flex flex-wrap items-center justify-center gap-x-3 gap-y-1"
    >
      <button
        v-for="enemy in line.enemies"
        :key="`${line.source}-${enemy.id}`"
        type="button"
        class="font-bold text-[10px] 2xl:text-[13px] tracking-wide transition-all flex items-center justify-center focus:outline-none rounded px-1 cursor-pointer text-red-400 hover:text-red-200 drop-shadow-[0_0_2px_rgba(158,55,55,0.8)]"
        @click="handleOpenEnemy(enemy.id)"
      >
        <span>{{ enemy.name }}</span>
        <span
          class="ml-0.5 text-[12px] 2xl:text-[16px]"
          :aria-label="getSourceAriaLabel(line.source)"
        >
          <span class="inline-flex leading-none text-[1.2rem] -translate-y-0.5">{{ getSourceIcon(line.source) }}</span>
        </span>
      </button>
    </div>
  </div>
</template>
