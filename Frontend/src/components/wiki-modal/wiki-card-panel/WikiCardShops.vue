<script setup lang="ts">
import { computed } from "vue";
import WikiCardShop from "@/components/wiki-modal/wiki-card-panel/WikiCardShop.vue";
import { WikiCardShopsPresenter } from "@/presenters/map/wiki-modal/wiki-card-shops.presenter";
import type { Quest } from "@/models";
import type { CardShopSourceViewModel } from "@/viewmodels/card/card-shop-source.viewmodel";

const props = defineProps<{
  cardShops: CardShopSourceViewModel[];
  mainQuest: Quest | null;
}>();

const emit = defineEmits<{
  (e: "open-store", storeId: string): void;
}>();

const stores = computed(() => {
  return WikiCardShopsPresenter.getViewModel(props.cardShops, props.mainQuest);
});

const handleSelect = (storeId: string): void => {
  emit("open-store", storeId);
};
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
      <WikiCardShop
        v-for="store in stores"
        :key="store.storeId"
        :store="store"
        @select="handleSelect"
      />
    </div>
  </section>
</template>
