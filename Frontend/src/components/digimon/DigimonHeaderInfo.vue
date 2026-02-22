<script setup lang="ts">
import ExpProgressBar from '../ui/ExpProgressBar.vue'
import ProgressBar from '../ui/ProgressBar.vue'

// Temporary mock type until we fetch from store
type DigimonData = {
  name: string
  level: number
  currentExp: number
  expForNextLevel: number
  currentHp: number
  maxHp: number
  currentMp: number
  maxMp: number
}

defineProps<{
  digimon: DigimonData
}>()
</script>

<template>
  <div class="flex flex-col gap-2 p-3 bg-gray-200 border-2 border-purple-400 rounded-md">
    
    <!-- Header row: Name, Icon Box, Level & Exp -->
    <div class="flex items-start gap-4">
      <!-- "Icon" placeholder block -->
      <div class="w-16 h-16 bg-green-500 rounded text-white flex items-center justify-center font-bold shadow">
        Icon
      </div>

      <div class="flex-1 flex flex-col gap-1">
        <div class="flex justify-between items-baseline mb-1">
          <h2 class="text-xl font-bold text-gray-800 leading-none">{{ digimon.name }}</h2>
          <span class="text-sm font-semibold text-gray-600">Level {{ digimon.level }}</span>
        </div>
        
        <!-- specialized EXP Bar -->
        <ExpProgressBar 
          :current-exp="digimon.currentExp" 
          :exp-for-next-level="digimon.expForNextLevel" 
        />
      </div>
    </div>

    <!-- Status Bars -->
    <div class="flex flex-col gap-1 mt-1">
       <ProgressBar 
          :current-value="digimon.currentHp" 
          :max-value="digimon.maxHp" 
          color-class="bg-red-500" 
        />
        <ProgressBar 
          :current-value="digimon.currentMp" 
          :max-value="digimon.maxMp" 
          color-class="bg-blue-600" 
        />
    </div>

  </div>
</template>
