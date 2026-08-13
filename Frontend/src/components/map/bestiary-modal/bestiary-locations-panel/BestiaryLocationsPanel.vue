<script setup lang="ts">
import { computed, ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import { ImageCatalog } from "@/catalogs/image.catalog";
import { LocationRepository } from "@/repositories/location.repository";
import type { EnemyLocationRaw } from "@/repositories/tables/raws/enemy/enemy-location.raw";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

const props = defineProps<{
  enemy: EnemyViewModel;
}>();

const emit = defineEmits<{
  (e: "back"): void;
}>();

const { t } = useI18n();

const locationEntries = computed((): EnemyLocationRaw[] => {
  return [...(props.enemy.locations ?? [])].sort((first, second) => {
    return first.id.localeCompare(second.id);
  });
});

const selectedId = ref<string | null>(null);

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

const selectedEntry = computed(() => {
  return (
    locationEntries.value.find((entry) => {
      return entry.id === selectedId.value;
    }) ?? null
  );
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
          :key="entry.id"
          type="button"
          class="w-full text-left px-2.5 py-2 rounded border transition-colors cursor-pointer"
          :class="
            selectedId === entry.id
              ? 'bg-blue-900/40 border-blue-400 text-blue-100'
              : 'border-transparent text-gray-300 hover:bg-blue-900/20'
          "
          @click="selectEntry(entry)"
        >
          <span class="block text-[11px] font-bold tracking-wide">
            {{ t(`location.${entry.id}`) }}
          </span>
          <span class="mt-1 flex flex-wrap items-center gap-1.5">
            <span
              v-for="source in entry.sources"
              :key="`${entry.id}-${source}`"
              class="inline-block px-1.5 py-0.5 rounded text-[9px] font-bold uppercase tracking-wider bg-amber-900/40 text-amber-300 border border-amber-700/50"
            >
              {{ t(`enemy.locationSource.${source}`) }}
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
