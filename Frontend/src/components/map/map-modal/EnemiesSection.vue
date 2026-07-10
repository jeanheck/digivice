<script setup lang="ts">
import { computed } from "vue";
import Enemy from "@/components/map/map-modal/Enemy.vue";
import { ImageCatalog } from "@/catalogs/image.catalog.ts";
import { MapEnemiesPresenter } from "@/presenters/map/map-enemies.presenter.ts";
import { EnemyModalPresenter } from "@/presenters/map/enemy-modal.presenter.ts";

const props = defineProps<{
  enemyIds: string[];
  selectedEnemyId: string | null;
}>();

const emit = defineEmits<{
  (e: "select-enemy", enemyId: string): void;
}>();

const resumedEnemies = computed(() => {
  return MapEnemiesPresenter.getResumedEnemiesByIds(props.enemyIds);
});

const selectedEnemy = computed(() => {
  if (props.selectedEnemyId === null) {
    return null;
  }

  return EnemyModalPresenter.getEnemyById(props.selectedEnemyId);
});

const enemyImageUrl = computed(() => {
  if (selectedEnemy.value === null) {
    return null;
  }

  return ImageCatalog.getEnemyIconUrl(selectedEnemy.value.name);
});

const selectEnemy = (enemyId: string) => {
  emit("select-enemy", enemyId);
};
</script>

<template>
  <div class="flex flex-1 min-h-0 w-full flex-col p-4">
    <div class="flex flex-1 min-h-0 flex-wrap items-center justify-center gap-x-5 w-full">
      <button
        v-for="enemy in resumedEnemies"
        :key="enemy.id"
        type="button"
        class="font-bold tracking-wide transition-all flex items-center justify-center focus:outline-none rounded px-1 cursor-pointer"
        :class="selectedEnemyId === enemy.id
          ? 'text-sm text-[#ffd9d9] drop-shadow-[0_0_2px_rgba(158,55,55,0.8)]'
          : enemy.boss
            ? 'text-sm text-amber-400 drop-shadow-[0_0_5px_rgba(255,191,0,0.8)]'
            : 'text-sm text-[#bc3737] hover:text-[#ffd9d9] drop-shadow-[0_0_2px_rgba(158,55,55,0.8)]'"
        @click="selectEnemy(enemy.id)"
      >
        {{ enemy.name }}
      </button>
    </div>

    <div class="mt-auto min-h-0 w-full">
      <Enemy
        v-if="selectedEnemy"
        :enemy="selectedEnemy"
        :enemy-image-url="enemyImageUrl"
        class="min-h-130"
      />
    </div>
  </div>
</template>
