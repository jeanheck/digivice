<script setup lang="ts">
import { ref } from 'vue'

// Importando alguns ícones genéricos do pacote @iconify-json/pixelarticons para os equipamentos
import IconHead from '~icons/pixelarticons/user'
import IconBody from '~icons/pixelarticons/shield'
import IconWeapon from '~icons/pixelarticons/zap' // Dagger
import IconOffhand from '~icons/pixelarticons/add-box' // Buckler
import IconAcc1 from '~icons/pixelarticons/heart' // Power Ring
import IconAcc2 from '~icons/pixelarticons/wind' // Speed Ring

// Mock de slots de equipamento focados na nomenclatura do DW3
const equipments = ref([
  { slot: 'Head', item: 'Ribbon', icon: IconHead, color: 'text-pink-400' },
  { slot: 'Body', item: 'Leather Coat', icon: IconBody, color: 'text-orange-700' },
  { slot: 'Right', item: 'Dagger', icon: IconWeapon, color: 'text-gray-400' },
  { slot: 'Left', item: 'Buckler', icon: IconOffhand, color: 'text-orange-900' },
  { slot: 'Accessory', item: 'Power Ring', icon: IconAcc1, color: 'text-red-500' },
  { slot: 'Accessory', item: 'Speed Ring', icon: IconAcc2, color: 'text-cyan-300' }
])
</script>

<template>
  <div class="relative overflow-hidden flex flex-col w-full bg-[#000a2b]">
    <!-- Borda externa brilhante simuluada via clip-path background -->
    <div class="absolute inset-0 bg-[#0077ff] pointer-events-none evo-border"></div>
    
    <!-- Fundo interno escuro (1 pixel menor que a borda) -->
    <div class="absolute inset-[1.5px] bg-[#000a2b] pointer-events-none evo-inner"></div>

    <div class="relative z-10 equipments-panel w-full flex flex-col pt-2 p-3 text-white text-sm shadow-inner">
      <div 
        v-for="(equip, index) in equipments" 
        :key="index"
        class="equip-row flex justify-between items-center py-1 border-b-[1px] border-[#0033aa]/50 last:border-0"
      >
        <!-- Slot Label (Esquerda) -->
        <span class="font-bold tracking-widest text-[#0077ff] text-shadow-sm min-w-[90px]">
          {{ equip.slot }}
        </span>

        <!-- Container do Nome do Item + Ícone (Direita) -->
        <div class="flex items-center justify-end gap-2 flex-1 truncate">
          <!-- Nome do Item (Agora na esquerda do container direito) -->
          <span class="text-gray-300 text-shadow-sm truncate font-medium">
            {{ equip.item }}
          </span>
          
          <!-- Ícone do Equipamento (Agora na direita do nome) -->
          <component :is="equip.icon" class="w-4 h-4 flex-shrink-0" :class="equip.color" />
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.evo-border, .evo-inner {
  clip-path: polygon(4px 0, 100% 0, 100% calc(100% - 4px), calc(100% - 4px) 100%, 0 100%, 0 4px);
}

.text-shadow-sm {
  text-shadow: 1px 1px 0 #000;
}

.equipments-panel {
  /* Fundo sutil para separar as seções, se necessário */
  background-color: rgba(0, 10, 43, 0.4);
}
</style>
