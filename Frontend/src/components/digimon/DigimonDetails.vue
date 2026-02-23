<script setup lang="ts">
import { ref } from 'vue'

// Importando os ícones via unplugin-icons do pacote @iconify-json/pixelarticons
import IconUser from '~icons/pixelarticons/user'
import IconShield from '~icons/pixelarticons/shield'
import IconHeart from '~icons/pixelarticons/heart'
import IconZap from '~icons/pixelarticons/zap'
import IconDrop from '~icons/pixelarticons/drop'
import IconSun from '~icons/pixelarticons/sun'
import IconMoon from '~icons/pixelarticons/moon'
import IconWind from '~icons/pixelarticons/wind'
import IconBug from '~icons/pixelarticons/bug'

// Mocks baseados no Digimon World 3 (Stat -> Value)
const attributes = ref([
  { label: 'Strength', value: 51, icon: IconUser, color: 'text-orange-400' },
  { label: 'Defense', value: 28, icon: IconShield, color: 'text-gray-400' },
  { label: 'Spirit', value: 26, icon: IconHeart, color: 'text-pink-400' },
  { label: 'Wisdom', value: 43, icon: IconBug, color: 'text-yellow-600' },
  { label: 'Speed', value: 52, icon: IconZap, color: 'text-green-400' },
  { label: 'Charisma', value: 1, icon: IconSun, color: 'text-yellow-400' },
])

const resistances = ref([
  { label: 'Fire', value: 115, icon: IconSun, color: 'text-red-500' },
  { label: 'Water', value: 80, icon: IconDrop, color: 'text-blue-400' },
  { label: 'Ice', value: 100, icon: IconDrop, color: 'text-cyan-200' },
  { label: 'Wind', value: 130, icon: IconWind, color: 'text-gray-100' },
  { label: 'Thunder', value: 60, icon: IconZap, color: 'text-yellow-400' },
  { label: 'Machine', value: 115, icon: IconShield, color: 'text-gray-500' },
  { label: 'Dark', value: 80, icon: IconMoon, color: 'text-purple-500' },
])
</script>

<template>
  <div class="relative overflow-hidden flex flex-col w-full bg-[#000a2b]">
    <!-- Borda externa brilhante simuluada via clip-path background -->
    <div class="absolute inset-0 bg-[#0077ff] pointer-events-none evo-border"></div>
    
    <!-- Fundo interno escuro (1 pixel menor que a borda) -->
    <div class="absolute inset-[1.5px] bg-[#000a2b] pointer-events-none evo-inner"></div>

    <div class="relative z-10 details-panel flex gap-4 w-full p-4 text-white text-sm">
      
      <!-- Coluna 1: Atributos Base -->
      <div class="flex-1 flex flex-col gap-1">
        <div 
          v-for="attr in attributes" 
          :key="attr.label"
          class="flex items-center gap-2"
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
      <div class="flex-1 flex flex-col gap-1">
        <div 
          v-for="res in resistances" 
          :key="res.label"
          class="flex items-center gap-2"
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
</style>
