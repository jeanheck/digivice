<script setup lang="ts">
import { computed, ref } from "vue";
import BlastGaugeProgressBar from "@/components/party/digimon/profile/progress-bar/BlastGaugeProgressBar.vue";
import ExperienceProgressBar from "@/components/party/digimon/profile/progress-bar/ExperienceProgressBar.vue";
import HpProgressBar from "@/components/party/digimon/profile/progress-bar/HpProgressBar.vue";
import MpProgressBar from "@/components/party/digimon/profile/progress-bar/MpProgressBar.vue";
import Icon from "@/components/party/digimon/profile/Icon.vue";
import TrainingPoints from "@/components/party/digimon/profile/TrainingPoints.vue";
import DigievolutionsButton from "@/components/party/digimon/profile/DigievolutionsButton.vue";
import Tooltip from "@/components/tooltip/Tooltip.vue";
import type { Digimon } from "@/models/party/digimon/digimon.ts";
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

function onShowTooltip(event: MouseEvent, value: string): void {
  tooltipTitle.value = value;
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
</script>

<template>
  <div class="dw3-panel flex flex-col">
    <div class="dw3-panel-border dw3-beveled"></div>
    <div class="dw3-panel-inner dw3-beveled"></div>

    <div class="dw3-panel-content p-2">
      <div class="grid grid-cols-[auto_1fr] grid-rows-[auto_1fr_auto_auto_auto] gap-x-2 gap-y-1">
        <div class="col-start-1 row-start-1 row-span-3 w-20">
          <Icon :digimon-name="digimonName" class="w-full aspect-square" />
        </div>

        <DigievolutionsButton
          class="col-start-1 row-start-4"
          @open-digievolutions="onOpenDigievolutions"
          @show-tooltip="onShowTooltip($event, t(`digimon.digievolutions`))"
          @move-tooltip="onMoveTooltip"
          @hide-tooltip="onHideTooltip"
        />

        <TrainingPoints
          class="col-start-1 row-start-5"
          :tp="digimon.tp"
          @show-tooltip="onShowTooltip($event, t(`digimon.tp`))"
          @move-tooltip="onMoveTooltip"
          @hide-tooltip="onHideTooltip"
        />

        <div class="col-start-2 row-start-1 flex justify-between items-baseline min-w-0">
          <h2 class="text-sm font-bold text-white leading-none truncate pr-2 tracking-wide">
            {{ digimonName }}
          </h2>
          <span class="text-[0.6rem] font-medium text-yellow-400 shrink-0 leading-none">
            Nv {{ digimon.level }}
          </span>
        </div>

        <div class="col-start-2 row-start-2 min-w-0 flex flex-col">
          <ExperienceProgressBar
            :digimon-id="digimonId"
            :level="digimon.level"
            :experience="digimon.experience"
            @show-tooltip="onShowTooltip($event, t(`digimon.experience`))"
            @move-tooltip="onMoveTooltip"
            @hide-tooltip="onHideTooltip"
          />
          <div class="mt-auto pt-1 border-b border-[#0033aa]/50"></div>
        </div>

        <div class="col-start-2 row-start-3 min-w-0 h-6">
          <HpProgressBar
            :current-hp="digimon.vitals.currentHP"
            :max-hp="digimon.vitals.maxHP"
            @show-tooltip="onShowTooltip($event, t(`digimon.hp`))"
            @move-tooltip="onMoveTooltip"
            @hide-tooltip="onHideTooltip"
          />
        </div>

        <div class="col-start-2 row-start-4 min-w-0 h-6">
          <MpProgressBar
            :current-mp="digimon.vitals.currentMP"
            :max-mp="digimon.vitals.maxMP"
            @show-tooltip="onShowTooltip($event, t(`digimon.mp`))"
            @move-tooltip="onMoveTooltip"
            @hide-tooltip="onHideTooltip"
          />
        </div>

        <div class="col-start-2 row-start-5 min-w-0 h-6">
          <BlastGaugeProgressBar
            :blast-gauge="digimon.blastGauge"
            @show-tooltip="onShowTooltip($event, t(`digimon.blastGauge`))"
            @move-tooltip="onMoveTooltip"
            @hide-tooltip="onHideTooltip"
          />
        </div>
      </div>
    </div>

    <Tooltip :show="show" :x="x" :y="y" :title="tooltipTitle" :max-width="350" />
  </div>
</template>
