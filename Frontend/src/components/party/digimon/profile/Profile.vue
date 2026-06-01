<script setup lang="ts">
import { computed } from "vue";
import ProgressBar from "@/components/party/digimon/profile/ProgressBar.vue";
import Icon from "@/components/party/digimon/profile/Icon.vue";
import Vitals from "@/components/party/digimon/profile/Vitals.vue";
import type { Digimon } from "@/models";
import { ProgressBarVariant } from "@/constants/progress-bar-variant";
import { ProfilePresenter } from "@/presenters/profile.presenter";

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
</script>

<template>
  <div class="relative overflow-hidden flex flex-col w-full bg-[#000a2b]">
    <div class="absolute inset-0 bg-[#0077ff] pointer-events-none dw3-beveled"></div>
    <div class="absolute inset-[1.5px] bg-[#000a2b] pointer-events-none dw3-beveled"></div>

    <div class="relative z-10 flex flex-col gap-2 p-3">
      <div class="flex items-start gap-4">
        <Icon :digimon-name="digimonName" class="w-16 h-16" />

        <div class="flex-1 flex flex-col gap-1 min-w-0">
          <div class="flex justify-between items-baseline mb-1 border-b border-[#00154a] pb-1">
            <h2 class="text-sm font-bold text-white leading-none truncate pr-2 tracking-wide">{{ digimonName }}</h2>

            <div class="relative flex items-center justify-center shrink-0">
              <span class="text-[0.6rem] font-medium text-yellow-500">
                {{ $t("digimon.level") }} {{ digimon.level }}
              </span>
            </div>
          </div>

          <ProgressBar
            :variant="ProgressBarVariant.Experience"
            :current-value="digimon.experience"
            :max-value="experienceToReachNextLevel"
            :percentage="experiencePercentageToReachNextLevel"
          />
        </div>
      </div>

      <Vitals :vitals="digimon.vitals" />
    </div>
  </div>
</template>
