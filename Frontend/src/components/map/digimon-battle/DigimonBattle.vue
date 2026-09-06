<script setup lang="ts">
import { computed, ref } from "vue";
import { useI18n } from "vue-i18n";
import { ImageCatalog } from "@/catalogs/image.catalog";
import DigimonBattleEnemyImage from "@/components/map/digimon-battle/DigimonBattleEnemyImage.vue";
import DigimonBattleEnemyLevel from "@/components/map/digimon-battle/DigimonBattleEnemyLevel.vue";
import DigimonBattleEnemySpecie from "@/components/map/digimon-battle/DigimonBattleEnemySpecie.vue";
import DigimonBattleEnemyStatus from "@/components/map/digimon-battle/DigimonBattleEnemyStatus.vue";
import DigimonBattleField from "@/components/map/digimon-battle/DigimonBattleField.vue";
import DigimonBattleJunior from "@/components/map/digimon-battle/DigimonBattleJunior.vue";
import DigimonBattleStats from "@/components/map/digimon-battle/DigimonBattleStats.vue";
import EnemyBuffStatsTooltip from "@/components/map/digimon-battle/EnemyBuffStatsTooltip.vue";
import HpProgressBar from "@/components/party/digimon/profile/progress-bar/HpProgressBar.vue";
import TinyTooltip from "@/components/tooltip/TinyTooltip.vue";
import {
  useTooltipPosition,
  type TooltipHorizontalAlign,
  type TooltipPlacement,
} from "@/composables/use-tooltip-position";
import { DigimonBattlePresenter } from "@/presenters/map/digimon-battle.presenter";
import { ProfilePresenter } from "@/presenters/party/digimon/profile.presenter";
import { useGameStore } from "@/stores/use-game-store";
import type { EnemyConditionViewModel } from "@/viewmodels/enemy/enemy-condition.viewmodel";
import type { EnemyStatViewModel } from "@/viewmodels/enemy/enemy-stat.viewmodel";

type TooltipVariant = "none" | "tiny" | "buff";

const emit = defineEmits<{
  (e: "open-enemy-modal", enemyId: string): void;
}>();

const store = useGameStore();
const { t } = useI18n();

const activeVariant = ref<TooltipVariant>("none");
const { x, y, showAt, move, hide } = useTooltipPosition(0);
const tooltipTitle = ref("");
const buffTooltipContent = ref({ title: "", base: 0, delta: 0, total: 0 });
const tooltipPlacement = ref<TooltipPlacement>("below");
const tooltipAlign = ref<TooltipHorizontalAlign>("right");

const battleFieldId = computed(() => {
  return store.currentState?.digimonBattle?.field ?? 0;
});

const fieldImageUrl = computed(() => ImageCatalog.getDigimonBattleFieldImageUrl(battleFieldId.value));

const enemy = computed(() => {
  return store.currentState?.digimonBattle?.enemy ?? null;
});

const digimonBattleViewModel = computed(() => {
  return DigimonBattlePresenter.getViewModel(enemy.value);
});

const enemyCondition = computed(() => {
  return enemy.value?.condition ?? 0;
});

const conditionTooltipTitle = computed(() => {
  return t(
    ProfilePresenter.getConditionTooltipKey(enemyCondition.value, digimonBattleViewModel.value.hp),
  );
});

const canOpenWiki = computed(() => {
  return digimonBattleViewModel.value.enemyId !== null;
});

const titleClass = computed(() => {
  if (digimonBattleViewModel.value.isBoss) {
    return "text-amber-400 drop-shadow-[0_0_5px_rgba(255,191,0,0.8)]";
  }

  return "text-red-400 drop-shadow-[0_0_2px_rgba(158,55,55,0.8)]";
});

const hasStats = computed(() => {
  return (
    digimonBattleViewModel.value.attributes.length > 0 ||
    digimonBattleViewModel.value.conditions.length > 0
  );
});

const firstHalfConditions = computed(() => {
  const conditions = digimonBattleViewModel.value.conditions;
  const mid = Math.ceil(conditions.length / 2);
  return conditions.slice(0, mid);
});

const secondHalfConditions = computed(() => {
  const conditions = digimonBattleViewModel.value.conditions;
  const mid = Math.ceil(conditions.length / 2);
  return conditions.slice(mid);
});

function openEnemyWiki(): void {
  const enemyId = digimonBattleViewModel.value.enemyId;
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
  activeVariant.value = "tiny";
  showAt(event, {
    placement: tooltipPlacement.value,
    align: tooltipAlign.value,
  });
}

function onShowStatTooltip(event: MouseEvent, stat: EnemyStatViewModel): void {
  onShowTooltip(event, t(`stat.${stat.statKey}`));
}

function onShowBuffTooltip(event: MouseEvent, stat: EnemyStatViewModel): void {
  buffTooltipContent.value = {
    title: t(`stat.${stat.statKey}`),
    base: stat.baseValue ?? stat.value,
    delta: stat.delta ?? 0,
    total: stat.value,
  };
  activeVariant.value = "buff";
  showAt(event, {
    placement: tooltipPlacement.value,
    align: tooltipAlign.value,
  });
}

function onMoveTooltip(event: MouseEvent): void {
  move(event, tooltipPlacement.value);
}

function onHideTooltip(): void {
  activeVariant.value = "none";
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
      class="relative z-1 -mt-1.5 -mx-3 w-[calc(100%+1.5rem)] pt-1.5 pb-1 grid grid-cols-[1fr_auto_auto_auto] gap-x-2 gap-y-2 items-center shrink-0 px-2 bg-black/80"
    >
      <h4
        class="col-span-4 text-[11px] font-bold tracking-widest leading-tight text-center min-w-0 truncate"
        :class="[titleClass, canOpenWiki ? 'cursor-pointer' : '']"
        @click="openEnemyWiki"
      >
        {{ digimonBattleViewModel.title }}
      </h4>

      <HpProgressBar
        class="min-w-0 w-full justify-self-start"
        :hp="digimonBattleViewModel.hp"
        @show-tooltip="onShowTooltip($event, t('digimon.hp'))"
        @move-tooltip="onMoveTooltip"
        @hide-tooltip="onHideTooltip"
      />
      <DigimonBattleEnemyStatus
        :condition="enemyCondition"
        :hp="digimonBattleViewModel.hp"
        @show-tooltip="onShowTooltip($event, conditionTooltipTitle, { align: 'left' })"
        @move-tooltip="onMoveTooltip"
        @hide-tooltip="onHideTooltip"
      />
      <DigimonBattleEnemyLevel :level="digimonBattleViewModel.level" />
      <DigimonBattleEnemySpecie
        :species-emoji="digimonBattleViewModel.speciesEmoji"
        @show-tooltip="
          onShowTooltip($event, t(`species.${digimonBattleViewModel.species}`), { align: 'left' })
        "
        @move-tooltip="onMoveTooltip"
        @hide-tooltip="onHideTooltip"
      />
    </div>

    <div
      class="relative z-1 flex-1 min-h-0 overflow-visible -mx-3 -mb-1.5 w-[calc(100%+1.5rem)]"
    >
      <DigimonBattleEnemyImage
        v-if="digimonBattleViewModel.enemyImageUrl"
        :image-url="digimonBattleViewModel.enemyImageUrl"
        :clickable="canOpenWiki"
        @click="openEnemyWiki"
      />

      <DigimonBattleJunior />

      <DigimonBattleField :battle-field-id="battleFieldId" />

      <DigimonBattleStats
        :attributes="digimonBattleViewModel.attributes"
        :elements="digimonBattleViewModel.elements"
        :enabled="hasStats"
        @show-tooltip="onShowStatTooltip"
        @move-tooltip="onMoveTooltip"
        @hide-tooltip="onHideTooltip"
        @show-buff-tooltip="onShowBuffTooltip"
        @move-buff-tooltip="onMoveTooltip"
        @hide-buff-tooltip="onHideTooltip"
      >
        <div class="flex flex-col gap-1 min-w-0">
          <div
            v-for="condition in firstHalfConditions"
            :key="condition.conditionKey"
            class="flex items-center gap-1.5 min-w-0"
          >
            <div
              class="flex items-center w-5 shrink-0 justify-center select-none cursor-help"
              @mouseenter="onShowTooltip($event, t(`conditions.${condition.conditionKey}.name`))"
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
              <span class="shadow-text cursor-default">{{ getConditionValue(condition) }}</span>
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
              <span class="shadow-text cursor-default">{{ getConditionValue(condition) }}</span>
            </div>
          </div>
        </div>
      </DigimonBattleStats>
    </div>

    <TinyTooltip
      :show="activeVariant === 'tiny'"
      :x="x"
      :y="y"
      :title="tooltipTitle"
      :max-width="140"
      :placement="tooltipPlacement"
      :horizontal-align="tooltipAlign"
    />

    <EnemyBuffStatsTooltip
      :show="activeVariant === 'buff'"
      :x="x"
      :y="y"
      :title="buffTooltipContent.title"
      :base="buffTooltipContent.base"
      :delta="buffTooltipContent.delta"
      :total="buffTooltipContent.total"
      :placement="tooltipPlacement"
    />
  </div>
</template>
