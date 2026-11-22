<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { WikiEnemyDropsPresenter } from "@/presentation/presenters/map/wiki-modal/wiki-enemy-drops.presenter";
import type { DropType } from "@/presentation/repositories/tables/raws/drop/drop-type";
import type { EnemyDropViewModel } from "@/presentation/viewmodels/enemy/enemy-drop.viewmodel";
import type { WikiEnemyDropViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-enemy-drop.viewmodel";

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
  return t("enemy.locationOnly", { location: t(`map.${locationOnly}`) });
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
    class="h-full min-h-32 bg-panel border border-frame/50 rounded p-4 shadow-inner text-sm flex flex-col min-w-0"
  >
    <h4
      class="text-[10px] uppercase font-bold tracking-widest text-accent mb-1 w-full"
    >
      {{ $t(sectionLabelKey) }}
    </h4>

    <span
      v-if="!hasDrops"
      class="flex flex-1 min-h-0 items-center justify-center text-ink text-xs"
    >
      {{ $t("drops.none") }}
    </span>
    <div v-else class="flex flex-1 min-h-0 flex-wrap content-center justify-center gap-2">
      <button
        v-for="drop in drops"
        :key="`${drop.dropId}-${drop.locationOnly ?? ''}`"
        type="button"
        class="text-center px-2.5 py-2 rounded text-[9px] fhdscaled:text-[11px] font-bold tracking-wide transition-colors cursor-pointer hover:bg-accent/20 text-ink-muted border border-frame/50 bg-surface in-[[data-theme=seabed]]:text-yellow-700"
        @click="handleDropClick(drop)"
      >
        <span class="block">{{ $t(drop.labelKey) }}</span>
        <span
          class="block min-h-3 text-[9px] font-normal text-ink/70 in-[[data-theme=seabed]]:text-yellow-700/80 leading-tight"
        >
          {{ drop.locationOnly ? locationOnlyLabel(drop.locationOnly) : "" }}
        </span>
      </button>
    </div>
  </div>
</template>
