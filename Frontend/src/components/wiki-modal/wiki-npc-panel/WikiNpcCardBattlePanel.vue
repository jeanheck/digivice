<script setup lang="ts">
import { computed } from "vue";
import WikiNpcBattleStatusLabel from "@/components/wiki-modal/wiki-npc-panel/WikiNpcBattleStatusLabel.vue";
import WikiNpcDeckCard from "@/components/wiki-modal/wiki-npc-panel/WikiNpcDeckCard.vue";
import WikiProfileDrops from "@/components/wiki-modal/wiki-profile-panel/WikiProfileDrops.vue";
import { WikiNpcCardBattlePresenter } from "@/presenters/map/wiki-modal/wiki-npc-card-battle.presenter";
import type { NpcBattleStatus } from "@/services/npc.service";

const props = defineProps<{
  npcId: string;
  battleId: string;
  battleStatus: NpcBattleStatus;
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
    <div class="relative shrink-0 flex items-center justify-center min-h-7 w-full">
      <p class="text-blue-500 uppercase font-bold text-center">
        {{ $t(battleViewModel.nameKey) }}
      </p>

      <WikiNpcBattleStatusLabel
        class="absolute right-0 top-1/2 -translate-y-1/2"
        :battle-status="battleStatus"
      />
    </div>

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
