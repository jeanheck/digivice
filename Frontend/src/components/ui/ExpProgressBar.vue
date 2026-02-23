<script setup lang="ts">
import { computed } from 'vue'

const props = defineProps<{
  currentExp: number
  expForNextLevel: number
}>()

const percentage = computed(() => {
  if (props.expForNextLevel <= 0) return 100
  return Math.min(100, Math.max(0, (props.currentExp / props.expForNextLevel) * 100))
})
</script>

<template>
  <div class="relative w-full h-6 bg-gray-300 rounded overflow-hidden shadow-inner flex items-center justify-center border border-gray-400">
    <div 
      class="absolute left-0 top-0 h-full bg-cyan-400 transition-all duration-300 bg-opacity-90"
      :style="{ width: `${percentage}%` }"
    ></div>
    
    <span class="relative z-10 text-[0.6rem] font-bold text-gray-800 drop-shadow-sm px-1 tracking-wider">
      EXP: {{ currentExp }} / {{ expForNextLevel }}
    </span>
  </div>
</template>
