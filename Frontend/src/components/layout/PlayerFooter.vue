<script setup lang="ts">
import { computed, ref } from 'vue'
import { useGameStore } from '../../stores/useGameStore'
import { useI18n } from 'vue-i18n'
import { invoke } from '@tauri-apps/api/core'
import LanguageSelector from '../ui/LanguageSelector.vue'
import EquipmentsData from '../../data/static/Equipments.json'
import type { Digimon } from '../../models'

const store = useGameStore()
const { t } = useI18n()

const getCharismaEquipBonus = (digimon: Digimon | null) => {
    if (!digimon || !digimon.equipments) return 0;
    let total = 0;
    
    const equipIds = Object.values(digimon.equipments).filter(id => id > 0);
    
    const items = equipIds.map(id => EquipmentsData.equipments.find(e => e.Id === id)).filter(i => i) as typeof EquipmentsData.equipments;
    
    const uniqueItems = items.filter((item, index, self) => {
      if (item.Type === "WeaponTwoHanded") {
        return self.findIndex(i => i.Id === item.Id) === index;
      }
      return true;
    });

    uniqueItems.forEach(item => {
        if (item.Attributes) {
            item.Attributes.forEach(attr => {
                if (attr.Attribute?.toLowerCase() === 'charisma') {
                    if (attr.Type === "Addition") total += attr.Value;
                    else if (attr.Type === "Subtraction") total -= attr.Value;
                }
            });
        }
    });
    return total;
}

const playerName = computed(() => store.gameState?.player?.name ?? t('common.connecting'))
const bits = computed(() => store.gameState?.player?.bits ?? 0)
const groupCharisma = computed(() => {
  if (!store.gameState?.party?.slots) return 0
  return store.gameState.party.slots.reduce((sum, digimon) => {
    const baseCharisma = digimon?.attributes?.charisma ?? 0;
    const equipBonus = getCharismaEquipBonus(digimon);
    return sum + baseCharisma + equipBonus;
  }, 0)
})
const isConnected = computed(() => store.isConnected)

const activeTooltip = ref({
  show: false,
  x: 0,
  y: 0,
  title: '',
  text: ''
})

const openLogsFolder = async () => {
  try {
    await invoke('open_logs_folder')
  } catch (err) {
    console.error('Failed to open logs folder:', err)
  }
}

const showTooltip = (event: MouseEvent, title: string, text: string) => {
  activeTooltip.value = {
    show: true,
    title,
    text,
    x: 0,
    y: 0
  }
  moveTooltip(event)
}

const hideTooltip = () => {
  activeTooltip.value.show = false
}

const moveTooltip = (event: MouseEvent) => {
  if (!activeTooltip.value.show) return
  
  const tooltipWidth = 250
  let posX = event.clientX + 15
  if (posX + tooltipWidth > window.innerWidth) {
    posX = event.clientX - tooltipWidth - 10
  }
  
  activeTooltip.value.x = posX
  activeTooltip.value.y = event.clientY - 15
}
</script>

<template>
  <footer class="w-full bg-[#000a2b] text-white p-3 rounded-md shadow-lg border-2 border-[#0033aa] border-t-orange-500 flex items-center gap-12 px-6 relative">
    
    <!-- Language Selector & Connection Status -->
    <div class="absolute right-4 top-1/2 -translate-y-1/2 flex items-center gap-4 text-sm opacity-80">
      <div class="flex items-center gap-3 border-r border-blue-900 pr-4 mr-2">
        <button @click="openLogsFolder" class="px-2 py-1 text-xs bg-blue-900/50 hover:bg-blue-800 rounded border border-blue-700 transition-colors text-blue-200 hover:text-white uppercase tracking-wider">
          Logs
        </button>
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
    
    <div class="font-bold text-lg flex items-baseline cursor-help"
         @mouseenter="e => showTooltip(e, $t('player.groupCharisma'), $t('player.groupCharismaWarning'))"
         @mousemove="moveTooltip"
         @mouseleave="hideTooltip">
      <span class="opacity-80 text-[0.7rem] mr-2 font-normal text-blue-300 tracking-wider uppercase">{{ $t('player.groupCharisma') }}:</span>
      <span class="text-white">{{ groupCharisma }}</span>
    </div>
  </footer>

  <!-- Teleported Tooltip -->
  <Teleport to="body">
    <Transition name="fade">
      <div 
        v-if="activeTooltip.show"
        class="fixed z-[9999] pointer-events-none p-3 max-w-[250px] bg-[#001133ee] border-[2px] border-[#0066cc] rounded-sm shadow-[0_4px_12px_rgba(0,0,0,0.8)] flex flex-col gap-1 backdrop-blur-sm"
        :style="{ top: `${activeTooltip.y}px`, left: `${activeTooltip.x}px`, transform: 'translateY(-100%)' }"
      >
        <div>
           <div class="font-bold text-yellow-300 text-sm border-b border-[#0066cc]/50 pb-1 mb-1 shadow-black text-shadow-sm uppercase tracking-wider">
              {{ activeTooltip.title }}
           </div>
           <div class="text-gray-100 text-xs leading-relaxed shadow-black text-shadow-sm">
              {{ activeTooltip.text }}
           </div>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>

<style scoped>
.text-shadow-sm {
  text-shadow: 1px 1px 0 #000;
}
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.15s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
