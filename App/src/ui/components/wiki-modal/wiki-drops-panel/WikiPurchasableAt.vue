<script setup lang="ts">
import type { PurchasableShopKind } from "@/presentation/viewmodels/wiki-modal/wiki-drops-panel.viewmodel";

defineProps<{
  shopIds: string[];
  shopKind: PurchasableShopKind;
}>();

const emit = defineEmits<{
  (e: "open-shop", shopId: string): void;
}>();

const handleSelect = (shopId: string): void => {
  emit("open-shop", shopId);
};
</script>

<template>
  <section class="shrink-0 w-1/2 bg-[#000a1a] in-[[data-theme=seabed]]:bg-[var(--color-surface-deep)] border border-blue-900/50 in-[[data-theme=seabed]]:border-[var(--color-frame)]/50 rounded p-4 shadow-inner">
    <h4 class="text-[10px] uppercase font-bold tracking-widest text-blue-500 in-[[data-theme=seabed]]:text-[var(--color-accent)] mb-3">
      {{ $t("enemy.purchasableAt") }}
    </h4>

    <p v-if="shopIds.length === 0" class="text-xs text-gray-400 in-[[data-theme=seabed]]:text-[var(--color-ink)]/70 italic">
      {{ $t("enemy.purchasableAtNone") }}
    </p>
    <div v-else class="flex flex-wrap gap-2">
      <button
        v-for="shopId in shopIds"
        :key="shopId"
        type="button"
        class="flex items-center gap-2 px-2.5 py-2 rounded text-left transition-colors cursor-pointer bg-blue-900/30 in-[[data-theme=seabed]]:bg-[var(--color-accent)]/15 hover:bg-blue-900/50 in-[[data-theme=seabed]]:hover:bg-[var(--color-accent)]/25 border border-blue-800/60 in-[[data-theme=seabed]]:border-[var(--color-frame)]/50"
        @click="handleSelect(shopId)"
      >
        <span class="min-w-0">
          <span class="block text-xs font-bold text-blue-200 in-[[data-theme=seabed]]:text-[var(--color-ink)] tracking-wide">
            {{ $t(`${shopKind}.${shopId}.name`) }}
          </span>
        </span>
      </button>
    </div>
  </section>
</template>
