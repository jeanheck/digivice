<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { CardBattlePresenter } from "@/presenters/map/card-battle.presenter";
import { useGameStore } from "@/stores/use-game-store";

const emit = defineEmits<{
  (e: "open-npc-modal", npcId: string): void;
}>();

const store = useGameStore();
const { t } = useI18n();

const cardBattleViewModel = computed(() => {
  const cardBattleId = store.currentState?.cardBattle?.id ?? 0;
  return CardBattlePresenter.getViewModel(cardBattleId);
});

const titleClass =
  "text-xs sm:text-sm font-bold text-white tracking-widest uppercase drop-shadow-[0_0_5px_rgba(0,170,255,0.8)] leading-tight";

function openNpcWiki(): void {
  const npcId = cardBattleViewModel.value.npcId;
  if (npcId === null) {
    return;
  }

  emit("open-npc-modal", npcId);
}
</script>

<template>
  <div class="relative z-10 flex flex-col flex-1 min-h-0 pt-1">
    <div
      v-if="cardBattleViewModel.backgroundImageUrl"
      class="absolute -left-3 -right-3 -top-1.5 -bottom-1.5 z-0 bg-cover bg-center pointer-events-none"
      :style="{ backgroundImage: `url(${cardBattleViewModel.backgroundImageUrl})` }"
    />

    <div class="relative z-1 w-full flex justify-center shrink-0">
      <div class="map-info-panel-fit text-center">
        <button
          v-if="cardBattleViewModel.npcId !== null"
          type="button"
          :class="[titleClass, 'cursor-pointer transition-colors hover:text-blue-300']"
          @click="openNpcWiki"
        >
          {{ cardBattleViewModel.titleKey !== null ? t(cardBattleViewModel.titleKey) : "" }}
        </button>
        <h4 v-else :class="titleClass">
          {{ cardBattleViewModel.titleKey !== null ? t(cardBattleViewModel.titleKey) : "" }}
        </h4>
      </div>
    </div>

    <div class="relative z-1 flex-1 min-h-0" />
  </div>
</template>
