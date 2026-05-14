<script setup lang="ts">
import { computed, ref } from 'vue'
import { useLocalization } from '../../composables/useLocalization'
import type { Equipments, Equipament } from '../../models'
import DigimonEquipament from './DigimonEquipament.vue'
import DigimonEquipmentTooltip from './DigimonEquipmentTooltip.vue'

const props = defineProps<{
  equipments: Equipments
}>()

const { t } = useLocalization()

const equipmentsList = computed(() => {
  const eq = props.equipments
  return [
    { slot: t('digimon.slots.head'), item: eq?.head || null, color: 'text-gray-400' },
    { slot: t('digimon.slots.body'), item: eq?.body || null, color: 'text-gray-400' },
    { slot: t('digimon.slots.right'), item: eq?.rightHand || null, color: 'text-gray-400' },
    { slot: t('digimon.slots.left'), item: eq?.leftHand || null, color: 'text-gray-400' },
    { slot: t('digimon.slots.accessory1'), item: eq?.accessory1 || null, color: 'text-cyan-300' },
    { slot: t('digimon.slots.accessory2'), item: eq?.accessory2 || null, color: 'text-cyan-300' }
  ]
})

const activeTooltip = ref({ show: false, item: null as Equipament | null, x: 0, y: 0 })

const showTooltip = (event: MouseEvent, equipObj: Equipament) => {
  let posX = event.clientX + 15
  if (posX + 250 > window.innerWidth) {
      posX = event.clientX - 260
  }
  
  activeTooltip.value = { show: true, item: equipObj, x: posX, y: event.clientY + 15 }
}

const hideTooltip = () => {
  activeTooltip.value.show = false
}

const moveTooltip = (event: MouseEvent) => {
  if (!activeTooltip.value.show) {
      return
  }
  
  let posX = event.clientX + 15
  if (posX + 250 > window.innerWidth) {
      posX = event.clientX - 260
  }
  
  activeTooltip.value.x = posX
  activeTooltip.value.y = event.clientY + 15
}
</script>

<template>
  <div class="relative overflow-hidden flex flex-col w-full bg-[#000a2b]">
    <div class="absolute inset-0 bg-[#0077ff] pointer-events-none dw3-beveled"></div>
    <div class="absolute inset-[1.5px] bg-[#000a2b] pointer-events-none dw3-beveled"></div>

    <div class="relative z-10 w-full flex flex-col pt-2 p-3 text-white text-xs shadow-inner bg-[#000a2b]/40">
      <DigimonEquipament 
        v-for="(equip, index) in equipmentsList" 
        :key="index"
        :slotLabel="equip.slot"
        :equipament="equip.item"
        :colorClass="equip.color"
        @showTooltip="showTooltip"
        @moveTooltip="moveTooltip"
        @hideTooltip="hideTooltip"
      />
    </div>

    <DigimonEquipmentTooltip :activeTooltip="activeTooltip" />
  </div>
</template>
