<script setup lang="ts">
import { computed, ref } from 'vue'
import IconClose from '../icons/IconClose.vue'

const props = defineProps<{
  isOpen: boolean
  enemy: any | null
}>()

const emit = defineEmits<{
  (e: 'close'): void
}>()

const handleClose = () => {
  emit('close')
}

const attributesList = computed(() => {
  if (!props.enemy) return []
  return [
    { label: 'Strength', val: props.enemy.Strength, icon: '👊', color: 'text-[#fcd883]' },
    { label: 'Defense', val: props.enemy.Defense, icon: '🛡️', color: 'text-gray-400' },
    { label: 'Spirit', val: props.enemy.Spirit, icon: '🧙‍♂️', color: 'text-pink-300' },
    { label: 'Wisdom', val: props.enemy.Wisdom, icon: '📖', color: 'text-yellow-600' },
    { label: 'Speed', val: props.enemy.Speed, icon: '🏃', color: 'text-green-400' },
  ]
})

const elemTolsList = computed(() => {
  if (!props.enemy) return []
  return [
    { label: 'Fire', val: props.enemy.Fire, icon: '🔥', color: 'text-orange-500' },
    { label: 'Water', val: props.enemy.Water, icon: '💧', color: 'text-blue-400' },
    { label: 'Ice', val: props.enemy.Ice, icon: '🧊', color: 'text-cyan-200' },
    { label: 'Wind', val: props.enemy.Wind, icon: '🍃', color: 'text-gray-100' },
    { label: 'Thunder', val: props.enemy.Thunder, icon: '⚡', color: 'text-[#ffffcc]' },
    { label: 'Machine', val: props.enemy.Machine, icon: '⚙️', color: 'text-gray-500' },
    { label: 'Dark', val: props.enemy.Dark, icon: '🌑', color: 'text-purple-500' },
  ]
})

const statusTolsList = computed(() => {
  if (!props.enemy) return []
  return [
    { label: 'Poison', val: props.enemy.Poison, icon: '☠️', color: 'text-green-500' },
    { label: 'Paralyze', val: props.enemy.Paralyze, icon: '⚡', color: 'text-yellow-300' },
    { label: 'Confuse', val: props.enemy.Confuse, icon: '😵', color: 'text-pink-400' },
    { label: 'Sleep', val: props.enemy.Sleep, icon: '💤', color: 'text-blue-300' },
    { label: 'K.O.', val: props.enemy['K.O'] ?? (props.enemy as any)['K.O.'] ?? 0, icon: '💀', color: 'text-gray-500' },
    { label: 'Drain', val: props.enemy.Drain, icon: '🧛', color: 'text-red-500' },
    { label: 'Steal', val: props.enemy.Steal, icon: '🦝', color: 'text-amber-500' },
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
</script>

<template>
  <Teleport to="body">
    <Transition name="fade">
      <div 
        v-if="isOpen && enemy" 
        class="fixed inset-0 z-[100] flex items-center justify-center p-4 bg-black/60 backdrop-blur-sm"
        @click.self="handleClose"
      >
        <div class="relative w-full max-w-[500px] bg-[#001122] border-2 border-[#0055ff] shadow-[0_0_20px_rgba(0,119,255,0.4)] rounded-lg flex flex-col overflow-hidden animate-slide-up">
          
          <!-- Cyberpunk Hexagon Pattern Background -->
          <div class="absolute inset-0 opacity-[0.03] pointer-events-none" 
               style="background-image: url('data:image/svg+xml,%3Csvg width=\'60\' height=\'60\' viewBox=\'0 0 60 60\' xmlns=\'http://www.w3.org/2000/svg\'%3E%3Cpath d=\'M30 0l25.98 15v30L30 60 4.02 45V15z\' stroke=\'%230077ff\' stroke-width=\'1\' fill=\'none\'/%3E%3C/svg%3E');">
          </div>

          <!-- Header -->
          <header class="flex items-center justify-between p-3 bg-gradient-to-r from-[#002244] to-[#001122] border-b border-[#0055ff]/50 relative z-10">
            <h2 class="text-white font-bold tracking-widest text-[#00aaff] drop-shadow flex items-center gap-2 uppercase">
              {{ enemy?.Name || 'Enemy Details' }}
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
            <div class="flex flex-col gap-3 flex-1">
              <!-- Vitals Card -->
              <div class="bg-[#000a1a] border border-blue-900/50 rounded p-3 shadow-inner">
                <div class="flex justify-between items-center mb-2 pb-1 border-b border-blue-900/30">
                  <span class="text-[10px] font-bold text-gray-400 tracking-wider">LEVEL</span>
                  <span class="text-sm font-bold text-amber-400">{{ enemy.Level }}</span>
                </div>
                
                <div class="flex flex-col gap-2">
                  <div class="w-full">
                    <div class="flex justify-between text-[10px] mb-1">
                      <span class="text-[#22cc22] font-bold tracking-widest uppercase">HP</span>
                      <span class="text-gray-300">{{ enemy.HP }} / {{ enemy.HP }}</span>
                    </div>
                    <div class="h-1 w-full bg-[#001122] rounded overflow-hidden border border-[#22cc22]/30">
                      <div class="h-full bg-gradient-to-r from-[#005500] to-[#22cc22] w-full"></div>
                    </div>
                  </div>
                  
                  <div class="w-full">
                    <div class="flex justify-between text-[10px] mb-1">
                      <span class="text-[#cc33cc] font-bold tracking-widest uppercase">MP</span>
                      <span class="text-gray-300">{{ enemy.MP }} / {{ enemy.MP }}</span>
                    </div>
                    <div class="h-1 w-full bg-[#001122] rounded overflow-hidden border border-[#cc33cc]/30">
                      <div class="h-full bg-gradient-to-r from-[#550055] to-[#cc33cc] w-full"></div>
                    </div>
                  </div>
                </div>
              </div>

              <!-- Details Card -->
              <div class="bg-[#000a1a] border border-blue-900/50 rounded p-3 shadow-inner text-sm grid grid-cols-2 gap-x-2 gap-y-3">
                <div>
                  <span class="block text-[9px] text-blue-500 uppercase tracking-wider mb-0.5">Species</span>
                  <span class="text-gray-300 text-xs capitalize">{{ enemy.Species || 'Unknown' }}</span>
                </div>
                <div>
                  <span class="block text-[9px] text-blue-500 uppercase tracking-wider mb-0.5">Possible Drop</span>
                  <span class="text-amber-300 text-xs truncate max-w-[100px] block" :title="enemy.ItemHeld !== 'N/A' ? enemy.ItemHeld : 'None'">{{ enemy.ItemHeld !== 'N/A' ? enemy.ItemHeld : 'None' }}</span>
                </div>
                <div>
                  <span class="block text-[9px] text-blue-500 uppercase tracking-wider mb-0.5">Base EXP</span>
                  <span class="text-gray-300 text-xs">{{ enemy.EXP }}</span>
                </div>
                <div>
                  <span class="block text-[9px] text-blue-500 uppercase tracking-wider mb-0.5">Base BITS</span>
                  <span class="text-yellow-400 text-xs">{{ enemy.BITS }}</span>
                </div>
                <div class="col-span-2">
                  <span class="block text-[9px] text-blue-500 uppercase tracking-wider mb-0.5">Digivolve EXP (DVXP)</span>
                  <span class="text-green-400 text-xs">{{ enemy.DVXP }}</span>
                </div>
              </div>
              
              <!-- Attacks Card -->
              <div class="bg-[#000a1a] border border-red-900/30 rounded p-3 shadow-inner text-sm mt-auto">
                 <h4 class="text-[9px] uppercase font-bold tracking-[0.2em] text-red-500 mb-2 border-b border-red-900/30 pb-1">Combat Actions</h4>
                 
                 <div class="mb-3 flex flex-col gap-0.5">
                   <span class="text-[9px] text-gray-500 uppercase tracking-wider">Regular Attack</span>
                   <span class="text-gray-200 text-xs relative pl-2 before:content-[''] before:absolute before:left-0 before:top-1.5 before:w-1 before:h-1 before:bg-red-500 before:rounded-full">{{ enemy.RegularAttack || 'Unknown' }}</span>
                 </div>
                 
                 <div class="flex flex-col gap-0.5">
                   <span class="text-[9px] text-gray-500 uppercase tracking-wider">Technique</span>
                   <span class="text-gray-200 text-xs relative pl-2 before:content-[''] before:absolute before:left-0 before:top-1.5 before:w-1 before:h-1 before:bg-orange-500 before:rounded-full">{{ enemy.Technique || 'None' }}</span>
                 </div>
              </div>
            </div>

            <!-- RIGHT COLUMN: Attributes & Tols -->
            <div class="flex-1">
               <div class="bg-[#000a1a] border border-blue-900/50 rounded p-3 shadow-inner flex flex-row gap-4 h-full">
                 
                 <!-- Attr List -->
                 <div class="flex-[0.8] flex flex-col gap-1.5">
                   <h4 class="text-[9px] uppercase font-bold tracking-widest text-blue-500 mb-2 border-b border-blue-900/30 pb-1">Attr</h4>
                   <div v-for="attr in attributesList" :key="attr.label" class="flex items-center gap-1">
                       <div class="flex items-center w-[16px] justify-center cursor-help select-none z-20"
                            @mouseenter="e => showTooltip(e, attr.label)" @mousemove="moveTooltip" @mouseleave="hideTooltip">
                         <span class="text-[14px] font-emoji opacity-90 drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-0.5" :class="attr.color">{{ attr.icon }}</span>
                       </div>
                       <div class="font-bold tracking-widest text-shadow text-white text-xs">
                         {{ attr.val }}
                       </div>
                   </div>
                 </div>

                 <!-- Elem Tol List -->
                 <div class="flex-1 flex flex-col gap-1.5 border-l border-blue-900/30 pl-3">
                   <h4 class="text-[9px] uppercase font-bold tracking-widest text-blue-500 mb-2 border-b border-blue-900/30 pb-1">Elem</h4>
                   <div v-for="res in elemTolsList" :key="res.label" class="flex items-center justify-between gap-1 w-full max-w-[60px]">
                       <div class="flex items-center w-[16px] justify-center cursor-help select-none z-20"
                            @mouseenter="e => showTooltip(e, res.label)" @mousemove="moveTooltip" @mouseleave="hideTooltip">
                         <span class="text-[14px] font-emoji opacity-90 drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-0.5" :class="res.color">{{ res.icon }}</span>
                       </div>
                       <div class="font-bold tracking-widest text-shadow text-white text-xs">
                         {{ res.val }}
                       </div>
                   </div>
                 </div>

                 <!-- Status Tol List -->
                 <div class="flex-1 flex flex-col gap-1.5 border-l border-blue-900/30 pl-3">
                   <h4 class="text-[9px] uppercase font-bold tracking-widest text-blue-500 mb-2 border-b border-blue-900/30 pb-1">Status</h4>
                   <div v-for="st in statusTolsList" :key="st.label" class="flex items-center justify-between gap-1 w-full max-w-[60px]">
                       <div class="flex items-center w-[16px] justify-center cursor-help select-none z-20"
                            @mouseenter="e => showTooltip(e, st.label)" @mousemove="moveTooltip" @mouseleave="hideTooltip">
                         <span class="text-[14px] font-emoji opacity-90 drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-0.5" :class="st.color">{{ st.icon }}</span>
                       </div>
                       <div class="font-bold tracking-widest text-shadow text-xs" :class="(st.val === 'Yes') ? 'text-red-400' : 'text-white'">
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
