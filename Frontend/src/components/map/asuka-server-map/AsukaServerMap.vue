<script setup lang="ts">
import { computed } from "vue";
import Location from "@/components/map/Location.vue";
import Enemies from "@/components/map/Enemies.vue";
import { useGameState } from "@/composables/use-game-state";
import { AsukaServerMapPresenter } from "@/presenters/map/asuka-server-map.presenter";

const emit = defineEmits<{
  (e: "open-enemy-modal", enemyId: string): void;
  (e: "open-location-wiki", locationId: string): void;
  (e: "open-npc-modal", npcId: string): void;
}>();

const gameState = useGameState();

const asukaServerMapViewModel = computed(() => {
  const state = gameState.value;

  return AsukaServerMapPresenter.getViewModel(
    state.player.mapId,
    state.journal.mainQuest,
    state.journal.sideQuests,
    state.party.slots,
    state.player.previousMapId,
    state.npcs,
    state.importantItems,
  );
});

const isSafeZone = computed(() => {
  return (
    asukaServerMapViewModel.value.enemies.length === 0 &&
    asukaServerMapViewModel.value.boss.length === 0
  );
});
</script>

<template>
  <div class="relative z-10 flex flex-col flex-1 min-h-0 pt-1">
    <div class="flex flex-col items-center gap-2 shrink-0">
      <Location
        :location-id="asukaServerMapViewModel.locationId"
        :is-safe-zone="isSafeZone"
        @open-location-wiki="emit('open-location-wiki', $event)"
      />
      <Enemies
        :enemy-ids="asukaServerMapViewModel.enemies"
        :boss-ids="asukaServerMapViewModel.boss"
        :fishing-ids="asukaServerMapViewModel.fishing"
        :kicking-tree-ids="asukaServerMapViewModel.kickingTree"
        :npcs="asukaServerMapViewModel.npcs"
        @open-enemy-modal="emit('open-enemy-modal', $event)"
        @open-npc-modal="emit('open-npc-modal', $event)"
      />
    </div>

    <div class="flex-1 min-h-0" />
  </div>
</template>
