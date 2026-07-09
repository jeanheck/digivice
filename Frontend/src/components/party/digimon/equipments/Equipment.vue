<script setup lang="ts">
import { EquipmentConstant } from "@/constants/equipment.constant";
import { useI18n } from "vue-i18n";
import type { EquipmentViewModel } from "@/viewmodels/digimon/equipment.viewmodel";
import { computed } from "vue";

const props = defineProps<{
  slotKey: EquipmentConstant;
  equipment: EquipmentViewModel | null;
}>();

const { t } = useI18n();

const emit = defineEmits<{
  (e: "showTooltip", event: MouseEvent, equipment: EquipmentViewModel): void;
  (e: "moveTooltip", event: MouseEvent): void;
  (e: "hideTooltip"): void;
}>();

const isEquipped = computed(() => {
  return props.equipment?.id != null;
});

const displayText = computed(() => {
  if (isEquipped.value) {
    return t(`equipments.${props.equipment!.id}.name`);
  }

  return t(`digimon.equipmentSlot.${props.slotKey}`);
});

function onMouseEnter(event: MouseEvent): void {
  if (!isEquipped.value || props.equipment === null) {
    return;
  }

  emit("showTooltip", event, props.equipment);
}
</script>

<template>
  <div
    :class="['flex items-center py-1 border-b border-[#0033aa] last:border-0',
             isEquipped ? 'cursor-help' : '']"
    @mouseenter="onMouseEnter"
    @mousemove="(event) => emit('moveTooltip', event)"
    @mouseleave="emit('hideTooltip')"
  >
    <span
      class="w-full truncate shadow-text font-bold text-center"
      :class="isEquipped
        ? 'text-xs tracking-widest text-[#0077ff]'
        : 'text-gray-400 text-xs'"
    >
      {{ displayText }}
    </span>
  </div>
</template>
