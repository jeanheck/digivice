<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

const props = defineProps<{
  enemy: EnemyViewModel;
}>();

const { t } = useI18n();

const dropLabel = computed(() => {
  const drop = props.enemy.drop;
  if (drop === undefined) {
    return t("drops.none");
  }

  if (typeof drop === "string") {
    return t(`drops.${drop}`);
  }

  return t("drops.variousBooster");
});
</script>

<template>
  <div
    class="flex-1 min-h-0 overflow-hidden bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-col justify-start gap-2.5"
  >
    <div class="flex items-center justify-between text-[10px]">
      <span class="font-bold text-blue-500 tracking-wider uppercase">{{ $t("enemy.drop") }}:</span>
      <span class="font-bold text-gray-300">{{ dropLabel }}</span>
    </div>

    <div
      v-if="enemy.boss && enemy.drop"
      class="flex items-center justify-between text-[10px]"
    >
      <span class="font-bold text-blue-500 tracking-wider uppercase"
        >{{ $t("enemy.dropRate") }}:</span
      >
      <span class="font-bold text-gray-300">100%</span>
    </div>
  </div>
</template>
