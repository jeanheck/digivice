<script setup lang="ts">
import { computed, ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import { ImageCatalog } from "@/catalogs/image.catalog";
import { LocationRepository } from "@/repositories/location.repository";
import type { EnemyLocationSource } from "@/repositories/tables/raws/enemy/enemy-location.raw";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

interface LocationListEntry {
  step: string;
  locationId: string;
  sources: EnemyLocationSource[];
}

const props = defineProps<{
  enemy: EnemyViewModel;
}>();

const emit = defineEmits<{
  (e: "back"): void;
}>();

const { t } = useI18n();

const locationEntries = computed((): LocationListEntry[] => {
  const locationsByStep = props.enemy.locations;
  if (locationsByStep === undefined) {
    return [];
  }

  const entries: LocationListEntry[] = [];
  const sortedSteps = Object.keys(locationsByStep).sort((firstStep, secondStep) => {
    return Number(firstStep) - Number(secondStep);
  });

  for (const step of sortedSteps) {
    const locationsAtStep = locationsByStep[step] ?? {};
    const sortedLocationIds = Object.keys(locationsAtStep).sort();

    for (const locationId of sortedLocationIds) {
      entries.push({
        step,
        locationId,
        sources: locationsAtStep[locationId].sources,
      });
    }
  }

  return entries;
});

const entryKey = (entry: LocationListEntry): string => {
  return `${entry.step}-${entry.locationId}`;
};

const selectedKey = ref<string | null>(null);

watch(
  locationEntries,
  (entries) => {
    if (entries.length === 0) {
      selectedKey.value = null;
      return;
    }

    const stillSelected = entries.some((entry) => {
      return entryKey(entry) === selectedKey.value;
    });

    if (!stillSelected) {
      selectedKey.value = entryKey(entries[0]);
    }
  },
  { immediate: true },
);

const selectedEntry = computed(() => {
  return (
    locationEntries.value.find((entry) => {
      return entryKey(entry) === selectedKey.value;
    }) ?? null
  );
});

const selectedLocationImageUrl = computed(() => {
  if (selectedEntry.value === null) {
    return null;
  }

  const locationRaw = LocationRepository.getLocationById(selectedEntry.value.locationId);
  return ImageCatalog.getLocationImageUrl(locationRaw.imageName);
});

const selectEntry = (entry: LocationListEntry): void => {
  selectedKey.value = entryKey(entry);
};
</script>

<template>
  <div class="p-4 flex flex-col gap-3 h-full min-h-0 overflow-hidden">
    <div class="relative flex shrink-0 items-center min-h-6">
      <button
        type="button"
        class="shrink-0 inline-flex items-center gap-1 text-[10px] 2xl:text-xs font-bold uppercase tracking-widest text-gray-400 hover:text-blue-300 transition-colors cursor-pointer"
        @click="emit('back')"
      >
        <span class="text-[1.2rem] -translate-y-0.5" aria-hidden="true">⬅️</span>
        {{ $t("enemy.back") }}
      </button>

      <div class="absolute inset-0 flex items-center justify-center pointer-events-none">
        <h3
          class="text-[10px] 2xl:text-xs font-bold uppercase tracking-widest text-blue-500"
        >
          {{ $t("enemy.locations") }}
        </h3>
      </div>
    </div>

    <div class="flex-1 min-h-0 grid grid-cols-[minmax(14rem,18rem)_1fr] gap-4">
      <div
        class="min-h-0 overflow-y-auto custom-scroll bg-[#000a1a] border border-blue-900/50 rounded p-2 shadow-inner flex flex-col gap-1.5"
      >
        <button
          v-for="entry in locationEntries"
          :key="entryKey(entry)"
          type="button"
          class="w-full text-left px-2.5 py-2 rounded border transition-colors cursor-pointer"
          :class="
            selectedKey === entryKey(entry)
              ? 'bg-blue-900/40 border-blue-400 text-blue-100'
              : 'border-transparent text-gray-300 hover:bg-blue-900/20'
          "
          @click="selectEntry(entry)"
        >
          <span class="block text-[11px] font-bold tracking-wide">
            {{ t(`location.${entry.locationId}`) }}
          </span>
          <span class="mt-1 flex flex-wrap items-center gap-1.5">
            <span
              v-for="source in entry.sources"
              :key="`${entryKey(entry)}-${source}`"
              class="inline-block px-1.5 py-0.5 rounded text-[9px] font-bold uppercase tracking-wider bg-amber-900/40 text-amber-300 border border-amber-700/50"
            >
              {{ t(`enemy.locationSource.${source}`) }}
            </span>
            <span
              v-if="Number(entry.step) > 0"
              class="text-[9px] font-normal text-gray-400"
            >
              {{ t("enemy.locationStep", { step: entry.step }) }}
            </span>
          </span>
        </button>
      </div>

      <div
        class="min-h-0 bg-[#000a1a] border border-blue-900/50 rounded p-3 shadow-inner flex items-center justify-center overflow-hidden"
      >
        <img
          v-if="selectedLocationImageUrl"
          :src="selectedLocationImageUrl"
          :alt="
            selectedEntry
              ? t(`location.${selectedEntry.locationId}`)
              : ''
          "
          class="max-w-full max-h-full object-contain"
        />
        <span v-else class="text-xs text-gray-500 font-bold tracking-wide">
          {{
            selectedEntry
              ? t(`location.${selectedEntry.locationId}`)
              : ""
          }}
        </span>
      </div>
    </div>
  </div>
</template>
