<script setup lang="ts">
import { useI18n } from "vue-i18n";
import PartyEquipmentListItem from "@/ui-next/components/party/equipments/PartyEquipmentListItem.vue";
import type { EquipmentSlot } from "@/shared/types/equipment-slot.type";
import type { PartyEquipmentsMock } from "@/ui-next/mocks/party.mock";

const { equipments } = defineProps<{
  equipments: PartyEquipmentsMock;
}>();

const { t } = useI18n();

const EquipmentSlots = [
  "head",
  "body",
  "right",
  "left",
  "accessory1",
  "accessory2",
] as const satisfies readonly EquipmentSlot[];

const FullWidthSlots: readonly EquipmentSlot[] = ["head", "body"];

function getName(equipmentSlot: EquipmentSlot): string {
  const equipmentId = equipments[equipmentSlot];

  if (equipmentId === null) {
    return t(`digimon.equipmentSlot.${equipmentSlot}`);
  }

  return t(`equipments.${equipmentId}.name`);
}

function getTitle(equipmentSlot: EquipmentSlot): string {
  const slotLabel = t(`digimon.equipmentSlot.${equipmentSlot}`);

  if (equipments[equipmentSlot] === null) {
    return slotLabel;
  }

  return `${slotLabel}: ${getName(equipmentSlot)}`;
}
</script>

<template>
  <div class="relative text-foreground dw3-beveled">
    <div class="absolute inset-0 bg-progress-frame dw3-beveled pointer-events-none"></div>
    <div class="absolute inset-[1.5px] bg-surface-card dw3-beveled pointer-events-none"></div>

    <div class="relative z-10 px-2 pt-0.5 pb-1.5 grid grid-cols-2 gap-x-3">
      <PartyEquipmentListItem
        v-for="equipmentSlot in EquipmentSlots"
        :key="equipmentSlot"
        :class="{ 'col-span-2': FullWidthSlots.includes(equipmentSlot) }"
        :name="getName(equipmentSlot)"
        :title="getTitle(equipmentSlot)"
        :is-equipped="equipments[equipmentSlot] !== null"
      />
    </div>
  </div>
</template>
