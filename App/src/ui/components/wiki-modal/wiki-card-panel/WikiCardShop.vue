<script setup lang="ts">
import type { CardShopViewModel } from "@/presentation/viewmodels/card/card-shop.viewmodel";

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
    class="flex items-center gap-2 px-2.5 py-2 rounded text-left transition-colors cursor-pointer bg-accent/15 hover:bg-accent/25 border border-frame/50"
    @click="handleSelect(cardShop.id)"
  >
    <span class="min-w-0">
      <span class="block text-xs font-bold text-ink tracking-wide">
        {{ $t(`cardShop.${cardShop.id}.name`) }}
      </span>
    </span>
  </button>
</template>
