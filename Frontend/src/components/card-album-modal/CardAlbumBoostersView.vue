<script setup lang="ts">
import { computed, ref } from "vue";
import CardAlbumBoosterCard from "@/components/card-album-modal/CardAlbumBoosterCard.vue";
import { useGameState } from "@/composables/use-game-state";
import { CardAlbumBoosterPresenter } from "@/presenters/card-album/card-album-booster.presenter";

const emit = defineEmits<{
  (e: "open-booster", boosterId: number): void;
  (e: "switch-to-cards"): void;
}>();

const gameState = useGameState();

type BoosterFilter = "all" | "seriesA" | "seriesB" | "seriesR" | "incomplete" | "complete";
const boosterFilter = ref<BoosterFilter>("all");

const boosterList = computed(() => {
  return CardAlbumBoosterPresenter.getItems(gameState.value.cardInventory);
});

const totalBoosters = computed(() => boosterList.value.length);
const completedBoostersCount = computed(() => boosterList.value.filter((b) => b.isComplete).length);

const filteredBoosters = computed(() => {
  if (boosterFilter.value === "complete") {
    return boosterList.value.filter((b) => b.isComplete);
  }
  if (boosterFilter.value === "incomplete") {
    return boosterList.value.filter((b) => !b.isComplete);
  }
  if (boosterFilter.value === "seriesA") {
    return boosterList.value.filter((b) => b.series === "A");
  }
  if (boosterFilter.value === "seriesB") {
    return boosterList.value.filter((b) => b.series === "B");
  }
  if (boosterFilter.value === "seriesR") {
    return boosterList.value.filter((b) => b.series === "R");
  }
  return boosterList.value;
});

const handleSelectBooster = (boosterId: number) => {
  emit("open-booster", boosterId);
};
</script>

<template>
  <div class="flex-1 min-h-0 flex flex-col overflow-hidden">
    <!-- Boosters Cards Grid Container -->
    <div class="flex-1 min-h-0 bg-[#000a18]/60 border border-blue-900/40 rounded-lg p-3 shadow-inner overflow-y-auto custom-scroll">
      <div class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-3.5">
        <CardAlbumBoosterCard
          v-for="booster in filteredBoosters"
          :key="booster.id"
          :booster="booster"
          @select="handleSelectBooster"
        />
      </div>
    </div>

    <!-- Boosters Bottom Control Bar with Filter Tabs -->
    <div class="flex items-center justify-between mt-3 pt-2 border-t border-blue-900/50 shrink-0">
      <!-- Filter Tabs -->
      <div class="flex items-center gap-1.5 flex-wrap">
        <button
          type="button"
          class="px-2.5 py-1 rounded text-xs font-bold transition-colors cursor-pointer border"
          :class="
            boosterFilter === 'all'
              ? 'bg-cyan-950 border-cyan-500 text-cyan-300 shadow-xs'
              : 'bg-blue-950/60 border-blue-800 text-gray-400 hover:text-white'
          "
          @click="boosterFilter = 'all'"
        >
          {{ $t("cardAlbum.allBoosters") }} ({{ totalBoosters }})
        </button>
        <button
          type="button"
          class="px-2.5 py-1 rounded text-xs font-bold transition-colors cursor-pointer border"
          :class="
            boosterFilter === 'seriesA'
              ? 'bg-blue-950 border-cyan-400 text-cyan-300 shadow-xs'
              : 'bg-blue-950/60 border-blue-800 text-cyan-400/80 hover:text-white'
          "
          @click="boosterFilter = 'seriesA'"
        >
          {{ $t("cardAlbum.seriesA") }}
        </button>
        <button
          type="button"
          class="px-2.5 py-1 rounded text-xs font-bold transition-colors cursor-pointer border"
          :class="
            boosterFilter === 'seriesB'
              ? 'bg-rose-950 border-rose-500 text-rose-300 shadow-xs'
              : 'bg-blue-950/60 border-blue-800 text-rose-400/80 hover:text-white'
          "
          @click="boosterFilter = 'seriesB'"
        >
          {{ $t("cardAlbum.seriesB") }}
        </button>
        <button
          type="button"
          class="px-2.5 py-1 rounded text-xs font-bold transition-colors cursor-pointer border"
          :class="
            boosterFilter === 'seriesR'
              ? 'bg-amber-950 border-amber-500 text-amber-300 shadow-xs'
              : 'bg-blue-950/60 border-blue-800 text-amber-400/80 hover:text-white'
          "
          @click="boosterFilter = 'seriesR'"
        >
          {{ $t("cardAlbum.seriesR") }}
        </button>
        <button
          type="button"
          class="px-2.5 py-1 rounded text-xs font-bold transition-colors cursor-pointer border"
          :class="
            boosterFilter === 'incomplete'
              ? 'bg-amber-950/80 border-amber-500 text-amber-300 shadow-xs'
              : 'bg-blue-950/60 border-blue-800 text-gray-400 hover:text-white'
          "
          @click="boosterFilter = 'incomplete'"
        >
          {{ $t("cardAlbum.incomplete") }} ({{ totalBoosters - completedBoostersCount }})
        </button>
        <button
          type="button"
          class="px-2.5 py-1 rounded text-xs font-bold transition-colors cursor-pointer border"
          :class="
            boosterFilter === 'complete'
              ? 'bg-emerald-950 border-emerald-500 text-emerald-300 shadow-xs'
              : 'bg-blue-950/60 border-blue-800 text-gray-400 hover:text-white'
          "
          @click="boosterFilter = 'complete'"
        >
          {{ $t("cardAlbum.complete") }} ({{ completedBoostersCount }})
        </button>
      </div>

      <!-- Switch Back to Cards Button -->
      <button
        type="button"
        class="px-3 py-1 rounded text-xs font-bold bg-blue-950/90 border border-blue-700 text-cyan-300 hover:bg-blue-900 hover:text-white hover:border-cyan-400 transition-colors cursor-pointer flex items-center gap-1.5 shadow-xs shrink-0 ml-2"
        @click="emit('switch-to-cards')"
      >
        <span>🎴</span>
        <span>{{ $t("cardAlbum.cards") }}</span>
      </button>
    </div>
  </div>
</template>
