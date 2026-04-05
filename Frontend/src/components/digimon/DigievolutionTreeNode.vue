<script setup lang="ts">
import { computed } from 'vue'
import type { Digimon } from '../../types/backend'
import { EvolutionGraph, type EvolutionRequirement } from '../../logic/EvolutionGraph'

const props = defineProps<{
  node: { name: string, requirements: EvolutionRequirement[] }
  digimon: Digimon
  isSelected?: boolean
}>()

const emit = defineEmits<{
  (e: 'select'): void
}>()

const isUnlocked = computed(() => {
    return EvolutionGraph.checkRequirements(props.digimon, props.node)
})

const getRequirementText = (req: EvolutionRequirement) => {
    switch (req.Type) {
        case 'DigimonLevel': return `${props.digimon.basicInfo.name} Lv ${req.Value}`
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
            // Logic for checking specific digievolution level would go here
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
  <div 
    @click="emit('select')"
    class="node-container group relative w-54 h-20 transition-all duration-300 cursor-pointer"
  >
    <!-- Node Frame -->
    <div
      class="absolute inset-0 border-2 rounded-md overflow-hidden transition-all duration-300 shadow-[0_4px_10px_black]"
      :class="[
        isSelected ? 'border-cyan-400 bg-cyan-900/40 shadow-[0_0_20px_rgba(0,255,255,0.5)] scale-105 z-10' : 'border-[#1a1c35] bg-[#0c0d1a]/95 hover:border-cyan-700 hover:bg-cyan-950/20',
        !isUnlocked ? 'grayscale-[0.4] opacity-80' : ''
      ]"
    >
        <!-- Background Avatar -->
        <img v-if="getAvatar(node.name)" 
             :src="getAvatar(node.name)" 
             class="absolute inset-0 w-full h-[150%] object-cover object-[center_15%] pointer-events-none saturate-100 transition-all duration-500"
             :class="isUnlocked ? 'opacity-100' : 'opacity-60 group-hover:opacity-100'" />
             
        <div class="absolute inset-0 bg-gradient-to-t from-black/80 via-black/30 to-transparent opacity-50 z-0 pointer-events-none"></div>

        <!-- Header: Name & Lock -->
        <div class="absolute top-1.5 left-2 right-2 z-10 flex justify-between items-center">
            <span class="text-[10px] sm:text-[11px] font-cyber font-bold select-none text-white drop-shadow-[0_2px_2px_black] truncate pr-1">
              {{ node.name }}
            </span>
            <span v-if="!isUnlocked" class="text-[10px] opacity-70 grayscale">🔒</span>
            <span v-else class="text-[10px] opacity-90">🔓</span>
        </div>

        <!-- Bottom: Requirements (Compact) -->
        <div class="absolute bottom-1.5 left-2 right-2 z-10 flex flex-col gap-0.5 pointer-events-none">
            <template v-for="(req, idx) in node.requirements.slice(0, 2)" :key="idx">
                <span class="text-[8px] font-cyber font-bold tracking-tight truncate"
                      :class="isReqMet(req) ? 'text-emerald-400' : 'text-[#ddd]'">
                    {{ getRequirementText(req) }}
                </span>
            </template>
        </div>
    </div>
  </div>
</template>

<style scoped>
.node-container {
    user-select: none;
}
</style>
