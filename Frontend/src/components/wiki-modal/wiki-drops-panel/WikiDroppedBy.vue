<script setup lang="ts">
import WikiDroppedByEnemy from "@/components/wiki-modal/wiki-drops-panel/WikiDroppedByEnemy.vue";
import type { WikiDroppedByEnemyViewModel } from "@/viewmodels/wiki-modal/wiki-dropped-by-enemy.viewmodel";

defineProps<{
  sources: WikiDroppedByEnemyViewModel[];
}>();

const emit = defineEmits<{
  (e: "open-enemy", enemyId: string): void;
}>();

const handleSelect = (enemyId: string): void => {
  emit("open-enemy", enemyId);
};
</script>

<template>
  <section
    class="shrink-0 w-full bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner"
  >
    <h4 class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-3">
      {{ $t("enemy.droppedBy") }}
    </h4>

    <p
      v-if="sources.length === 0"
      class="text-xs text-gray-400 italic"
    >
      {{ $t("enemy.droppedByNone") }}
    </p>
    <div
      v-else
      class="flex flex-wrap gap-2"
    >
      <WikiDroppedByEnemy
        v-for="source in sources"
        :key="`${source.enemyId}-${source.locationOnly ?? ''}`"
        :source="source"
        @select="handleSelect(source.enemyId)"
      />
    </div>
  </section>
</template>
