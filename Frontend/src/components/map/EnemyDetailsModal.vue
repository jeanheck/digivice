<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref } from 'vue'
import IconClose from '@/components/modal/IconClose.vue'
import { useLocalization } from '../../composables/useLocalization'

const props = defineProps<{
  isOpen: boolean
  enemyId: string | null
}>()

const emit = defineEmits<{
  (e: 'close'): void
}>()

const { t, getLocalized } = useLocalization()

const handleClose = () => {
  emit('close')
}

const handleKeydown = (e: KeyboardEvent) => {
  if (e.key === 'Escape' && props.isOpen) {
    handleClose()
  }
}

onMounted(() => {
  window.addEventListener('keydown', handleKeydown)
})

onUnmounted(() => {
  window.removeEventListener('keydown', handleKeydown)
})

const attributesList = computed(() => {
  if (!props.enemyId) return []
  return [
    { label: t('attributes.strength'), val: props.enemyId.strength, icon: '👊', color: 'text-[#fcd883]' },
    { label: t('attributes.defense'), val: props.enemyId.defense, icon: '🛡️', color: 'text-gray-400' },
    { label: t('attributes.spirit'), val: props.enemyId.spirit, icon: '🧙‍♂️', color: 'text-pink-300' },
    { label: t('attributes.wisdom'), val: props.enemyId.wisdom, icon: '📖', color: 'text-yellow-600' },
    { label: t('attributes.speed'), val: props.enemyId.speed, icon: '🏃', color: 'text-green-400' },
  ]
})

const elemTolsList = computed(() => {
  if (!props.enemyId) return []
  return [
    { label: t('resistances.fire'), val: props.enemyId.fire, icon: '🔥', color: 'text-orange-500' },
    { label: t('resistances.water'), val: props.enemyId.water, icon: '💧', color: 'text-blue-400' },
    { label: t('resistances.ice'), val: props.enemyId.ice, icon: '🧊', color: 'text-cyan-200' },
    { label: t('resistances.wind'), val: props.enemyId.wind, icon: '🍃', color: 'text-gray-100' },
    { label: t('resistances.thunder'), val: props.enemyId.thunder, icon: '⚡', color: 'text-[#ffffcc]' },
    { label: t('resistances.machine'), val: props.enemyId.machine, icon: '⚙️', color: 'text-gray-500' },
    { label: t('resistances.dark'), val: props.enemyId.dark, icon: '🌑', color: 'text-purple-500' },
  ]
})

const statusTolsList = computed(() => {
  if (!props.enemyId) return []
  return [
    { label: props.enemyId.canPoison ? t('conditions.resistancePoison') : t('conditions.canPoison'), val: props.enemyId.canPoison ? `${props.enemyId.poison}%` : 'No', icon: '☠️', color: 'text-green-500', valColor: props.enemyId.canPoison ? 'text-white' : 'text-red-400' },
    { label: props.enemyId.canParalyze ? t('conditions.resistanceParalyze') : t('conditions.canParalyze'), val: props.enemyId.canParalyze ? `${props.enemyId.paralyze}%` : 'No', icon: '⚡', color: 'text-yellow-300', valColor: props.enemyId.canParalyze ? 'text-white' : 'text-red-400' },
    { label: props.enemyId.canConfuse ? t('conditions.resistanceConfuse') : t('conditions.canConfuse'), val: props.enemyId.canConfuse ? `${props.enemyId.confuse}%` : 'No', icon: '😵', color: 'text-pink-400', valColor: props.enemyId.canConfuse ? 'text-white' : 'text-red-400' },
    { label: props.enemyId.canSleep ? t('conditions.resistanceSleep') : t('conditions.canSleep'), val: props.enemyId.canSleep ? `${props.enemyId.sleep}%` : 'No', icon: '💤', color: 'text-blue-300', valColor: props.enemyId.canSleep ? 'text-white' : 'text-red-400' },
    { label: props.enemyId.canKO ? t('conditions.resistanceKo') : t('conditions.canKo'), val: props.enemyId.canKO ? `${props.enemyId.ko ?? 0}%` : 'No', icon: '💀', color: 'text-gray-500', valColor: props.enemyId.canKO ? 'text-white' : 'text-red-400' },
    { label: t('conditions.drain'), val: props.enemyId.canDrain ? 'Yes' : 'No', icon: '🧛', color: 'text-red-500', valColor: props.enemyId.canDrain ? 'text-green-400' : 'text-red-400' },
    { label: t('conditions.steal'), val: props.enemyId.canSteal ? 'Yes' : 'No', icon: '🦝', color: 'text-amber-500', valColor: props.enemyId.canSteal ? 'text-green-400' : 'text-red-400' },
    { label: t('conditions.escape'), val: props.enemyId.canEscape ? 'Yes' : 'No', icon: '🏃', color: 'text-gray-400', valColor: props.enemyId.canEscape ? 'text-green-400' : 'text-red-400' },
  ]
})

// Tooltip logic
const activeTooltip = ref({ show: false, label: '', val: '', x: 0, y: 0 })

const showTooltip = (event: MouseEvent, label: string) => {
  let posX = event.clientX + 15
  if (posX + 150 > window.innerWidth) posX = event.clientX - 160
  
  activeTooltip.value = { show: true, label, val: '', x: posX, y: event.clientY + 15 }
}

const hideTooltip = () => {
  activeTooltip.value.show = false
}

const moveTooltip = (event: MouseEvent) => {
  if (!activeTooltip.value.show) return
  let posX = event.clientX + 15
  activeTooltip.value.x = posX
  activeTooltip.value.y = event.clientY + 15
}

// Dynamic image loading
const enemyImages = import.meta.glob('../../assets/icons/enemies/*.png', { eager: true, as: 'url' })

const enemyImageUrl = computed(() => {
  if (!props.enemyId?.name) return null
  const name = props.enemyId.name
  // Try with exact name
  const path = `../../assets/icons/enemies/${name}.png`
  return (enemyImages[path] as string) || null
})
</script>

<template>
  <Teleport to="body">
    <Transition name="fade">
      <div 
        v-if="isOpen && enemyId" 
        class="fixed inset-0 z-[100] flex items-center justify-center p-4 bg-black/60 backdrop-blur-sm"
        @click.self="handleClose"
      >
        <div class="relative w-full max-w-[1000px] bg-[#001122] border-2 border-[#0055ff] shadow-[0_0_20px_rgba(0,119,255,0.4)] rounded-lg flex flex-col overflow-hidden animate-slide-up">
          
          <!-- Cyberpunk Hexagon Pattern Background -->
          <div class="absolute inset-0 opacity-[0.03] pointer-events-none" 
               style="background-image: url('data:image/svg+xml,%3Csvg width=\'60\' height=\'60\' viewBox=\'0 0 60 60\' xmlns=\'http://www.w3.org/2000/svg\'%3E%3Cpath d=\'M30 0l25.98 15v30L30 60 4.02 45V15z\' stroke=\'%230077ff\' stroke-width=\'1\' fill=\'none\'/%3E%3C/svg%3E');">
          </div>

          <!-- Header -->
          <header class="flex items-center justify-between p-3 bg-gradient-to-r from-[#002244] to-[#001122] border-b border-[#0055ff]/50 relative z-10">
            <h2 class="text-white font-bold tracking-widest text-[#00aaff] drop-shadow flex items-center gap-2 uppercase">
              {{ enemyId?.name || $t('enemy.detailsTitle') }}
            </h2>
            <button 
              @click="handleClose"
              class="text-gray-400 hover:text-red-400 transition-colors bg-black/30 w-7 h-7 flex items-center justify-center rounded border border-gray-700 hover:border-red-500"
            >
              <IconClose class="w-5 h-5" />
            </button>
          </header>

          <!-- Content Body -->
          <div class="p-4 flex flex-col sm:flex-row gap-4 relative z-10 max-h-[70vh] overflow-y-auto custom-scroll">
            
            <!-- LEFT COLUMN: Vital Stats & Core Info -->
            <div class="flex flex-col gap-4 flex-1">
              <!-- Top Section: Image & Vitals -->
              <div class="flex gap-4">
                <!-- Enemy Image Container (Left 50%) -->
                <div class="w-1/2 aspect-square bg-[#000a1a] border border-blue-900/50 rounded flex items-center justify-center shadow-inner relative group overflow-hidden">
                  <div class="absolute inset-0 opacity-10 bg-gradient-to-br from-blue-500/20 to-transparent pointer-events-none"></div>
                  <img v-if="enemyImageUrl" 
                       :src="enemyImageUrl" 
                       class="w-full h-full object-cover drop-shadow-[0_0_15px_rgba(0,170,255,0.4)] group-hover:scale-110 transition-transform duration-500" 
                       :alt="enemyId.name" />
                  <div v-else class="text-4xl opacity-30 select-none">❓</div>
                </div>

                <!-- Vitals Stats (Right 50%) -->
                <div class="w-1/2 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-col justify-between gap-1.5 py-4">
                  <div class="flex items-center justify-between text-[11px]">
                    <span class="font-bold text-blue-500 tracking-wider uppercase">{{ $t('enemy.specie') }}:</span>
                    <span class="font-bold text-gray-300 capitalize">{{ $t(`species.${enemyId.species}`) }}</span>
                  </div>

                  <div class="flex items-center justify-between text-[11px]">
                    <span class="font-bold text-blue-500 tracking-wider uppercase">{{ $t('enemy.level') }}:</span>
                    <span class="font-bold text-gray-300">{{ enemyId.level }}</span>
                  </div>
                  
                  <div class="flex items-center justify-between text-[11px]">
                    <span class="font-bold text-blue-500 tracking-wider uppercase">HP:</span>
                    <span class="font-bold text-white">{{ enemyId.hp }}</span>
                  </div>
                  
                  <div class="flex items-center justify-between text-[11px]">
                    <span class="font-bold text-blue-500 tracking-wider uppercase">MP:</span>
                    <span class="font-bold text-white">{{ enemyId.mp }}</span>
                  </div>

                  <div class="flex items-center justify-between text-[11px]">
                    <span class="font-bold text-blue-500 tracking-wider uppercase">{{ $t('enemy.baseExp') }}:</span>
                    <span class="font-bold text-gray-300">{{ enemyId.exp }}</span>
                  </div>

                  <div class="flex items-center justify-between text-[11px]">
                    <span class="font-bold text-blue-500 tracking-wider uppercase">DVEXP:</span>
                    <span class="font-bold text-gray-300">{{ enemyId.dvxp }}</span>
                  </div>

                  <div class="flex items-center justify-between text-[11px]">
                    <span class="font-bold text-blue-500 tracking-wider uppercase">{{ $t('enemy.baseBits') }}:</span>
                    <span class="font-bold text-gray-300">{{ enemyId.bits }}</span>
                  </div>

                  <div class="flex flex-col text-[11px] mt-1">
                    <span class="font-bold text-blue-500 tracking-wider uppercase mb-1">{{ $t('enemy.possibleDrop') }}:</span>
                    <span class="font-bold text-gray-300 text-left" :title="enemyId.itemHeld && getLocalized(enemyId.itemHeld) !== 'N/A' ? getLocalized(enemyId.itemHeld) : t('drop.none')">
                      {{ enemyId.itemHeld && getLocalized(enemyId.itemHeld) !== 'N/A' ? getLocalized(enemyId.itemHeld) : t('drop.none') }}
                    </span>
                  </div>
                </div>
              </div>
              
              <!-- Attacks Card (Updated Columns) -->
              <div class="bg-[#000a1a] border border-red-900/30 rounded p-4 shadow-inner text-sm mt-auto">
                 <h4 class="text-[10px] uppercase font-bold tracking-[0.2em] text-red-500 mb-4 border-b border-red-900/30 pb-1">{{ $t('enemy.combatActions') }}</h4>
                 
                 <div class="flex gap-6">
                   <div class="flex-1 flex flex-col gap-1.5">
                     <span class="text-[9px] text-gray-500 uppercase font-bold tracking-wider">{{ $t('enemy.regularAttack') }}</span>
                     <span class="text-gray-200 text-xs">
                       {{ enemyId.regularAttack ? getLocalized(enemyId.regularAttack) : 'Unknown' }}
                     </span>
                   </div>
                   
                   <div class="flex-1 flex flex-col gap-1.5">
                     <span class="text-[9px] text-gray-500 uppercase font-bold tracking-wider">{{ $t('enemy.technique') }}</span>
                     <span class="text-gray-200 text-xs">
                       {{ enemyId.technique ? getLocalized(enemyId.technique) : 'None' }}
                     </span>
                   </div>
                 </div>
              </div>
            </div>

            <!-- RIGHT COLUMN: Attributes & Tols -->
            <div class="flex-1">
               <div class="bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-row justify-around gap-6 h-full items-start">
                 
                 <!-- Attr List -->
                 <div class="flex-1 flex flex-col items-center gap-1.5">
                   <h4 class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-2 border-b border-blue-900/30 pb-1 w-full text-center">{{ $t('enemy.attr') }}</h4>
                   <div v-for="attr in attributesList" :key="attr.label" class="flex items-center justify-center gap-2 w-full">
                       <div class="flex items-center w-[20px] justify-center cursor-help select-none z-20"
                            @mouseenter="e => showTooltip(e, attr.label)" @mousemove="moveTooltip" @mouseleave="hideTooltip">
                         <span class="text-[16px] font-emoji opacity-90 drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-[2.5px]" :class="attr.color">{{ attr.icon }}</span>
                       </div>
                       <div class="font-bold tracking-widest text-shadow text-white text-sm w-12 text-center">
                         {{ attr.val }}
                       </div>
                   </div>
                 </div>

                 <!-- Elem Tol List -->
                 <div class="flex-1 flex flex-col items-center gap-1.5 border-l border-blue-900/10 pl-3">
                   <h4 class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-2 border-b border-blue-900/30 pb-1 w-full text-center">{{ $t('enemy.elem') }}</h4>
                   <div v-for="res in elemTolsList" :key="res.label" class="flex items-center justify-center gap-2 w-full">
                       <div class="flex items-center w-[20px] justify-center cursor-help select-none z-20"
                            @mouseenter="e => showTooltip(e, res.label)" @mousemove="moveTooltip" @mouseleave="hideTooltip">
                         <span class="text-[16px] font-emoji opacity-90 drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-[2.5px]" :class="res.color">{{ res.icon }}</span>
                       </div>
                       <div class="font-bold tracking-widest text-shadow text-white text-sm w-12 text-center">
                         {{ res.val }}
                       </div>
                   </div>
                 </div>

                 <!-- Status Tol List -->
                 <div class="flex-1 flex flex-col items-center gap-1.5 border-l border-blue-900/10 pl-3">
                   <h4 class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-2 border-b border-blue-900/30 pb-1 w-full text-center">{{ $t('enemy.status') }}</h4>
                   <div v-for="st in statusTolsList" :key="st.label" class="flex items-center justify-center gap-2 w-full">
                       <div class="flex items-center w-[20px] justify-center cursor-help select-none z-20"
                            @mouseenter="e => showTooltip(e, st.label)" @mousemove="moveTooltip" @mouseleave="hideTooltip">
                         <span class="text-[16px] font-emoji opacity-90 drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-[2.5px]" :class="st.color">{{ st.icon }}</span>
                       </div>
                       <div class="font-bold tracking-widest text-shadow text-sm w-12 text-center" :class="st.valColor">
                         {{ st.val }}
                       </div>
                   </div>
                 </div>

               </div>
            </div>

          </div>

          <!-- Footer Decorative -->
          <div class="h-1.5 w-full bg-gradient-to-r from-[#0033aa] via-cyan-500 to-[#0033aa] relative z-10"></div>
        </div>
      </div>
    </Transition>

    <!-- Tooltip Teleport -->
    <Teleport to="body">
      <Transition name="fade">
        <div v-if="activeTooltip.show"
             class="fixed z-[9999] pointer-events-none p-2 bg-[#001133ee] border-[2px] border-[#0066cc] rounded-sm shadow-[0_4px_12px_rgba(0,0,0,0.8)] flex flex-col backdrop-blur-sm"
             :style="{ top: `${activeTooltip.y}px`, left: `${activeTooltip.x}px` }">
           <div class="font-bold text-yellow-300 text-xs shadow-black text-shadow-sm uppercase tracking-wider">
              {{ activeTooltip.label }}
           </div>
        </div>
      </Transition>
    </Teleport>
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
