<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref, watch } from 'vue'
import { useLocalization } from '../../composables/useLocalization'
import IconClose from '@/components/modal/IconClose.vue';
import asukaMapUrl from '../../assets/AsukaMap.webp'
import type { QuestViewModel } from '@/viewmodels/quest/quest.viewmodel';
import type { StepViewModel } from '@/viewmodels/quest/step.viewmodel';
import { ImageCatalog } from '@/catalogs/image.catalog.ts';
import { QuestDetailsModalPresenter } from '@/presenters/quest-details-modal.presenter';

const props = defineProps<{
  questViewModel: QuestViewModel | null
  isOpen: boolean
}>()

const { t } = useLocalization()

const emit = defineEmits(['close'])

const closeModal = () => {
  emit('close')
}

const handleKeydown = (e: KeyboardEvent) => {
  if (e.key === 'Escape' && props.isOpen) {
    closeModal()
  }
}

onMounted(() => {
  window.addEventListener('keydown', handleKeydown)
})

onUnmounted(() => {
  window.removeEventListener('keydown', handleKeydown)
})

const isQuestDone = computed(() => {
  if (!props.questViewModel || !props.questViewModel.steps || props.questViewModel.steps.length === 0) return false;
  return props.questViewModel.steps.every((s: any) => s.isDone);
})

const isQuestLocked = computed(() => {
  if (!props.questViewModel || !props.questViewModel.requisites || props.questViewModel.requisites.length === 0) return false;
  return !props.questViewModel.requisites.every((p: any) => p.isDone);
})

const hasRequisites = computed(() => {
  return props.questViewModel?.requisites && props.questViewModel.requisites.length > 0;
})

// Geographic Integration
const selectedStep = ref<StepViewModel | null>(null)
const currentLocationIndex = ref(0)

const selectStep = (step: StepViewModel) => {
  selectedStep.value = step
  currentLocationIndex.value = 0
}

const currentLocation = computed(() => {
  if (!selectedStep.value?.zoomedLocations || selectedStep.value.zoomedLocations.length === 0) return null;
  return selectedStep.value.zoomedLocations[currentLocationIndex.value];
})

// Reset selection when modal opens with a different quest
watch(() => props.questViewModel, () => {
  selectedStep.value = null
  currentLocationIndex.value = 0
})

const getLocalMapUrl = (locationId?: string) => {
  if (!locationId) {
    return null;
  }
  const locationViewModel = QuestDetailsModalPresenter.getLocationById(locationId);
  return ImageCatalog.getMapImageUrl(locationViewModel.image);
};

function formatTooltipText(text: string) {
  if (!text) return ''
  const words = text.split(' ')
  const lines: string[] = []
  let currentLine = ''

  for (const word of words) {
    if (currentLine === '') {
      currentLine = word
    } else {
      // Logic: if current line already has more than 10 chars, next word starts a new line
      if (currentLine.length > 10) {
        lines.push(currentLine)
        currentLine = word
      } else {
        currentLine += ' ' + word
      }
    }
  }
  if (currentLine) lines.push(currentLine)
  return lines.join('<br/>')
}
</script>

<template>
  <Teleport to="body">
    <Transition name="fade">
      <div 
        v-if="isOpen && questViewModel" 
        class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60 backdrop-blur-sm"
        @click.self="closeModal"
      >
        <div class="relative w-full max-w-5xl bg-[#001122] border-2 border-[#0055ff] shadow-[0_0_20px_rgba(0,119,255,0.4)] rounded-lg flex flex-col overflow-hidden animate-slide-up">
          
          <!-- Cyberpunk Hexagon Pattern Background -->
          <div class="absolute inset-0 opacity-[0.03] pointer-events-none" 
               style="background-image: url('data:image/svg+xml,%3Csvg width=\'60\' height=\'60\' viewBox=\'0 0 60 60\' xmlns=\'http://www.w3.org/2000/svg\'%3E%3Cpath d=\'M30 0l25.98 15v30L30 60 4.02 45V15z\' stroke=\'%230077ff\' stroke-width=\'1\' fill=\'none\'/%3E%3C/svg%3E');">
          </div>

          <!-- Header -->
          <header class="flex items-center justify-between p-3 bg-linear-to-r from-[#002244] to-[#001122] border-b border-[#0055ff]/50 relative z-10">
            <h2 class="text-white font-bold tracking-widest drop-shadow flex items-center gap-2">
              <span v-if="isQuestDone" class="text-green-400 text-lg">✔</span>
              <span v-else-if="isQuestLocked" class="text-lg">🔒</span>
              {{ t(`${questViewModel?.id}.title`) }}
            </h2>
            <button 
              @click="closeModal"
              class="text-gray-400 hover:text-red-400 transition-colors bg-black/30 w-7 h-7 flex items-center justify-center rounded border border-gray-700 hover:border-red-500"
            >
              <IconClose class="w-5 h-5" />
            </button>
          </header>

          <!-- Content Body -->
          <div class="flex flex-col lg:flex-row p-4 gap-6 relative z-10 min-h-125 max-h-[80vh] overflow-hidden">
            
            <!-- Left Info Panel -->
            <div class="flex-1 flex flex-col gap-4 overflow-y-auto custom-scroll pr-2">
              
              <!-- Description Box -->
            <div class="bg-[#000a1a] p-3 rounded border border-blue-900/50 shadow-inner">
              <p class="text-gray-300 text-sm leading-relaxed font-medium">
                {{ t(`${questViewModel?.id}.description`) }}
              </p>
            </div>

            <!-- Requisites Section -->
            <div v-if="hasRequisites" class="flex flex-col gap-2">
              <h3 class="text-xs text-amber-500 font-bold uppercase tracking-wider mb-1 border-b border-amber-900/40 pb-1">
                {{ $t('journal.prerequisites') }}
              </h3>
              <div
                v-for="(requisiteViewModel, idx) in questViewModel.requisites"
                :key="idx"
                class="flex items-start gap-3 p-2 rounded transition-colors"
                :class="requisiteViewModel.isDone ? 'bg-green-900/10 border border-green-800/30' : 'bg-red-900/10 border border-red-800/30'"
              >
                <div class="mt-0.5 shrink-0 w-5 h-5 rounded border-2 flex items-center justify-center transition-colors shadow-inner"
                     :class="requisiteViewModel.isDone ? 'bg-green-500/20 border-green-500 text-green-400 shadow-[0_0_8px_rgba(0,255,0,0.3)]' : 'bg-red-500/10 border-red-500/60 text-red-400'">
                  <span v-if="requisiteViewModel.isDone" class="text-xs">✔</span>
                  <span v-else class="text-xs">✘</span>
                </div>
                <p class="text-sm flex-1 leading-snug transition-colors"
                   :class="requisiteViewModel.isDone ? 'text-gray-400 line-through decoration-green-900' : 'text-red-300'">
                  {{ t(`${questViewModel.id}.requisites.${requisiteViewModel.id}`) }}
                </p>
              </div>
            </div>

            <!-- Steps Progress -->
            <div class="flex flex-col gap-2">
              <h3 class="text-xs text-blue-500 font-bold uppercase tracking-wider mb-1 border-b border-blue-900/40 pb-1">
                {{ $t('journal.missionSteps') }}
              </h3>
              
              <div 
                v-for="stepViewModel in questViewModel.steps" 
                :key="stepViewModel.number"
                @click="selectStep(stepViewModel)"
                class="flex items-start gap-3 p-2 rounded transition-all cursor-pointer group"
                :class="[
                  stepViewModel.isDone ? 'bg-green-900/10 border border-green-800/30' : 'bg-white/5 border border-white/10',
                  selectedStep?.number === stepViewModel.number ? 'ring-1 ring-cyan-500 shadow-[0_0_10px_rgba(0,255,255,0.2)] bg-[#001a33]' : 'hover:bg-active-hover hover:border-blue-500/30'
                ]"
              >
                <!-- Checkbox Indicator -->
                <div class="mt-0.5 shrink-0 w-5 h-5 rounded border-2 flex items-center justify-center transition-colors shadow-inner"
                     :class="stepViewModel.isDone ? 'bg-green-500/20 border-green-500 text-green-400 shadow-[0_0_8px_rgba(0,255,0,0.3)]' : 'bg-black/50 border-gray-600'">
                  <span v-if="stepViewModel.isDone" class="text-xs">✔</span>
                </div>
                
                <!-- Description -->
                <div class="flex-1">
                  <p class="text-sm leading-snug transition-colors"
                     :class="stepViewModel.isDone ? 'text-gray-400 line-through decoration-green-900' : 'text-gray-200'">
                    {{ t(`${questViewModel.id}.steps.${stepViewModel.number}.description`) }}
                  </p>
                  <!-- Step Requisites (items checklist) -->
                  <div v-if="stepViewModel.requisites && stepViewModel.requisites.length > 0" class="mt-1.5 ml-1 flex flex-col gap-1">
                    <div
                      v-for="(requisiteViewModel, pidx) in stepViewModel.requisites"
                      :key="pidx"
                      class="flex items-center gap-2 text-xs"
                    >
                      <span :class="requisiteViewModel.isDone ? 'text-green-400' : 'text-gray-500'">{{ requisiteViewModel.isDone ? '✔' : '○' }}</span>
                      <span :class="requisiteViewModel.isDone ? 'text-gray-400 line-through' : 'text-gray-400'">{{ t(`${questViewModel.id}.steps.${stepViewModel.number}.requisites.${requisiteViewModel.id}`) }}</span>
                    </div>
                  </div>
                </div>
              </div>
              
              <div v-if="!questViewModel.steps || questViewModel.steps.length === 0" class="text-center p-3 opacity-50">
                 <span class="text-gray-500 text-sm italic">{{ $t('journal.noSteps') }}</span>
              </div>
            </div>
            </div>

            <!-- Right Geographic Intel Panel -->
            <div class="w-full lg:w-112.5 shrink-0 flex flex-col gap-4 lg:border-l lg:border-[#0055ff]/30 lg:pl-6 overflow-y-auto custom-scroll">
              
              <div v-if="!selectedStep" class="flex-1 flex flex-col items-center justify-center border border-cyan-900/40 bg-[#000a1a] rounded min-h-100">
                  <span class="text-cyan-500/50 font-cyber text-sm tracking-widest text-center px-8 animate-pulse" v-html="$t('journal.clickStep').replace('\n', '<br/>')"></span>
              </div>
              
              <div v-else-if="!selectedStep.location && (!selectedStep.zoomedLocations || selectedStep.zoomedLocations.length === 0)" class="flex-1 flex flex-col items-center justify-center border border-red-900/40 bg-[#1a0000] rounded min-h-100">
                  <span class="text-red-500/50 font-cyber text-sm tracking-widest text-center px-8" v-html="$t('journal.noSignal').replace('\n', '<br/>')"></span>
              </div>

              <template v-else>
                  <!-- World Map Intel -->
                  <div v-if="selectedStep.location" class="relative w-full aspect-4/3 bg-[#00051a] border border-cyan-800/50 rounded overflow-hidden shadow-[0_0_15px_rgba(0,170,255,0.1)] group shrink-0">
                       <img :src="asukaMapUrl" class="w-full h-full object-cover opacity-60 mix-blend-screen saturate-50 group-hover:saturate-100 transition-all duration-500" />
                       <div class="absolute inset-0 bg-blue-900/10 z-0 pointer-events-none"></div>
                       
                        <!-- Map Radar Ping -->
                        <div v-if="selectedStep.coordinates" 
                             class="absolute w-8 h-8 -translate-x-1/2 -translate-y-1/2 z-10 flex items-center justify-center pointer-events-none"
                             :style="{ left: selectedStep.coordinates.x + '%', top: selectedStep.coordinates.y + '%' }">
                             <div class="absolute inset-0 rounded-full border border-cyan-400 animate-ping opacity-90"></div>
                             <div class="w-2.5 h-2.5 rounded-full bg-cyan-400 shadow-[0_0_10px_rgba(0,255,255,1)]"></div>
                             <div class="absolute left-1/2 -translate-x-1/2 text-[10px] font-cyber text-cyan-100 drop-shadow bg-cyan-950/95 px-3 py-1 rounded border border-cyan-700/80 max-w-37.5 leading-tight text-center z-20 shadow-[0_0_10px_rgba(0,0,0,0.5)]"
                                  :class="selectedStep.coordinates.y < 20 ? 'top-6.5' : 'bottom-6.5'"
                                  v-html="formatTooltipText(t(`location.${selectedStep.location}`))">
                             </div>
                        </div>
                  </div>

                  <!-- Local Map Intel Carousel -->
                  <div v-if="currentLocation" class="relative w-full aspect-4/3 bg-[#00051a] border border-cyan-800/50 rounded overflow-hidden shadow-[0_0_15px_rgba(0,170,255,0.1)] group flex flex-col shrink-0">
                       <div class="relative flex-1 w-full bg-black/50 overflow-hidden">
                           <img :src="getLocalMapUrl(currentLocation.location)" class="absolute inset-0 w-full h-full object-cover opacity-70 group-hover:opacity-100 transition-opacity duration-500" />
                           
                            <!-- Local Radar Ping -->
                            <div v-if="currentLocation.coordinates" 
                                 class="absolute w-6 h-6 -translate-x-1/2 -translate-y-1/2 z-10 flex items-center justify-center pointer-events-none transition-all duration-300"
                                 :style="{ left: currentLocation.coordinates.x + '%', top: currentLocation.coordinates.y + '%' }">
                                 <div class="absolute inset-0 rounded-full border border-cyan-400 animate-ping opacity-90"></div>
                                 <div class="w-2 h-2 rounded-full bg-cyan-400 shadow-[0_0_8px_rgba(0,255,255,1)]"></div>
                                 <div class="absolute left-1/2 -translate-x-1/2 text-[9px] font-cyber text-cyan-100 drop-shadow bg-cyan-950/95 px-2 py-0.5 rounded border border-cyan-700/80 max-w-30 leading-tight text-center z-20 shadow-[0_0_10px_rgba(0,0,0,0.5)]"
                                      :class="currentLocation.coordinates.y < 25 ? 'top-6.25' : 'bottom-6.25'"
                                      v-html="formatTooltipText(t(`${questViewModel.id}.steps.${selectedStep.number}.locations.${currentLocationIndex}.locationTarget`))">
                                 </div>
                            </div>
                       </div>
                       
                       <!-- Carousel Controls Overlay -->
                       <div v-if="selectedStep.zoomedLocations && selectedStep.zoomedLocations.length > 1" class="absolute bottom-3 left-0 right-0 flex items-center justify-center gap-3 z-20">
                            <button @click.prevent="currentLocationIndex = (currentLocationIndex - 1 + selectedStep.zoomedLocations.length) % selectedStep.zoomedLocations.length" class="w-7 h-7 rounded bg-black/80 border border-cyan-800 flex items-center justify-center text-cyan-400 hover:bg-cyan-900/80 hover:border-cyan-400 hover:text-white transition-all font-bold text-sm shadow-[0_0_10px_rgba(0,170,255,0.2)]">&lt;</button>
                            
                            <div class="flex gap-2 px-3 py-1.5 bg-black/80 rounded border border-cyan-900/80 shadow-[0_0_10px_rgba(0,170,255,0.2)]">
                                <div v-for="(_, idx) in selectedStep.zoomedLocations" :key="idx" 
                                     class="w-2 h-2 rounded-full transition-all cursor-pointer" 
                                     :class="Number(idx) === currentLocationIndex ? 'bg-cyan-400 shadow-[0_0_8px_rgba(0,255,255,1)] scale-110' : 'bg-cyan-900 hover:bg-cyan-600'"
                                     @click.prevent="currentLocationIndex = Number(idx)"></div>
                            </div>
                            
                            <button @click.prevent="currentLocationIndex = (currentLocationIndex + 1) % selectedStep.zoomedLocations.length" class="w-7 h-7 rounded bg-black/80 border border-cyan-800 flex items-center justify-center text-cyan-400 hover:bg-cyan-900/80 hover:border-cyan-400 hover:text-white transition-all font-bold text-sm shadow-[0_0_10px_rgba(0,170,255,0.2)]">&gt;</button>
                       </div>
                  </div>
              </template>
            </div>

          </div>

          <!-- Footer Decorative -->
          <div class="h-1.5 w-full bg-linear-to-r from-blue-900 via-cyan-500 to-blue-900"></div>
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
