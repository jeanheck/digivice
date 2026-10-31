<script setup lang="ts">
import { useI18n } from "vue-i18n";
import type { EquipmentSlotViewModel } from "@/presentation/viewmodels/digimon/equipment-slot.viewmodel";
import type { EquipmentViewModel } from "@/presentation/viewmodels/digimon/equipment.viewmodel";
import { computed } from "vue";

const props = defineProps<{
  equipmentSlot: EquipmentSlotViewModel;
}>();

const { t } = useI18n();

const emit = defineEmits<{
  (e: "showTooltip", event: MouseEvent, equipment: EquipmentViewModel): void;
  (e: "moveTooltip", event: MouseEvent): void;
  (e: "hideTooltip"): void;
}>();

const isEquipped = computed(() => {
  return props.equipmentSlot.equipment?.id != null;
});

const displayText = computed(() => {
  if (isEquipped.value) {
    return t(`equipments.${props.equipmentSlot.equipment!.id}.name`);
  }

  return t(`digimon.equipmentSlot.${props.equipmentSlot.slotKey}`);
});

function onMouseEnter(event: MouseEvent): void {
  if (!isEquipped.value || props.equipmentSlot.equipment === null) {
    return;
  }

  emit("showTooltip", event, props.equipmentSlot.equipment);
}
</script>

<template>
  <div
    :class="[
      'flex items-center py-1 border-b border-[#0033aa] last:border-0',
      isEquipped ? 'cursor-help' : 'cursor-default',
    ]"
    @mouseenter="onMouseEnter"
    @mousemove="(event) => emit('moveTooltip', event)"
    @mouseleave="emit('hideTooltip')"
  >
    <span
      class="w-full truncate shadow-text font-bold text-center"
      :class="
        isEquipped
          ? 'text-[10px] tracking-widest text-[#0077ff]'
          : 'text-gray-400 text-[10px]'
      "
    >
      {{ displayText }}
    </span>
  </div>
</template>
