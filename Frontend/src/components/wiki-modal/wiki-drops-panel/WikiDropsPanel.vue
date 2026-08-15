<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { ImageCatalog } from "@/catalogs/image.catalog";
import { DropRepository } from "@/repositories/drop.repository";
import { WikiModalPresenter } from "@/presenters/map/wiki-modal.presenter";

const props = defineProps<{
  dropId: string;
}>();

const emit = defineEmits<{
  (e: "open-enemy", enemyId: string): void;
}>();

const { t } = useI18n();

const dropType = computed(() => {
  const dropRaw = DropRepository.getDropByKey(props.dropId);
  return dropRaw?.type ?? null;
});

const dropSources = computed(() => {
  return WikiModalPresenter.getDropSources(props.dropId);
});

const locationOnlyLabel = (locationOnly: string): string => {
  return t("enemy.locationOnly", { location: t(`location.${locationOnly}`) });
};

const enemyIconUrl = (enemyName: string): string | null => {
  return ImageCatalog.getEnemyIconUrl(enemyName);
};

const handleOpenEnemy = (enemyId: string): void => {
  emit("open-enemy", enemyId);
};
</script>

<template>
  <div class="p-4 flex flex-col gap-4 h-full min-h-0 overflow-hidden">
    <section
      class="flex-1 w-full min-h-0 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-col"
    >
      <h4 class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-2">
        {{ t("enemy.dropDetails") }}
      </h4>
      <!-- Drop detail placeholder (type-specific components later) -->
      <p class="text-xs text-gray-400 italic">
        <template v-if="dropType !== null">
          {{ dropType }}
        </template>
      </p>
    </section>

    <section
      class="shrink-0 w-full bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner"
    >
      <h4 class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-3">
        {{ t("enemy.droppedBy") }}
      </h4>

      <p
        v-if="dropSources.length === 0"
        class="text-xs text-gray-400 italic"
      >
        {{ t("enemy.droppedByNone") }}
      </p>
      <div
        v-else
        class="flex flex-wrap gap-2"
      >
        <button
          v-for="source in dropSources"
          :key="`${source.enemyId}-${source.locationOnly ?? ''}`"
          type="button"
          class="flex items-center gap-2 px-2.5 py-2 rounded text-left transition-colors cursor-pointer bg-blue-900/30 hover:bg-blue-900/50 border border-blue-800/60"
          @click="handleOpenEnemy(source.enemyId)"
        >
          <img
            v-if="enemyIconUrl(source.enemyName)"
            :src="enemyIconUrl(source.enemyName)!"
            :alt="source.enemyName"
            class="w-8 h-8 object-contain rendering-pixelated"
          />
          <span class="min-w-0">
            <span class="block text-xs font-bold text-blue-200 tracking-wide">
              {{ source.enemyName }}
            </span>
            <span
              v-if="source.locationOnly"
              class="block text-[10px] text-gray-400"
            >
              {{ locationOnlyLabel(source.locationOnly) }}
            </span>
          </span>
        </button>
      </div>
    </section>
  </div>
</template>
