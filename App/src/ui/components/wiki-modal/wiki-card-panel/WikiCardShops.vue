<script setup lang="ts">
import { computed } from "vue";
import WikiCardShop from "@/ui/components/wiki-modal/wiki-card-panel/WikiCardShop.vue";
import { WikiCardShopsPresenter } from "@/presentation/presenters/map/wiki-modal/wiki-card-shops.presenter";
import type { Quest } from "@/state/models";
import type { CardShopViewModel } from "@/presentation/viewmodels/card/card-shop.viewmodel";

const props = defineProps<{
  cardShops: CardShopViewModel[];
  mainQuest: Quest;
}>();

const emit = defineEmits<{
  (e: "open-card-shop", cardShopId: string): void;
}>();

const shops = computed(() => {
  return WikiCardShopsPresenter.getCardShopsAccordingMainQuest(props.cardShops, props.mainQuest);
});

const handleSelect = (cardShopId: string): void => {
  emit("open-card-shop", cardShopId);
};
</script>

<template>
  <section class="shrink-0 w-1/2 bg-panel border border-frame/50 rounded p-4 shadow-inner">
    <h4 class="text-[10px] uppercase font-bold tracking-widest text-accent mb-3">
      {{ $t("enemy.purchasableAt") }}
    </h4>

    <p v-if="shops.length === 0" class="text-xs text-ink/70 italic">
      {{ $t("enemy.purchasableAtNone") }}
    </p>
    <div v-else class="flex flex-wrap gap-2">
      <WikiCardShop v-for="shop in shops" :key="shop.id" :card-shop="shop" @select="handleSelect" />
    </div>
  </section>
</template>
