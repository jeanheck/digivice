<script setup lang="ts">
import { computed } from 'vue';
import type { EnrichedEquipment } from '@/models';
import { useLocalization } from '@/composables/useLocalization';

const props = defineProps<{
  slotKey: 'head' | 'body' | 'rightHand' | 'leftHand' | 'accessory1' | 'accessory2';
  enrichedEquipment: EnrichedEquipment | null;
}>();

const { t, getLocalized } = useLocalization();

const emit = defineEmits<{
  (e: 'showTooltip', event: MouseEvent, equip: EnrichedEquipment): void;
  (e: 'moveTooltip', event: MouseEvent): void;
  (e: 'hideTooltip'): void;
}>();

const slotLabel = computed(() => {
  if (props.slotKey === 'rightHand') {
    return t('digimon.slots.right');
  }
  if (props.slotKey === 'leftHand') {
    return t('digimon.slots.left');
  }
  return t(`digimon.slots.${props.slotKey}`);
});

const colorClass = computed(() => {
  if (props.slotKey === 'accessory1' || props.slotKey === 'accessory2') {
    return 'text-cyan-300';
  }
  return 'text-gray-400';
});
</script>

<template>
  <div 
    :class="['flex justify-between items-center py-1 border-b-[1px] border-[#0033aa] last:border-0', 
             enrichedEquipment ? 'cursor-help' : '']"
    @mouseenter="e => enrichedEquipment ? emit('showTooltip', e, enrichedEquipment) : null"
    @mousemove="e => emit('moveTooltip', e)"
    @mouseleave="emit('hideTooltip')"
  >
    <span class="font-bold tracking-widest text-[#0077ff] shadow-text min-w-[75px]">
      {{ slotLabel }}
    </span>

    <span :class="[colorClass, 'shadow-text truncate font-medium text-[11px] text-right']">
      {{ enrichedEquipment ? getLocalized(enrichedEquipment.name) : $t('digimon.states.empty') }}
    </span>
  </div>
</template>
