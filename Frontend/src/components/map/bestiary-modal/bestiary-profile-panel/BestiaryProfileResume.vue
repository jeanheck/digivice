<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";
import type { EnemyDropRaw } from "@/repositories/tables/raws/enemy/enemy-drop.raw";

const props = defineProps<{
  enemy: EnemyViewModel;
}>();

const emit = defineEmits<{
  (e: "open-drops", dropId: string): void;
}>();

const { t } = useI18n();

const enemyDrops = computed(() => {
  return props.enemy.drops ?? [];
});

const isVariousBoosterOnly = computed(() => {
  return enemyDrops.value.length === 1 && enemyDrops.value[0].id === "variousBooster";
});

const hasInteractiveDrops = computed(() => {
  return enemyDrops.value.length > 0 && !isVariousBoosterOnly.value;
});

const dropSectionLabel = computed(() => {
  if (hasInteractiveDrops.value && enemyDrops.value.length > 1) {
    return t("enemy.drops");
  }

  return t("enemy.drop");
});

const dropFallbackLabel = computed(() => {
  if (isVariousBoosterOnly.value) {
    return t("drops.variousBooster");
  }

  return t("drops.none");
});

const sectorOnlyLabel = (sectorOnly: string): string => {
  return t("enemy.sectorOnly", { sector: t(`sectors.${sectorOnly}`) });
};

const handleDropClick = (drop: EnemyDropRaw): void => {
  if (drop.id === "variousBooster") {
    return;
  }

  emit("open-drops", drop.id);
};
</script>

<template>
  <div
    class="h-full bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-col justify-start gap-2.5 min-h-0"
  >
    <div class="flex items-center justify-between text-xs">
      <span class="font-bold text-blue-500 tracking-wider uppercase"
        >{{ $t("enemy.specie") }}:</span
      >
      <span class="font-bold text-gray-300 capitalize">{{
        $t(`species.${enemy.species}`)
      }}</span>
    </div>

    <div class="flex items-center justify-between text-xs">
      <span class="font-bold text-blue-500 tracking-wider uppercase"
        >{{ $t("enemy.level") }}:</span
      >
      <span class="font-bold text-gray-300">{{ enemy.level }}</span>
    </div>

    <div class="flex items-center justify-between text-xs">
      <span class="font-bold text-blue-500 tracking-wider uppercase">HP:</span>
      <span class="font-bold text-white">{{ enemy.hp }}</span>
    </div>

    <div class="flex items-center justify-between text-xs">
      <span class="font-bold text-blue-500 tracking-wider uppercase"
        >{{ $t("enemy.baseExp") }}:</span
      >
      <span class="font-bold text-gray-300">{{ enemy.exp }}</span>
    </div>

    <div class="flex items-center justify-between text-xs">
      <span class="font-bold text-blue-500 tracking-wider uppercase"
        >{{ $t("enemy.baseBits") }}:</span
      >
      <span class="font-bold text-gray-300">{{ enemy.bits }}</span>
    </div>

    <div
      v-if="!hasInteractiveDrops"
      class="flex items-center justify-between text-xs"
    >
      <span class="font-bold text-blue-500 tracking-wider uppercase"
        >{{ dropSectionLabel }}:</span
      >
      <span class="font-bold text-gray-300">{{ dropFallbackLabel }}</span>
    </div>
    <div v-else class="flex flex-col gap-1 min-h-0 text-xs">
      <span class="font-bold text-blue-500 tracking-wider uppercase">{{
        dropSectionLabel
      }}</span>
      <button
        v-for="drop in enemyDrops"
        :key="`${drop.id}-${drop.sectorOnly ?? ''}`"
        type="button"
        class="w-full text-left px-2.5 rounded text-[10px] 2xl:text-xs font-bold tracking-wide transition-colors cursor-pointer bg-amber-900/40 text-amber-300 border border-amber-300"
        :class="drop.sectorOnly ? 'py-2' : 'py-1.5'"
        @click="handleDropClick(drop)"
      >
        <span class="block">{{ $t(`drops.${drop.id}`) }}</span>
        <span
          v-if="drop.sectorOnly"
          class="block text-[9px] font-normal text-gray-300 leading-tight"
        >
          {{ sectorOnlyLabel(drop.sectorOnly) }}
        </span>
      </button>
    </div>
  </div>
</template>
