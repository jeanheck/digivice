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

const getRequirementText = (req: EvolutionRequirement) => {
    switch (req.Type) {
        case 'DigimonLevel': return `Rookie Lv ${req.Value}`
        case 'Attribute': return `${req.Attribute} >= ${req.Value}`
        case 'DigievolutionLevel': return `${req.Digievolution} Lv ${req.Value}`
        default: return 'Unknown Parameter'
    }
}

const isReqMet = (req: EvolutionRequirement) => {
    switch (req.Type) {
        case 'DigimonLevel': return props.digimon.basicInfo.level >= req.Value
        case 'Attribute': 
            const val = props.digimon.attributes[req.Attribute?.toLowerCase() as keyof typeof props.digimon.attributes] || 0
            return val >= req.Value
        case 'DigievolutionLevel':
            // Base evolutions sync will fix this universally soon
            return false
        default: return false
    }
}

const avatarModules = import.meta.glob('../../assets/icons/digievolutions/*.png', { eager: true })

const getAvatar = (name: string) => {
    const path = `../../assets/icons/digievolutions/${name}.png`
    return avatarModules[path] ? (avatarModules[path] as any).default || avatarModules[path] : null
}
</script>

<template>
  <button 
    @click="emit('select')"
    class="relative group h-24 flex flex-col justify-end p-2 border transition-all duration-300 overflow-hidden text-left cursor-pointer"
    :class="[
      isSelected ? 'border-cyan-400 bg-cyan-900/40 shadow-[0_0_10px_rgba(0,255,255,0.3)]' : 'border-[#1a1c35] bg-[#0c0d1a]/80 hover:border-cyan-700 hover:bg-cyan-900/20'
    ]"
  >
    <div class="absolute inset-0 bg-[url('/src/assets/bg-pattern.svg')] opacity-10 z-0 pointer-events-none"></div>
    
    <img v-if="getAvatar(evolution.name)" 
         :src="getAvatar(evolution.name)" 
         class="absolute inset-0 w-full h-[150%] object-cover object-[center_15%] opacity-75 pointer-events-none saturate-100 group-hover:opacity-100 transition-all duration-500" />
         
    <div class="absolute inset-0 bg-gradient-to-t from-[#000000] via-[#000000]/60 to-transparent opacity-95 z-0 pointer-events-none"></div>
    
    <!-- Header: Name and Lock -->
    <div class="absolute top-2 left-2 right-2 z-10 flex justify-between items-start">
      <div class="text-[11px] sm:text-xs font-cyber font-bold select-none transition-colors leading-tight pr-2 break-words text-[#ffffff] drop-shadow-[0_2px_2px_rgba(0,0,0,1)]">
        {{ evolution.name }}
      </div>
      
      <div class="transition-opacity flex-shrink-0" :class="isSelected ? 'opacity-100' : 'opacity-60 group-hover:opacity-100'">
        <span v-if="isUnlocked" class="text-[10px] sm:text-[11px] drop-shadow-[0_0_2px_rgba(0,0,0,1)] flex">🔓</span>
        <span v-else class="text-[10px] sm:text-[11px] drop-shadow-[0_0_2px_rgba(0,0,0,1)] opacity-50 grayscale flex">🔒</span>
      </div>
    </div>

    <!-- Inner Content (Requirements Text Bottom-Left) -->
    <div class="relative z-10 flex flex-col gap-0.5 mt-auto">
        <template v-for="(req, idx) in evolution.requirements" :key="idx">
            <span class="text-[9px] font-cyber font-bold tracking-wide w-full truncate text-left"
                  :class="isReqMet(req) ? 'text-emerald-400' : 'text-[#bb3333]'">
                {{ getRequirementText(req) }}
            </span>
        </template>
        <span v-if="evolution.requirements.length === 0" class="text-[9px] font-cyber tracking-wide text-emerald-500/50 italic opacity-80 border border-emerald-900/30 bg-emerald-950/10 px-1 rounded w-fit">
            No Core Reqs
        </span>
    </div>
  </button>
</template>
