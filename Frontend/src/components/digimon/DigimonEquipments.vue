<script setup lang="ts">
import { computed, ref } from 'vue'

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

function getItem(id: number) {
  return equipmentData.equipments.find(e => e.Id === id) || { Name: `Item ID: ${id}`, Id: id, Type: 'Unknown', Attributes: [], EquipableDigimon: [] };
}

const equipments = computed(() => {
  const eq = props.digimon.equipments
  return [
    { slot: 'Head', item: getItem(eq?.head || 0), icon: IconHead, color: 'text-gray-400' },
    { slot: 'Body', item: getItem(eq?.body || 0), icon: IconBody, color: 'text-gray-400' },
    { slot: 'Right', item: getItem(eq?.rightHand || 0), icon: IconWeapon, color: 'text-gray-400' },
    { slot: 'Left', item: getItem(eq?.leftHand || 0), icon: IconOffhand, color: 'text-gray-400' },
    { slot: 'Accessory 1', item: getItem(eq?.accessory1 || 0), icon: IconAcc1, color: 'text-cyan-300' },
    { slot: 'Accessory 2', item: getItem(eq?.accessory2 || 0), icon: IconAcc2, color: 'text-cyan-300' }
  ]
})

const activeTooltip = ref({ show: false, item: null as any, x: 0, y: 0 })

const showTooltip = (event: MouseEvent, equipObj: any) => {
  if (!equipObj || equipObj.Id === 0 || equipObj.Id === null) return
  
  let posX = event.clientX + 15
  if (posX + 250 > window.innerWidth) posX = event.clientX - 260
  
  activeTooltip.value = { show: true, item: equipObj, x: posX, y: event.clientY + 15 }
}

const hideTooltip = () => {
  activeTooltip.value.show = false
}

const moveTooltip = (event: MouseEvent) => {
  if (!activeTooltip.value.show) return
  
  let posX = event.clientX + 15
  if (posX + 250 > window.innerWidth) posX = event.clientX - 260
  
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

    <div class="relative z-10 equipments-panel w-full flex flex-col pt-2 p-3 text-white text-xs shadow-inner">
      <div 
        v-for="(equip, index) in equipments" 
        :key="index"
        :class="['equip-row flex justify-between items-center py-1 border-b-[1px] border-[#0033aa] last:border-0', 
                 (equip.item.Id && equip.item.Id !== 0) ? 'cursor-help' : '']"
        @mouseenter="e => showTooltip(e, equip.item)"
        @mousemove="moveTooltip"
        @mouseleave="hideTooltip"
      >
        <!-- Slot Label (Esquerda) -->
        <span class="font-bold tracking-widest text-[#0077ff] text-shadow-sm min-w-[75px]">
          {{ equip.slot }}
        </span>

        <!-- Container do Nome do Item + Ícone (Direita) -->
        <div class="flex items-center justify-end gap-1 flex-1 truncate">
          <!-- Nome do Item (Agora na esquerda do container direito) -->
          <span class="text-gray-300 text-shadow-sm truncate font-medium text-[11px]">
            {{ equip.item.Name }}
          </span>
          
          <!-- Ícone do Equipamento (Agora na direita do nome) -->
          <component :is="equip.icon" class="w-4 h-4 flex-shrink-0" :class="equip.color" />
        </div>
      </div>
    </div>

    <!-- Teleported Tooltip (Breaks out of modal/panel clipping) -->
    <Teleport to="body">
      <Transition name="fade">
        <div 
          v-if="activeTooltip.show && activeTooltip.item"
          class="fixed z-[9999] pointer-events-none p-3 max-w-[250px] bg-[#001133ee] border-[2px] border-[#0066cc] rounded-sm shadow-[0_4px_12px_rgba(0,0,0,0.8)] flex flex-col gap-1 backdrop-blur-sm"
          :style="{ top: `${activeTooltip.y}px`, left: `${activeTooltip.x}px` }"
        >
          <!-- Equipments Tooltip -->
          <div class="flex flex-col gap-1 w-full min-w-[170px]">
             
             <!-- Título (Nome do Item) -->
             <div class="font-bold text-yellow-300 text-sm border-b border-[#0066cc]/50 pb-1 mb-1 shadow-black text-shadow-sm uppercase tracking-wider text-center">
                {{ activeTooltip.item.Name }}
             </div>
             
             <!-- Tipo (Weapon, Head, Body, etc) -->
             <div v-if="activeTooltip.item.TypeDescription && activeTooltip.item.TypeDescription !== 'Unknown'" class="text-blue-300 text-[10px] tracking-widest uppercase mb-1 text-center font-bold">
                {{ activeTooltip.item.TypeDescription }}
             </div>

             <!-- Atributos -->
             <div v-if="activeTooltip.item.Attributes && activeTooltip.item.Attributes.length > 0" class="flex flex-col gap-[2px] mb-1">
                 <div v-for="attr in activeTooltip.item.Attributes" :key="attr.Attribute" class="flex justify-between text-xs items-center bg-[#002266]/40 px-1 rounded-sm">
                    <span class="text-gray-200">{{ attr.Attribute }}</span>
                    <span :class="attr.Type === 'Addition' ? 'text-green-400' : 'text-red-400'" class="font-bold tracking-wider">
                      {{ attr.Type === 'Addition' ? '+' : '-' }}{{ attr.Value }}
                    </span>
                 </div>
             </div>

             <!-- Equipable Digimons -->
             <div class="mt-1 pt-1 border-t border-[#0033aa]/50 flex flex-col gap-1">
                <span class="text-gray-400 text-[9px] uppercase tracking-widest leading-none">Equipable by</span>
                <span class="text-gray-200 text-[11px] leading-tight">
                  <template v-if="!activeTooltip.item.EquipableDigimon || activeTooltip.item.EquipableDigimon.length === 0">
                    None
                  </template>
                  <template v-else-if="activeTooltip.item.EquipableDigimon.length === 8">
                    <span class="text-purple-300 font-bold uppercase tracking-wider text-[10px]">All Digimon</span>
                  </template>
                  <template v-else>
                    {{ activeTooltip.item.EquipableDigimon.join(', ') }}
                  </template>
                </span>
             </div>

             <!-- Notes -->
             <div v-if="activeTooltip.item.Note" class="mt-1 pt-1 border-t border-[#0033aa]/50 text-gray-400 text-[10px] italic leading-tight">
                "{{ activeTooltip.item.Note }}"
             </div>
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

.text-shadow-sm {
  text-shadow: 1px 1px 0 #000;
}

.equipments-panel {
  /* Fundo sutil para separar as seções, se necessário */
  background-color: rgba(0, 10, 43, 0.4);
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
