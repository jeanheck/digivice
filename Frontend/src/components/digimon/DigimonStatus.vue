<script setup lang="ts">
import type { DigimonStatus } from '../../models/Digimon';

defineProps<{
  status: DigimonStatus;
  label: string;
  icon: string;
  colorClass: string;
}>();

const emit = defineEmits<{
  (e: 'showIconTooltip', event: MouseEvent, title: string, propertyKey: string): void;
  (e: 'showMathTooltip', event: MouseEvent, title: string, base: number, equip: number, digi: number, total: number): void;
  (e: 'moveTooltip', event: MouseEvent): void;
  (e: 'hideTooltip'): void;
}>();
</script>

<template>
  <div class="flex items-center gap-2">
    <div class="flex items-center w-[20px] justify-center cursor-help select-none z-20 tooltip-anchor relative"
         @mouseenter="e => emit('showIconTooltip', e, label, status.type)"
         @mousemove="e => emit('moveTooltip', e)"
         @mouseleave="emit('hideTooltip')">
      <span class="text-base font-emoji drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-1" :class="colorClass">{{ icon }}</span>
    </div>
    
    <div class="font-bold tracking-widest cursor-help flex items-center"
         @mouseenter="e => emit('showMathTooltip', e, label, status.fromDigimon, status.fromEquipaments, status.fromDigievolution, status.sumBetweenDigimonAndEquipaments)"
         @mousemove="e => emit('moveTooltip', e)"
         @mouseleave="emit('hideTooltip')">
      <span class="shadow-text">{{ status.sumBetweenDigimonAndEquipaments }}</span>
      <span v-if="status.fromDigievolution > 0" class="ml-2 font-bold bg-gradient-to-b from-[#ffcc00] to-[#ff6600] text-transparent bg-clip-text shadow-text-dark tracking-normal">+{{ status.fromDigievolution }}</span>
    </div>
  </div>
</template>
