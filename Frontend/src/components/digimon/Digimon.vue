<script setup lang="ts">
import { computed } from "vue";
import ExperienceProgressBar from "@/components/digimon/ExperienceProgressBar.vue";
import DigimonIcon from "@/components/digimon/DigimonIcon.vue";
import DigimonVitals from "@/components/digimon/DigimonVitals.vue";
import type { Digimon } from "@/models";
import { DigimonPresenter } from "@/presenters/digimon-presenter";

const props = defineProps<{
  digimon: Digimon;
  digimonId: number;
}>();

const digimonName = computed(() => {
  return DigimonPresenter.getDigimonNameById(props.digimonId);
});

const experienceToReachNextLevel = computed(() => {
  return DigimonPresenter.getRequiredExperienceForNextLevel(digimonName.value, props.digimon.level);
});

const experiencePercentageToReachNextLevel = computed(() => {
  return DigimonPresenter.getProgressPercentageForNextLevel(digimonName.value, props.digimon.level, props.digimon.experience);
});
</script>

<template>
  <div class="relative overflow-hidden flex flex-col w-full bg-[#000a2b]">
    <div class="absolute inset-0 bg-[#0077ff] pointer-events-none dw3-beveled"></div>
    <div class="absolute inset-[1.5px] bg-[#000a2b] pointer-events-none dw3-beveled"></div>

    <div class="relative z-10 flex flex-col gap-2 p-3">
      <div class="flex items-start gap-4">
        <DigimonIcon :digimon-name="digimonName" class="w-16 h-16" />

        <div class="flex-1 flex flex-col gap-1 min-w-0">
          <div class="flex justify-between items-baseline mb-1 border-b border-[#00154a] pb-1">
            <h2 class="text-sm font-bold text-white leading-none truncate pr-2 tracking-wide">{{ digimonName }}</h2>

            <div class="relative flex items-center justify-center shrink-0">
              <span class="text-[0.6rem] font-medium text-yellow-500">
                {{ $t("digimon.level") }} {{ digimon.level }}
              </span>
            </div>
          </div>

          <ExperienceProgressBar
            :experience-to-reach-next-level="experienceToReachNextLevel"
            :experience-percentage-to-reach-next-level="experiencePercentageToReachNextLevel"
            :experience="digimon.experience"
          />
        </div>
      </div>

      <DigimonVitals :vitals="digimon.vitals" />
    </div>
  </div>
</template>
