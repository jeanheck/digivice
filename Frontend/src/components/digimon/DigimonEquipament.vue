<script setup lang="ts">
import type { Equipament } from '../../models'
import { useLocalization } from '../../composables/useLocalization'

defineProps<{
  slotLabel: string;
  equipament: Equipament | null;
  colorClass?: string;
}>();

const { getLocalized } = useLocalization();

const emit = defineEmits<{
  (e: 'showTooltip', event: MouseEvent, equip: Equipament): void;
  (e: 'moveTooltip', event: MouseEvent): void;
  (e: 'hideTooltip'): void;
}>();
</script>

<template>
  <div 
    :class="['flex justify-between items-center py-1 border-b-[1px] border-[#0033aa] last:border-0', 
             equipament ? 'cursor-help' : '']"
    @mouseenter="e => equipament ? emit('showTooltip', e, equipament) : null"
    @mousemove="e => emit('moveTooltip', e)"
    @mouseleave="emit('hideTooltip')"
  >
    <span class="font-bold tracking-widest text-[#0077ff] shadow-text min-w-[75px]">
      {{ slotLabel }}
    </span>

    <span :class="[colorClass || 'text-gray-300', 'shadow-text truncate font-medium text-[11px] text-right']">
      {{ equipament ? getLocalized(equipament.name) : $t('digimon.states.empty') }}
    </span>
  </div>
</template>
