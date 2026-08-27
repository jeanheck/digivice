<script setup lang="ts">
import { computed } from "vue";
import WikiNpcDeckCard from "@/components/wiki-modal/wiki-npc-panel/WikiNpcDeckCard.vue";
import WikiProfileDrops from "@/components/wiki-modal/wiki-profile-panel/WikiProfileDrops.vue";
import { WikiNpcCardBattlePresenter } from "@/presenters/map/wiki-modal/wiki-npc-card-battle.presenter";

const props = defineProps<{
  npcId: string;
  battleId: string;
}>();

const emit = defineEmits<{
  (e: "open-card", cardId: string): void;
  (e: "open-drops", dropId: string): void;
}>();

const battleViewModel = computed(() => {
  return WikiNpcCardBattlePresenter.getBattleViewModel(props.npcId, props.battleId);
});

const handleSelect = (cardId: string): void => {
  emit("open-card", cardId);
};
</script>

<template>
  <div
    v-if="battleViewModel !== null"
    class="flex flex-col flex-1 min-h-0 overflow-hidden text-xs text-center gap-4 p-3"
  >
    <p class="shrink-0 text-blue-500 uppercase font-bold text-center">
      {{ $t(battleViewModel.nameKey) }}
    </p>

    <div class="flex-1 min-h-0 overflow-y-auto overflow-x-hidden custom-scroll">
      <div class="flex flex-wrap content-start justify-center gap-2 w-full">
        <WikiNpcDeckCard
          v-for="card in battleViewModel.cards"
          :key="card.cardId"
          :card="card"
          @select="handleSelect"
        />
      </div>
    </div>

    <WikiProfileDrops
      class="!h-auto max-h-[20%] shrink-0"
      :drops="battleViewModel.drops"
      @open-drops="emit('open-drops', $event)"
    />
  </div>
</template>
