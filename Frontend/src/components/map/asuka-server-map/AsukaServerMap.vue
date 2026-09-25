<script setup lang="ts">
import { computed } from "vue";
import Location from "../Location.vue";
import Enemies from "../Enemies.vue";
import { useGameStore } from "@/stores/use-game-store";
import { AsukaServerMapPresenter } from "@/presenters/map/asuka-server-map.presenter.ts";

const emit = defineEmits<{
  (e: "open-enemy-modal", enemyId: string): void;
  (e: "open-location-wiki", locationId: string): void;
  (e: "open-npc-modal", npcId: string): void;
}>();

const store = useGameStore();

const asukaServerMapViewModel = computed(() => {
  const locationId = store.currentState?.player?.location ?? null;
  if (locationId === null) {
    return null;
  }

  const mainQuest = store.currentState?.journal?.mainQuest ?? null;
  const sideQuests = store.currentState?.journal?.sideQuests ?? [];
  const digimonSlots = store.currentState?.party?.slots ?? [];
  const previousMapId = store.currentState?.player?.previousMapId ?? "";
  const npcs = store.currentState?.npcs ?? null;
  const importantItems = store.currentState?.importantItems ?? null;

  return AsukaServerMapPresenter.getViewModel(
    locationId,
    mainQuest,
    sideQuests,
    digimonSlots,
    previousMapId,
    npcs,
    importantItems,
  );
});

const isSafeZone = computed(() => {
  if (asukaServerMapViewModel.value === null) {
    return false;
  }

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
        :location-id="asukaServerMapViewModel?.locationId ?? null"
        :is-safe-zone="isSafeZone"
        @open-location-wiki="emit('open-location-wiki', $event)"
      />
      <Enemies
        :enemy-ids="asukaServerMapViewModel?.enemies ?? []"
        :boss-ids="asukaServerMapViewModel?.boss ?? []"
        :fishing-ids="asukaServerMapViewModel?.fishing ?? []"
        :kicking-tree-ids="asukaServerMapViewModel?.kickingTree ?? []"
        :npcs="asukaServerMapViewModel?.npcs ?? []"
        @open-enemy-modal="emit('open-enemy-modal', $event)"
        @open-npc-modal="emit('open-npc-modal', $event)"
      />
    </div>

    <div class="flex-1 min-h-0" />
  </div>
</template>
