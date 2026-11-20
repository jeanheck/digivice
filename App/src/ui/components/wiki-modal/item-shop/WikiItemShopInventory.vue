<script setup lang="ts">
import WikiShopInventoryColumns from "@/ui/components/wiki-modal/shop/WikiShopInventoryColumns.vue";
import WikiItemShopInventoryItem from "@/ui/components/wiki-modal/item-shop/WikiItemShopInventoryItem.vue";
import type { WikiItemShopItemViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-item-shop-item.viewmodel";

defineProps<{
  items: WikiItemShopItemViewModel[];
}>();

const emit = defineEmits<{
  (e: "open-consumable-item", consumableItemId: string): void;
}>();

const getItemKey = (item: WikiItemShopItemViewModel): string => {
  return item.consumableItemId;
};

const handleSelect = (consumableItemId: string): void => {
  emit("open-consumable-item", consumableItemId);
};
</script>

<template>
  <WikiShopInventoryColumns :items="items" :item-key="getItemKey">
    <template #item="{ item }">
      <WikiItemShopInventoryItem :item="item" @select="handleSelect" />
    </template>
  </WikiShopInventoryColumns>
</template>
