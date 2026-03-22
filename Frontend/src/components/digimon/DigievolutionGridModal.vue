<script setup lang="ts">
import { ref, watch, nextTick } from 'vue'
import type { Digimon } from '../../types/backend'
import { EvolutionGraph, type EvolutionRequirement } from '../../logic/EvolutionGraph'
import DigievolutionGridItem from './DigievolutionGridItem.vue'
import DigievolutionDetailPanel from './DigievolutionDetailPanel.vue'
import IconClose from '../icons/IconClose.vue'

const props = defineProps<{
  isOpen: boolean
  digimon: Digimon
}>()

const emit = defineEmits<{
  (e: 'close'): void
}>()

const evolutionsList = ref<{ name: string, requirements: EvolutionRequirement[] }[]>([])
const selectedEvolution = ref<{ name: string, requirements: EvolutionRequirement[] } | null>(null)

watch(() => props.isOpen, (isOpen) => {
  if (isOpen && props.digimon) {
    evolutionsList.value = EvolutionGraph.getAllEvolutions(props.digimon.basicInfo.name)
    selectedEvolution.value = null
  }
})
</script>

<template>
  <Teleport to="body">
    <transition name="fade">
      <div 
        v-if="isOpen && digimon" 
        class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60 backdrop-blur-sm"
        @click.self="emit('close')"
      >
        <div class="relative w-full max-w-[90vw] lg:max-w-5xl max-h-[90vh] bg-[#001122] border-2 border-[#0055ff] shadow-[0_0_20px_rgba(0,119,255,0.4)] rounded-lg flex flex-col overflow-hidden animate-slide-up">
          
          <!-- Cyberpunk Hexagon Pattern Background -->
          <div class="absolute inset-0 opacity-[0.03] pointer-events-none" 
               style="background-image: url('data:image/svg+xml,%3Csvg width=\'60\' height=\'60\' viewBox=\'0 0 60 60\' xmlns=\'http://www.w3.org/2000/svg\'%3E%3Cpath d=\'M30 0l25.98 15v30L30 60 4.02 45V15z\' stroke=\'%230077ff\' stroke-width=\'1\' fill=\'none\'/%3E%3C/svg%3E');">
          </div>

          <!-- Header -->
          <header class="flex items-center justify-between p-3 bg-gradient-to-r from-[#002244] to-[#001122] border-b border-[#0055ff]/50 relative z-10 shrink-0">
            <h2 class="text-white font-bold tracking-widest text-[#00aaff] drop-shadow flex items-center gap-2">
              <span class="text-blue-400 text-lg leading-none">●</span>
              {{ digimon.basicInfo.name }} Digievolutions
            </h2>
            <button 
              @click="emit('close')"
              class="text-gray-400 hover:text-red-400 transition-colors bg-black/30 w-7 h-7 flex items-center justify-center rounded border border-gray-700 hover:border-red-500"
            >
              <IconClose class="w-5 h-5" />
            </button>
          </header>

          <!-- Content Body -->
          <div class="flex flex-col lg:flex-row min-h-[500px] w-full p-4 lg:p-6 gap-6 relative z-10 overflow-hidden">
            <!-- Left List (Scrollable Grid) -->
            <div class="flex-1 overflow-y-auto custom-scroll pr-2">
              <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
                <DigievolutionGridItem 
                  v-for="evo in evolutionsList" 
                  :key="evo.name"
                  :evolution="evo"
                  :digimon="digimon"
                  :is-selected="selectedEvolution?.name === evo.name"
                  @select="selectedEvolution = evo"
                />
              </div>
              <div v-if="evolutionsList.length === 0" class="text-white text-center mt-10 font-cyber">
                NO DATA NODES DETECTED.
              </div>
            </div>
            
            <!-- Right Panel (Interactive Focus) -->
            <div class="w-full lg:w-[450px] shrink-0 bg-[#0f1020]/90 border border-[#0055ff]/30 rounded-lg shadow-[0_0_15px_rgba(0,119,255,0.15)] overflow-y-auto custom-scroll p-1">
              <DigievolutionDetailPanel 
                v-if="selectedEvolution"
                :evolution="selectedEvolution"
                :digimon="digimon"
                :all-evolutions="evolutionsList"
                @select-evolution="name => selectedEvolution = evolutionsList.find(e => e.name === name) || selectedEvolution"
              />
              <div v-else class="flex flex-col items-center justify-center h-full text-cyan-200/50 font-cyber p-8 text-center gap-4">
                <div class="w-16 h-16 border-2 border-dashed border-[#0055ff] rounded-full animate-[spin_10s_linear_infinite] opacity-50 relative flex items-center justify-center">
                   <div class="w-2 h-2 bg-[#00aaff] rounded-full"></div>
                </div>
                Select a Data Node to inspect properties & requirements.
              </div>
            </div>
          </div>

        </div>
      </div>
    </transition>
  </Teleport>
</template>

<style scoped>
.custom-scroll::-webkit-scrollbar { width: 4px; }
.custom-scroll::-webkit-scrollbar-track { background: transparent; }
.custom-scroll::-webkit-scrollbar-thumb { background: #0055ff55; border-radius: 2px; }
.custom-scroll::-webkit-scrollbar-thumb:hover { background: #0077ff88; }
</style>
