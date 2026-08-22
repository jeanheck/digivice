<script setup lang="ts">
import { computed } from "vue";
import WikiLocation from "@/components/wiki-modal/wiki-locations-panel/WikiLocation.vue";
import { WikiLocationsPanelPresenter } from "@/presenters/map/wiki-modal/wiki-locations-panel.presenter";
import { useGameStore } from "@/stores/use-game-store";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

const props = defineProps<{
  enemy: EnemyViewModel;
}>();

const emit = defineEmits<{
  (e: "open-location", locationId: string): void;
}>();

const store = useGameStore();

const mainQuest = computed(() => {
  return store.currentState?.journal?.mainQuest ?? null;
});

const locations = computed(() => {
  return WikiLocationsPanelPresenter.getViewModel(
    props.enemy.locations,
    null,
    mainQuest.value,
  ).locations;
});

const handleLocationSelect = (locationId: string): void => {
  emit("open-location", locationId);
};
</script>

<template>
  <div class="flex flex-col flex-1 min-h-0">
    <div class="flex-1 min-h-0 overflow-y-auto custom-scroll flex flex-col gap-1">
      <WikiLocation
        v-for="location in locations"
        :key="location.id"
        variant="list"
        :location="location"
        :is-selected="false"
        @select="handleLocationSelect(location.id)"
      />
    </div>
  </div>
</template>
