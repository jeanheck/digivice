<script setup lang="ts">
import { computed, onUnmounted, ref, watch, type Ref } from "vue";
import MapFrame from "@/components/map-frame/MapFrame.vue";
import WikiLocalMap from "@/components/wiki-modal/wiki-locations-panel/WikiLocalMap.vue";
import WikiLocationEncounters from "@/components/wiki-modal/wiki-locations-panel/WikiLocationEncounters.vue";
import { ImageCatalog } from "@/catalogs/image.catalog";
import { MAP_FRAME_WIDTH_PX } from "@/constants/map-display.constant";
import { useImageNaturalAspectRatio } from "@/composables/use-image-natural-aspect-ratio";
import { WikiLocationsPanelPresenter } from "@/presenters/map/wiki-modal/wiki-locations-panel.presenter";
import { useGameStore } from "@/stores/use-game-store";

const props = defineProps<{
  locationId: string;
}>();

const emit = defineEmits<{
  (e: "open-enemy", enemyId: string): void;
  (e: "open-npc", npcId: string): void;
}>();

const store = useGameStore();

const mainQuest = computed(() => {
  return store.currentState?.journal?.mainQuest ?? null;
});

const sideQuests = computed(() => {
  return store.currentState?.journal?.sideQuests ?? [];
});

const previousMapId = computed(() => {
  return store.currentState?.player?.previousMapId ?? "";
});

const locationsViewModel = computed(() => {
  return WikiLocationsPanelPresenter.getLocationPanelViewModel(
    props.locationId,
    mainQuest.value,
    sideQuests.value,
    previousMapId.value,
  );
});

const asukaImageUrl = computed(() => {
  return ImageCatalog.getLocationImageUrl("Asuka");
});

const localImageUrl = computed(() => {
  return locationsViewModel.value.localSlides[0]?.imageUrl ?? null;
});

const { aspectRatio: asukaAspectRatio } = useImageNaturalAspectRatio(asukaImageUrl);
const { aspectRatio: localAspectRatio } = useImageNaturalAspectRatio(localImageUrl);

const LOCAL_MAP_MAX_SLOT_RATIO = 0.95;

function computeMapWidth(
  slotHeightPx: number,
  aspectRatio: number | null,
  slotWidthPx: number = MAP_FRAME_WIDTH_PX,
  maxSlotRatio: number = 1,
): number {
  if (aspectRatio === null || slotHeightPx === 0) {
    return MAP_FRAME_WIDTH_PX;
  }

  const maxWidth = Math.floor(slotWidthPx * maxSlotRatio);
  const maxHeight = Math.floor(slotHeightPx * maxSlotRatio);
  const widthIfFullWidth = maxWidth;
  const heightIfFullWidth = widthIfFullWidth * aspectRatio;

  if (heightIfFullWidth <= maxHeight) {
    return Math.max(1, widthIfFullWidth);
  }

  return Math.max(1, Math.floor(maxHeight / aspectRatio));
}

function observeMapSlot(element: HTMLElement | null, heightRef: Ref<number>): ResizeObserver | null {
  if (element === null) {
    heightRef.value = 0;
    return null;
  }

  const observer = new ResizeObserver((entries) => {
    const entry = entries[0];
    if (entry !== undefined) {
      heightRef.value = Math.round(entry.contentRect.height);
    }
  });

  observer.observe(element);
  heightRef.value = Math.round(element.getBoundingClientRect().height);
  return observer;
}

const worldMapSlotElement = ref<HTMLElement | null>(null);
const localMapSlotElement = ref<HTMLElement | null>(null);
const worldMapSlotHeightPx = ref(0);
const localMapSlotHeightPx = ref(0);

let worldMapResizeObserver: ResizeObserver | null = null;
let localMapResizeObserver: ResizeObserver | null = null;

watch(worldMapSlotElement, (element) => {
  worldMapResizeObserver?.disconnect();
  worldMapResizeObserver = observeMapSlot(element, worldMapSlotHeightPx);
});

watch(localMapSlotElement, (element) => {
  localMapResizeObserver?.disconnect();
  localMapResizeObserver = observeMapSlot(element, localMapSlotHeightPx);
});

onUnmounted(() => {
  worldMapResizeObserver?.disconnect();
  localMapResizeObserver?.disconnect();
});

const worldMapWidth = computed(() => {
  return computeMapWidth(worldMapSlotHeightPx.value, asukaAspectRatio.value);
});

const localMapWidth = computed(() => {
  return computeMapWidth(
    localMapSlotHeightPx.value,
    localAspectRatio.value,
    MAP_FRAME_WIDTH_PX,
    LOCAL_MAP_MAX_SLOT_RATIO,
  );
});

const handleOpenEnemy = (enemyId: string): void => {
  emit("open-enemy", enemyId);
};

const handleOpenNpc = (npcId: string): void => {
  emit("open-npc", npcId);
};
</script>

<template>
  <div class="p-4 flex flex-col h-full min-h-0 overflow-hidden">
    <div class="flex-1 min-h-0 flex gap-4 items-stretch justify-center overflow-hidden">
      <div
        class="flex flex-col min-h-0 h-full self-stretch shrink-0"
        :style="{ width: `${MAP_FRAME_WIDTH_PX}px` }"
      >
        <WikiLocationEncounters
          class="flex-[0_0_25%] min-h-0 overflow-y-auto custom-scroll"
          :lines="locationsViewModel.encounterLines"
          @open-enemy="handleOpenEnemy"
        />

        <div
          ref="worldMapSlotElement"
          class="flex-[0_0_75%] min-h-0 flex flex-col justify-end items-center"
        >
          <MapFrame
            v-if="locationsViewModel.asukaSlides.length > 0"
            :slides="locationsViewModel.asukaSlides"
            :width="worldMapWidth"
            :max-height="null"
          />
        </div>
      </div>

      <div
        v-if="locationsViewModel.localSlides.length > 0"
        ref="localMapSlotElement"
        class="h-full min-h-0 shrink-0 flex flex-col justify-center items-center"
        :style="{ width: `${MAP_FRAME_WIDTH_PX}px` }"
      >
        <WikiLocalMap
          :image-url="localImageUrl"
          :width="localMapWidth"
          :markers="locationsViewModel.mapMarkers"
          @open-enemy="handleOpenEnemy"
          @open-npc="handleOpenNpc"
        />
      </div>
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
