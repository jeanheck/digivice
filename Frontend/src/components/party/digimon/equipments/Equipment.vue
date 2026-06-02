<script setup lang="ts">
import { EquipmentConstant } from "@/constants/equipment.constant";
import { useLocalization } from "@/composables/useLocalization";
import type { EquipmentViewModel } from "@/viewmodels/digimon/equipment.viewmodel";
import { computed } from "vue";

const props = defineProps<{
  slotKey: EquipmentConstant;
  equipment: EquipmentViewModel | null;
}>();

const { t } = useLocalization();

const emit = defineEmits<{
  (e: "showTooltip", event: MouseEvent, equipment: EquipmentViewModel): void;
  (e: "moveTooltip", event: MouseEvent): void;
  (e: "hideTooltip"): void;
}>();

const equipmentName = computed(() => props.equipment?.id ? t(`equipments.${props.equipment.id}.name`) : t("digimon.states.empty"));
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
      {{ equipmentName }}
    </span>
  </div>
</template>
