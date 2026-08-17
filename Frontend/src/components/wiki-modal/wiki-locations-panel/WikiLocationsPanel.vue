<script setup lang="ts">
import { computed, nextTick, ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import { ImageCatalog } from "@/catalogs/image.catalog";
import MapFrame from "@/components/map-frame/MapFrame.vue";
import { IconConstant } from "@/constants/icon.constant";
import { LocationRepository } from "@/repositories/location.repository";
import type { EnemyLocationViewModel } from "@/viewmodels/enemy/enemy-location.viewmodel";
import type { EnemyLocationSourceViewModel } from "@/viewmodels/enemy/enemy-location-source.viewmodel";
import type { MapFrameSlideViewModel } from "@/viewmodels/map-frame/map-frame-slide.viewmodel";
import type { CoordinatesViewModel } from "@/viewmodels/quest/coordinates.viewmodel";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

const props = defineProps<{
  enemy: EnemyViewModel;
}>();

const { t } = useI18n();

const chipElementsById = ref<Record<string, HTMLElement | null>>({});

const locationEntries = computed((): EnemyLocationViewModel[] => {
  return [...(props.enemy.locations ?? [])].sort((first, second) => {
    return first.id.localeCompare(second.id);
  });
});

const selectedId = ref<string | null>(null);

const setChipElementRef = (locationId: string, element: unknown): void => {
  if (element instanceof HTMLElement) {
    chipElementsById.value[locationId] = element;
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
  locationEntries,
  (entries) => {
    if (entries.length === 0) {
      selectedId.value = null;
      return;
    }

    const stillSelected = entries.some((entry) => {
      return entry.id === selectedId.value;
    });

    if (!stillSelected) {
      selectedId.value = entries[0]?.id ?? null;
    }
  },
  { immediate: true },
);

watch(selectedId, () => {
  void scrollSelectedChipIntoCenter();
});

const selectedEntry = computed(() => {
  return (
    locationEntries.value.find((entry) => {
      return entry.id === selectedId.value;
    }) ?? null
  );
});

const asukaMapImageUrl = computed(() => {
  return ImageCatalog.getLocationImageUrl("Asuka");
});

const selectedLocationImageUrl = computed(() => {
  if (selectedEntry.value === null) {
    return null;
  }

  const locationRaw = LocationRepository.getLocationById(selectedEntry.value.id);
  return ImageCatalog.getLocationImageUrl(locationRaw.imageName);
});

const asukaCoordinates = computed((): CoordinatesViewModel | null => {
  if (selectedEntry.value === null) {
    return null;
  }

  return LocationRepository.getLocationById(selectedEntry.value.id).worldLocation ?? null;
});

const localCoordinates = computed((): CoordinatesViewModel | null => {
  return selectedEntry.value?.localCoordinates ?? null;
});

function buildSingleSlide(
  imageUrl: string,
  coordinates: CoordinatesViewModel | null,
): MapFrameSlideViewModel[] {
  if (coordinates === null) {
    return [
      {
        imageUrl,
        pins: [],
      },
    ];
  }

  return [
    {
      imageUrl,
      pins: [
        {
          coordinates,
        },
      ],
    },
  ];
}

const asukaSlides = computed((): MapFrameSlideViewModel[] => {
  if (asukaMapImageUrl.value === null) {
    return [];
  }

  return buildSingleSlide(asukaMapImageUrl.value, asukaCoordinates.value);
});

const localSlides = computed((): MapFrameSlideViewModel[] => {
  if (selectedLocationImageUrl.value === null) {
    return [];
  }

  return buildSingleSlide(selectedLocationImageUrl.value, localCoordinates.value);
});

const selectEntry = (entry: EnemyLocationViewModel): void => {
  selectedId.value = entry.id;
};

const sourceEmoji = (source: EnemyLocationSourceViewModel): string => {
  return IconConstant[source];
};
</script>

<template>
  <div class="p-4 flex flex-col gap-3 h-full min-h-0 overflow-hidden">
    <div class="flex shrink-0 items-center gap-3 min-h-6 min-w-0">
      <div class="locations-chips-scroll flex-1 min-w-0 overflow-x-auto">
        <div class="flex items-center gap-2 w-max pr-1 pb-1">
          <button
            v-for="entry in locationEntries"
            :key="entry.id"
            :ref="(element) => setChipElementRef(entry.id, element)"
            type="button"
            class="shrink-0 text-left px-2.5 py-1.5 rounded border transition-colors cursor-pointer"
            :class="
              selectedId === entry.id
                ? 'bg-blue-900/40 border-[#0033aa] text-blue-100'
                : 'border-transparent text-white hover:bg-blue-900/20'
            "
            @click="selectEntry(entry)"
          >
            <span class="inline-flex items-center gap-1.5 text-[11px] font-bold tracking-wide whitespace-nowrap">
              <span>{{ t(`location.${entry.id}`) }}</span>
              <span
                v-for="source in entry.sources"
                :key="`${entry.id}-${source}`"
                class="text-[12px] 2xl:text-[14px] leading-none"
                :aria-label="t(`enemy.locationSource.${source}`)"
              >
                <span class="inline-flex leading-none text-[1.2rem] -translate-y-1">{{ sourceEmoji(source) }}</span>
              </span>
            </span>
          </button>
        </div>
      </div>
    </div>

    <div class="flex-1 min-h-0 flex gap-4 items-center justify-center overflow-hidden">
      <MapFrame v-if="asukaSlides.length > 0" :slides="asukaSlides" />

      <MapFrame v-if="localSlides.length > 0" :slides="localSlides" />
      <div
        v-else
        class="flex items-center justify-center min-h-0 px-8"
      >
        <span class="text-xs text-gray-500 font-bold tracking-wide">
          {{ selectedEntry ? t(`location.${selectedEntry.id}`) : "" }}
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
