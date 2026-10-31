<script setup lang="ts">
import { computed, ref } from "vue";
import { useI18n } from "vue-i18n";
import DigimonBattleEnemyAffinities from "@/ui/components/map/digimon-battle/DigimonBattleEnemyAffinities.vue";
import DigimonBattleEnemyImage from "@/ui/components/map/digimon-battle/DigimonBattleEnemyImage.vue";
import DigimonBattleEnemyLevel from "@/ui/components/map/digimon-battle/DigimonBattleEnemyLevel.vue";
import DigimonBattleEnemySpecie from "@/ui/components/map/digimon-battle/DigimonBattleEnemySpecie.vue";
import DigimonBattleEnemyStatus from "@/ui/components/map/digimon-battle/DigimonBattleEnemyStatus.vue";
import DigimonBattleField from "@/ui/components/map/digimon-battle/DigimonBattleField.vue";
import DigimonBattleFieldImage from "@/ui/components/map/digimon-battle/DigimonBattleFieldImage.vue";
import DigimonBattleJunior from "@/ui/components/map/digimon-battle/DigimonBattleJunior.vue";
import DigimonBattleStats from "@/ui/components/map/digimon-battle/DigimonBattleStats.vue";
import EnemyBuffStatsTooltip from "@/ui/components/map/digimon-battle/EnemyBuffStatsTooltip.vue";
import HpProgressBar from "@/ui/components/party/digimon/profile/progress-bar/HpProgressBar.vue";
import TinyTooltip from "@/ui/components/tooltip/TinyTooltip.vue";
import {
  useTooltipPosition,
  type TooltipHorizontalAlign,
  type TooltipPlacement,
} from "@/ui/composables/use-tooltip-position";
import { DigimonBattlePresenter } from "@/presentation/presenters/map/digimon-battle.presenter";
import { useGameState } from "@/ui/composables/use-game-state";
import type { EnemyConditionViewModel } from "@/presentation/viewmodels/enemy/enemy-condition.viewmodel";
import type { EnemyStatViewModel } from "@/presentation/viewmodels/enemy/enemy-stat.viewmodel";

type TooltipVariant = "none" | "tiny" | "buff";

const emit = defineEmits<{
  (e: "open-enemy-modal", enemyId: string): void;
}>();

const gameState = useGameState();
const { t } = useI18n();

const activeVariant = ref<TooltipVariant>("none");
const { x, y, showAt, move, hide } = useTooltipPosition(0);
const tooltipTitle = ref("");
const buffTooltipContent = ref({ title: "", base: 0, delta: 0, total: 0 });
const tooltipPlacement = ref<TooltipPlacement>("below");
const tooltipAlign = ref<TooltipHorizontalAlign>("right");

const battleFieldId = computed(() => {
  return gameState.value.digimonBattle.field;
});

const enemy = computed(() => {
  return gameState.value.digimonBattle.enemy;
});

const digimonBattleViewModel = computed(() => {
  return DigimonBattlePresenter.getViewModel(enemy.value);
});

const enemyCondition = computed(() => {
  return enemy.value?.condition ?? 0;
});

const digimonStatusTooltip = computed(() => {
  const status = DigimonBattlePresenter.getStatus(
    enemyCondition.value,
    digimonBattleViewModel.value.hp,
  );

  if (status !== "debuffed") {
    return t(`digimon.status.${status}`);
  }

  return DigimonBattlePresenter.getActiveDebuffs(enemyCondition.value)
    .map((debuff) => t(`digimon.debuff.${debuff}.affected`))
    .join(", ");
});

const canOpenWiki = computed(() => {
  return digimonBattleViewModel.value.enemyId !== null;
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

function onShowConditionTooltip(
  event: MouseEvent,
  condition: EnemyConditionViewModel,
  align?: TooltipHorizontalAlign,
): void {
  onShowTooltip(event, t(`conditions.${condition.conditionKey}.name`), {
    align: align ?? "right",
  });
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
</script>

<template>
  <div class="relative z-10 flex flex-col flex-1 min-h-0">
    <DigimonBattleFieldImage :battle-field-id="battleFieldId" />

    <div
      class="relative z-1 -mt-1.5 -mx-3 w-[calc(100%+1.5rem)] pt-1.5 pb-1 grid grid-cols-[1fr_auto_auto_auto] gap-x-2 gap-y-2 items-center shrink-0 px-2 bg-black/80"
    >
      <DigimonBattleEnemyAffinities
        class="col-span-4"
        :effective="digimonBattleViewModel.affinities.effective"
        :not-effective="digimonBattleViewModel.affinities.notEffective"
      />

      <HpProgressBar
        class="min-w-0 w-[calc(100%+2px)] -ml-0.5 justify-self-start"
        :hp="digimonBattleViewModel.hp"
        @show-tooltip="onShowTooltip($event, t('digimon.hp'))"
        @move-tooltip="onMoveTooltip"
        @hide-tooltip="onHideTooltip"
      />
      <DigimonBattleEnemyStatus
        :condition="enemyCondition"
        :hp="digimonBattleViewModel.hp"
        @show-tooltip="onShowTooltip($event, digimonStatusTooltip, { align: 'left' })"
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

    <div class="relative z-1 flex-1 min-h-0 overflow-visible -mx-3 -mb-1.5 w-[calc(100%+1.5rem)]">
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
        :conditions="digimonBattleViewModel.conditions"
        @show-tooltip="onShowStatTooltip"
        @move-tooltip="onMoveTooltip"
        @hide-tooltip="onHideTooltip"
        @show-buff-tooltip="onShowBuffTooltip"
        @move-buff-tooltip="onMoveTooltip"
        @hide-buff-tooltip="onHideTooltip"
        @show-condition-tooltip="onShowConditionTooltip"
      />
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
