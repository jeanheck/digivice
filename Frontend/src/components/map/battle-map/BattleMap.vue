<script setup lang="ts">
import { computed, ref } from "vue";
import { useI18n } from "vue-i18n";
import { ImageCatalog } from "@/catalogs/image.catalog";
import HpProgressBar from "@/components/party/digimon/profile/progress-bar/HpProgressBar.vue";
import { BattleMapPresenter } from "@/presenters/map/battle-map.presenter";
import { useGameStore } from "@/stores/use-game-store";
import type { EnemyConditionViewModel } from "@/viewmodels/enemy/enemy-condition.viewmodel";

const store = useGameStore();
const { t } = useI18n();

const isStatsOpen = ref(false);

const fieldImageUrl = ImageCatalog.getBattleFieldUrl();
const juniorImageUrl = ImageCatalog.getBattleJuniorUrl();

const battleMapViewModel = computed(() => {
  const enemy = store.currentState?.battle?.enemy ?? null;

  return BattleMapPresenter.getViewModel(enemy?.id ?? null, enemy?.hp ?? null);
});

const titleClass = computed(() => {
  if (battleMapViewModel.value.isBoss) {
    return "text-amber-400 drop-shadow-[0_0_5px_rgba(255,191,0,0.8)]";
  }

  return "text-red-400 drop-shadow-[0_0_2px_rgba(158,55,55,0.8)]";
});

const hasStats = computed(() => {
  return (
    battleMapViewModel.value.attributes.length > 0 ||
    battleMapViewModel.value.conditions.length > 0
  );
});

const firstHalfConditions = computed(() => {
  const conditions = battleMapViewModel.value.conditions;
  const mid = Math.ceil(conditions.length / 2);
  return conditions.slice(0, mid);
});

const secondHalfConditions = computed(() => {
  const conditions = battleMapViewModel.value.conditions;
  const mid = Math.ceil(conditions.length / 2);
  return conditions.slice(mid);
});

function toggleStatsPanel(): void {
  isStatsOpen.value = !isStatsOpen.value;
}

function isBooleanCondition(condition: EnemyConditionViewModel): boolean {
  return !("value" in condition);
}

function getConditionValue(condition: EnemyConditionViewModel): string {
  if (isBooleanCondition(condition)) {
    return condition.can ? t("conditions.yes") : t("conditions.no");
  }

  return condition.value && Number(condition.value) >= 0
    ? `${condition.value}%`
    : t("conditions.no");
}

function getConditionColorClass(condition: EnemyConditionViewModel): string {
  if (isBooleanCondition(condition)) {
    return condition.can ? "text-green-400" : "text-red-400";
  }

  return condition.can ? "text-white" : "text-red-400";
}
</script>

<template>
  <div class="relative z-10 flex flex-col flex-1 min-h-0 pt-1">
    <div
      class="relative z-[1] grid grid-cols-[1fr_auto_auto] gap-x-2 gap-y-2 items-center shrink-0 w-full px-2"
    >
      <h4
        class="col-span-3 text-[11px] font-bold tracking-widest leading-tight text-center min-w-0 truncate"
        :class="titleClass"
      >
        {{ battleMapViewModel.title }}
      </h4>

      <HpProgressBar class="min-w-0 w-full justify-self-start" :hp="battleMapViewModel.hp" />
      <span
        v-if="battleMapViewModel.level !== null"
        class="text-[10px] font-bold text-gray-300 shrink-0 justify-self-center"
      >
        Lv.{{ battleMapViewModel.level }}
      </span>
      <span class="font-emoji text-sm shrink-0 justify-self-center" aria-hidden="true"
        >🪰</span
      >
    </div>

    <div
      class="relative z-[1] top-1 flex-1 min-h-0 overflow-hidden -mx-3 -mb-1.5 w-[calc(100%+1.5rem)]"
    >
      <div
        v-if="fieldImageUrl"
        class="absolute inset-0 z-0 bg-cover bg-center pointer-events-none"
        :style="{ backgroundImage: `url(${fieldImageUrl})` }"
      />

      <img
        v-if="battleMapViewModel.enemyImageUrl"
        :src="battleMapViewModel.enemyImageUrl"
        :alt="battleMapViewModel.title"
        class="absolute top-2 left-2 z-[1] w-[35%] max-h-[45%] object-contain pointer-events-none"
      />

      <img
        v-if="juniorImageUrl"
        :src="juniorImageUrl"
        alt=""
        class="absolute bottom-2 right-2 z-[2] w-[22%] max-h-[30%] object-contain pointer-events-none"
      />

      <button
        v-if="hasStats"
        type="button"
        class="absolute bottom-2 right-2 z-20 cursor-pointer rounded bg-black/80 border border-blue-800 px-2 py-1 flex items-center justify-center text-blue-500 hover:bg-blue-900/80 hover:border-blue-500 hover:text-blue-400 transition-all font-bold text-[9px] tracking-wide shadow-[0_0_10px_rgba(0,170,255,0.2)]"
        :aria-expanded="isStatsOpen"
        @click="toggleStatsPanel"
      >
        {{ isStatsOpen ? t("map.hideDetails") : t("map.showDetails") }}
      </button>

      <Transition name="fade">
        <div
          v-if="hasStats && isStatsOpen"
          class="map-info-panel absolute inset-0 z-10 !max-w-none w-full !border-0 pb-8 text-white text-xs"
        >
          <div class="grid grid-cols-4 w-fit gap-x-2">
            <div class="flex flex-col gap-1 min-w-0">
              <div
                v-for="stat in battleMapViewModel.attributes"
                :key="stat.statKey"
                class="flex items-center gap-1.5 min-w-0"
              >
                <div class="flex items-center w-5 shrink-0 justify-center select-none">
                  <span
                    class="text-sm 2xl:text-base font-emoji drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-1"
                    >{{ stat.icon }}</span
                  >
                </div>
                <div
                  class="font-bold tracking-wide flex items-center min-w-0 text-[10px] 2xl:text-base"
                >
                  <span class="shadow-text">{{ stat.value }}</span>
                </div>
              </div>
            </div>

            <div class="flex flex-col gap-1 min-w-0">
              <div
                v-for="stat in battleMapViewModel.elements"
                :key="stat.statKey"
                class="flex items-center gap-1.5 min-w-0"
              >
                <div class="flex items-center w-5 shrink-0 justify-center select-none">
                  <span
                    class="text-sm 2xl:text-base font-emoji drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-1"
                    >{{ stat.icon }}</span
                  >
                </div>
                <div
                  class="font-bold tracking-wide flex items-center min-w-0 text-[10px] 2xl:text-base"
                >
                  <span class="shadow-text">{{ stat.value }}</span>
                </div>
              </div>
            </div>

            <div class="flex flex-col gap-1 min-w-0">
              <div
                v-for="condition in firstHalfConditions"
                :key="condition.conditionKey"
                class="flex items-center gap-1.5 min-w-0"
              >
                <div class="flex items-center w-5 shrink-0 justify-center select-none">
                  <span
                    class="text-sm 2xl:text-base font-emoji drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-1"
                    >{{ condition.icon }}</span
                  >
                </div>
                <div
                  class="font-bold tracking-wide flex items-center min-w-0 text-[10px] 2xl:text-base"
                  :class="getConditionColorClass(condition)"
                >
                  <span class="shadow-text">{{ getConditionValue(condition) }}</span>
                </div>
              </div>
            </div>

            <div class="flex flex-col gap-1 min-w-0">
              <div
                v-for="condition in secondHalfConditions"
                :key="condition.conditionKey"
                class="flex items-center gap-1.5 min-w-0"
              >
                <div class="flex items-center w-5 shrink-0 justify-center select-none">
                  <span
                    class="text-sm 2xl:text-base font-emoji drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-1"
                    >{{ condition.icon }}</span
                  >
                </div>
                <div
                  class="font-bold tracking-wide flex items-center min-w-0 text-[10px] 2xl:text-base"
                  :class="getConditionColorClass(condition)"
                >
                  <span class="shadow-text">{{ getConditionValue(condition) }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </Transition>
    </div>
  </div>
</template>
