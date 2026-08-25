<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { WikiProfileDropsPresenter } from "@/presenters/map/wiki-modal/wiki-profile-drops.presenter";
import type { EnemyDropViewModel } from "@/viewmodels/enemy/enemy-drop.viewmodel";
import type { WikiProfileDropItemViewModel } from "@/viewmodels/wiki-modal/wiki-profile-drop-item.viewmodel";

const props = defineProps<{
  drops?: EnemyDropViewModel[];
}>();

const emit = defineEmits<{
  (e: "open-drops", dropId: string): void;
}>();

const { t } = useI18n();

const dropsViewModel = computed(() => {
  return WikiProfileDropsPresenter.getViewModel(props.drops);
});

const locationOnlyLabel = (locationOnly: string): string => {
  return t("enemy.locationOnly", { location: t(`location.${locationOnly}`) });
};

const handleDropClick = (drop: WikiProfileDropItemViewModel): void => {
  if (!drop.isClickable) {
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
      {{ $t(dropsViewModel.sectionLabelKey) }}
    </h4>

    <span
      v-if="!dropsViewModel.hasInteractiveDrops"
      class="flex flex-1 min-h-0 items-center justify-center text-gray-200 text-xs"
    >
      {{ $t(dropsViewModel.fallbackLabelKey) }}
    </span>
    <div
      v-else
      class="flex flex-1 min-h-0 flex-wrap content-center justify-center gap-2"
    >
      <button
        v-for="drop in dropsViewModel.drops"
        :key="`${drop.id}-${drop.locationOnly ?? ''}`"
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
