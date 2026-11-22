<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { EnemyViewModel } from "@/presentation/viewmodels/enemy/enemy.viewmodel";

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

const techniqueLabels = computed(() => {
  return props.enemy.techniqueIds.map((techniqueId) => {
    return t(`enemyTechniques.${techniqueId}`);
  });
});
</script>

<template>
  <div
    class="h-full min-h-32 bg-panel border border-frame/50 rounded p-4 shadow-inner text-sm"
  >
    <div class="flex flex-col gap-5">
      <div class="flex flex-col gap-1.5">
        <h4 class="text-[10px] uppercase font-bold tracking-widest text-accent w-full">
          {{ $t("enemy.regularAttack") }}
        </h4>
        <span class="text-ink text-xs">
          {{ regularAttackLabel }}
        </span>
      </div>

      <div class="flex flex-col gap-1.5">
        <h4 class="text-[10px] uppercase font-bold tracking-widest text-accent w-full">
          {{ $t("enemy.technique") }}
        </h4>
        <div class="flex flex-col gap-1">
          <span
            v-for="(techniqueLabel, index) in techniqueLabels"
            :key="index"
            class="text-ink text-xs"
          >
            {{ techniqueLabel }}
          </span>
        </div>
      </div>
    </div>
  </div>
</template>
