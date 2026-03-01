<script setup lang="ts">
import { computed } from 'vue'

// Importando alguns ícones genéricos do pacote @iconify-json/pixelarticons para os equipamentos
import IconHead from '../icons/IconHelmet.vue'
import IconBody from '../icons/IconArmor.vue'
import IconWeapon from '../icons/IconHandRight.vue' // Dagger
import IconOffhand from '../icons/IconHandLeft.vue' // Buckler
import IconAcc1 from '../icons/IconDiamond.vue' // Power Ring
import IconAcc2 from '../icons/IconDiamond.vue' // Speed Ring

import type { Digimon } from '../../types/backend'
import equipmentData from '../../data/static/Equipments.json'

const props = defineProps<{
  digimon: Digimon
}>()

const equipMap = new Map(
  equipmentData.equipments.map(e => [e.id, e.name])
)

function getItemName(id: number) {
  return equipMap.get(id) ?? `Item ID: ${id}`
}

const equipments = computed(() => {
  const eq = props.digimon.equipments
  return [
    { slot: 'Head', item: getItemName(eq?.head || 0), icon: IconHead, color: 'text-gray-400' },
    { slot: 'Body', item: getItemName(eq?.body || 0), icon: IconBody, color: 'text-gray-400' },
    { slot: 'Right', item: getItemName(eq?.rightHand || 0), icon: IconWeapon, color: 'text-gray-400' },
    { slot: 'Left', item: getItemName(eq?.leftHand || 0), icon: IconOffhand, color: 'text-gray-400' },
    { slot: 'Accessory 1', item: getItemName(eq?.accessory1 || 0), icon: IconAcc1, color: 'text-cyan-300' },
    { slot: 'Accessory 2', item: getItemName(eq?.accessory2 || 0), icon: IconAcc2, color: 'text-cyan-300' }
  ]
})
</script>

<template>
  <div class="relative overflow-hidden flex flex-col w-full bg-[#000a2b]">
    <!-- Borda externa brilhante simuluada via clip-path background -->
    <div class="absolute inset-0 bg-[#0077ff] pointer-events-none evo-border"></div>
    
    <!-- Fundo interno escuro (1 pixel menor que a borda) -->
    <div class="absolute inset-[1.5px] bg-[#000a2b] pointer-events-none evo-inner"></div>

    <div class="relative z-10 equipments-panel w-full flex flex-col pt-2 p-3 text-white text-xs shadow-inner">
      <div 
        v-for="(equip, index) in equipments" 
        :key="index"
        class="equip-row flex justify-between items-center py-1 border-b-[1px] border-[#0033aa] last:border-0"
      >
        <!-- Slot Label (Esquerda) -->
        <span class="font-bold tracking-widest text-[#0077ff] text-shadow-sm min-w-[75px]">
          {{ equip.slot }}
        </span>

        <!-- Container do Nome do Item + Ícone (Direita) -->
        <div class="flex items-center justify-end gap-1 flex-1 truncate">
          <!-- Nome do Item (Agora na esquerda do container direito) -->
          <span class="text-gray-300 text-shadow-sm truncate font-medium text-[11px]">
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
