<script setup lang="ts">
import { computed } from 'vue'

const props = defineProps<{
  currentValue: number
  maxValue: number
  colorClass: string
  label?: string
}>()

const percentage = computed(() => {
  if (props.maxValue <= 0) return 0
  return Math.min(100, Math.max(0, (props.currentValue / props.maxValue) * 100))
})
</script>

<template>
  <div class="relative w-full h-6 bg-gray-300 rounded overflow-hidden shadow-inner flex items-center justify-center border border-gray-400">
    <div 
      class="absolute left-0 top-0 h-full transition-all duration-300 bg-opacity-90"
      :class="colorClass"
      :style="{ width: `${percentage}%` }"
    ></div>
    
    <span class="relative z-10 text-[0.6rem] font-bold text-white drop-shadow-md px-1 tracking-wider">
      {{ label || `${currentValue} / ${maxValue}` }}
    </span>
  </div>
</template>
