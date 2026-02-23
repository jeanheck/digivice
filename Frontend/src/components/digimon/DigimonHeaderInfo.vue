<script setup lang="ts">
import ExpProgressBar from '../ui/ExpProgressBar.vue'
import ProgressBar from '../ui/ProgressBar.vue'
import type { Digimon } from '../../types/backend'

defineProps<{
  digimon: Digimon
}>()

// ExpForNextLevel will depend on the backend later, setting arbitrary 1000 for now to avoid breaking UI.
const getExpForNextLevel = (level: number) => level * 100

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
</script>

<template>
  <div class="flex flex-col gap-2 p-3 bg-gray-200 border-2 border-purple-400 rounded-md">
    
    <!-- Header row: Name, Icon Box, Level & Exp -->
    <div class="flex items-start gap-4">
      <!-- "Icon" placeholder block -->
      <div class="w-16 h-16 bg-green-500 rounded text-white flex items-center justify-center font-bold shadow text-xs text-center">
        Icon
      </div>

      <div class="flex-1 flex flex-col gap-1">
        <div class="flex justify-between items-baseline mb-1">
          <h2 class="text-sm font-bold text-gray-800 leading-none truncate">{{ digimon.basicInfo.name }}</h2>
          <span class="text-[0.6rem] font-semibold text-gray-600">Lv{{ digimon.basicInfo.level }}</span>
        </div>
        
        <!-- specialized EXP Bar -->
        <ExpProgressBar 
          :current-exp="digimon.basicInfo.experience" 
          :exp-for-next-level="getExpForNextLevel(digimon.basicInfo.level)" 
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
</template>
