<script setup lang="ts">
import { computed } from "vue";
import WikiDropBooster from "@/ui/components/wiki-modal/wiki-drops-panel/WikiDropBooster.vue";
import WikiDropConsumableItem from "@/ui/components/wiki-modal/wiki-drops-panel/WikiDropConsumableItem.vue";
import WikiDropEquipment from "@/ui/components/wiki-modal/wiki-drops-panel/WikiDropEquipment.vue";
import WikiDroppedBy from "@/ui/components/wiki-modal/wiki-drops-panel/WikiDroppedBy.vue";
import WikiPurchasableAt from "@/ui/components/wiki-modal/wiki-drops-panel/WikiPurchasableAt.vue";
import { WikiDropsPanelPresenter } from "@/presentation/presenters/map/wiki-modal/wiki-drops-panel.presenter";
import type { DropType } from "@/presentation/repositories/tables/raws/drop/drop-type";
import type { DropSourceKind } from "@/shared/types/drop-source-kind.type";
const props = defineProps<{
  dropId: string;
  dropType: DropType;
}>();

const emit = defineEmits<{
  (e: "open-source", payload: { kind: DropSourceKind; sourceId: string }): void;
  (e: "open-card", cardId: string): void;
  (e: "open-equipment-shop", equipmentShopId: string): void;
  (e: "open-item-shop", itemShopId: string): void;
}>();

const dropsViewModel = computed(() => {
  return WikiDropsPanelPresenter.getViewModel(props.dropId, props.dropType);
});

const handleOpenSource = (payload: { kind: DropSourceKind; sourceId: string }): void => {
  emit("open-source", payload);
};

const handleOpenCard = (cardId: string): void => {
  emit("open-card", cardId);
};

const handleOpenShop = (shopId: string): void => {
  if (dropsViewModel.value.purchasableShopKind === "itemShop") {
    emit("open-item-shop", shopId);
    return;
  }

  emit("open-equipment-shop", shopId);
};
</script>

<template>
  <div class="p-4 flex flex-col gap-4 h-full min-h-0 overflow-hidden">
    <section
      class="flex-1 w-full min-h-0 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-col"
    >
      <WikiDropConsumableItem
        v-if="dropsViewModel.dropType === 'consumableItem'"
        :consumable-item-id="dropsViewModel.dropNumericId"
      />
      <WikiDropEquipment
        v-else-if="dropsViewModel.dropType === 'equipment'"
        :equipment-id="dropsViewModel.dropNumericId"
      />
      <WikiDropBooster
        v-else-if="dropsViewModel.dropType === 'booster'"
        :booster-id="dropsViewModel.dropNumericId"
        @open-card="handleOpenCard"
      />
    </section>

    <div v-if="dropsViewModel.purchasableShopKind !== null" class="flex gap-4 shrink-0 w-full">
      <WikiDroppedBy
        class="w-1/2"
        :sources="dropsViewModel.sources"
        :section-label-key="dropsViewModel.sourcesSectionLabelKey"
        :empty-label-key="dropsViewModel.sourcesEmptyLabelKey"
        @open-source="handleOpenSource"
      />
      <WikiPurchasableAt
        :shop-ids="dropsViewModel.purchasableShopIds"
        :shop-kind="dropsViewModel.purchasableShopKind"
        @open-shop="handleOpenShop"
      />
    </div>
    <WikiDroppedBy
      v-else
      class="w-full"
      :sources="dropsViewModel.sources"
      :section-label-key="dropsViewModel.sourcesSectionLabelKey"
      :empty-label-key="dropsViewModel.sourcesEmptyLabelKey"
      @open-source="handleOpenSource"
    />
  </div>
</template>
