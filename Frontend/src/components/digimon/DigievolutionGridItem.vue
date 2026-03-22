<script setup lang="ts">
import { computed } from 'vue'
import type { Digimon } from '../../types/backend'
import { EvolutionGraph, type EvolutionRequirement } from '../../logic/EvolutionGraph'

const props = defineProps<{
  evolution: { name: string, requirements: EvolutionRequirement[] }
  digimon: Digimon
  isSelected: boolean
}>()

const emit = defineEmits<{
  (e: 'select'): void
}>()

const isUnlocked = computed(() => {
    // Generate a mock GraphNode on the fly to fulfill the existing interface
    const mockNode = { 
      id: '', 
      name: props.evolution.name, 
      requirements: props.evolution.requirements, 
      children: [] 
    }
    return EvolutionGraph.checkRequirements(props.digimon, mockNode)
})
</script>

<template>
  <button 
    @click="emit('select')"
    class="relative group h-24 flex flex-col justify-end p-2 border transition-all duration-300 overflow-hidden text-left cursor-pointer"
    :class="[
      isSelected ? 'border-cyan-400 bg-cyan-900/40 shadow-[0_0_10px_rgba(0,255,255,0.3)]' : 'border-[#1a1c35] bg-[#0c0d1a]/80 hover:border-cyan-700 hover:bg-cyan-900/20',
      !isUnlocked ? 'opacity-70 saturate-50 hover:opacity-100' : ''
    ]"
  >
    <div class="absolute inset-0 bg-gradient-to-t from-[#02030a] via-[#02030a]/40 to-transparent opacity-90 z-0 pointer-events-none"></div>
    
    <div class="absolute top-2 right-2 z-10 transition-opacity" :class="isSelected ? 'opacity-100' : 'opacity-60 group-hover:opacity-100'">
      <span v-if="isUnlocked" class="text-xs drop-shadow-[0_0_2px_rgba(0,0,0,1)]">🔓</span>
      <span v-else class="text-xs drop-shadow-[0_0_2px_rgba(0,0,0,1)] opacity-50 grayscale">🔒</span>
    </div>

    <!-- Inner Content -->
    <div class="relative z-10 text-[11px] sm:text-xs text-white font-cyber select-none mt-auto transition-colors break-words whitespace-normal leading-tight pr-4"
         :class="isSelected ? 'text-cyan-300' : 'group-hover:text-cyan-200'">
      {{ evolution.name }}
    </div>
  </button>
</template>
