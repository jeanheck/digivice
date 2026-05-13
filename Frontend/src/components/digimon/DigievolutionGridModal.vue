<script setup lang="ts">
import { onMounted, onUnmounted, ref, watch, computed } from 'vue'
import type { Digimon } from '../../models'
import { EvolutionGraph, type EvolutionRequirement } from '../../logic/EvolutionGraph'
import DigievolutionFamilyTree from './DigievolutionFamilyTree.vue'
import DigievolutionDetailPanel from './DigievolutionDetailPanel.vue'
import IconClose from '../icons/IconClose.vue'

const props = defineProps<{
  isOpen: boolean
  digimon: Digimon
}>()

const emit = defineEmits<{
  (e: 'close'): void
}>()

const handleKeydown = (e: KeyboardEvent) => {
  if (e.key === 'Escape' && props.isOpen) {
    emit('close')
  }
}

onMounted(() => {
  window.addEventListener('keydown', handleKeydown)
})

onUnmounted(() => {
  window.removeEventListener('keydown', handleKeydown)
})

const allEvolutions = ref<{ name: string, requirements: EvolutionRequirement[] }[]>([])
const selectedEvolution = ref<{ name: string, requirements: EvolutionRequirement[] } | null>(null)

watch(() => props.isOpen, (isOpen) => {
  if (isOpen && props.digimon) {
    allEvolutions.value = EvolutionGraph.getAllEvolutions(props.digimon.basicInfo.name)
    selectedEvolution.value = null
  }
})

const handleSelectNode = (name: string) => {
  const evo = allEvolutions.value.find(e => e.name === name)
  if (evo) {
    selectedEvolution.value = evo
  } else if (name === props.digimon.basicInfo.name) {
    selectedEvolution.value = null
  }
}

// Search Logic
const searchQuery = ref('')
const showDropdown = ref(false)

const filteredEvolutions = computed(() => {
  const query = searchQuery.value.toLowerCase()
  if (!query) return []
  return allEvolutions.value.filter(evo => evo.name.toLowerCase().includes(query))
})

const handleSearchSelect = (name: string) => {
  handleSelectNode(name)
  searchQuery.value = ''
  showDropdown.value = false
}

const handleBlur = () => {
  window.setTimeout(() => {
    showDropdown.value = false
  }, 200)
}
</script>

<template>
  <Teleport to="body">
    <transition name="fade">
      <div 
        v-if="isOpen && digimon" 
        class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60 backdrop-blur-sm"
        @click.self="emit('close')"
      >
        <div class="relative w-[98vw] max-w-[1800px] h-[92vh] max-h-[1000px] bg-[#001122] border-2 border-[#0055ff] shadow-[0_0_20px_rgba(0,119,255,0.4)] rounded-lg flex flex-col overflow-hidden animate-slide-up">
          
          <!-- Hexagon Pattern Background -->
          <div class="absolute inset-0 opacity-[0.03] pointer-events-none" 
               style="background-image: url('data:image/svg+xml,%3Csvg width=\'60\' height=\'60\' viewBox=\'0 0 60 60\' xmlns=\'http://www.w3.org/2000/svg\'%3E%3Cpath d=\'M30 0l25.98 15v30L30 60 4.02 45V15z\' stroke=\'%230077ff\' stroke-width=\'1\' fill=\'none\'/%3E%3C/svg%3E');">
          </div>

          <!-- Header -->
          <header class="flex items-center justify-between p-3 bg-gradient-to-r from-[#002244] to-[#001122] border-b border-[#0055ff]/50 relative z-20 shrink-0 gap-8">
            <div class="flex items-center gap-6 flex-1">
                <h2 class="text-[#00aaff] font-bold tracking-widest drop-shadow flex items-center gap-2 whitespace-nowrap">
                    {{ $t('digievolution.title', { name: digimon.basicInfo.name }) }}
                </h2>
                
                <!-- Search Bar -->
                <div class="relative w-128">
                <input 
                    type="text" 
                    v-model="searchQuery" 
                    @focus="showDropdown = true"
                    @blur="handleBlur"
                    :placeholder="$t('digievolution.searchDigimon')"
                    class="w-full bg-[#001a33]/60 border border-[#0055ff]/50 rounded px-3 py-1 text-xs text-[#00aaff] placeholder-[#00aaff]/40 outline-none focus:border-[#00aaff] focus:bg-[#002244] font-cyber transition-all"
                />
                <div 
                    v-if="showDropdown && searchQuery && filteredEvolutions.length > 0" 
                    class="absolute top-full left-0 right-0 mt-1 bg-[#001122] border border-[#0055ff]/50 rounded shadow-[0_4px_12px_rgba(0,119,255,0.2)] max-h-48 overflow-y-auto custom-scroll z-50 flex flex-col"
                >
                    <div 
                    v-for="evo in filteredEvolutions" 
                    :key="evo.name"
                    @click.stop="handleSearchSelect(evo.name)"
                    class="px-3 py-1.5 text-xs text-[#00aaff] hover:bg-[#0033aa] hover:text-white cursor-pointer transition-colors font-cyber border-b last:border-b-0 border-[#0055ff]/20"
                    >
                    {{ evo.name }}
                    </div>
                </div>
                </div>
            </div>

            <button 
              @click="emit('close')"
              class="text-gray-400 hover:text-red-400 transition-colors bg-black/30 w-7 h-7 flex items-center justify-center rounded border border-gray-700 hover:border-red-500 shrink-0"
            >
              <IconClose class="w-5 h-5" />
            </button>
          </header>

          <!-- Content Body -->
          <div class="flex-1 flex overflow-hidden relative z-10">
            
            <!-- Left: Family Tree (75%) -->
            <div class="w-[75%] h-full border-r border-[#0055ff]/30 relative">
              <DigievolutionFamilyTree 
                :rookie-name="digimon.basicInfo.name"
                :digimon="digimon"
                :selected-node-name="selectedEvolution?.name"
                @select-node="handleSelectNode"
              />
            </div>
            
            <!-- Right: Detail Panel (25%) -->
            <div class="w-[25%] h-full bg-[#000a1a]/60 overflow-y-auto custom-scroll flex flex-col">
              <div v-if="selectedEvolution" class="flex-1 flex flex-col p-1">
                <DigievolutionDetailPanel 
                  :evolution="selectedEvolution"
                  :digimon="digimon"
                  :all-evolutions="allEvolutions"
                  @select-evolution="handleSelectNode"
                />
              </div>
              
              <!-- Empty State -->
              <div v-else class="flex-1 flex flex-col items-center justify-center p-12 text-center">
                 <p class="text-[10px] text-blue-300/40 font-cyber leading-relaxed max-w-[200px]">
                    {{ $t('digievolution.selectNode') }}
                 </p>
              </div>
            </div>

          </div>

          <!-- Footer gradient bar -->
          <div class="h-1.5 w-full bg-gradient-to-r from-blue-900 via-cyan-500 to-blue-900 shrink-0"></div>

        </div>
      </div>
    </transition>
  </Teleport>
</template>

<style scoped>
.custom-scroll::-webkit-scrollbar { width: 6px; }
.custom-scroll::-webkit-scrollbar-track { background: rgba(0, 0, 0, 0.3); border-radius: 4px; }
.custom-scroll::-webkit-scrollbar-thumb { background: #0044aa; border-radius: 4px; }
.custom-scroll::-webkit-scrollbar-thumb:hover { background: #0077ff; }

.fade-enter-active, .fade-leave-active { transition: opacity 0.3s ease; }
.fade-enter-from, .fade-leave-to { opacity: 0; }

@keyframes slide-up {
  from { transform: translateY(20px); opacity: 0; }
  to { transform: translateY(0); opacity: 1; }
}
.animate-slide-up { animation: slide-up 0.3s cubic-bezier(0.16, 1, 0.3, 1) forwards; }
</style>
