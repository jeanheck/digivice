<script setup lang="ts">
import { computed, ref } from "vue";
import { useI18n } from "vue-i18n";
import { ImageCatalog } from "@/catalogs/image.catalog";
import EnemyConditionSquare from "@/components/map/battle-map/EnemyConditionSquare.vue";
import HpProgressBar from "@/components/party/digimon/profile/progress-bar/HpProgressBar.vue";
import TinyTooltip from "@/components/tooltip/TinyTooltip.vue";
import {
  useTooltipPosition,
  type TooltipHorizontalAlign,
  type TooltipPlacement,
} from "@/composables/use-tooltip-position";
import { BattleFieldPresenter } from "@/presenters/map/battle-field.presenter";
import { BattleMapPresenter } from "@/presenters/map/battle-map.presenter";
import { ProfilePresenter } from "@/presenters/party/digimon/profile.presenter";
import { useGameStore } from "@/stores/use-game-store";
import type { EnemyConditionViewModel } from "@/viewmodels/enemy/enemy-condition.viewmodel";

const emit = defineEmits<{
  (e: "open-enemy-modal", enemyId: string): void;
}>();

const store = useGameStore();
const { t } = useI18n();

const isStatsOpen = ref(false);
const { show, x, y, showAt, move, hide } = useTooltipPosition(0);
const tooltipTitle = ref("");
const tooltipPlacement = ref<TooltipPlacement>("below");
const tooltipAlign = ref<TooltipHorizontalAlign>("right");

const fieldImageUrl = computed(() => {
  const fieldId = store.currentState?.battle?.field ?? 0;
  return ImageCatalog.getBattleFieldUrl(fieldId);
});
const juniorImageUrl = ImageCatalog.getBattleJuniorUrl();

const battleFieldId = computed(() => {
  return store.currentState?.battle?.field ?? 0;
});

const fieldLabels = computed(() => {
  return BattleFieldPresenter.getLabels(battleFieldId.value, t);
});

const enemy = computed(() => {
  return store.currentState?.battle?.enemy ?? null;
});

const battleMapViewModel = computed(() => {
  return BattleMapPresenter.getViewModel(enemy.value?.id ?? null, enemy.value?.hp ?? null);
});

const enemyCondition = computed(() => {
  return enemy.value?.condition ?? 0;
});

const conditionTooltipTitle = computed(() => {
  return t(
    ProfilePresenter.getConditionTooltipKey(enemyCondition.value, battleMapViewModel.value.hp),
  );
});

const canOpenWiki = computed(() => {
  return battleMapViewModel.value.enemyId !== null;
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

function openEnemyWiki(): void {
  const enemyId = battleMapViewModel.value.enemyId;
  if (enemyId === null) {
    return;
  }

  emit("open-enemy-modal", enemyId);
}

function onShowTooltip(
  event: MouseEvent,
  value: string,
  options?: { placement?: TooltipPlacement; align?: TooltipHorizontalAlign },
): void {
  tooltipTitle.value = value;
  tooltipPlacement.value = options?.placement ?? "below";
  tooltipAlign.value = options?.align ?? "right";
  showAt(event, {
    placement: tooltipPlacement.value,
    align: tooltipAlign.value,
  });
}

function onMoveTooltip(event: MouseEvent): void {
  move(event, tooltipPlacement.value);
}

function onHideTooltip(): void {
  hide();
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
  <div class="relative z-10 flex flex-col flex-1 min-h-0">
    <div
      v-if="fieldImageUrl"
      class="absolute -left-3 -right-3 -top-1.5 -bottom-1.5 z-0 bg-cover bg-center pointer-events-none"
      :style="{ backgroundImage: `url(${fieldImageUrl})` }"
    />

    <div
      class="relative z-[1] -mt-1.5 -mx-3 w-[calc(100%+1.5rem)] pt-1.5 pb-1 grid grid-cols-[1fr_auto_auto_auto] gap-x-2 gap-y-2 items-center shrink-0 px-2 bg-black/80"
    >
      <h4
        class="col-span-4 text-[11px] font-bold tracking-widest leading-tight text-center min-w-0 truncate"
        :class="[titleClass, canOpenWiki ? 'cursor-pointer' : '']"
        @click="openEnemyWiki"
      >
        {{ battleMapViewModel.title }}
      </h4>

      <HpProgressBar
        class="min-w-0 w-full justify-self-start"
        :hp="battleMapViewModel.hp"
        @show-tooltip="onShowTooltip($event, t('digimon.hp'))"
        @move-tooltip="onMoveTooltip"
        @hide-tooltip="onHideTooltip"
      />
      <EnemyConditionSquare
        :condition="enemyCondition"
        :hp="battleMapViewModel.hp"
        @show-tooltip="onShowTooltip($event, conditionTooltipTitle, { align: 'left' })"
        @move-tooltip="onMoveTooltip"
        @hide-tooltip="onHideTooltip"
      />
      <span
        v-if="battleMapViewModel.level !== null"
        class="text-[10px] font-bold text-gray-300 shrink-0 justify-self-center cursor-default"
      >
        {{ t("digimon.lv") }}.{{ battleMapViewModel.level }}
      </span>
      <span
        v-if="battleMapViewModel.speciesEmoji && battleMapViewModel.species"
        class="font-emoji text-[16px] shrink-0 justify-self-center -translate-y-1 cursor-help"
        aria-hidden="true"
        @mouseenter="
          onShowTooltip($event, t(`species.${battleMapViewModel.species}`), { align: 'left' })
        "
        @mousemove="onMoveTooltip"
        @mouseleave="onHideTooltip"
        >{{ battleMapViewModel.speciesEmoji }}</span
      >
    </div>

    <div
      class="relative z-[1] flex-1 min-h-0 overflow-visible -mx-3 -mb-1.5 w-[calc(100%+1.5rem)]"
    >
      <img
        v-if="battleMapViewModel.enemyImageUrl"
        :src="battleMapViewModel.enemyImageUrl"
        :alt="battleMapViewModel.title"
        class="absolute top-2 left-2 z-[1] w-[50%] max-h-[65%] object-contain drop-shadow-[0_6px_14px_rgba(0,0,0,1)]"
        :class="canOpenWiki ? 'cursor-pointer' : 'pointer-events-none'"
        @click="openEnemyWiki"
      />

      <div
        v-if="juniorImageUrl"
        class="absolute -bottom-12 -right-6 z-[2] w-[100%] h-[110%] overflow-hidden pointer-events-none"
      >
        <img
          :src="juniorImageUrl"
          alt=""
          class="absolute top-0 right-0 w-full max-w-none h-[260%] object-contain object-top"
        />
      </div>

      <button
        v-if="hasStats"
        type="button"
        class="absolute bottom-2 right-2 z-20 cursor-pointer rounded bg-black/80 border border-blue-800 px-2 py-1 flex items-center justify-center text-blue-500 hover:bg-blue-900/80 hover:border-blue-500 hover:text-blue-400 transition-all font-bold text-[9px] tracking-wide shadow-[0_0_10px_rgba(0,170,255,0.2)]"
        :aria-expanded="isStatsOpen"
        @click="toggleStatsPanel"
      >
        {{ isStatsOpen ? t("map.hideDetails") : t("map.showDetails") }}
      </button>

      <div class="absolute bottom-2 left-2 z-20 flex flex-col gap-0.5 pointer-events-none">
        <span
          class="text-[10px] 2xl:text-sm font-bold tracking-wide text-white text-outline-black-glow leading-tight"
        >
          {{ fieldLabels.title }}
        </span>
        <span
          v-if="fieldLabels.strengthenLabel"
          class="text-[10px] 2xl:text-sm font-bold tracking-wide text-green-400 text-outline-black-glow leading-tight"
        >
          {{ fieldLabels.strengthenLabel }}
        </span>
        <span
          v-if="fieldLabels.weakenLabel"
          class="text-[10px] 2xl:text-sm font-bold tracking-wide text-red-400 text-outline-black-glow leading-tight"
        >
          {{ fieldLabels.weakenLabel }}
        </span>
      </div>

      <Transition name="fade">
        <div
          v-if="hasStats && isStatsOpen"
          class="map-info-panel absolute inset-0 z-10 !max-w-none w-full !border-0 !rounded-none !backdrop-blur-none pb-8 text-white text-xs"
        >
          <div class="grid grid-cols-4 w-full">
            <div class="flex flex-col gap-1 min-w-0">
              <div
                v-for="stat in battleMapViewModel.attributes"
                :key="stat.statKey"
                class="flex items-center gap-1.5 min-w-0"
              >
                <div
                  class="flex items-center w-5 shrink-0 justify-center select-none cursor-help"
                  @mouseenter="onShowTooltip($event, t(`stat.${stat.statKey}`))"
                  @mousemove="onMoveTooltip"
                  @mouseleave="onHideTooltip"
                >
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
                <div
                  class="flex items-center w-5 shrink-0 justify-center select-none cursor-help"
                  @mouseenter="onShowTooltip($event, t(`stat.${stat.statKey}`))"
                  @mousemove="onMoveTooltip"
                  @mouseleave="onHideTooltip"
                >
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
                <div
                  class="flex items-center w-5 shrink-0 justify-center select-none cursor-help"
                  @mouseenter="
                    onShowTooltip($event, t(`conditions.${condition.conditionKey}.name`))
                  "
                  @mousemove="onMoveTooltip"
                  @mouseleave="onHideTooltip"
                >
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
                <div
                  class="flex items-center w-5 shrink-0 justify-center select-none cursor-help"
                  @mouseenter="
                    onShowTooltip($event, t(`conditions.${condition.conditionKey}.name`), {
                      align: 'left',
                    })
                  "
                  @mousemove="onMoveTooltip"
                  @mouseleave="onHideTooltip"
                >
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

    <TinyTooltip
      :show="show"
      :x="x"
      :y="y"
      :title="tooltipTitle"
      :max-width="140"
      :placement="tooltipPlacement"
      :horizontal-align="tooltipAlign"
    />
  </div>
</template>
