<script setup lang="ts">
import { computed, ref } from 'vue'
import DigimonDetailsTable from '../../data/static/DigimonDetailsTable.json'
import EquipmentsData from '../../data/static/Equipments.json'
import DigievolutionData from '../../data/static/Digievolution.json'

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

const getEquipBonus = (prop: string) => {
    let total = 0;
    
    // Filtro inicial para pegar os IDs das armas ignorando zeros
    const equipIds = Object.values(props.digimon.equipments).filter(id => id > 0);
    
    // Obter array de objetos dos itens
    const items = equipIds.map(id => EquipmentsData.equipments.find(e => e.Id === id)).filter(i => i) as typeof EquipmentsData.equipments;
    
    // Para Weapons Two Handed que ocupam duas mãos, removemos duplicatas pelo ID
    // Assumimos que a duplicata vem por ocupar o slot LeftHand e RightHand
    const uniqueItems = items.filter((item, index, self) => {
      if (item.Type === "WeaponTwoHanded") {
        return self.findIndex(i => i.Id === item.Id) === index;
      }
      return true;
    });

    uniqueItems.forEach(item => {
        if (item.Attributes) {
            item.Attributes.forEach(attr => {
                if (attr.Attribute?.toLowerCase() === prop.toLowerCase()) {
                    if (attr.Type === "Addition") total += attr.Value;
                    else if (attr.Type === "Subtraction") total -= attr.Value;
                }
            });
        }
    });
    return total;
}

const getDigiBonus = (type: 'attributes' | 'resistances', prop: string) => {
    const digiId = props.digimon.activeDigievolutionId;
    if (digiId) {
        const digi = DigievolutionData.digievolutions.find(d => d.id === digiId);
        if (digi) {
            const jsonField = type === 'attributes' ? digi.Attributes : digi.Resistances;
            if (jsonField) {
                const PascalProp = prop.charAt(0).toUpperCase() + prop.slice(1);
                return (jsonField as any)[PascalProp] || 0;
            }
        }
    }
    return 0;
}

// Mapeamento dinâmico baseado no Digimon World 3 (Stat -> Value)
const attributes = computed(() => {
  const attrs = props.digimon.attributes
  return [
    { label: 'Strength', base: attrs.strength, prop: 'strength', icon: IconStrengh, color: 'text-[#fcd883]' },
    { label: 'Defense', base: attrs.defense, prop: 'defense', icon: IconDefense, color: 'text-gray-400' },
    { label: 'Spirit', base: attrs.spirit, prop: 'spirit', icon: IconSpirit, color: 'text-pink-300' },
    { label: 'Wisdom', base: attrs.wisdom, prop: 'wisdom', icon: IconWisdom, color: 'text-yellow-600' },
    { label: 'Speed', base: attrs.speed, prop: 'speed', icon: IconSpeed, color: 'text-green-400' },
    { label: 'Charisma', base: attrs.charisma, prop: 'charisma', icon: IconCharisma, color: 'text-yellow-300' },
  ].map(a => {
    const equip = getEquipBonus(a.prop);
    const digi = getDigiBonus('attributes', a.prop);
    return { ...a, equip, digi, total: a.base + equip }
  })
})

const resistances = computed(() => {
  const res = props.digimon.resistances
  return [
    { label: 'Fire', base: res.fire, prop: 'fire', icon: IconFireResistance, color: 'text-orange-500' },
    { label: 'Water', base: res.water, prop: 'water', icon: IconWaterResistance, color: 'text-blue-400' },
    { label: 'Ice', base: res.ice, prop: 'ice', icon: IconIceResistance, color: 'text-cyan-200' },
    { label: 'Wind', base: res.wind, prop: 'wind', icon: IconWindResistance, color: 'text-gray-100' },
    { label: 'Thunder', base: res.thunder, prop: 'thunder', icon: IconThunderResistance, color: 'text-[#ffffcc]' },
    { label: 'Machine', base: res.machine, prop: 'machine', icon: IconMachineResistance, color: 'text-gray-500' },
    { label: 'Dark', base: res.dark, prop: 'dark', icon: IconDarkResistance, color: 'text-purple-500' },
  ].map(a => {
    const equip = getEquipBonus(a.prop);
    const digi = getDigiBonus('resistances', a.prop);
    return { ...a, equip, digi, total: a.base + equip }
  })
})

// Tooltip Logic
const activeTooltip = ref({ show: false, title: '', text: '', isMath: false, base: 0, equip: 0, digi: 0, total: 0, x: 0, y: 0 })

const showIconTooltip = (event: MouseEvent, title: string, dictionaryType: 'attributes' | 'resistances') => {
  const text = DigimonDetailsTable[dictionaryType][title as keyof typeof DigimonDetailsTable[typeof dictionaryType]]
  if (!text) return
  
  let posX = event.clientX + 15
  if (posX + 250 > window.innerWidth) posX = event.clientX - 260
  
  activeTooltip.value = { show: true, isMath: false, title, text, base: 0, equip: 0, digi: 0, total: 0, x: posX, y: event.clientY + 15 }
}

const showMathTooltip = (event: MouseEvent, title: string, base: number, equip: number, digi: number, total: number) => {
  let posX = event.clientX + 15
  if (posX + 250 > window.innerWidth) posX = event.clientX - 260
  
  activeTooltip.value = { show: true, isMath: true, title, text: '', base, equip, digi, total, x: posX, y: event.clientY + 15 }
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
      <div class="flex gap-20 -ml-16">
        <!-- Coluna 1: Atributos Base -->
        <div class="flex flex-col gap-1 w-24">
        <div 
          v-for="attr in attributes" 
          :key="attr.label"
          class="flex items-center gap-2"
        >
          <!-- Ícone Box com Borda -->
          <div class="icon-box w-[28px] h-[28px] bg-[#000a2b] border-[#0033aa] flex items-center justify-center aspect-square cursor-help"
               @mouseenter="e => showIconTooltip(e, attr.label, 'attributes')"
               @mousemove="moveTooltip"
               @mouseleave="hideTooltip">
            <component :is="attr.icon" class="w-5 h-5" :class="attr.color" />
          </div>
          
          <!-- Valor Numérico -->
          <div class="font-bold tracking-widest cursor-help flex items-center"
               @mouseenter="e => showMathTooltip(e, attr.label, attr.base, attr.equip, attr.digi, attr.total)"
               @mousemove="moveTooltip"
               @mouseleave="hideTooltip">
            <span class="text-shadow">{{ attr.total }}</span>
            <span v-if="attr.digi > 0" class="ml-2 font-bold bg-gradient-to-b from-[#ffcc00] to-[#ff6600] text-transparent bg-clip-text shadow-text-dark tracking-normal">+{{ attr.digi }}</span>
          </div>
        </div>
      </div>

        <!-- Coluna 2: Resistências Elementais -->
        <div class="flex flex-col gap-1 w-24">
          <div 
          v-for="res in resistances" 
          :key="res.label"
          class="flex items-center gap-2"
        >
          <!-- Ícone Box com Borda -->
          <div class="icon-box w-[28px] h-[28px] bg-[#000a2b] border-[#0033aa] flex items-center justify-center aspect-square cursor-help"
               @mouseenter="e => showIconTooltip(e, res.label, 'resistances')"
               @mousemove="moveTooltip"
               @mouseleave="hideTooltip">
            <component :is="res.icon" class="w-5 h-5" :class="res.color" />
          </div>
          
          <!-- Valor Numérico -->
          <div class="font-bold tracking-widest cursor-help flex items-center"
               @mouseenter="e => showMathTooltip(e, res.label, res.base, res.equip, res.digi, res.total)"
               @mousemove="moveTooltip"
               @mouseleave="hideTooltip">
            <span class="text-shadow">{{ res.total }}</span>
            <span v-if="res.digi > 0" class="ml-2 font-bold bg-gradient-to-b from-[#ffcc00] to-[#ff6600] text-transparent bg-clip-text shadow-text-dark tracking-normal">+{{ res.digi }}</span>
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
          <div v-if="!activeTooltip.isMath">
             <!-- Normal Text Tooltip (Icons) -->
             <div class="font-bold text-yellow-300 text-sm border-b border-[#0066cc]/50 pb-1 mb-1 shadow-black text-shadow-sm uppercase tracking-wider">
                {{ activeTooltip.title }}
             </div>
             <div class="text-gray-100 text-xs leading-relaxed shadow-black text-shadow-sm">
                {{ activeTooltip.text }}
             </div>
          </div>
          <div v-else class="flex flex-col w-full min-w-[170px]">
             <!-- Math Tooltip (Values) -->
             <div class="font-bold text-yellow-300 text-sm border-b border-[#0066cc]/50 pb-1 mb-2 shadow-black text-shadow-sm uppercase tracking-wider text-center">
                {{ activeTooltip.title }}
             </div>
             
             <!-- Header Value with Parentheses Details -->
             <div class="text-white text-base font-bold text-center mb-2 tracking-wider text-shadow whitespace-nowrap">
                {{ activeTooltip.total }} 
                <span class="text-[10px] text-gray-400 tracking-normal ml-1">(<span class="text-white">{{ activeTooltip.base }}</span> + <span class="text-[#0077ff] font-bold">{{ activeTooltip.equip }}</span>)</span>
             </div>

             <!-- Legenda / Breakdown -->
             <div class="flex flex-col gap-[2px]">
                 <div class="flex justify-between text-xs items-center">
                    <span class="text-white">Base Digimon</span>
                 </div>
                 <div class="flex justify-between text-xs items-center">
                    <span class="text-[#0077ff] font-bold">Equipments</span>
                 </div>
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
