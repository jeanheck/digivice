<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import ExpProgressBar from '../ui/ExpProgressBar.vue'
import ProgressBar from '../ui/ProgressBar.vue'
import DigimonIcon from '../ui/DigimonIcon.vue'
import { ExperienceCalculator } from '../../logic/ExperienceCalculator'
import type { Digimon } from '../../types/backend'

const props = defineProps<{
  digimon: Digimon
}>()

const isLevelingUp = ref(false)

watch(() => props.digimon.basicInfo.level, (newLevel, oldLevel) => {
  if (oldLevel > 0 && newLevel > oldLevel) {
    isLevelingUp.value = true
    setTimeout(() => {
      isLevelingUp.value = false
    }, 1500)
  }
})

const requiredExpForNextLevel = computed(() => {
  return ExperienceCalculator.getRequiredExpForNextLevel(
    props.digimon.basicInfo.name, 
    props.digimon.basicInfo.level
  )
})

const requiredExpForCurrentLevel = computed(() => {
  return ExperienceCalculator.getRequiredExpForCurrentLevel(
    props.digimon.basicInfo.name, 
    props.digimon.basicInfo.level
  )
})

const getHpColor = (current: number, max: number) => {
  if (max === 0) return 'bg-red-500'
  const percentage = current / max
  return percentage < 0.3 ? 'bg-red-500' : 'bg-green-500'
}

const getMpColor = (current: number, max: number) => {
  if (max === 0) return 'bg-yellow-400'
  const percentage = current / max
  return percentage < 0.3 ? 'bg-yellow-400' : 'bg-blue-600'
}

const getIconUrl = (name: string) => {
  try {
    return new URL(`../../icons/digimons/${name}.png`, import.meta.url).href
  } catch (e) {
    // Falha silenciosa: retorna string vazia ou placeholder caso nÃ£o encontre.
    return ''
  }
}
</script>

<template>
  <div class="relative overflow-hidden flex flex-col w-full bg-[#000a2b]">
    <!-- Borda externa brilhante simuluada via clip-path background -->
    <div class="absolute inset-0 bg-[#0077ff] pointer-events-none evo-border"></div>
    
    <!-- Fundo interno escuro (1 pixel menor que a borda) -->
    <div class="absolute inset-[1.5px] bg-[#000a2b] pointer-events-none evo-inner"></div>

    <div class="relative z-10 flex flex-col gap-2 p-3">
      <!-- Header row: Name, Icon Box, Level & Exp -->
      <div class="flex items-start gap-4">
        
        <!-- Icon Image using Vite dynamic URL -->
        <!-- Icon Image using Vite dynamic URL -->
        <DigimonIcon :digimon-name="digimon.basicInfo.name" class="w-16 h-16" />

        <div class="flex-1 flex flex-col gap-1 min-w-0">
          <div class="flex justify-between items-baseline mb-1 border-b border-[#00154a] pb-1">
            <h2 class="text-sm font-bold text-white leading-none truncate pr-2 tracking-wide">{{ digimon.basicInfo.name }}</h2>
            
            <div class="relative flex items-center justify-center flex-shrink-0">
              <span 
                class="text-[0.6rem] font-medium text-yellow-500 transition-all duration-300"
                :class="isLevelingUp ? 'scale-150 text-white drop-shadow-[0_0_8px_rgba(255,255,255,0.8)]' : 'scale-100'"
              >
                Level {{ digimon.basicInfo.level }}
              </span>
              
              <!-- Sparkles effect -->
              <div v-if="isLevelingUp" class="absolute inset-0 pointer-events-none flex items-center justify-center">
                <div class="absolute w-1 h-1 bg-white rounded-full animate-ping" style="top: -4px; left: -4px;"></div>
                <div class="absolute w-1.5 h-1.5 bg-yellow-300 rounded-full animate-ping" style="bottom: -2px; right: -8px; animation-delay: 100ms;"></div>
                <div class="absolute w-1 h-1 bg-white rounded-full animate-ping" style="top: 50%; left: 110%; animation-delay: 200ms;"></div>
              </div>
            </div>
          </div>
          
          <ExpProgressBar 
            :current-exp="digimon.basicInfo.experience" 
            :exp-for-next-level="requiredExpForNextLevel" 
            :exp-for-current-level="requiredExpForCurrentLevel"
          />
        </div>
      </div>

      <!-- Status Bars -->
      <div class="flex flex-col gap-2 mt-2">
         <ProgressBar 
            :current-value="digimon.basicInfo.currentHP" 
            :max-value="digimon.basicInfo.maxHP" 
            :color-class="getHpColor(digimon.basicInfo.currentHP, digimon.basicInfo.maxHP)" 
          />
          <ProgressBar 
            :current-value="digimon.basicInfo.currentMP" 
            :max-value="digimon.basicInfo.maxMP" 
            :color-class="getMpColor(digimon.basicInfo.currentMP, digimon.basicInfo.maxMP)" 
          />
      </div>
    </div>
  </div>
</template>

<style scoped>
.evo-border, .evo-inner {
  clip-path: polygon(4px 0, 100% 0, 100% calc(100% - 4px), calc(100% - 4px) 100%, 0 100%, 0 4px);
}
</style>
