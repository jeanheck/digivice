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
  <div class="equipments-panel w-full flex flex-col pt-2 text-white text-sm">
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
</template>

<style scoped>
.text-shadow-sm {
  text-shadow: 1px 1px 0 #000;
}

.equipments-panel {
  /* Fundo sutil para separar as seções, se necessário */
  background-color: rgba(0, 10, 43, 0.4);
  border-radius: 4px;
  padding: 8px;
}
</style>
