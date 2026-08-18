<script setup lang="ts">
import { useI18n } from "vue-i18n";
import type { WikiDroppedByEnemyViewModel } from "@/viewmodels/wiki-modal/wiki-dropped-by-enemy.viewmodel";

defineProps<{
  source: WikiDroppedByEnemyViewModel;
}>();

defineEmits<{
  select: [];
}>();

const { t } = useI18n();

const locationOnlyLabel = (locationOnly: string): string => {
  return t("enemy.locationOnly", { location: t(`location.${locationOnly}`) });
};
</script>

<template>
  <button
    type="button"
    class="flex items-center gap-2 px-2.5 py-2 rounded text-left transition-colors cursor-pointer bg-blue-900/30 hover:bg-blue-900/50 border border-blue-800/60"
    @click="$emit('select')"
  >
    <img
      v-if="source.iconUrl"
      :src="source.iconUrl"
      :alt="source.enemyName"
      class="w-8 h-8 object-contain rendering-pixelated"
    />
    <span class="min-w-0">
      <span class="block text-xs font-bold text-blue-200 tracking-wide">
        {{ source.enemyName }}
      </span>
      <span class="block min-h-3 text-[10px] text-gray-400 leading-tight">
        {{ source.locationOnly ? locationOnlyLabel(source.locationOnly) : "" }}
      </span>
    </span>
  </button>
</template>
