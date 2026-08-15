<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";
import type { EnemyDropRaw } from "@/repositories/tables/raws/enemy/enemy-drop.raw";
import { WikiModalPresenter } from "@/presenters/map/wiki-modal.presenter";

const props = defineProps<{
  enemy: EnemyViewModel;
}>();

const emit = defineEmits<{
  (e: "open-drops", dropId: string): void;
}>();

const { t } = useI18n();

const enemyDrops = computed((): EnemyDropRaw[] => {
  return props.enemy.drops ?? [];
});

const isVariousBoosterOnly = computed(() => {
  return enemyDrops.value.length === 1 && enemyDrops.value[0]?.id === "variousBooster";
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

const dropLabel = (dropId: string): string => {
  return t(WikiModalPresenter.getDropLabelKey(dropId));
};

const locationOnlyLabel = (locationOnly: string): string => {
  return t("enemy.locationOnly", { location: t(`location.${locationOnly}`) });
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
    class="h-full min-h-32 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner text-sm flex flex-col min-w-0"
  >
    <h4
      class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-1 w-full"
    >
      {{ dropSectionLabel }}
    </h4>

    <span
      v-if="!hasInteractiveDrops"
      class="flex flex-1 min-h-0 items-center justify-center text-gray-200 text-xs"
    >
      {{ dropFallbackLabel }}
    </span>
    <div
      v-else
      class="flex flex-1 min-h-0 flex-wrap content-center justify-center gap-2"
    >
      <button
        v-for="drop in enemyDrops"
        :key="`${drop.id}-${drop.locationOnly ?? ''}`"
        type="button"
        class="text-center px-2.5 py-2 rounded text-[9px] 2xl:text-[11px] font-bold tracking-wide transition-colors cursor-pointer  hover:bg-blue-900/60 text-blue-300 border border-blue-700/60 bg-blue-950/40"
        @click="handleDropClick(drop)"
      >
        <span class="block">{{ dropLabel(drop.id) }}</span>
        <span class="block min-h-3 text-[9px] font-normal text-gray-300 leading-tight">
          {{ drop.locationOnly ? locationOnlyLabel(drop.locationOnly) : "" }}
        </span>
      </button>
    </div>
  </div>
</template>
