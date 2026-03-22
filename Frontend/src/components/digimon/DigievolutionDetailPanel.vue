<script setup lang="ts">
import { computed } from 'vue'
import type { Digimon } from '../../types/backend'
import { EvolutionGraph, type EvolutionRequirement } from '../../logic/EvolutionGraph'

const props = defineProps<{
  evolution: { name: string, requirements: EvolutionRequirement[] }
  digimon: Digimon
  allEvolutions: { name: string, requirements: EvolutionRequirement[] }[]
}>()

const emit = defineEmits<{
  (e: 'select-evolution', name: string): void
}>()

const isUnlocked = computed(() => {
    const mockNode = { id: '', name: props.evolution.name, requirements: props.evolution.requirements, children: [] }
    return EvolutionGraph.checkRequirements(props.digimon, mockNode)
})

const avatarModules = import.meta.glob('../../assets/icons/digievolutions/*.png', { eager: true })

const getAvatar = (name: string) => {
    const path = `../../assets/icons/digievolutions/${name}.png`
    return avatarModules[path] ? (avatarModules[path] as any).default || avatarModules[path] : null
}

const getRequirementText = (req: EvolutionRequirement) => {
    switch (req.Type) {
        case 'DigimonLevel': return `Rookie Level ${req.Value} or higher`
        case 'Attribute': return `${req.Attribute} Attribute >= ${req.Value}`
        case 'DigievolutionLevel': return `${req.Digievolution} Level ${req.Value}`
        default: return 'Unknown Parameter'
    }
}

// Verifica se o requisito específico foi atendido (visual logic fallback)
const isReqMet = (req: EvolutionRequirement) => {
    switch (req.Type) {
        case 'DigimonLevel': return props.digimon.basicInfo.level >= req.Value
        case 'Attribute': 
            const val = props.digimon.attributes[req.Attribute?.toLowerCase() as keyof typeof props.digimon.attributes] || 0
            return val >= req.Value
        case 'DigievolutionLevel':
            // Backend currently does not provide full dictionary of all unlocked evolutions globally.
            // Returning false visually for now is consistent with DMW3 mechanics until DB is fully synced.
            return false
        default: return false
    }
}

const derivatives = computed(() => {
    return props.allEvolutions.filter(evo => 
        evo.requirements.some(req => req.Type === 'DigievolutionLevel' && req.Digievolution === props.evolution.name)
    )
})
</script>

<template>
  <div class="flex flex-col h-full bg-[#0c0d1b] rounded overflow-hidden relative">
    <!-- Header Hero -->
    <div class="relative flex-none p-5 flex flex-col items-center justify-center border-b border-[#001133] bg-gradient-to-b from-[#00051a] to-transparent shrink-0">
        <div class="relative w-full h-40 bg-gradient-to-r from-cyan-950/40 to-[#001533] rounded-lg border border-cyan-800/50 flex flex-col items-center justify-center shadow-inner overflow-hidden group">
            
            <img v-if="getAvatar(evolution.name)" 
                 :src="getAvatar(evolution.name)" 
                 class="absolute inset-0 w-full h-full object-cover object-[center_15%] opacity-30 mix-blend-screen pointer-events-none drop-shadow-[0_0_15px_rgba(0,170,255,0.4)] transition-opacity duration-500" 
                 alt="Avatar Overlay" />

            <!-- Pattern Overlay -->
            <h2 class="text-xl font-bold font-cyber text-white tracking-widest text-center drop-shadow-[0_0_8px_rgba(0,255,255,0.5)]">
                {{ evolution.name }}
            </h2>
            
            <div class="mt-6 px-4 py-1.5 rounded-full text-[10px] sm:text-xs font-bold font-cyber tracking-wider border ring-1"
                 :class="isUnlocked ? 'bg-emerald-950/60 text-emerald-400 border-emerald-500/50 ring-emerald-500/20' : 'bg-red-950/60 text-red-400 border-red-500/50 ring-red-500/20'">
                {{ isUnlocked ? 'UNLOCKED' : 'LOCKED' }}
            </div>
        </div>
    </div>

    <div class="p-5 flex-1 flex flex-col gap-8 overflow-y-auto custom-scroll">
        <!-- Requirements Section -->
        <div>
            <div class="text-xs text-cyan-500/80 mb-4 border-b border-cyan-900 pb-2 flex items-center font-cyber uppercase tracking-widest">
                <span class="text-sm mr-2 opacity-80 drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-0.5">📊</span>
                Requirements
            </div>
            
            <ul v-if="evolution.requirements.length > 0" class="flex flex-col gap-2">
                <template v-for="(req, idx) in evolution.requirements" :key="idx">
                    <button 
                        v-if="req.Type === 'DigievolutionLevel' && allEvolutions.some(e => e.name === req.Digievolution)"
                        @click="emit('select-evolution', req.Digievolution!)"
                        class="text-[11px] p-2.5 rounded font-cyber flex items-center shadow-inner tracking-wide w-full text-left transition-colors cursor-pointer"
                        :class="isReqMet(req) 
                            ? 'bg-emerald-950/20 text-emerald-200 border border-emerald-800/50 hover:bg-emerald-900/40' 
                            : 'bg-red-950/20 text-red-200 border border-red-900/50 hover:bg-red-900/40'"
                    >
                        {{ getRequirementText(req) }}
                    </button>
                    <div 
                        v-else
                        class="text-[11px] p-2.5 rounded font-cyber flex items-center shadow-inner tracking-wide"
                        :class="isReqMet(req) 
                            ? 'bg-emerald-950/20 text-emerald-200 border border-emerald-800/50' 
                            : 'bg-red-950/20 text-red-200 border border-red-900/50'"
                    >
                        {{ getRequirementText(req) }}
                    </div>
                </template>
            </ul>
            <div v-else class="text-xs text-emerald-400/70 italic font-cyber opacity-80 border border-emerald-900/30 bg-emerald-950/10 p-3 rounded">
                Base Digievolution Route. No core prerequisites needed.
            </div>
        </div>

        <!-- Derivatives Section -->
        <div v-if="derivatives.length > 0" class="mt-auto">
            <div class="text-xs text-indigo-400/80 mb-3 border-b border-indigo-900 pb-2 flex items-center font-cyber uppercase tracking-widest">
                <span class="text-sm mr-2 opacity-80 drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-0.5">🧬</span>
                Next Digievolutions
            </div>
            <div class="flex flex-wrap gap-2">
                <button v-for="deriv in derivatives" :key="deriv.name"
                      @click="emit('select-evolution', deriv.name)"
                      class="cursor-pointer hover:bg-indigo-700/50 transition-colors text-[10px] px-2 py-1.5 bg-indigo-950/30 text-indigo-200 border border-indigo-900/40 rounded font-cyber flex items-center">
                    <div class="w-1.5 h-1.5 rounded-full mr-2 bg-indigo-500/50"></div>
                    {{ deriv.name }}
                </button>
            </div>
        </div>
    </div>
  </div>
</template>
