<script setup lang="ts">
import { computed, ref } from "vue";
import { EQUIPMENT_SLOT_KEYS } from "@/constants/equipment.constant.ts";
import type { Equipments } from "@/models";
import Equipment from "./Equipment.vue";
import EquipmentsTooltip from "./EquipmentsTooltip.vue";
import { useTooltipPosition } from "@/composables/use-tooltip-position";
import { EquipmentsPresenter } from "@/presenters/party/digimon/equipments.presenter.ts";
import type { EquipmentViewModel } from "@/viewmodels/digimon/equipment.viewmodel";

const props = defineProps<{
  equipments: Equipments;
}>();

const equipmentSlots = computed(() => {
  return EQUIPMENT_SLOT_KEYS.map((slotKey) => {
    return {
      slotKey,
      equipment: EquipmentsPresenter.getEquipmentBySlot(props.equipments, slotKey),
    };
  });
});

const tooltipPlacement = "below" as const;
const tooltipPosition = useTooltipPosition(300);
const { show: tooltipShow, x: tooltipX, y: tooltipY, showAt, move, hide } = tooltipPosition;
const selectedEquipment = ref<EquipmentViewModel | null>(null);

const showTooltip = (event: MouseEvent, equipment: EquipmentViewModel) => {
  selectedEquipment.value = equipment;
  showAt(event, { maxWidth: 300, placement: tooltipPlacement });
};

const hideTooltip = () => {
  hide();
  selectedEquipment.value = null;
};

const moveTooltip = (event: MouseEvent) => {
  move(event, tooltipPlacement);
};
</script>

<template>
  <div class="dw3-panel flex flex-col">
    <div class="dw3-panel-border dw3-beveled"></div>
    <div class="dw3-panel-inner dw3-beveled"></div>

    <div class="dw3-panel-content w-full flex flex-col pt-2 p-3 text-white text-xs">
      <Equipment
        v-for="equipmentSlot in equipmentSlots"
        :key="equipmentSlot.slotKey"
        :slot-key="equipmentSlot.slotKey"
        :equipment="equipmentSlot.equipment"
        @show-tooltip="showTooltip"
        @move-tooltip="moveTooltip"
        @hide-tooltip="hideTooltip"
      />
    </div>

    <EquipmentsTooltip
      :show="tooltipShow"
      :x="tooltipX"
      :y="tooltipY"
      :equipment="selectedEquipment"
      placement="below"
    />
  </div>
</template>
