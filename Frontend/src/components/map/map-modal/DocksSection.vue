<script setup lang="ts">
import { computed, ref, watch } from "vue";
import asukaMapUrl from "@/assets/AsukaMap.webp";
import Dock from "@/components/map/map-modal/Dock.vue";
import Docks from "@/components/map/map-modal/Docks.vue";
import { DocksSectionPresenter } from "@/presenters/map/docks-section.presenter";
import { useGameStore } from "@/stores/use-game-store";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

const props = defineProps<{
  location: LocationViewModel | null;
}>();

const store = useGameStore();

const selectedLocationId = ref<string | null>(null);

function syncSelectedLocationIdFromProps(): void {
  if (props.location !== null && props.location.dock === true) {
    selectedLocationId.value = props.location.id;
    return;
  }

  selectedLocationId.value = null;
}

watch(
  () => props.location,
  () => {
    syncSelectedLocationIdFromProps();
  },
  { immediate: true }
);

const dockViewModel = computed(() => {
  return DocksSectionPresenter.getDockByLocationId(
    selectedLocationId.value,
    store.currentState?.journal?.mainQuest ?? null
  );
});

function onSelectDock(locationId: string): void {
  selectedLocationId.value = locationId;
}
</script>

<template>
  <div class="flex flex-1 min-h-0 w-full gap-4 p-4 overflow-y-auto overflow-x-hidden">
    <div class="flex flex-1 min-w-0 min-h-0 max-h-full items-center justify-center">
      <Docks :image-url="asukaMapUrl" @select-dock="onSelectDock" />
    </div>
    <div class="flex flex-1 min-w-0 min-h-0 max-h-full items-center justify-center">
      <Dock
        :image-url="dockViewModel?.imageUrl ?? null"
        :coordinates="dockViewModel?.coordinates ?? null"
      />
    </div>
  </div>
</template>
