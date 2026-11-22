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
  <section class="shrink-0 w-1/2 bg-panel border border-frame/50 rounded p-4 shadow-inner">
    <h4 class="text-[10px] uppercase font-bold tracking-widest text-accent mb-3">
      {{ $t("enemy.purchasableAt") }}
    </h4>

    <p v-if="shopIds.length === 0" class="text-xs text-ink/70 italic">
      {{ $t("enemy.purchasableAtNone") }}
    </p>
    <div v-else class="flex flex-wrap gap-2">
      <button
        v-for="shopId in shopIds"
        :key="shopId"
        type="button"
        class="flex items-center gap-2 px-2.5 py-2 rounded text-left transition-colors cursor-pointer bg-accent/15 hover:bg-accent/25 border border-frame/50"
        @click="handleSelect(shopId)"
      >
        <span class="min-w-0">
          <span class="block text-xs font-bold text-ink tracking-wide">
            {{ $t(`${shopKind}.${shopId}.name`) }}
          </span>
        </span>
      </button>
    </div>
  </section>
</template>
