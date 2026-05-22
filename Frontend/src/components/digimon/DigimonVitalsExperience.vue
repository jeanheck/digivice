<script setup lang="ts">
import { computed } from 'vue';
import ExperienceProgressBar from '../ui/ExperienceProgressBar.vue';
import ProgressBar from '../ui/ProgressBar.vue';
import DigimonIcon from '../ui/DigimonIcon.vue';
import type { Digimon } from '../../models';
import { ProgressBarTypes } from '@/types/ui';
import { DigimonRepository } from '../../repositories/digimon-repository';
import { DigimonExperienceCalculator } from '../../logic/DigimonExperienceCalculator';

const props = defineProps<{
  digimon: Digimon;
  digimonId: number;
}>();

const name = computed(() => {
  return DigimonRepository.getDigimonNameById(props.digimonId);
});

const experienceToReachNextLevel = computed(() => {
  return DigimonExperienceCalculator.getRequiredExperienceForNextLevel(name.value, props.digimon.level);
});

const experiencePercentageToReachNextLevel = computed(() => {
  return DigimonExperienceCalculator.getProgressPercentageForNextLevel(name.value, props.digimon.level, props.digimon.experience);
});
</script>

<template>
  <div class="relative overflow-hidden flex flex-col w-full bg-[#000a2b]">
    <div class="absolute inset-0 bg-[#0077ff] pointer-events-none dw3-beveled"></div>
    <div class="absolute inset-[1.5px] bg-[#000a2b] pointer-events-none dw3-beveled"></div>

    <div class="relative z-10 flex flex-col gap-2 p-3">
      <div class="flex items-start gap-4">
        <DigimonIcon :digimon-name="name" class="w-16 h-16" />

        <div class="flex-1 flex flex-col gap-1 min-w-0">
          <div class="flex justify-between items-baseline mb-1 border-b border-[#00154a] pb-1">
            <h2 class="text-sm font-bold text-white leading-none truncate pr-2 tracking-wide">{{ name }}</h2>
            
            <div class="relative flex items-center justify-center shrink-0">
              <span class="text-[0.6rem] font-medium text-yellow-500">
                {{ $t('common.level') }} {{ digimon.level }}
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

      <div class="flex flex-col gap-2 mt-2">
         <ProgressBar 
            :current-value="digimon.vitals.currentHP" 
            :max-value="digimon.vitals.maxHP" 
            :type="ProgressBarTypes.HP"
          />
          <ProgressBar 
            :current-value="digimon.vitals.currentMP" 
            :max-value="digimon.vitals.maxMP" 
            :type="ProgressBarTypes.MP"
          />
      </div>
    </div>
  </div>
</template>
