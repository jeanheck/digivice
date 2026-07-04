<script setup lang="ts">
import { computed, ref } from "vue";
import ProgressBar from "@/components/party/digimon/profile/ProgressBar.vue";
import Icon from "@/components/party/digimon/profile/Icon.vue";
import TrainingPoints from "@/components/party/digimon/profile/TrainingPoints.vue";
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

const { t } = useI18n();
const { show, x, y, showAt, move, hide } = useTooltipPosition();
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
</script>

<template>
  <div class="dw3-panel flex flex-col">
    <div class="dw3-panel-border dw3-beveled"></div>
    <div class="dw3-panel-inner dw3-beveled"></div>

    <div class="dw3-panel-content flex flex-col gap-2 p-3">
      <div class="flex items-start gap-4">
        <div class="flex flex-col gap-2">
          <Icon :digimon-name="digimonName" class="w-16 h-16" />
          <TrainingPoints />
        </div>

        <div class="flex-1 flex flex-col gap-1 min-w-0">
          <div class="flex justify-between items-baseline mb-1">
            <h2 class="text-sm font-bold text-white leading-none truncate pr-2 tracking-wide">{{ digimonName }}</h2>

            <div class="relative flex items-center justify-center shrink-0">
              <span class="text-[0.6rem] font-medium text-yellow-400">
                {{ $t("digimon.level") }} {{ digimon.level }}
              </span>
            </div>
          </div>

          <ProgressBar
            :variant="ProgressBarConstant.experience"
            :current-value="digimon.experience"
            :max-value="experienceToReachNextLevel"
            :percentage="experiencePercentageToReachNextLevel"
            @show-tooltip="onShowTooltip($event, ProgressBarConstant.experience)"
            @move-tooltip="onMoveTooltip"
            @hide-tooltip="onHideTooltip"
          />

          <ProgressBar
            :variant="ProgressBarConstant.hp"
            :current-value="digimon.vitals.currentHP"
            :max-value="digimon.vitals.maxHP"
            :percentage="hpPercentage"
            @show-tooltip="onShowTooltip($event, ProgressBarConstant.hp)"
            @move-tooltip="onMoveTooltip"
            @hide-tooltip="onHideTooltip"
          />

          <ProgressBar
            :variant="ProgressBarConstant.mp"
            :current-value="digimon.vitals.currentMP"
            :max-value="digimon.vitals.maxMP"
            :percentage="mpPercentage"
            @show-tooltip="onShowTooltip($event, ProgressBarConstant.mp)"
            @move-tooltip="onMoveTooltip"
            @hide-tooltip="onHideTooltip"
          />

          <ProgressBar
            :variant="ProgressBarConstant.blast"
            :current-value="50"
            :max-value="100"
            :percentage="50"
            @show-tooltip="onShowTooltip($event, ProgressBarConstant.blast)"
            @move-tooltip="onMoveTooltip"
            @hide-tooltip="onHideTooltip"
          />
        </div>
      </div>
    </div>

    <Tooltip :show="show" :x="x" :y="y" :title="tooltipTitle" />
  </div>
</template>
