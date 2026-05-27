<script setup lang="ts">
import { computed } from "vue";
import Tooltip from "@/components/tooltip/Tooltip.vue";
import { useLocalization } from "@/composables/useLocalization";
import { EquipmentsAttributesOperationType, type EnrichedEquipment } from "@/models";

const props = withDefaults(
    defineProps<{
        show: boolean;
        x: number;
        y: number;
        equipment: EnrichedEquipment | null;
        maxWidth?: number;
    }>(),
    {
        maxWidth: 300
    }
);

const { t, getLocalized, getLocalizedEquipmentName, getLocalizedEquipmentNote } = useLocalization();

const equipmentTitle = computed(() => {
    if (!props.equipment) {
        return "";
    }

    return getLocalizedEquipmentName(props.equipment);
});

const equipmentWithOptionalFields = computed(() => {
    return props.equipment as EnrichedEquipment & {
        typeDescription?: Record<string, string>;
        note?: Record<string, string>;
    } | null;
});
</script>

<template>
  <Tooltip
    :show="show && !!equipment"
    :x="x"
    :y="y"
    :title="equipmentTitle"
    :max-width="maxWidth"
    placement="below"
  >
    <div v-if="equipmentWithOptionalFields" class="flex flex-col gap-1 w-full min-w-42.5">
      <div
        v-if="equipmentWithOptionalFields.typeDescription"
        class="text-blue-300 text-[10px] tracking-widest uppercase mb-1 text-center font-bold"
      >
        {{ getLocalized(equipmentWithOptionalFields.typeDescription) }}
      </div>

      <div
        v-if="equipmentWithOptionalFields.attributes && equipmentWithOptionalFields.attributes.length > 0"
        class="flex flex-col gap-0.5 mb-1"
      >
        <div
          v-for="attribute in equipmentWithOptionalFields.attributes"
          :key="attribute.attribute"
          class="flex justify-between text-xs items-center bg-[#002266]/40 px-1 rounded-sm"
        >
          <span class="text-gray-200">{{ t("attribute." + attribute.attribute.toLowerCase()) }}</span>
          <span
            :class="attribute.type === EquipmentsAttributesOperationType.Addition ? 'text-green-400' : 'text-red-400'"
            class="font-bold tracking-wider"
          >
            {{ attribute.type === EquipmentsAttributesOperationType.Addition ? "+" : "-" }}{{ attribute.value }}
          </span>
        </div>
      </div>

      <div class="mt-1 pt-1 border-t border-[#0033aa]/50 flex flex-col gap-1">
        <span class="text-gray-400 text-[9px] uppercase tracking-widest leading-none">{{ $t("digimon.equipableBy") }}</span>
        <span class="text-gray-200 text-[11px] leading-tight">
          <template v-if="!equipmentWithOptionalFields.equipableDigimon || equipmentWithOptionalFields.equipableDigimon.length === 0">
            {{ $t("digimon.states.none") }}
          </template>
          <template v-else-if="equipmentWithOptionalFields.equipableDigimon.length === 8">
            <span class="text-purple-300 font-bold uppercase tracking-wider text-[10px]">{{ $t("digimon.allDigimon") }}</span>
          </template>
          <template v-else>
            {{ equipmentWithOptionalFields.equipableDigimon.join(", ") }}
          </template>
        </span>
      </div>

      <div
        v-if="equipmentWithOptionalFields.note"
        class="mt-1 pt-1 border-t border-[#0033aa]/50 text-gray-400 text-[10px] italic leading-tight"
      >
        "{{ getLocalizedEquipmentNote(equipmentWithOptionalFields) }}"
      </div>
    </div>
  </Tooltip>
</template>
