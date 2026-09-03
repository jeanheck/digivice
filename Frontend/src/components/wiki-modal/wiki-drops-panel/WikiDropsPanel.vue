<script setup lang="ts">
import { computed } from "vue";
import WikiDropBooster from "@/components/wiki-modal/wiki-drops-panel/WikiDropBooster.vue";
import WikiDropConsumableItem from "@/components/wiki-modal/wiki-drops-panel/WikiDropConsumableItem.vue";
import WikiDropEquipment from "@/components/wiki-modal/wiki-drops-panel/WikiDropEquipment.vue";
import WikiDroppedBy from "@/components/wiki-modal/wiki-drops-panel/WikiDroppedBy.vue";
import { WikiDropsPanelPresenter } from "@/presenters/map/wiki-modal/wiki-drops-panel.presenter";
import type { DropSourceKind } from "@/viewmodels/drop/drop-source.viewmodel";

const props = defineProps<{
  dropId: string;
}>();

const emit = defineEmits<{
  (e: "open-source", payload: { kind: DropSourceKind; sourceId: string }): void;
  (e: "open-card", cardId: string): void;
}>();

const dropsViewModel = computed(() => {
  return WikiDropsPanelPresenter.getViewModel(props.dropId);
});

const handleOpenSource = (payload: { kind: DropSourceKind; sourceId: string }): void => {
  emit("open-source", payload);
};

const handleOpenCard = (cardId: string): void => {
  emit("open-card", cardId);
};
</script>

<template>
  <div class="p-4 flex flex-col gap-4 h-full min-h-0 overflow-hidden">
    <section
      class="flex-1 w-full min-h-0 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-col"
    >
      <WikiDropConsumableItem
        v-if="dropsViewModel.dropType === 'consumableItem' && dropsViewModel.dropNumericId !== null"
        :consumable-item-id="dropsViewModel.dropNumericId"
      />
      <WikiDropEquipment
        v-else-if="dropsViewModel.dropType === 'equipment' && dropsViewModel.dropNumericId !== null"
        :equipment-id="dropsViewModel.dropNumericId"
      />
      <WikiDropBooster
        v-else-if="dropsViewModel.dropType === 'booster' && dropsViewModel.dropNumericId !== null"
        :booster-id="dropsViewModel.dropNumericId"
        @open-card="handleOpenCard"
      />
    </section>

    <WikiDroppedBy
      :sources="dropsViewModel.sources"
      :section-label-key="dropsViewModel.sourcesSectionLabelKey"
      :empty-label-key="dropsViewModel.sourcesEmptyLabelKey"
      @open-source="handleOpenSource"
    />
  </div>
</template>
