<script setup lang="ts">
import WikiCardStore from "@/components/wiki-modal/wiki-cards-panel/WikiCardStore.vue";
import type { WikiCardStoreViewModel } from "@/viewmodels/wiki-modal/wiki-card-store.viewmodel";

defineProps<{
  stores: WikiCardStoreViewModel[];
}>();
</script>

<template>
  <section
    class="shrink-0 w-1/2 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner"
  >
    <h4 class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-3">
      {{ $t("enemy.purchasableAt") }}
    </h4>

    <p
      v-if="stores.length === 0"
      class="text-xs text-gray-400 italic"
    >
      {{ $t("enemy.purchasableAtNone") }}
    </p>
    <div
      v-else
      class="flex flex-wrap gap-2"
    >
      <WikiCardStore
        v-for="store in stores"
        :key="store.storeId"
        :store="store"
      />
    </div>
  </section>
</template>
