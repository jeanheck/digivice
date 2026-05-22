<script setup lang="ts">
import { computed, ref } from 'vue';
import type { Equipments, EnrichedEquipment } from '@/models';
import { EquipamentRepository } from '@/repositories/equipament-repository';
import DigimonEquipament from './DigimonEquipament.vue';
import DigimonEquipmentTooltip from './DigimonEquipmentTooltip.vue';

const props = defineProps<{
  equipments: Equipments;
}>();

const slotKeys = [
  'head',
  'body',
  'rightHand',
  'leftHand',
  'accessory1',
  'accessory2'
] as const;

const enrichedEquipments = computed(() => {
  return EquipamentRepository.getEquipmentsByIds(props.equipments);
});

const getEnrichedEquipment = (
  slotKey: 'head' | 'body' | 'rightHand' | 'leftHand' | 'accessory1' | 'accessory2'
): EnrichedEquipment | null => {
  const equipmentId = props.equipments?.[slotKey];
  if (!equipmentId) {
    return null;
  }
  const foundEquipment = enrichedEquipments.value.find((equipment) => {
    return equipment.id === equipmentId;
  });
  return foundEquipment || null;
};

const activeTooltip = ref({ show: false, item: null as EnrichedEquipment | null, x: 0, y: 0 });

const showTooltip = (event: MouseEvent, equipObj: EnrichedEquipment) => {
  let posX = event.clientX + 15;
  if (posX + 250 > window.innerWidth) {
      posX = event.clientX - 260;
  }
  
  activeTooltip.value = { show: true, item: equipObj, x: posX, y: event.clientY + 15 };
};

const hideTooltip = () => {
  activeTooltip.value.show = false;
};

const moveTooltip = (event: MouseEvent) => {
  if (!activeTooltip.value.show) {
      return;
  }
  
  let posX = event.clientX + 15;
  if (posX + 250 > window.innerWidth) {
      posX = event.clientX - 260;
  }
  
  activeTooltip.value.x = posX;
  activeTooltip.value.y = event.clientY + 15;
};
</script>

<template>
  <div class="relative overflow-hidden flex flex-col w-full bg-[#000a2b]">
    <div class="absolute inset-0 bg-[#0077ff] pointer-events-none dw3-beveled"></div>
    <div class="absolute inset-[1.5px] bg-[#000a2b] pointer-events-none dw3-beveled"></div>

    <div class="relative z-10 w-full flex flex-col pt-2 p-3 text-white text-xs shadow-inner bg-[#000a2b]/40">
      <DigimonEquipament 
        v-for="slotKey in slotKeys" 
        :key="slotKey"
        :slotKey="slotKey"
        :enrichedEquipment="getEnrichedEquipment(slotKey)"
        @showTooltip="showTooltip"
        @moveTooltip="moveTooltip"
        @hideTooltip="hideTooltip"
      />
    </div>

    <DigimonEquipmentTooltip :activeTooltip="activeTooltip" />
  </div>
</template>
