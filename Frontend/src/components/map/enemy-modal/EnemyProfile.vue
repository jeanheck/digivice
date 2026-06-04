<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

const props = defineProps<{
  enemy: EnemyViewModel;
  enemyImageUrl: string | null;
}>();

const { t } = useI18n();

const dropLabel = computed(() => {
  if (!props.enemy.dropId) {
    return t("drops.none");
  }

  return t(`drops.${props.enemy.dropId}`);
});

const regularAttackLabel = computed(() => {
  if (!props.enemy.regularAttackId) {
    return "";
  }

  return t(`regularAttacks.${props.enemy.regularAttackId}`);
});

const techniqueLabel = computed(() => {
  if (!props.enemy.techniqueId) {
    return "";
  }

  return t(`enemyTechniques.${props.enemy.techniqueId}`);
});
</script>

<template>
  <div class="flex flex-col gap-4 flex-1">
    <div class="flex gap-4">
      <div class="w-1/2 aspect-square bg-[#000a1a] border border-blue-900/50 rounded flex items-center justify-center shadow-inner relative group overflow-hidden">
        <div class="absolute inset-0 opacity-10 bg-linear-to-br from-blue-500/20 to-transparent pointer-events-none"></div>
        <img
          v-if="enemyImageUrl"
          :src="enemyImageUrl"
          class="w-full h-full object-cover drop-shadow-[0_0_15px_rgba(0,170,255,0.4)] group-hover:scale-110 transition-transform duration-500"
          :alt="enemy.name"
        />
        <div v-else class="text-4xl opacity-30 select-none">❓</div>
      </div>

      <div class="w-1/2 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-col justify-between gap-1.5 py-4">
        <div class="flex items-center justify-between text-[11px]">
          <span class="font-bold text-blue-500 tracking-wider uppercase">{{ $t("enemy.specie") }}:</span>
          <span class="font-bold text-gray-300 capitalize">{{ $t(`species.${enemy.species}`) }}</span>
        </div>

        <div class="flex items-center justify-between text-[11px]">
          <span class="font-bold text-blue-500 tracking-wider uppercase">{{ $t("enemy.level") }}:</span>
          <span class="font-bold text-gray-300">{{ enemy.level }}</span>
        </div>

        <div class="flex items-center justify-between text-[11px]">
          <span class="font-bold text-blue-500 tracking-wider uppercase">HP:</span>
          <span class="font-bold text-white">{{ enemy.hp }}</span>
        </div>

        <div class="flex items-center justify-between text-[11px]">
          <span class="font-bold text-blue-500 tracking-wider uppercase">MP:</span>
          <span class="font-bold text-white">{{ enemy.mp }}</span>
        </div>

        <div class="flex items-center justify-between text-[11px]">
          <span class="font-bold text-blue-500 tracking-wider uppercase">{{ $t("enemy.baseExp") }}:</span>
          <span class="font-bold text-gray-300">{{ enemy.exp }}</span>
        </div>

        <div class="flex items-center justify-between text-[11px]">
          <span class="font-bold text-blue-500 tracking-wider uppercase">Dvxp:</span>
          <span class="font-bold text-gray-300">{{ enemy.dvxp }}</span>
        </div>

        <div class="flex items-center justify-between text-[11px]">
          <span class="font-bold text-blue-500 tracking-wider uppercase">{{ $t("enemy.baseBits") }}:</span>
          <span class="font-bold text-gray-300">{{ enemy.bits }}</span>
        </div>

        <div class="flex flex-col text-[11px] mt-1">
          <span class="font-bold text-blue-500 tracking-wider uppercase mb-1">{{ $t("enemy.possibleDrop") }}:</span>
          <span
            class="font-bold text-gray-300 text-left"
            :title="dropLabel"
          >
            {{ dropLabel }}
          </span>
        </div>
      </div>
    </div>

    <div class="bg-[#000a1a] border border-red-900/30 rounded p-4 shadow-inner text-sm mt-auto">
      <h4 class="text-[10px] uppercase font-bold tracking-[0.2em] text-red-500 mb-4 border-b border-red-900/30 pb-1">{{ $t("enemy.combatActions") }}</h4>

      <div class="flex gap-6">
        <div class="flex-1 flex flex-col gap-1.5">
          <span class="text-[9px] text-gray-500 uppercase font-bold tracking-wider">{{ $t("enemy.regularAttack") }}</span>
          <span class="text-gray-200 text-xs">
            {{ regularAttackLabel }}
          </span>
        </div>

        <div class="flex-1 flex flex-col gap-1.5">
          <span class="text-[9px] text-gray-500 uppercase font-bold tracking-wider">{{ $t("enemy.technique") }}</span>
          <span class="text-gray-200 text-xs">
            {{ techniqueLabel }}
          </span>
        </div>
      </div>
    </div>
  </div>
</template>
