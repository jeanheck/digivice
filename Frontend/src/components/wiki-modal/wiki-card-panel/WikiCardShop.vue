<script setup lang="ts">
import type { CardShopViewModel } from "@/viewmodels/card/card-shop.viewmodel";

defineProps<{
  cardShop: CardShopViewModel;
}>();

const emit = defineEmits<{
  (e: "select", cardShopId: string): void;
}>();

const handleSelect = (cardShopId: string): void => {
  emit("select", cardShopId);
};
</script>

<template>
  <button
    type="button"
    class="flex items-center gap-2 px-2.5 py-2 rounded text-left transition-colors cursor-pointer bg-blue-900/30 hover:bg-blue-900/50 border border-blue-800/60"
    @click="handleSelect(cardShop.id)"
  >
    <span class="min-w-0">
      <span class="block text-xs font-bold text-blue-200 tracking-wide">
        {{ $t(`cardShops.${cardShop.id}.name`) }}
      </span>
    </span>
  </button>
</template>
