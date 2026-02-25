<script setup lang="ts">
import { computed } from 'vue'

const props = defineProps<{
  currentExp: number
  expForNextLevel: number
  expForCurrentLevel?: number
}>()

const percentage = computed(() => {
  if (props.expForNextLevel <= 0) return 100
  
  const currentBase = props.expForCurrentLevel || 0
  const totalRange = props.expForNextLevel - currentBase
  
  if (totalRange <= 0) return 100
  
  return Math.min(100, Math.max(0, ((props.currentExp - currentBase) / totalRange) * 100))
})
</script>

<template>
  <div class="relative w-full h-6 bg-[#000e3f] rounded overflow-hidden shadow-inner flex items-center justify-center border-2 border-[#00154a]">
    <div 
      class="absolute left-0 top-0 h-full transition-all duration-300 bg-opacity-90 bg-gradient-to-r from-orange-600 to-yellow-500"
      :style="{ width: `${percentage}%` }"
    ></div>
    
    <span class="relative z-10 text-[0.6rem] font-bold text-white drop-shadow-md px-1 tracking-wider">
      EXP: {{ currentExp }} / {{ expForNextLevel }}
    </span>
  </div>
</template>
