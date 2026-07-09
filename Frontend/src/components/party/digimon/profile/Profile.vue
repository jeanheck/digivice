<script setup lang="ts">
import { computed, ref } from "vue";
import ProgressBar from "@/components/party/digimon/profile/ProgressBar.vue";
import Icon from "@/components/party/digimon/profile/Icon.vue";
import TrainingPoints from "@/components/party/digimon/profile/TrainingPoints.vue";
import DigievolutionsButton from "@/components/party/digimon/profile/DigievolutionsButton.vue";
import Tooltip from "@/components/tooltip/Tooltip.vue";
import type { Digimon } from "@/models/party/digimon/digimon.ts";
import { ProgressBarConstant } from "@/constants/progress-bar.constant";
import { ProfilePresenter } from "@/presenters/party/digimon/profile.presenter";
import { useTooltipPosition } from "@/composables/use-tooltip-position";
import { useI18n } from "vue-i18n";

const props = defineProps<{
  digimon: Digimon;
  digimonId: number;
}>();

const emit = defineEmits<{
  openDigievolutions: [];
}>();

const { t } = useI18n();
const { show, x, y, showAt, move, hide } = useTooltipPosition(350);
const tooltipTitle = ref("");

function onShowTooltip(event: MouseEvent, variant: string): void {
  tooltipTitle.value = t(`digimon.${variant}`);
  showAt(event);
}

function onMoveTooltip(event: MouseEvent): void {
  move(event);
}

function onHideTooltip(): void {
  hide();
}

function onOpenDigievolutions(): void {
  emit("openDigievolutions");
}

const digimonName = computed(() => {
  return ProfilePresenter.getNameById(props.digimonId);
});

const experienceToReachNextLevel = computed(() => {
  return ProfilePresenter.getRequiredExperienceForNextLevel(props.digimonId, props.digimon.level);
});

const experiencePercentageToReachNextLevel = computed(() => {
  return ProfilePresenter.calculateProgressPercentageForNextLevel(props.digimonId, props.digimon.level, props.digimon.experience);
});

const hpPercentage = computed(() => {
  return ProfilePresenter.getHpPercentage(props.digimon.vitals.currentHP, props.digimon.vitals.maxHP);
});

const mpPercentage = computed(() => {
  return ProfilePresenter.getMpPercentage(props.digimon.vitals.currentMP, props.digimon.vitals.maxMP);
});

const blastGaugePercentage = computed(() => {
  return ProfilePresenter.getBlastGaugePercentage(props.digimon.blastGauge);
});
</script>

<template>
  <div class="dw3-panel flex flex-col">
    <div class="dw3-panel-border dw3-beveled"></div>
    <div class="dw3-panel-inner dw3-beveled"></div>

    <div class="dw3-panel-content p-2">
      <div class="grid grid-cols-[auto_1fr] grid-rows-5 gap-x-2 gap-y-1">
        <div class="col-start-1 row-start-1 row-span-3 w-20">
          <Icon :digimon-name="digimonName" class="w-full aspect-square" />
        </div>

        <DigievolutionsButton
          class="col-start-1 row-start-4"
          @open-digievolutions="onOpenDigievolutions"
          @show-tooltip="onShowTooltip($event, 'digievolutions')"
          @move-tooltip="onMoveTooltip"
          @hide-tooltip="onHideTooltip"
        />

        <TrainingPoints
          class="col-start-1 row-start-5"
          :tp="digimon.tp"
          @show-tooltip="onShowTooltip($event, 'tp')"
          @move-tooltip="onMoveTooltip"
          @hide-tooltip="onHideTooltip"
        />

        <div class="col-start-2 row-start-1 flex justify-between items-baseline min-w-0 h-6">
          <h2 class="text-sm font-bold text-white leading-none truncate pr-2 tracking-wide">{{ digimonName }}</h2>
          <span class="text-[0.6rem] font-medium text-yellow-400 shrink-0">
            Nv {{ digimon.level }}
          </span>
        </div>

        <div class="col-start-2 row-start-2 min-w-0 h-6">
          <ProgressBar
            :variant="ProgressBarConstant.experience"
            :current-value="digimon.experience"
            :max-value="experienceToReachNextLevel"
            :percentage="experiencePercentageToReachNextLevel"
            @show-tooltip="onShowTooltip($event, ProgressBarConstant.experience)"
            @move-tooltip="onMoveTooltip"
            @hide-tooltip="onHideTooltip"
          />
        </div>

        <div class="col-start-2 row-start-3 min-w-0 h-6">
          <ProgressBar
            :variant="ProgressBarConstant.hp"
            :current-value="digimon.vitals.currentHP"
            :max-value="digimon.vitals.maxHP"
            :percentage="hpPercentage"
            @show-tooltip="onShowTooltip($event, ProgressBarConstant.hp)"
            @move-tooltip="onMoveTooltip"
            @hide-tooltip="onHideTooltip"
          />
        </div>

        <div class="col-start-2 row-start-4 min-w-0 h-6">
          <ProgressBar
            :variant="ProgressBarConstant.mp"
            :current-value="digimon.vitals.currentMP"
            :max-value="digimon.vitals.maxMP"
            :percentage="mpPercentage"
            @show-tooltip="onShowTooltip($event, ProgressBarConstant.mp)"
            @move-tooltip="onMoveTooltip"
            @hide-tooltip="onHideTooltip"
          />
        </div>

        <div class="col-start-2 row-start-5 min-w-0 h-6">
          <ProgressBar
            :variant="ProgressBarConstant.blastGauge"
            :current-value="digimon.blastGauge"
            :max-value="1000"
            :percentage="blastGaugePercentage"
            @show-tooltip="onShowTooltip($event, ProgressBarConstant.blastGauge)"
            @move-tooltip="onMoveTooltip"
            @hide-tooltip="onHideTooltip"
          />
        </div>
      </div>
    </div>

    <Tooltip :show="show" :x="x" :y="y" :title="tooltipTitle" :max-width="350" />
  </div>
</template>
