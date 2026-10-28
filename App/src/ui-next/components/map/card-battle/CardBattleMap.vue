<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import MapFrame from "@/ui-next/components/map/MapFrame.vue";
import MapTitle from "@/ui-next/components/map/MapTitle.vue";
import { CardBattlePresenter } from "@/presentation/presenters/map/card-battle.presenter";
import { CardBattleMapMock } from "@/ui-next/mocks/map.mock";

const { t } = useI18n();

const cardBattleViewModel = computed(() => {
  return CardBattlePresenter.getViewModel(CardBattleMapMock.cardBattleId);
});

const title = computed(() => {
  const titleKey = cardBattleViewModel.value.titleKey;
  return titleKey !== null ? t(titleKey) : "";
});
</script>

<template>
  <MapFrame>
    <template #background>
      <div
        v-if="cardBattleViewModel.backgroundImageUrl"
        class="absolute inset-0 bg-cover bg-center"
        :style="{ backgroundImage: `url(${cardBattleViewModel.backgroundImageUrl})` }"
      ></div>
    </template>

    <div class="flex-1 px-2 py-2 flex flex-col">
      <MapTitle class="self-center" :title="title" />
    </div>
  </MapFrame>
</template>
