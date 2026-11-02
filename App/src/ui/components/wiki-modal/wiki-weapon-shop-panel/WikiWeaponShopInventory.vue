<script setup lang="ts">
import { computed } from "vue";
import WikiWeaponShopInventoryItem from "@/ui/components/wiki-modal/wiki-weapon-shop-panel/WikiWeaponShopInventoryItem.vue";
import type { WikiWeaponShopItemViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-weapon-shop-item.viewmodel";

const props = defineProps<{
  items: WikiWeaponShopItemViewModel[];
}>();

const emit = defineEmits<{
  (e: "open-equipment", equipmentId: string): void;
}>();

const leftColumnSize = computed(() => {
  return Math.ceil(props.items.length / 2);
});

const leftColumnItems = computed(() => {
  return props.items.slice(0, leftColumnSize.value);
});

const rightColumnItems = computed(() => {
  return props.items.slice(leftColumnSize.value);
});

const handleSelect = (equipmentId: string): void => {
  emit("open-equipment", equipmentId);
};
</script>

<template>
  <div class="flex flex-col flex-1 min-h-0 overflow-hidden">
    <div class="flex-1 min-h-0 overflow-y-auto overflow-x-hidden custom-scroll">
      <div class="grid grid-cols-2 divide-x divide-blue-900/50 w-full">
        <div class="flex flex-col gap-2 pr-4">
          <WikiWeaponShopInventoryItem
            v-for="item in leftColumnItems"
            :key="item.equipmentId"
            :item="item"
            @select="handleSelect"
          />
        </div>
        <div class="flex flex-col gap-2 pl-4">
          <WikiWeaponShopInventoryItem
            v-for="item in rightColumnItems"
            :key="item.equipmentId"
            :item="item"
            @select="handleSelect"
          />
        </div>
      </div>
    </div>
  </div>
</template>
