<script setup lang="ts">
/**
 * ExperienceProgressBar
 * Purely presentational component.
 * Receives pre-calculated experience data from the domain model.
 */
defineProps<{
  currentExperience: number;
  experienceToReachNextLevel: number;
  experiencePercentageToReachNextLevel: number;
}>();
</script>

<template>
  <div class="relative w-full h-6 bg-[#000e3f] rounded overflow-hidden shadow-inner flex items-center justify-center border-2 border-[#00154a]">
    <!-- Animated Progress Bar -->
    <div 
      class="absolute left-0 top-0 h-full transition-all duration-500 bg-opacity-90 bg-gradient-to-r from-orange-600 to-yellow-500"
      :style="{ width: `${experiencePercentageToReachNextLevel}%` }"
    ></div>
    
    <!-- Text Overlay -->
    <span class="relative z-10 text-[0.6rem] font-bold text-white drop-shadow-md px-1 tracking-wider">
      EXP: {{ currentExperience }} / {{ experienceToReachNextLevel }}
    </span>
  </div>
</template>
