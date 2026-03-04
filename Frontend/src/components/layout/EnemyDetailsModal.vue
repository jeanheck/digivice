<script setup lang="ts">
import Modal from '../ui/Modal.vue'

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
</script>

<template>
  <Modal :isOpen="isOpen" :title="enemy?.Name || 'Enemy Details'" @close="handleClose">
    <div v-if="enemy" class="flex flex-col md:flex-row gap-4 w-full max-w-4xl max-h-[75vh]">
      
      <!-- LEFT COLUMN: Vital Stats & Core Info -->
      <div class="flex-1 flex flex-col gap-3">
        <!-- Vitals Card -->
        <div class="bg-[#001122]/80 border border-blue-900 rounded p-3 shadow-inner">
          <div class="flex justify-between items-center mb-2 pb-1 border-b border-blue-900/50">
            <span class="text-xs font-bold text-gray-400 tracking-wider">LEVEL</span>
            <span class="text-lg font-bold text-amber-400">{{ enemy.Level }}</span>
          </div>
          
          <div class="flex flex-col gap-2">
            <div class="w-full">
              <div class="flexjustify-between text-xs mb-1">
                <span class="text-[#00ffbb] font-bold tracking-widest uppercase">HP</span>
                <span class="text-white">{{ enemy.HP }} / {{ enemy.HP }}</span>
              </div>
              <div class="h-1.5 w-full bg-[#001122] rounded overflow-hidden border border-[#00ffbb]/30">
                <div class="h-full bg-gradient-to-r from-[#008855] to-[#00ffbb] w-full"></div>
              </div>
            </div>
            
            <div class="w-full">
              <div class="flex justify-between text-xs mb-1">
                <span class="text-[#ff00ee] font-bold tracking-widest uppercase">MP</span>
                <span class="text-white">{{ enemy.MP }} / {{ enemy.MP }}</span>
              </div>
              <div class="h-1.5 w-full bg-[#001122] rounded overflow-hidden border border-[#ff00ee]/30">
                <div class="h-full bg-gradient-to-r from-[#880077] to-[#ff00ee] w-full"></div>
              </div>
            </div>
          </div>
        </div>

        <!-- Details Card -->
        <div class="bg-[#001122]/80 border border-blue-900 rounded p-3 shadow-inner text-sm grid grid-cols-2 gap-x-2 gap-y-3">
          <div>
            <span class="block text-[10px] text-blue-400 uppercase tracking-wider mb-0.5">Species</span>
            <span class="text-gray-200 capitalize">{{ enemy.Species || 'Unknown' }}</span>
          </div>
          <div>
            <span class="block text-[10px] text-blue-400 uppercase tracking-wider mb-0.5">Possible Drop</span>
            <span class="text-amber-300">{{ enemy.ItemHeld !== 'N/A' ? enemy.ItemHeld : 'None' }}</span>
          </div>
          <div>
            <span class="block text-[10px] text-blue-400 uppercase tracking-wider mb-0.5">Base EXP</span>
            <span class="text-gray-200">{{ enemy.EXP }}</span>
          </div>
          <div>
            <span class="block text-[10px] text-blue-400 uppercase tracking-wider mb-0.5">Base BITS</span>
            <span class="text-yellow-400">{{ enemy.BITS }}</span>
          </div>
          <div class="col-span-2">
            <span class="block text-[10px] text-blue-400 uppercase tracking-wider mb-0.5">Digivolve EXP (DVXP)</span>
            <span class="text-green-400">{{ enemy.DVXP }}</span>
          </div>
        </div>
        
        <!-- Attacks Card -->
        <div class="bg-[#001122]/80 border border-red-900/50 rounded p-3 shadow-inner text-sm mt-auto">
           <h4 class="text-[10px] uppercase font-bold tracking-[0.2em] text-red-400 mb-2 border-b border-red-900/30 pb-1">Combat Actions</h4>
           
           <div class="mb-2">
             <span class="inline-block text-[10px] text-gray-400 uppercase tracking-wider mr-2">Regular Attack</span>
             <span class="text-white relative pl-2 before:content-[''] before:absolute before:left-0 before:top-1.5 before:w-1 before:h-1 before:bg-red-500 before:rounded-full">{{ enemy.RegularAttack || 'Unknown' }}</span>
           </div>
           
           <div>
             <span class="inline-block text-[10px] text-gray-400 uppercase tracking-wider mr-2">Technique</span>
             <span class="text-white relative pl-2 before:content-[''] before:absolute before:left-0 before:top-1.5 before:w-1 before:h-1 before:bg-orange-500 before:rounded-full">{{ enemy.Technique || 'None' }}</span>
           </div>
        </div>
      </div>

      <!-- MIDDLE COLUMN: Attributes -->
      <div class="flex-1">
        <div class="h-full bg-[#001122]/80 border border-blue-900 rounded p-3 shadow-inner">
          <h4 class="text-[10px] uppercase font-bold tracking-[0.2em] text-[#00aaff] mb-3 border-b border-blue-900/50 pb-1">Attributes</h4>
          
          <div class="flex flex-col gap-2 relative mt-4">
             <!-- Cyberpunk connection line overlay inside attributes -->
             <div class="absolute left-[14px] top-4 bottom-4 w-px bg-gradient-to-b from-[#00aaff] via-[#00aaff]/50 to-transparent pointer-events-none"></div>
             
             <div class="flex items-center group">
                <div class="w-7 h-7 bg-red-900/40 border border-red-500/50 rounded-full flex items-center justify-center text-xs shadow-[0_0_8px_rgba(255,0,0,0.2)] z-10 shrink-0">STR</div>
                <div class="ml-3 flex-1 flex justify-between items-center text-sm bg-[#000e2b] p-1.5 rounded pr-3 border-y border-r border-[#001133] group-hover:border-[#0033aa] transition-colors relative h-8">
                   <div class="absolute -left-3 top-1/2 w-3 border-t border-dashed border-[#00aaff]/40"></div>
                   <span class="text-gray-400 text-xs">Strength</span>
                   <span class="font-mono text-white">{{ enemy.Strength }}</span>
                </div>
             </div>
             
             <div class="flex items-center group">
                <div class="w-7 h-7 bg-blue-900/40 border border-blue-500/50 rounded-full flex items-center justify-center text-xs shadow-[0_0_8px_rgba(0,100,255,0.2)] z-10 shrink-0">DEF</div>
                <div class="ml-3 flex-1 flex justify-between items-center text-sm bg-[#000e2b] p-1.5 rounded pr-3 border-y border-r border-[#001133] group-hover:border-[#0033aa] transition-colors relative h-8">
                   <div class="absolute -left-3 top-1/2 w-3 border-t border-dashed border-[#00aaff]/40"></div>
                   <span class="text-gray-400 text-xs">Defense</span>
                   <span class="font-mono text-white">{{ enemy.Defense }}</span>
                </div>
             </div>
             
             <div class="flex items-center group">
                <div class="w-7 h-7 bg-purple-900/40 border border-purple-500/50 rounded-full flex items-center justify-center text-xs shadow-[0_0_8px_rgba(150,0,255,0.2)] z-10 shrink-0">SPR</div>
                <div class="ml-3 flex-1 flex justify-between items-center text-sm bg-[#000e2b] p-1.5 rounded pr-3 border-y border-r border-[#001133] group-hover:border-[#0033aa] transition-colors relative h-8">
                   <div class="absolute -left-3 top-1/2 w-3 border-t border-dashed border-[#00aaff]/40"></div>
                   <span class="text-gray-400 text-xs">Spirit</span>
                   <span class="font-mono text-white">{{ enemy.Spirit }}</span>
                </div>
             </div>
             
             <div class="flex items-center group">
                <div class="w-7 h-7 bg-yellow-900/40 border border-yellow-500/50 rounded-full flex items-center justify-center text-xs shadow-[0_0_8px_rgba(255,200,0,0.2)] z-10 shrink-0">WIS</div>
                <div class="ml-3 flex-1 flex justify-between items-center text-sm bg-[#000e2b] p-1.5 rounded pr-3 border-y border-r border-[#001133] group-hover:border-[#0033aa] transition-colors relative h-8">
                   <div class="absolute -left-3 top-1/2 w-3 border-t border-dashed border-[#00aaff]/40"></div>
                   <span class="text-gray-400 text-xs">Wisdom</span>
                   <span class="font-mono text-white">{{ enemy.Wisdom }}</span>
                </div>
             </div>
             
             <div class="flex items-center group">
                <div class="w-7 h-7 bg-cyan-900/40 border border-cyan-500/50 rounded-full flex items-center justify-center text-xs shadow-[0_0_8px_rgba(0,255,255,0.2)] z-10 shrink-0">SPD</div>
                <div class="ml-3 flex-1 flex justify-between items-center text-sm bg-[#000e2b] p-1.5 rounded pr-3 border-y border-r border-[#001133] group-hover:border-[#0033aa] transition-colors relative h-8">
                   <div class="absolute -left-3 top-1/2 w-3 border-t border-dashed border-[#00aaff]/40"></div>
                   <span class="text-gray-400 text-xs">Speed</span>
                   <span class="font-mono text-white">{{ enemy.Speed }}</span>
                </div>
             </div>
          </div>
        </div>
      </div>

      <!-- RIGHT COLUMN: Resistances & Status -->
      <div class="flex-1 flex flex-col gap-3">
        <!-- Elements -->
        <div class="bg-[#001122]/80 border border-blue-900 rounded p-3 shadow-inner flex-1">
          <h4 class="text-[10px] uppercase font-bold tracking-[0.2em] text-[#00aaff] mb-2 border-b border-blue-900/50 pb-1">Elem Tol</h4>
          
          <div class="grid grid-cols-2 gap-x-3 gap-y-1.5 mt-2">
             <div class="flex justify-between items-center bg-[#000a20] px-2 py-0.5 rounded text-xs border-l-2 border-red-500">
               <span class="text-gray-400">Fire</span><span class="text-white">{{ enemy.Fire }}</span>
             </div>
             <div class="flex justify-between items-center bg-[#000a20] px-2 py-0.5 rounded text-xs border-l-2 border-blue-400">
               <span class="text-gray-400">Water</span><span class="text-white">{{ enemy.Water }}</span>
             </div>
             <div class="flex justify-between items-center bg-[#000a20] px-2 py-0.5 rounded text-xs border-l-2 border-cyan-300">
               <span class="text-gray-400">Ice</span><span class="text-white">{{ enemy.Ice }}</span>
             </div>
             <div class="flex justify-between items-center bg-[#000a20] px-2 py-0.5 rounded text-xs border-l-2 border-green-400">
               <span class="text-gray-400">Wind</span><span class="text-white">{{ enemy.Wind }}</span>
             </div>
             <div class="flex justify-between items-center bg-[#000a20] px-2 py-0.5 rounded text-xs border-l-2 border-yellow-400">
               <span class="text-gray-400">Thund</span><span class="text-white">{{ enemy.Thunder }}</span>
             </div>
             <div class="flex justify-between items-center bg-[#000a20] px-2 py-0.5 rounded text-xs border-l-2 border-gray-400">
               <span class="text-gray-400">Mach</span><span class="text-white">{{ enemy.Machine }}</span>
             </div>
             <div class="flex justify-between items-center bg-[#000a20] px-2 py-0.5 rounded text-xs border-l-2 border-purple-500 col-span-2 mx-auto w-1/2">
               <span class="text-gray-400">Dark</span><span class="text-white">{{ enemy.Dark }}</span>
             </div>
          </div>
        </div>

        <!-- Status Effects -->
        <div class="bg-[#001122]/80 border border-blue-900 rounded p-3 shadow-inner flex-1">
          <h4 class="text-[10px] uppercase font-bold tracking-[0.2em] text-[#00aaff] mb-2 border-b border-blue-900/50 pb-1">Status Tol</h4>
          
          <div class="grid grid-cols-2 gap-x-3 gap-y-1.5 mt-2">
             <div class="flex justify-between items-center bg-[#000a20] px-2 py-0.5 rounded text-xs">
               <span class="text-gray-400">Poison</span><span class="text-white">{{ enemy.Poison }}</span>
             </div>
             <div class="flex justify-between items-center bg-[#000a20] px-2 py-0.5 rounded text-xs">
               <span class="text-gray-400">Paralze</span><span class="text-white">{{ enemy.Paralyze }}</span>
             </div>
             <div class="flex justify-between items-center bg-[#000a20] px-2 py-0.5 rounded text-xs">
               <span class="text-gray-400">Confuse</span><span class="text-white">{{ enemy.Confuse }}</span>
             </div>
             <div class="flex justify-between items-center bg-[#000a20] px-2 py-0.5 rounded text-xs">
               <span class="text-gray-400">Sleep</span><span class="text-white">{{ enemy.Sleep }}</span>
             </div>
             <div class="flex justify-between items-center bg-[#000a20] px-2 py-0.5 rounded text-xs">
               <span class="text-gray-400">K.O.</span><span class="text-white">{{ enemy['K.O'] ?? (enemy as any)['K.O.'] ?? 0 }}</span>
             </div>
             <div class="flex justify-between items-center bg-[#000a20] px-2 py-0.5 rounded text-xs">
               <span class="text-gray-400">Drain</span>
               <span :class="enemy.Drain === 'Yes' ? 'text-red-400' : 'text-gray-500'">{{ enemy.Drain }}</span>
             </div>
             <div class="flex justify-between items-center bg-[#000a20] px-2 py-0.5 rounded text-xs col-span-2 mx-auto w-1/2">
               <span class="text-gray-400">Steal</span>
               <span :class="enemy.Steal === 'Yes' ? 'text-red-400' : 'text-gray-500'">{{ enemy.Steal }}</span>
             </div>
          </div>
        </div>
      </div>

    </div>
  </Modal>
</template>
