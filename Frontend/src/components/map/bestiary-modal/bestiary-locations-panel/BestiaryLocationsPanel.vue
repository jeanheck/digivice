<script setup lang="ts">
import { computed, nextTick, ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import { ImageCatalog } from "@/catalogs/image.catalog";
import { LocationRepository } from "@/repositories/location.repository";
import type {
  EnemyLocationRaw,
  EnemyLocationSource,
} from "@/repositories/tables/raws/enemy/enemy-location.raw";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

const LOCATION_SOURCE_EMOJI: Record<EnemyLocationSource, string> = {
  walking: "🏃‍➡️",
  fishing: "🎣",
  kickingTree: "🌴",
  boss: "☠️",
};

const props = defineProps<{
  enemy: EnemyViewModel;
}>();

const { t } = useI18n();

const chipElementsById = ref<Record<string, HTMLElement | null>>({});

const locationEntries = computed((): EnemyLocationRaw[] => {
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
      selectedId.value = entries[0].id;
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

const selectEntry = (entry: EnemyLocationRaw): void => {
  selectedId.value = entry.id;
};

const sourceEmoji = (source: EnemyLocationSource): string => {
  return LOCATION_SOURCE_EMOJI[source];
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

    <div class="flex-1 min-h-0 grid grid-cols-2 gap-4">
      <div
        class="min-h-0 bg-[#000a1a] border border-blue-900/50 rounded p-3 shadow-inner flex items-center justify-center overflow-hidden"
      >
        <img
          v-if="asukaMapImageUrl"
          :src="asukaMapImageUrl"
          alt="Asuka"
          class="max-w-full max-h-full object-contain"
        />
      </div>

      <div
        class="min-h-0 bg-[#000a1a] border border-blue-900/50 rounded p-3 shadow-inner flex items-center justify-center overflow-hidden"
      >
        <img
          v-if="selectedLocationImageUrl"
          :src="selectedLocationImageUrl"
          :alt="selectedEntry ? t(`location.${selectedEntry.id}`) : ''"
          class="max-w-full max-h-full object-contain"
        />
        <span v-else class="text-xs text-gray-500 font-bold tracking-wide">
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
