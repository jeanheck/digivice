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

import TechniquesTable from '../../data/static/TechniquesTable.json'
import DigievolutionTechniques from '../../data/static/DigievolutionTechniques.json'

interface TechEntry {
  id: string
  name: string
  type: string
  element: string
  elementStrength: number
  mp: number
  power: number
  description: string
}

interface DigivolutionTechEntry {
  techniqueId: string
  learnLevel: number
}

// Lookup maps
const techniqueById = Object.fromEntries(
  (TechniquesTable as { techniques: TechEntry[] }).techniques.map(t => [t.id, t])
)

const techniques = computed(() => {
  const entry = (DigievolutionTechniques as { digievolutions: { name: string; techniques: DigivolutionTechEntry[] }[] })
    .digievolutions.find(d => d.name === props.evolution.name)
  if (!entry) return []

  const maxLearnLevel = Math.max(...entry.techniques.map(t => t.learnLevel))

  return entry.techniques.map(t => {
    const base = techniqueById[t.techniqueId]
    return {
      ...base,
      learnLevel: t.learnLevel,
      isSignature: t.learnLevel === maxLearnLevel
    }
  })
})

function elementColor(element: string): string {
  const map: Record<string, string> = {
    'Fire': 'text-orange-400',
    'Water': 'text-blue-400',
    'Ice': 'text-cyan-300',
    'Wind': 'text-gray-300',
    'Thunder': 'text-yellow-300',
    'Dark': 'text-purple-400',
    'Machine': 'text-gray-400',
    'None': 'text-white/60',
  }
  return map[element] ?? 'text-white/60'
}

function typeIcon(type: string): string {
  if (type === 'Physical') return '👊'
  if (type === 'Magical') return '🧙‍♂️'
  if (type === 'Heal') return '💚'
  if (type === 'Support') return '🟡'
  return '?'
}

const reqEvolutions = computed(() => {
    return props.evolution.requirements
        .filter(req => req.Type === 'DigievolutionLevel')
        .map(req => req.Digievolution!)
})

const derivatives = computed(() => {
    return props.allEvolutions.filter(evo => 
        evo.requirements.some(req => req.Type === 'DigievolutionLevel' && req.Digievolution === props.evolution.name)
    )
})
</script>

<template>
  <div class="flex flex-col h-full bg-[#0c0d1b] rounded overflow-hidden relative">
    <!-- Header Hero -->
    <div class="relative flex-none pt-4 px-4 pb-2 flex flex-col items-center justify-center bg-gradient-to-b from-[#00051a] to-transparent shrink-0">
        <div class="relative w-full h-32 bg-gradient-to-r from-cyan-950/40 to-[#001533] rounded-lg border border-cyan-800/50 shadow-inner overflow-hidden group">
            
            <img v-if="getAvatar(evolution.name)" 
                 :src="getAvatar(evolution.name)" 
                 class="absolute inset-0 w-full h-full object-cover object-[center_15%] opacity-40 pointer-events-none drop-shadow-[0_0_15px_rgba(0,170,255,0.4)] transition-opacity duration-500" 
                 alt="Avatar Overlay" />

            <!-- Pattern Overlay -->
            <h2 class="absolute top-3 left-4 text-lg sm:text-xl font-bold font-cyber text-white tracking-widest drop-shadow-[0_0_8px_rgba(255,255,255,0.5)] z-10">
                {{ evolution.name }}
            </h2>
        </div>
    </div>

    <div class="px-4 pb-4 pt-2 flex-1 flex flex-col gap-4 overflow-y-auto custom-scroll">
        <!-- Compact Base Requirements Section -->
        <div v-if="reqEvolutions.length > 0" class="shrink-0">
            <div class="text-[9px] text-indigo-400/80 mb-2 flex items-center font-cyber uppercase tracking-widest">
                <span class="text-[11px] mr-2 opacity-80 drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-0.5">🧬</span>
                Requirement Digievolutions
            </div>
            
            <div class="flex flex-wrap gap-2">
                <button v-for="reqEvo in reqEvolutions" :key="reqEvo"
                      @click="emit('select-evolution', reqEvo)"
                      class="cursor-pointer hover:bg-indigo-700/50 transition-colors text-[10px] px-2 py-1.5 bg-indigo-950/30 text-indigo-200 border border-indigo-900/40 rounded font-cyber flex items-center">
                    <div class="w-1.5 h-1.5 rounded-full mr-2 bg-indigo-500/50"></div>
                    {{ reqEvo }}
                </button>
            </div>
        </div>

        <!-- Techniques Section -->
        <div class="flex flex-col">
            <div class="flex flex-col gap-[3px] pr-1">
                <div
                  v-for="tech in techniques"
                  :key="tech.id"
                  class="relative rounded px-3 py-2 flex items-start gap-3 border transition-all text-xs border-[#0055ff]/40 bg-[#001a33]/60"
                  :class="{ 'bg-yellow-950/30 border-yellow-500/60 shadow-[0_0_8px_rgba(234,179,8,0.2)]': tech.isSignature }"
                >
                  <span
                    v-if="tech.isSignature"
                    class="absolute top-1 right-2 text-[10px] text-yellow-400 font-bold tracking-widest"
                  >
                    ⭐ SIG
                  </span>

                  <span class="text-base leading-none mt-[1px] flex-shrink-0">
                    {{ typeIcon(tech.type ?? '') }}
                  </span>

                  <div class="flex-1 min-w-0">
                    <div class="flex items-center gap-1 mb-[2px]">
                      <span class="font-bold tracking-wide" :class="tech.isSignature ? 'text-yellow-300' : 'text-white'">
                        {{ tech.name }}
                      </span>
                      <span class="text-[10px] text-cyan-400/80 ml-1">Lv.{{ tech.learnLevel }}</span>
                    </div>

                    <p class="text-white/50 text-[10px] leading-snug">{{ tech.description }}</p>

                    <div class="flex gap-3 mt-1 text-[9px] uppercase tracking-wider">
                      <span :class="elementColor(tech.element ?? 'None')">{{ (tech.element ?? 'None') !== 'None' ? tech.element : 'Neutral' }}</span>
                      <span class="text-blue-300/70">MP {{ tech.mp }}</span>
                      <span class="text-red-300/70">PWR {{ tech.power }}</span>
                    </div>
                  </div>
                </div>
                
                <p v-if="techniques.length === 0" class="text-white/40 text-center py-4 text-[10px] italic font-cyber border border-white/5 rounded">
                  No technique data available.
                </p>
            </div>
        </div>

        <!-- Derivatives Section -->
        <div v-if="derivatives.length > 0" class="shrink-0">
            <div class="text-[9px] text-indigo-400/80 mb-2 flex items-center font-cyber uppercase tracking-widest">
                <span class="text-[11px] mr-2 opacity-80 drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-0.5">🧬</span>
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
