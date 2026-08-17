<script setup lang="ts">
import { computed } from "vue";
import WikiDroppedBy from "@/components/wiki-modal/wiki-drops-panel/WikiDroppedBy.vue";
import { WikiDropsPanelPresenter } from "@/presenters/map/wiki-modal/wiki-drops-panel.presenter";

const props = defineProps<{
  dropId: string;
}>();

const emit = defineEmits<{
  (e: "open-enemy", enemyId: string): void;
}>();

const dropsViewModel = computed(() => {
  return WikiDropsPanelPresenter.getViewModel(props.dropId);
});

const handleOpenEnemy = (enemyId: string): void => {
  emit("open-enemy", enemyId);
};
</script>

<template>
  <div class="p-4 flex flex-col gap-4 h-full min-h-0 overflow-hidden">
    <section
      class="flex-1 w-full min-h-0 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-col"
    >
      <h4 class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-2">
        {{ $t("enemy.dropDetails") }}
      </h4>
      <!-- Drop detail placeholder (type-specific components later) -->
      <p class="text-xs text-gray-400 italic">
        <template v-if="dropsViewModel.dropType !== null">
          {{ dropsViewModel.dropType }}
        </template>
      </p>
    </section>

    <WikiDroppedBy :sources="dropsViewModel.sources" @open-enemy="handleOpenEnemy" />
  </div>
</template>
