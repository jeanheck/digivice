<script setup lang="ts">
import { computed } from 'vue';
import { useLocalization } from '@/composables/useLocalization';
import type { EnrichedAttributeResistance } from '@/models';

const props = defineProps<{
  enrichedAttributeResistance: EnrichedAttributeResistance;
  propertyKey: string;
}>();

const emit = defineEmits<{
  (e: 'showIconTooltip', event: MouseEvent, title: string, propertyKey: string): void;
  (e: 'showMathTooltip', event: MouseEvent, title: string, base: number, equip: number, digi: number, total: number): void;
  (e: 'moveTooltip', event: MouseEvent): void;
  (e: 'hideTooltip'): void;
}>();

const { t } = useLocalization();

interface Metadata {
  labelKey: string;
  icon: string;
  colorClass: string;
}

const metadataMap: Record<string, Metadata> = {
  strength: { labelKey: 'attributes.strength', icon: '👊', colorClass: 'text-[#fcd883]' },
  defense: { labelKey: 'attributes.defense', icon: '🛡️', colorClass: 'text-gray-400' },
  spirit: { labelKey: 'attributes.spirit', icon: '🧙‍♂️', colorClass: 'text-pink-300' },
  wisdom: { labelKey: 'attributes.wisdom', icon: '📖', colorClass: 'text-yellow-600' },
  speed: { labelKey: 'attributes.speed', icon: '🏃', colorClass: 'text-green-400' },
  charisma: { labelKey: 'attributes.charisma', icon: '✨', colorClass: 'text-yellow-300' },

  fire: { labelKey: 'resistances.fire', icon: '🔥', colorClass: 'text-orange-500' },
  water: { labelKey: 'resistances.water', icon: '💧', colorClass: 'text-blue-400' },
  ice: { labelKey: 'resistances.ice', icon: '🧊', colorClass: 'text-cyan-200' },
  wind: { labelKey: 'resistances.wind', icon: '🍃', colorClass: 'text-gray-100' },
  thunder: { labelKey: 'resistances.thunder', icon: '⚡', colorClass: 'text-[#ffffcc]' },
  machine: { labelKey: 'resistances.machine', icon: '⚙️', colorClass: 'text-gray-500' },
  dark: { labelKey: 'resistances.dark', icon: '🌑', colorClass: 'text-purple-500' }
};

const metadata = computed(() => {
  return metadataMap[props.propertyKey] ?? { labelKey: '', icon: '', colorClass: '' };
});

const label = computed(() => {
  return t(metadata.value.labelKey);
});
</script>

<template>
  <div class="flex items-center gap-2">
    <div class="flex items-center w-[20px] justify-center cursor-help select-none z-20 tooltip-anchor relative"
         @mouseenter="e => emit('showIconTooltip', e, label, propertyKey)"
         @mousemove="e => emit('moveTooltip', e)"
         @mouseleave="emit('hideTooltip')">
      <span class="text-base font-emoji drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-1" :class="metadata.colorClass">{{ metadata.icon }}</span>
    </div>
    
    <div class="font-bold tracking-widest cursor-help flex items-center"
         @mouseenter="e => emit('showMathTooltip', e, label, enrichedAttributeResistance.fromDigimon, enrichedAttributeResistance.fromEquipaments, enrichedAttributeResistance.fromDigievolution, enrichedAttributeResistance.sumBetweenDigimonAndEquipaments)"
         @mousemove="e => emit('moveTooltip', e)"
         @mouseleave="emit('hideTooltip')">
      <span class="shadow-text">{{ enrichedAttributeResistance.sumBetweenDigimonAndEquipaments }}</span>
      <span v-if="enrichedAttributeResistance.fromDigievolution > 0" class="ml-2 font-bold bg-linear-to-b from-[#ffcc00] to-[#ff6600] text-transparent bg-clip-text shadow-text-dark tracking-normal">+{{ enrichedAttributeResistance.fromDigievolution }}</span>
    </div>
  </div>
</template>
