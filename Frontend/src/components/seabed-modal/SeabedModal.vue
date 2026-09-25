<script setup lang="ts">
import { computed, ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import Modal from "@/components/modal/Modal.vue";
import MapFrame from "@/components/map-frame/MapFrame.vue";
import SeabedDocks from "@/components/seabed-modal/SeabedDocks.vue";
import { MAP_FRAME_WIDTH_PX } from "@/constants/map-display.constant";
import { SeabedModalPresenter } from "@/presenters/seabed-modal/seabed-modal.presenter";
import { useGameStore } from "@/stores/use-game-store";
import type { MapFrameSlideViewModel } from "@/viewmodels/map-frame/map-frame-slide.viewmodel";

const props = defineProps<{
  isOpen: boolean;
}>();

const emit = defineEmits<{
  (e: "close"): void;
}>();

const store = useGameStore();
const { t } = useI18n();

const isModalOpen = computed(() => {
  return props.isOpen;
});

const selectedLocationId = ref<string | null>(null);

function syncSelectedLocationIdFromPlayer(): void {
  const playerLocationId = store.currentState?.player?.location ?? null;
  selectedLocationId.value = SeabedModalPresenter.getInitialSelectedLocationId(playerLocationId);
}

watch(
  () => props.isOpen,
  (isOpen) => {
    if (isOpen) {
      syncSelectedLocationIdFromPlayer();
    }
  },
  { immediate: true },
);

const seabedModalViewModel = computed(() => {
  return SeabedModalPresenter.getViewModel(selectedLocationId.value);
});

function onSelectDock(locationId: string): void {
  selectedLocationId.value = locationId;
}

const dockSlides = computed((): MapFrameSlideViewModel[] => {
  const viewModel = seabedModalViewModel.value;
  if (viewModel === null || viewModel.imageUrl === null) {
    return [];
  }

  return [
    {
      imageUrl: viewModel.imageUrl,
      pins: [
        {
          coordinates: viewModel.coordinates,
          label: t("map.dock"),
        },
      ],
    },
  ];
});

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
        {{ $t("map.seabed") }}
      </h2>
    </template>

    <div class="flex flex-1 min-h-0 h-full w-full p-4 overflow-visible items-center justify-center">
      <div class="flex gap-4 items-center min-h-0 max-h-full">
        <SeabedDocks @select-dock="onSelectDock" />
        <MapFrame v-if="dockSlides.length > 0" :slides="dockSlides" />
        <div
          v-else
          class="flex flex-col items-center justify-center gap-3 px-8"
          :style="{ width: `${MAP_FRAME_WIDTH_PX}px` }"
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
