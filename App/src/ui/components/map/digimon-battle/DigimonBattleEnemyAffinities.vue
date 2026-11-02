<script setup lang="ts">
import { useI18n } from "vue-i18n";
import type { EnemyAffinityViewModel } from "@/presentation/viewmodels/enemy/enemy-affinities.viewmodel";

defineProps<{
  effective: EnemyAffinityViewModel[];
  notEffective: EnemyAffinityViewModel[];
}>();

const { t } = useI18n();
</script>

<template>
  <div
    class="absolute top-2 right-2 z-5 flex flex-col items-end gap-1.25 pointer-events-none text-[8px] hd:text-[10px] font-bold tracking-wide text-outline-black-glow"
  >
    <div v-if="effective.length > 0" class="flex items-center gap-1">
      <span class="text-green-400">{{ t("digimonBattle.effective") }}</span>
      <template v-for="affinity in effective" :key="affinity.element">
        <img
          v-if="affinity.iconUrl"
          :src="affinity.iconUrl"
          :alt="t(`stat.${affinity.element}`)"
          class="w-4 h-4 fhdscaled:w-5 fhdscaled:h-5 object-contain rendering-pixelated"
        />
      </template>
    </div>
    <div v-if="notEffective.length > 0" class="flex items-center gap-1">
      <span class="text-red-400">{{ t("digimonBattle.notEffective") }}</span>
      <template v-for="affinity in notEffective" :key="affinity.element">
        <img
          v-if="affinity.iconUrl"
          :src="affinity.iconUrl"
          :alt="t(`stat.${affinity.element}`)"
          class="w-4 h-4 fhdscaled:w-5 fhdscaled:h-5 object-contain rendering-pixelated"
        />
      </template>
    </div>
  </div>
</template>
