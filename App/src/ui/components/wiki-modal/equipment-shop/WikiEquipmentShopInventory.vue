<script setup lang="ts">
import WikiShopInventoryColumns from "@/ui/components/wiki-modal/shop/WikiShopInventoryColumns.vue";
import WikiEquipmentShopInventoryItem from "@/ui/components/wiki-modal/equipment-shop/WikiEquipmentShopInventoryItem.vue";
import type { WikiEquipmentShopItemViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-equipment-shop-item.viewmodel";

defineProps<{
  items: WikiEquipmentShopItemViewModel[];
}>();

const emit = defineEmits<{
  (e: "open-equipment", equipmentId: string): void;
}>();

const getItemKey = (item: WikiEquipmentShopItemViewModel): string => {
  return item.equipmentId;
};

const handleSelect = (equipmentId: string): void => {
  emit("open-equipment", equipmentId);
};
</script>

<template>
  <WikiShopInventoryColumns :items="items" :item-key="getItemKey">
    <template #item="{ item }">
      <WikiEquipmentShopInventoryItem :item="item" @select="handleSelect" />
    </template>
  </WikiShopInventoryColumns>
</template>
