<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

const props = defineProps<{
  enemy: EnemyViewModel;
}>();

const { t } = useI18n();

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
  <div
    class="h-full min-h-32 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner text-sm"
  >
    <div class="flex flex-col gap-5">
      <div class="flex flex-col gap-1.5">
        <h4
          class="text-[10px] uppercase font-bold tracking-widest text-blue-500 w-full"
        >
          {{ $t("enemy.regularAttack") }}
        </h4>
        <span class="text-gray-200 text-xs">
          {{ regularAttackLabel }}
        </span>
      </div>

      <div class="flex flex-col gap-1.5">
        <h4
          class="text-[10px] uppercase font-bold tracking-widest text-blue-500 w-full"
        >
          {{ $t("enemy.technique") }}
        </h4>
        <span class="text-gray-200 text-xs">
          {{ techniqueLabel }}
        </span>
      </div>
    </div>
  </div>
</template>
