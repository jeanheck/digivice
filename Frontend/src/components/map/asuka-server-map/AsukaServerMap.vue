<script setup lang="ts">
import { computed } from "vue";
import Location from "../Location.vue";
import Enemies from "../Enemies.vue";
import { useGameStore } from "@/stores/use-game-store";
import { AsukaServerMapPresenter } from "@/presenters/map/asuka-server-map.presenter.ts";

const emit = defineEmits<{
  (e: "open-enemy-modal", enemyId: string): void;
}>();

const store = useGameStore();

const mapViewModel = computed(() => {
  const locationId = store.currentState?.player?.location ?? null;
  if (locationId === null) {
    return null;
  }

  const mainQuest = store.currentState?.journal?.mainQuest ?? null;
  const previousMapId = store.currentState?.player?.previousMapId ?? "";

  return AsukaServerMapPresenter.getViewModel(locationId, mainQuest, previousMapId);
});
</script>

<template>
  <div class="relative z-10 flex flex-col flex-1 min-h-0 pt-1">
    <div class="flex flex-col items-center gap-2 shrink-0">
      <Location :location-id="mapViewModel?.locationId ?? null" />
      <Enemies
        :enemy-ids="mapViewModel?.enemies ?? []"
        @open-enemy-modal="emit('open-enemy-modal', $event)"
      />
    </div>

    <div class="flex-1 min-h-0" />
  </div>
</template>
