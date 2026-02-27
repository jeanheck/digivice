<script setup lang="ts">
import { computed } from 'vue'
import type { MainQuest, SideQuest } from '../../types/backend'
import IconClose from '~icons/pixelarticons/close'

export interface QuestStep {
    number: number
    description: string
    isCompleted: boolean
}

const props = defineProps<{
  quest: MainQuest | SideQuest | null
  isOpen: boolean
}>()

const emit = defineEmits(['close'])

const closeModal = () => {
  emit('close')
}

const isQuestDone = computed(() => {
  if (!props.quest || !props.quest.steps || props.quest.steps.length === 0) return false;
  return props.quest.steps.every(s => s.isCompleted);
})
</script>

<template>
  <Teleport to="body">
    <Transition name="fade">
      <div 
        v-if="isOpen && quest" 
        class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60 backdrop-blur-sm"
        @click.self="closeModal"
      >
        <div class="relative w-full max-w-md bg-[#001122] border-2 border-[#0055ff] shadow-[0_0_20px_rgba(0,119,255,0.4)] rounded-lg flex flex-col overflow-hidden animate-slide-up">
          
          <!-- Cyberpunk Hexagon Pattern Background -->
          <div class="absolute inset-0 opacity-[0.03] pointer-events-none" 
               style="background-image: url('data:image/svg+xml,%3Csvg width=\'60\' height=\'60\' viewBox=\'0 0 60 60\' xmlns=\'http://www.w3.org/2000/svg\'%3E%3Cpath d=\'M30 0l25.98 15v30L30 60 4.02 45V15z\' stroke=\'%230077ff\' stroke-width=\'1\' fill=\'none\'/%3E%3C/svg%3E');">
          </div>

          <!-- Header -->
          <header class="flex items-center justify-between p-3 bg-gradient-to-r from-[#002244] to-[#001122] border-b border-[#0055ff]/50 relative z-10">
            <h2 class="text-white font-bold tracking-widest text-[#00aaff] drop-shadow flex items-center gap-2">
              <span v-if="isQuestDone" class="text-green-400 text-lg">✔</span>
              <span v-else class="text-blue-400 text-lg">●</span>
              {{ quest.title }}
            </h2>
            <button 
              @click="closeModal"
              class="text-gray-400 hover:text-red-400 transition-colors bg-black/30 w-7 h-7 flex items-center justify-center rounded border border-gray-700 hover:border-red-500"
            >
              <IconClose class="w-5 h-5" />
            </button>
          </header>

          <!-- Content Body -->
          <div class="p-4 flex flex-col gap-4 relative z-10 max-h-[70vh] overflow-y-auto custom-scroll">
            
            <!-- Description Box -->
            <div class="bg-[#000a1a] p-3 rounded border border-blue-900/50 shadow-inner">
              <p class="text-gray-300 text-sm leading-relaxed font-medium">
                {{ quest.description }}
              </p>
            </div>

            <!-- Steps Progress -->
            <div class="flex flex-col gap-2">
              <h3 class="text-xs text-blue-500 font-bold uppercase tracking-wider mb-1 border-b border-blue-900/40 pb-1">
                Mission Steps
              </h3>
              
              <div 
                v-for="step in quest.steps" 
                :key="step.number"
                class="flex items-start gap-3 p-2 rounded transition-colors group"
                :class="step.isCompleted ? 'bg-green-900/10 border border-green-800/30' : 'bg-white/5 border border-white/10'"
              >
                <!-- Checkbox Indicator -->
                <div class="mt-0.5 flex-shrink-0 w-5 h-5 rounded border-2 flex items-center justify-center transition-colors shadow-inner"
                     :class="step.isCompleted ? 'bg-green-500/20 border-green-500 text-green-400 shadow-[0_0_8px_rgba(0,255,0,0.3)]' : 'bg-black/50 border-gray-600'">
                  <span v-if="step.isCompleted" class="text-xs">✔</span>
                </div>
                
                <!-- Description -->
                <p class="text-sm flex-1 leading-snug transition-colors"
                   :class="step.isCompleted ? 'text-gray-400 line-through decoration-green-900' : 'text-gray-200'">
                  {{ step.description }}
                </p>
              </div>
              
              <div v-if="!quest.steps || quest.steps.length === 0" class="text-center p-3 opacity-50">
                 <span class="text-gray-500 text-sm italic">No specific steps tracked.</span>
              </div>
            </div>

          </div>

          <!-- Footer Decorative -->
          <div class="h-1.5 w-full bg-gradient-to-r from-blue-900 via-cyan-500 to-blue-900"></div>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

.animate-slide-up {
  animation: slideUp 0.3s cubic-bezier(0.16, 1, 0.3, 1) forwards;
}

@keyframes slideUp {
  from {
    transform: translateY(20px);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}

.custom-scroll::-webkit-scrollbar {
  width: 6px;
}
.custom-scroll::-webkit-scrollbar-track {
  background: rgba(0, 0, 0, 0.3);
  border-radius: 4px;
}
.custom-scroll::-webkit-scrollbar-thumb {
  background: #0044aa;
  border-radius: 4px;
}
.custom-scroll::-webkit-scrollbar-thumb:hover {
  background: #0077ff;
}
</style>
