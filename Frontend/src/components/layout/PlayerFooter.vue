<script setup lang="ts">
import { computed } from 'vue'
import { useGameStore } from '../../stores/useGameStore'

const store = useGameStore()

const playerName = computed(() => store.gameState?.player?.name ?? 'Connecting...')
const bits = computed(() => store.gameState?.player?.bits ?? 0)
const isConnected = computed(() => store.isConnected)
</script>

<template>
  <footer class="w-full bg-[#000a2b] text-white p-3 rounded-md shadow-lg border-2 border-[#0033aa] border-t-orange-500 flex items-center gap-12 px-6 relative">
    
    <!-- Connection Status Indicator -->
    <div class="absolute right-4 top-1/2 -translate-y-1/2 flex items-center gap-2 text-sm opacity-80">
      <span class="w-3 h-3 rounded-full" :class="isConnected ? 'bg-green-500' : 'bg-red-500'"></span>
      {{ isConnected ? 'Connected' : 'Disconnected' }}
    </div>

    <div class="font-bold text-lg">
      <span class="opacity-80 text-[0.7rem] mr-2 font-normal text-blue-300 tracking-wider uppercase">Tamer:</span>
      <span class="text-yellow-400 drop-shadow">{{ playerName }}</span>
    </div>
    
    <div class="font-bold text-lg flex items-baseline">
      <span class="opacity-80 text-[0.7rem] mr-2 font-normal text-blue-300 tracking-wider uppercase">Bits:</span>
      <span class="text-white">{{ bits }}</span>
    </div>
  </footer>
</template>
