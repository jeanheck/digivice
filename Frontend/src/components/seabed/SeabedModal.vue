<script setup lang="ts">
import { computed, ref, watch } from "vue";
import Modal from "@/components/modal/Modal.vue";
import asukaMapUrl from "@/assets/AsukaMap.webp";
import Dock from "@/components/seabed/Dock.vue";
import Docks from "@/components/seabed/Docks.vue";
import { DocksSectionPresenter } from "@/presenters/map/docks-section.presenter";
import { useGameStore } from "@/stores/use-game-store";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

const props = defineProps<{
  isOpen: boolean;
  location: LocationViewModel | null;
}>();

const emit = defineEmits<{
  (e: "close"): void;
}>();

const store = useGameStore();

const isModalOpen = computed(() => {
  return props.isOpen;
});

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

watch(
  () => props.isOpen,
  (isOpen) => {
    if (isOpen) {
      syncSelectedLocationIdFromProps();
    }
  }
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

const closeModal = () => {
  emit("close");
};
</script>

<template>
  <Modal
    :is-open="isModalOpen"
    max-width="max-w-[1300px]"
    max-height="h-[650px] max-h-[620px]"
    panel-class="w-[1300px]"
    @close="closeModal"
  >
    <template #header>
      <h2 class="text-white font-bold tracking-widest drop-shadow whitespace-nowrap shrink-0">
        {{ $t("map.seabedRoutes") }}
      </h2>
    </template>

    <div class="flex flex-1 min-h-0 h-full w-full p-4 overflow-hidden items-start justify-center">
      <div class="flex gap-4 items-start min-h-0 max-h-full">
        <Docks :image-url="asukaMapUrl" @select-dock="onSelectDock" />
        <Dock
          :image-url="dockViewModel?.imageUrl ?? null"
          :coordinates="dockViewModel?.coordinates ?? null"
        />
      </div>
    </div>
  </Modal>
</template>
