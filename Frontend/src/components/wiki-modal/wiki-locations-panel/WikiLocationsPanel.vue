<script setup lang="ts">
import { computed, nextTick, ref, watch } from "vue";
import MapFrame from "@/components/map-frame/MapFrame.vue";
import WikiLocation from "@/components/wiki-modal/wiki-locations-panel/WikiLocation.vue";
import { WikiLocationsPanelPresenter } from "@/presenters/map/wiki-modal/wiki-locations-panel.presenter";
import { useGameStore } from "@/stores/use-game-store";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

const props = defineProps<{
  enemy: EnemyViewModel;
}>();

const store = useGameStore();

const chipElementsById = ref<Record<string, HTMLElement | null>>({});

const selectedId = ref<string | null>(null);

const mainQuest = computed(() => {
  return store.currentState?.journal?.mainQuest ?? null;
});

const locationsViewModel = computed(() => {
  return WikiLocationsPanelPresenter.getViewModel(
    props.enemy.locations,
    selectedId.value,
    mainQuest.value,
  );
});

const setChipElementRef = (locationId: string, component: unknown): void => {
  if (
    component !== null &&
    typeof component === "object" &&
    "rootButton" in component &&
    component.rootButton instanceof HTMLElement
  ) {
    chipElementsById.value[locationId] = component.rootButton;
    return;
  }

  delete chipElementsById.value[locationId];
};

const scrollSelectedChipIntoCenter = async (): Promise<void> => {
  await nextTick();

  if (selectedId.value === null) {
    return;
  }

  const chipElement = chipElementsById.value[selectedId.value];
  if (chipElement === undefined || chipElement === null) {
    return;
  }

  chipElement.scrollIntoView({
    behavior: "smooth",
    inline: "center",
    block: "nearest",
  });
};

watch(
  () => props.enemy.locations,
  (locations) => {
    const entries = locations ?? [];
    if (entries.length === 0) {
      selectedId.value = null;
      return;
    }

    const stillSelected = entries.some((entry) => {
      return entry.id === selectedId.value;
    });

    if (!stillSelected) {
      const sortedEntries = [...entries].sort((first, second) => {
        return first.id.localeCompare(second.id);
      });
      selectedId.value = sortedEntries[0]?.id ?? null;
    }
  },
  { immediate: true },
);

watch(selectedId, () => {
  void scrollSelectedChipIntoCenter();
});

const selectLocation = (locationId: string): void => {
  selectedId.value = locationId;
};
</script>

<template>
  <div class="p-4 flex flex-col gap-3 h-full min-h-0 overflow-hidden">
    <div class="flex shrink-0 items-center gap-3 min-h-6 min-w-0">
      <div class="locations-chips-scroll flex-1 min-w-0 overflow-x-auto">
        <div class="flex items-center gap-2 w-max pr-1 pb-1">
          <WikiLocation
            v-for="location in locationsViewModel.locations"
            :key="location.id"
            :ref="(element) => setChipElementRef(location.id, element)"
            :location="location"
            :is-selected="selectedId === location.id"
            @select="selectLocation(location.id)"
          />
        </div>
      </div>
    </div>

    <div class="flex-1 min-h-0 flex gap-4 items-center justify-center overflow-hidden">
      <MapFrame v-if="locationsViewModel.asukaSlides.length > 0" :slides="locationsViewModel.asukaSlides" />

      <MapFrame v-if="locationsViewModel.localSlides.length > 0" :slides="locationsViewModel.localSlides" />
      <div
        v-else
        class="flex items-center justify-center min-h-0 px-8"
      >
        <span class="text-xs text-gray-500 font-bold tracking-wide">
          {{
            locationsViewModel.selectedLocationLabelKey
              ? $t(locationsViewModel.selectedLocationLabelKey)
              : ""
          }}
        </span>
      </div>
    </div>
  </div>
</template>

<style scoped>
.locations-chips-scroll::-webkit-scrollbar {
  height: 8px;
}

.locations-chips-scroll::-webkit-scrollbar-track {
  background: rgba(0, 0, 0, 0.2);
  border-radius: 4px;
}

.locations-chips-scroll::-webkit-scrollbar-thumb {
  background: #0033aa;
  border-radius: 4px;
}

.locations-chips-scroll::-webkit-scrollbar-thumb:hover {
  background: #0077ff;
}
</style>
