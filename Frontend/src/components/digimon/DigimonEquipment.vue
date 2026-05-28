<script setup lang="ts">
import { EquipmentSlotKey } from "@/constants/equipment-slot-key";
import { useLocalization } from "@/composables/useLocalization";
import type { EquipmentViewModel } from "@/viewmodels/digimon/equipment.viewmodel";

const props = defineProps<{
  slotKey: EquipmentSlotKey;
  equipment: EquipmentViewModel | null;
}>();

const { t, getLocalizedEquipmentName } = useLocalization();

const emit = defineEmits<{
  (e: "showTooltip", event: MouseEvent, equipment: EquipmentViewModel): void;
  (e: "moveTooltip", event: MouseEvent): void;
  (e: "hideTooltip"): void;
}>();
</script>

<template>
  <div
    :class="['flex justify-between items-center py-1 border-b border-[#0033aa] last:border-0',
             equipment ? 'cursor-help' : '']"
    @mouseenter="(event) => equipment ? emit('showTooltip', event, equipment) : null"
    @mousemove="(event) => emit('moveTooltip', event)"
    @mouseleave="emit('hideTooltip')"
  >
    <span class="font-bold tracking-widest text-[#0077ff] shadow-text min-w-18.75">
      {{ t(`digimon.equipmentSlot.${props.slotKey}`) }}
    </span>

    <span class="text-gray-400 shadow-text truncate font-medium text-[11px] text-right">
      {{ equipment ? getLocalizedEquipmentName(equipment) : $t("digimon.states.empty") }}
    </span>
  </div>
</template>
