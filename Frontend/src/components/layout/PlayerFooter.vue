<script setup lang="ts">
import { computed } from 'vue'
import { useGameStore } from '../../stores/useGameStore'

const store = useGameStore()

const playerName = computed(() => store.gameState?.player?.name ?? 'Connecting...')
const bits = computed(() => store.gameState?.player?.bits ?? 0)
const isConnected = computed(() => store.isConnected)
</script>

<template>
  <footer class="w-full bg-[#8B4513] text-[#FFE4B5] p-3 rounded-md shadow-lg border border-[#5c2e0c] flex items-center gap-12 px-6 relative">
    
    <!-- Connection Status Indicator -->
    <div class="absolute right-4 top-1/2 -translate-y-1/2 flex items-center gap-2 text-sm opacity-80">
      <span class="w-3 h-3 rounded-full" :class="isConnected ? 'bg-green-500' : 'bg-red-500'"></span>
      {{ isConnected ? 'Connected' : 'Disconnected' }}
    </div>

    <div class="font-bold text-lg">
      <span class="opacity-80 text-sm mr-2 font-normal">Tamer:</span>
      {{ playerName }}
    </div>
    
    <div class="font-bold text-lg">
      <span class="opacity-80 text-sm mr-2 font-normal">Bits:</span>
      {{ bits }}
    </div>
  </footer>
</template>
