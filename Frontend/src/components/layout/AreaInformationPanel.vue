<script setup lang="ts">
import { useGameStore } from '../../stores/useGameStore'
import { computed } from 'vue'
import locationsData from '../../data/static/Locations.json'

const store = useGameStore()
const locations = locationsData as Record<string, string>

const currentLocation = computed(() => {
    const loc = store.gameState?.currentLocation
    if (!loc || loc === 'Unknown') return 'Unknown Area'
    
    return locations[loc] ? locations[loc] : `Unknown Zone ${loc}`
})
</script>

<template>
  <aside class="w-full h-full bg-[#000e3f] rounded-md shadow-lg border-2 border-[#0033aa] p-3 flex flex-col pt-0 mb-1 overflow-hidden">
    <!-- Header banner -->
    <div class="w-full flex items-center justify-center border-b border-[#0033aa]/50 bg-[#000e3f] sticky top-0 py-2 z-10">
      <h3 class="font-bold tracking-widest text-[#0077ff] text-shadow-sm uppercase text-sm">Area Information</h3>
    </div>
    
    <div class="flex-1 flex flex-col mt-2 h-full">
        <!-- 100% Space: Image/Text representation -->
        <div class="flex-1 relative w-full rounded border-2 border-[#0044aa]/50 bg-black bg-opacity-60 flex items-center justify-center overflow-hidden shadow-inner">
            
            <!-- Animated grid background -->
            <div class="absolute inset-0 bg-grid-pattern opacity-10 pointer-events-none animate-pan-bg"></div>

            <!-- Scanline overlay -->
            <div class="absolute inset-0 bg-[linear-gradient(transparent_50%,rgba(0,0,0,0.5)_50%)] bg-[length:100%_4px] pointer-events-none opacity-40"></div>
            
            <!-- Text overlay -->
            <div class="relative z-10 px-3 py-2 bg-black/60 border border-[#00aaff]/40 rounded backdrop-blur-sm max-w-[90%] text-center">
                <h4 class="text-sm font-bold text-white tracking-widest uppercase drop-shadow-[0_0_5px_rgba(0,170,255,0.8)] truncate">
                    {{ currentLocation }}
                </h4>
            </div>

            <!-- Cyberpunk brackets -->
            <div class="absolute top-1 left-1 w-3 h-3 border-t-2 border-l-2 border-[#00aaff]/60"></div>
            <div class="absolute bottom-1 right-1 w-3 h-3 border-b-2 border-r-2 border-[#00aaff]/60"></div>
            <div class="absolute top-1 right-1 w-3 h-3 border-t-2 border-r-2 border-[#00aaff]/60"></div>
            <div class="absolute bottom-1 left-1 w-3 h-3 border-b-2 border-l-2 border-[#00aaff]/60"></div>
        </div>

        <!-- Area Enemies -->
        <div class="flex-[2] w-full mt-2 pt-2 border-t border-[#0033aa]/50 flex flex-col justify-center items-center">
             <h4 class="text-[9px] uppercase font-bold tracking-[0.2em] text-[#00aaff] mb-1">Area Enemies</h4>
             <div class="flex flex-wrap items-center justify-center gap-2">
                <!-- Generic placeholders for now -->
                <div v-for="i in 3" :key="i" class="w-[28px] h-[28px] rounded border border-[#0033aa] bg-[#001122] flex items-center justify-center shadow-inner opacity-80 hover:opacity-100 transition-opacity cursor-help" title="Unknown Enemy">
                   <span class="text-sm">💀</span>
                </div>
             </div>
        </div>
    </div>
  </aside>
</template>

<style scoped>
.bg-grid-pattern {
  background-image: 
    linear-gradient(to right, #0055ff 1px, transparent 1px),
    linear-gradient(to bottom, #0055ff 1px, transparent 1px);
  background-size: 15px 15px;
}

.animate-pan-bg {
    animation: panbg 20s linear infinite;
}

@keyframes panbg {
    0% { background-position: 0 0; }
    100% { background-position: 30px 30px; }
}
</style>
