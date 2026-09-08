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
  (e: "open-card-shop", cardShopId: string): void;
}>();

const shops = computed(() => {
  return WikiCardShopsPresenter.getViewModel(props.cardShops, props.mainQuest);
});

const handleSelect = (cardShopId: string): void => {
  emit("open-card-shop", cardShopId);
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
      v-if="shops.length === 0"
      class="text-xs text-gray-400 italic"
    >
      {{ $t("enemy.purchasableAtNone") }}
    </p>
    <div
      v-else
      class="flex flex-wrap gap-2"
    >
      <WikiCardShop
        v-for="shop in shops"
        :key="shop.id"
        :card-shop="shop"
        @select="handleSelect"
      />
    </div>
  </section>
</template>
