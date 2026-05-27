<script setup lang="ts">
import { ref } from "vue";
import type { Equipments, EnrichedEquipment } from "@/models";
import { EquipmentRepository } from "@/repositories/equipment.repository.ts";
import DigimonEquipament from "./DigimonEquipament.vue";
import EquipmentTooltip from "@/components/tooltip/EquipmentTooltip.vue";
import { useTooltipPosition } from "@/composables/use-tooltip-position";
import type { EquipmentRaw } from "@/repositories/tables/raws/equipment/equipment.raw.ts";

const props = defineProps<{
  equipments: Equipments;
}>();

const slotKeys = [
  "head",
  "body",
  "rightHand",
  "leftHand",
  "accessory1",
  "accessory2"
] as const;

const tooltipPosition = useTooltipPosition(300);
const { show: tooltipShow, x: tooltipX, y: tooltipY, showAt, move, hide } = tooltipPosition;
const selectedEquipment = ref<EnrichedEquipment | null>(null);

const getEnrichedEquipment = (
  slotKey: "head" | "body" | "rightHand" | "leftHand" | "accessory1" | "accessory2"
): EquipmentRaw | null => {
  const equipmentId = props.equipments?.[slotKey];
  if (!equipmentId) {
    return null;
  }

  return EquipmentRepository.getRawEquipmentById(equipmentId);
};

const showTooltip = (event: MouseEvent, enrichedEquipment: EnrichedEquipment) => {
  selectedEquipment.value = enrichedEquipment;
  showAt(event, { maxWidth: 300 });
};

const hideTooltip = () => {
  hide();
  selectedEquipment.value = null;
};

const moveTooltip = (event: MouseEvent) => {
  move(event);
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

    <EquipmentTooltip
      :show="tooltipShow"
      :x="tooltipX"
      :y="tooltipY"
      :equipment="selectedEquipment"
    />
  </div>
</template>
