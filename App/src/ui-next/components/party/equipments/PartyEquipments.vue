<script setup lang="ts">
import { useI18n } from "vue-i18n";
import PartyEquipment from "@/ui-next/components/party/equipments/PartyEquipment.vue";
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

// Code points without U+FE0F, otherwise WebView2 falls back to the colored Segoe UI Emoji.
const EquipmentSlotIcons = {
  head: "\u{1FA96}",
  body: "\u{1F94B}",
  right: "\u{1F5E1}",
  left: "\u{1F6E1}",
  accessory1: "\u{1F48D}",
  accessory2: "\u{1F4FF}",
} as const satisfies Record<EquipmentSlot, string>;
</script>

<template>
  <div class="relative w-10 shrink-0 dw3-beveled">
    <div class="absolute inset-0 bg-card-frame dw3-beveled pointer-events-none"></div>
    <div class="absolute inset-[1.5px] bg-surface dw3-beveled pointer-events-none"></div>

    <div class="relative z-10 h-full py-1.5 flex flex-col items-center justify-center gap-1.5">
      <PartyEquipment
        v-for="equipmentSlot in EquipmentSlots"
        :key="equipmentSlot"
        :icon="EquipmentSlotIcons[equipmentSlot]"
        :label="t(`digimon.equipmentSlot.${equipmentSlot}`)"
        :is-equipped="equipments[equipmentSlot] !== null"
      />
    </div>
  </div>
</template>
