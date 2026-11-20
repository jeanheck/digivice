<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import WikiLocatedIn from "@/ui/components/wiki-modal/WikiLocatedIn.vue";
import WikiItemShopInventory from "@/ui/components/wiki-modal/item-shop/WikiItemShopInventory.vue";
import { WikiItemShopPanelPresenter } from "@/presentation/presenters/map/wiki-modal/wiki-item-shop-panel.presenter";

const props = defineProps<{
  itemShopId: string;
}>();

const emit = defineEmits<{
  (e: "open-consumable-item", consumableItemId: string): void;
  (e: "open-location", locationId: string): void;
}>();

const { t } = useI18n();

const itemShopViewModel = computed(() => {
  return WikiItemShopPanelPresenter.getViewModel(props.itemShopId, (consumableItemId) => {
    return t(`consumableItems.${consumableItemId}.name`);
  });
});

const handleOpenConsumableItem = (consumableItemId: string): void => {
  emit("open-consumable-item", consumableItemId);
};

const handleOpenLocation = (locationId: string): void => {
  emit("open-location", locationId);
};
</script>

<template>
  <div class="p-4 flex flex-col gap-4 h-full min-h-0 overflow-hidden">
    <section
      class="flex-1 w-full min-h-0 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-col"
    >
      <WikiItemShopInventory
        :items="itemShopViewModel.items"
        @open-consumable-item="handleOpenConsumableItem"
      />
    </section>

    <WikiLocatedIn
      v-if="itemShopViewModel.locationId !== null"
      :location-id="itemShopViewModel.locationId"
      @open-location="handleOpenLocation"
    />
  </div>
</template>
