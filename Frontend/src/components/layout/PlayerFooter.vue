<script setup lang="ts">
import { computed } from 'vue'
import { useGameStore } from '../../stores/useGameStore'
import { useI18n } from 'vue-i18n'
import LanguageSelector from '../ui/LanguageSelector.vue'

const store = useGameStore()
const { t } = useI18n()

const playerName = computed(() => store.gameState?.player?.name ?? t('common.connecting'))
const bits = computed(() => store.gameState?.player?.bits ?? 0)
const isConnected = computed(() => store.isConnected)
</script>

<template>
  <footer class="w-full bg-[#000a2b] text-white p-3 rounded-md shadow-lg border-2 border-[#0033aa] border-t-orange-500 flex items-center gap-12 px-6 relative">
    
    <!-- Language Selector & Connection Status -->
    <div class="absolute right-4 top-1/2 -translate-y-1/2 flex items-center gap-4 text-sm opacity-80">
      <div class="flex items-center gap-2 border-r border-blue-900 pr-4 mr-2">
        <LanguageSelector />
      </div>
      
      <div class="flex items-center gap-2">
        <span class="w-3 h-3 rounded-full" :class="isConnected ? 'bg-green-500' : 'bg-red-500'"></span>
        {{ isConnected ? $t('common.connected') : $t('common.disconnected') }}
      </div>
    </div>

    <div class="font-bold text-lg">
      <span class="opacity-80 text-[0.7rem] mr-2 font-normal text-blue-300 tracking-wider uppercase">{{ $t('player.tamer') }}:</span>
      <span class="text-yellow-400 drop-shadow">{{ playerName }}</span>
    </div>
    
    <div class="font-bold text-lg flex items-baseline">
      <span class="opacity-80 text-[0.7rem] mr-2 font-normal text-blue-300 tracking-wider uppercase">{{ $t('player.bits') }}:</span>
      <span class="text-white">{{ bits }}</span>
    </div>
  </footer>
</template>
