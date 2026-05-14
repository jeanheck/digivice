<script setup lang="ts">
import { computed, ref } from 'vue';
import { useLocalization } from '../../composables/useLocalization';
import type { Attributes, Resistances } from '../../models';
import DigimonStatus from './DigimonStatus.vue';
import DigimonTooltip from './DigimonTooltip.vue';

const props = defineProps<{
  attributes: Attributes;
  resistances: Resistances;
}>();

const { t } = useLocalization();

const computedAttributes = computed(() => {
  const attrs = props.attributes;
  return [
    { label: t('attributes.strength'), status: attrs.strength, icon: '👊', color: 'text-[#fcd883]' },
    { label: t('attributes.defense'), status: attrs.defense, icon: '🛡️', color: 'text-gray-400' },
    { label: t('attributes.spirit'), status: attrs.spirit, icon: '🧙‍♂️', color: 'text-pink-300' },
    { label: t('attributes.wisdom'), status: attrs.wisdom, icon: '📖', color: 'text-yellow-600' },
    { label: t('attributes.speed'), status: attrs.speed, icon: '🏃', color: 'text-green-400' },
    { label: t('attributes.charisma'), status: attrs.charisma, icon: '✨', color: 'text-yellow-300' },
  ];
});

const computedResistances = computed(() => {
  const res = props.resistances;
  return [
    { label: t('resistances.fire'), status: res.fire, icon: '🔥', color: 'text-orange-500' },
    { label: t('resistances.water'), status: res.water, icon: '💧', color: 'text-blue-400' },
    { label: t('resistances.ice'), status: res.ice, icon: '🧊', color: 'text-cyan-200' },
    { label: t('resistances.wind'), status: res.wind, icon: '🍃', color: 'text-gray-100' },
    { label: t('resistances.thunder'), status: res.thunder, icon: '⚡', color: 'text-[#ffffcc]' },
    { label: t('resistances.machine'), status: res.machine, icon: '⚙️', color: 'text-gray-500' },
    { label: t('resistances.dark'), status: res.dark, icon: '🌑', color: 'text-purple-500' },
  ];
});

const activeTooltip = ref({ show: false, title: '', text: '', isMath: false, base: 0, equip: 0, digi: 0, total: 0, x: 0, y: 0 });

const showIconTooltip = (event: MouseEvent, title: string, propertyKey: string) => {
  const text = t(`tooltips.${propertyKey}`);
  if (!text) {
    return;
  }
  
  let posX = event.clientX + 15;
  if (posX + 250 > window.innerWidth) {
    posX = event.clientX - 260;
  }
  
  activeTooltip.value = { show: true, isMath: false, title, text, base: 0, equip: 0, digi: 0, total: 0, x: posX, y: event.clientY + 15 };
};

const showMathTooltip = (event: MouseEvent, title: string, base: number, equip: number, digi: number, total: number) => {
  let posX = event.clientX + 15;
  if (posX + 250 > window.innerWidth) {
    posX = event.clientX - 260;
  }
  
  activeTooltip.value = { show: true, isMath: true, title: title, text: '', base, equip, digi, total, x: posX, y: event.clientY + 15 };
};

const hideTooltip = () => {
  activeTooltip.value.show = false;
};

const moveTooltip = (event: MouseEvent) => {
  if (!activeTooltip.value.show) {
    return;
  }
  
  const tooltipWidth = 250;
  let posX = event.clientX + 15;
  if (posX + tooltipWidth > window.innerWidth) {
    posX = event.clientX - tooltipWidth - 10;
  }
  
  activeTooltip.value.x = posX;
  activeTooltip.value.y = event.clientY + 15;
};
</script>

<template>
  <div class="relative overflow-hidden flex flex-col w-full bg-[#000a2b]">
    <div class="absolute inset-0 bg-[#0077ff] pointer-events-none dw3-beveled"></div>
    <div class="absolute inset-[1.5px] bg-[#000a2b] pointer-events-none dw3-beveled"></div>

    <div class="relative z-10 details-panel flex justify-center w-full p-4 text-white text-sm">
      <div class="flex gap-20 -ml-16">
        <div class="flex flex-col gap-1 w-24">
          <DigimonStatus 
            v-for="attr in computedAttributes" 
            :key="attr.status.digimonStatusType"
            :status="attr.status"
            :label="attr.label"
            :icon="attr.icon"
            :colorClass="attr.color"
            @showIconTooltip="showIconTooltip"
            @showMathTooltip="showMathTooltip"
            @moveTooltip="moveTooltip"
            @hideTooltip="hideTooltip"
          />
        </div>

        <div class="flex flex-col gap-1 w-24">
          <DigimonStatus 
            v-for="res in computedResistances" 
            :key="res.status.digimonStatusType"
            :status="res.status"
            :label="res.label"
            :icon="res.icon"
            :colorClass="res.color"
            @showIconTooltip="showIconTooltip"
            @showMathTooltip="showMathTooltip"
            @moveTooltip="moveTooltip"
            @hideTooltip="hideTooltip"
          />
        </div>
      </div>
    </div>

    <DigimonTooltip :activeTooltip="activeTooltip" />
  </div>
</template>
