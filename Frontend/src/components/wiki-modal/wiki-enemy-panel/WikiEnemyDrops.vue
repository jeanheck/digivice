<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { WikiEnemyDropsPresenter } from "@/presenters/map/wiki-modal/wiki-enemy-drops.presenter";
import type { DropType } from "@/repositories/tables/raws/drop/drop-type";
import type { EnemyDropViewModel } from "@/viewmodels/enemy/enemy-drop.viewmodel";
import type { WikiEnemyDropViewModel } from "@/viewmodels/wiki-modal/wiki-enemy-drop.viewmodel";

const props = defineProps<{
  drops?: EnemyDropViewModel[];
}>();

const emit = defineEmits<{
  (e: "open-drops", payload: { dropId: string; dropType: DropType }): void;
}>();

const { t } = useI18n();

const drops = computed(() => {
  return WikiEnemyDropsPresenter.getViewModel(props.drops);
});

const sectionLabelKey = computed(() => {
  return drops.value.length > 1 ? "enemy.drops" : "enemy.drop";
});

const hasDrops = computed(() => {
  return drops.value.length > 0;
});

const locationOnlyLabel = (locationOnly: string): string => {
  return t("enemy.locationOnly", { location: t(`location.${locationOnly}`) });
};

const handleDropClick = (drop: WikiEnemyDropViewModel): void => {
  emit("open-drops", {
    dropId: String(drop.dropId),
    dropType: drop.type,
  });
};
</script>

<template>
  <div
    class="h-full min-h-32 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner text-sm flex flex-col min-w-0"
  >
    <h4
      class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-1 w-full"
    >
      {{ $t(sectionLabelKey) }}
    </h4>

    <span
      v-if="!hasDrops"
      class="flex flex-1 min-h-0 items-center justify-center text-gray-200 text-xs"
    >
      {{ $t("drops.none") }}
    </span>
    <div
      v-else
      class="flex flex-1 min-h-0 flex-wrap content-center justify-center gap-2"
    >
      <button
        v-for="drop in drops"
        :key="`${drop.dropId}-${drop.locationOnly ?? ''}`"
        type="button"
        class="text-center px-2.5 py-2 rounded text-[9px] 2xl:text-[11px] font-bold tracking-wide transition-colors cursor-pointer  hover:bg-blue-900/60 text-blue-300 border border-blue-700/60 bg-blue-950/40"
        @click="handleDropClick(drop)"
      >
        <span class="block">{{ $t(drop.labelKey) }}</span>
        <span class="block min-h-3 text-[9px] font-normal text-gray-300 leading-tight">
          {{ drop.locationOnly ? locationOnlyLabel(drop.locationOnly) : "" }}
        </span>
      </button>
    </div>
  </div>
</template>
