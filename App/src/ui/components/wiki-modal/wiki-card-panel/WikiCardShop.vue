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
    class="flex items-center gap-2 px-2.5 py-2 rounded text-left transition-colors cursor-pointer bg-blue-900/30 in-[[data-theme=seabed]]:bg-[var(--color-accent)]/15 hover:bg-blue-900/50 in-[[data-theme=seabed]]:hover:bg-[var(--color-accent)]/25 border border-blue-800/60 in-[[data-theme=seabed]]:border-[var(--color-frame)]/50"
    @click="handleSelect(cardShop.id)"
  >
    <span class="min-w-0">
      <span class="block text-xs font-bold text-blue-200 in-[[data-theme=seabed]]:text-[var(--color-ink)] tracking-wide">
        {{ $t(`cardShop.${cardShop.id}.name`) }}
      </span>
    </span>
  </button>
</template>
