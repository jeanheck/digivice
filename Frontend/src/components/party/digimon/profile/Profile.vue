<script setup lang="ts">
import { computed } from "vue";
import ProgressBar from "@/components/party/digimon/profile/ProgressBar.vue";
import Icon from "@/components/party/digimon/profile/Icon.vue";
import type { Digimon } from "@/models/party/digimon/digimon.ts";
import { ProgressBarConstant } from "@/constants/progress-bar.constant";
import { ProfilePresenter } from "@/presenters/party/digimon/profile.presenter";

const props = defineProps<{
  digimon: Digimon;
  digimonId: number;
}>();

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

          <div class="w-16 h-16 bg-[#000e3f] border-2 border-[#00154a] rounded flex flex-col items-center justify-center">
            <span class="text-[1.8rem] leading-none">🏋️‍♂️</span>
            <span class="text-[1rem] font-bold text-white mt-1">10</span>
          </div>
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
          />

          <ProgressBar
            :variant="ProgressBarConstant.hp"
            :current-value="digimon.vitals.currentHP"
            :max-value="digimon.vitals.maxHP"
            :percentage="hpPercentage"
          />

          <ProgressBar
            :variant="ProgressBarConstant.mp"
            :current-value="digimon.vitals.currentMP"
            :max-value="digimon.vitals.maxMP"
            :percentage="mpPercentage"
          />

          <ProgressBar
            :variant="ProgressBarConstant.mp"
            :current-value="50"
            :max-value="100"
            :percentage="50"
          />
        </div>
      </div>
    </div>
  </div>
</template>
