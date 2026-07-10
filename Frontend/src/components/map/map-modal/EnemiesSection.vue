<script setup lang="ts">
import { computed, ref, watch } from "vue";
import EnemyData from "@/components/map/map-modal/EnemyData.vue";
import { ImageCatalog } from "@/catalogs/image.catalog.ts";
import { MapEnemiesPresenter } from "@/presenters/map/map-enemies.presenter.ts";
import { EnemyModalPresenter } from "@/presenters/map/enemy-modal.presenter.ts";

const props = defineProps<{
  enemyIds: string[];
}>();

const resumedEnemies = computed(() => {
  return MapEnemiesPresenter.getResumedEnemiesByIds(props.enemyIds);
});

const selectedEnemyId = ref<string | null>(null);

const selectEnemy = (enemyId: string) => {
  selectedEnemyId.value = enemyId;
};

const selectedEnemy = computed(() => {
  if (selectedEnemyId.value === null) {
    return null;
  }

  return EnemyModalPresenter.getEnemyById(selectedEnemyId.value);
});

const enemyImageUrl = computed(() => {
  if (selectedEnemy.value === null) {
    return null;
  }

  return ImageCatalog.getEnemyIconUrl(selectedEnemy.value.name);
});

watch(
  () => props.enemyIds,
  () => {
    selectedEnemyId.value = null;
  }
);
</script>

<template>
  <div class="flex flex-1 min-h-0 w-full flex-col p-4 gap-4">
    <div class="flex flex-wrap items-center justify-center gap-x-5 gap-y-1 w-full shrink-0">
      <button
        v-for="enemy in resumedEnemies"
        :key="enemy.id"
        type="button"
        class="font-bold tracking-wide transition-all flex items-center justify-center focus:outline-none rounded px-1 cursor-pointer"
        :class="selectedEnemyId === enemy.id
          ? 'text-sm text-[#ffe4e4] drop-shadow-[0_0_2px_rgba(158,55,55,0.8)]'
          : enemy.boss
            ? 'text-sm text-amber-400 drop-shadow-[0_0_5px_rgba(255,191,0,0.8)]'
            : 'text-sm text-[#bc3737] hover:text-[#ffe4e4] drop-shadow-[0_0_2px_rgba(158,55,55,0.8)]'"
        @click="selectEnemy(enemy.id)"
      >
        {{ enemy.name }}
      </button>
    </div>

    <EnemyData
      v-if="selectedEnemy"
      :enemy="selectedEnemy"
      :enemy-image-url="enemyImageUrl"
      class="flex-1 min-h-0"
    />
  </div>
</template>
