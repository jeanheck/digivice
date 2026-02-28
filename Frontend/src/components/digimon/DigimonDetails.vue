<script setup lang="ts">
import { computed, ref } from 'vue'
import DigimonDetailsTable from '../../data/static/DigimonDetailsTable.json'

// Importando os ícones via unplugin-icons do pacote @iconify-json/pixelarticons
import IconStrengh from '../icons/IconHand.vue'
import IconDefense from '../icons/IconShield.vue'
import IconSpirit from '../icons/IconFaceId.vue'
import IconSpeed from '../icons/IconRunFast.vue'
import IconCharisma from '../icons/IconSparkles.vue'
import IconWisdom from '../icons/IconBookOpen.vue'
import IconFireResistance from '../icons/IconFire.vue'
import IconWaterResistance from '../icons/IconDrop.vue'
import IconIceResistance from '../icons/IconSnowflakeCold.vue'
import IconWindResistance from '../icons/IconWind.vue'
import IconThunderResistance from '../icons/IconLightning.vue'
import IconMachineResistance from '../icons/IconCog.vue'
import IconDarkResistance from '../icons/IconMoon.vue'

import type { Digimon } from '../../types/backend'

const props = defineProps<{
  digimon: Digimon
}>()

// Mapeamento dinâmico baseado no Digimon World 3 (Stat -> Value)
const attributes = computed(() => {
  const attrs = props.digimon.attributes
  return [
    { label: 'Strength', value: attrs.strength, icon: IconStrengh, color: 'text-[#fcd883]' },
    { label: 'Defense', value: attrs.defense, icon: IconDefense, color: 'text-gray-400' },
    { label: 'Spirit', value: attrs.spirit, icon: IconSpirit, color: 'text-pink-300' },
    { label: 'Wisdom', value: attrs.wisdom, icon: IconWisdom, color: 'text-yellow-600' },
    { label: 'Speed', value: attrs.speed, icon: IconSpeed, color: 'text-green-400' },
    { label: 'Charisma', value: attrs.charisma, icon: IconCharisma, color: 'text-yellow-300' },
  ]
})

const resistances = computed(() => {
  const res = props.digimon.resistances
  return [
    { label: 'Fire', value: res.fire, icon: IconFireResistance, color: 'text-orange-500' },
    { label: 'Water', value: res.water, icon: IconWaterResistance, color: 'text-blue-400' },
    { label: 'Ice', value: res.ice, icon: IconIceResistance, color: 'text-cyan-200' },
    { label: 'Wind', value: res.wind, icon: IconWindResistance, color: 'text-gray-100' },
    { label: 'Thunder', value: res.thunder, icon: IconThunderResistance, color: 'text-[#ffffcc]' },
    { label: 'Machine', value: res.machine, icon: IconMachineResistance, color: 'text-gray-500' },
    { label: 'Dark', value: res.dark, icon: IconDarkResistance, color: 'text-purple-500' },
  ]
})

// Tooltip Logic
const activeTooltip = ref({ show: false, title: '', text: '', x: 0, y: 0 })

const showTooltip = (event: MouseEvent, title: string, dictionaryType: 'attributes' | 'resistances') => {
  const text = DigimonDetailsTable[dictionaryType][title as keyof typeof DigimonDetailsTable[typeof dictionaryType]]
  if (!text) return
  
  // Predict horizontal overflow
  const tooltipWidth = 250 // safe max-width prediction
  let posX = event.clientX + 15
  if (posX + tooltipWidth > window.innerWidth) {
    posX = event.clientX - tooltipWidth - 10
  }
  
  activeTooltip.value = {
    show: true,
    title,
    text,
    x: posX,
    y: event.clientY + 15
  }
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
  activeTooltip.value.y = event.clientY + 15
}
</script>

<template>
  <div class="relative overflow-hidden flex flex-col w-full bg-[#000a2b]">
    <!-- Borda externa brilhante simuluada via clip-path background -->
    <div class="absolute inset-0 bg-[#0077ff] pointer-events-none evo-border"></div>
    
    <!-- Fundo interno escuro (1 pixel menor que a borda) -->
    <div class="absolute inset-[1.5px] bg-[#000a2b] pointer-events-none evo-inner"></div>

    <div class="relative z-10 details-panel flex justify-center w-full p-4 text-white text-sm">
      <div class="flex gap-12">
        <!-- Coluna 1: Atributos Base -->
        <div class="flex flex-col gap-1 w-24">
        <div 
          v-for="attr in attributes" 
          :key="attr.label"
          class="flex items-center gap-2 cursor-help"
          @mouseenter="e => showTooltip(e, attr.label, 'attributes')"
          @mousemove="moveTooltip"
          @mouseleave="hideTooltip"
        >
          <!-- Ícone Box com Borda -->
          <div class="icon-box w-[28px] h-[28px] bg-[#000a2b] border-[#0033aa] flex items-center justify-center aspect-square">
            <component :is="attr.icon" class="w-5 h-5" :class="attr.color" />
          </div>
          
          <!-- Valor Numérico -->
          <div class="font-bold tracking-widest text-shadow">
            {{ attr.value }}
          </div>
        </div>
      </div>

        <!-- Coluna 2: Resistências Elementais -->
        <div class="flex flex-col gap-1 w-24">
          <div 
          v-for="res in resistances" 
          :key="res.label"
          class="flex items-center gap-2 cursor-help"
          @mouseenter="e => showTooltip(e, res.label, 'resistances')"
          @mousemove="moveTooltip"
          @mouseleave="hideTooltip"
        >
          <!-- Ícone Box com Borda -->
          <div class="icon-box w-[28px] h-[28px] bg-[#000a2b] border-[#0033aa] flex items-center justify-center aspect-square">
            <component :is="res.icon" class="w-5 h-5" :class="res.color" />
          </div>
          
          <!-- Valor Numérico -->
          <div class="font-bold tracking-widest text-shadow">
            {{ res.value }}
          </div>
        </div>
        </div>
      </div>
    </div>

    <!-- Teleported Tooltip (Breaks out of modal/panel clipping) -->
    <Teleport to="body">
      <Transition name="fade">
        <div 
          v-if="activeTooltip.show"
          class="fixed z-[9999] pointer-events-none p-3 max-w-[250px] bg-[#001133ee] border-[2px] border-[#0066cc] rounded-sm shadow-[0_4px_12px_rgba(0,0,0,0.8)] flex flex-col gap-1 backdrop-blur-sm"
          :style="{ top: `${activeTooltip.y}px`, left: `${activeTooltip.x}px` }"
        >
          <div class="font-bold text-yellow-300 text-sm border-b border-[#0066cc]/50 pb-1 mb-1 shadow-black text-shadow-sm uppercase tracking-wider">
            {{ activeTooltip.title }}
          </div>
          <div class="text-gray-100 text-xs leading-relaxed shadow-black text-shadow-sm">
            {{ activeTooltip.text }}
          </div>
        </div>
      </Transition>
    </Teleport>
  </div>
</template>

<style scoped>
.evo-border, .evo-inner {
  clip-path: polygon(4px 0, 100% 0, 100% calc(100% - 4px), calc(100% - 4px) 100%, 0 100%, 0 4px);
}

.icon-box {
  /* Bordas em inset/outset para simular as caixinhas azuis do DW3 */
  border-width: 1px;
  border-style: solid;
  border-color: #0044cc #001155 #001155 #0044cc; 
}

.text-shadow {
  text-shadow: 1px 1px 0 #000;
}

/* Tooltip transition */
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.15s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
