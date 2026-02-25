<script setup lang="ts">
import { computed, ref, watch } from 'vue'

const props = defineProps<{
  currentExp: number
  expForNextLevel: number
  expForCurrentLevel?: number
}>()

const isLevelingUp = ref(false)

// Watching for an increase in the required EXP marks a formal Level Up boundary.
watch(() => props.expForNextLevel, (newExp, oldExp) => {
  if (oldExp > 0 && newExp > oldExp) {
    isLevelingUp.value = true
    setTimeout(() => {
      isLevelingUp.value = false
    }, 1500)
  }
})

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
      class="absolute left-0 top-0 h-full transition-all duration-300 bg-opacity-90 bg-gradient-to-r"
      :class="isLevelingUp ? 'from-yellow-300 to-white animate-pulse' : 'from-orange-600 to-yellow-500'"
      :style="{ width: `${percentage}%` }"
    ></div>
    
    <!-- Level up glow overlay -->
    <div 
      v-if="isLevelingUp" 
      class="absolute inset-0 bg-white opacity-40 animate-ping pointer-events-none"
    ></div>
    
    <span class="relative z-10 text-[0.6rem] font-bold text-white drop-shadow-md px-1 tracking-wider">
      EXP: {{ currentExp }} / {{ expForNextLevel }}
    </span>
  </div>
</template>
