<script setup lang="ts">
import { computed, ref, watch } from "vue";
import Modal from "@/components/modal/Modal.vue";
import SeabedDock from "@/components/seabed/SeabedDock.vue";
import SeabedDocks from "@/components/seabed/SeabedDocks.vue";
import { SeabedModalPresenter } from "@/presenters/seabed/seabed-modal.presenter";
import { useGameStore } from "@/stores/use-game-store";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";
import { SEABED_MAP_FRAME_WIDTH_PX } from "@/components/seabed/seabed-map-frame";

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
  { immediate: true },
);

watch(
  () => props.isOpen,
  (isOpen) => {
    if (isOpen) {
      syncSelectedLocationIdFromProps();
    }
  },
);

const dockViewModel = computed(() => {
  return SeabedModalPresenter.getDockByLocationId(
    selectedLocationId.value,
    store.currentState?.journal?.mainQuest ?? null,
    store.currentState?.player?.seabedRoute ?? 0,
    store.currentState?.player?.previousMapId ?? "",
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
    max-height="h-[650px] max-h-[650px]"
    panel-class="w-[1300px]"
    @close="closeModal"
  >
    <template #header>
      <h2 class="text-white font-bold tracking-widest drop-shadow whitespace-nowrap shrink-0">
        {{ $t("map.seabedRoutes") }}
      </h2>
    </template>

    <div class="flex flex-1 min-h-0 h-full w-full p-4 overflow-visible items-center justify-center">
      <div class="flex gap-4 items-center min-h-0 max-h-full">
        <SeabedDocks @select-dock="onSelectDock" />
        <SeabedDock
          v-if="dockViewModel?.imageUrl"
          :image-url="dockViewModel.imageUrl"
          :coordinates="dockViewModel.coordinates ?? null"
        />
        <div
          v-else
          class="flex flex-col items-center justify-center gap-3 px-8"
          :style="{ width: `${SEABED_MAP_FRAME_WIDTH_PX}px` }"
        >
          <span class="text-cyan-500/50 text-sm tracking-widest text-center animate-pulse">
            {{ $t("map.noDock") }}
          </span>
          <span class="text-cyan-500/50 text-sm tracking-widest text-center animate-pulse">
            {{ $t("map.noDockHint") }}
          </span>
        </div>
      </div>
    </div>
  </Modal>
</template>
