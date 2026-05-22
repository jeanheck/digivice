<script setup lang="ts">
import { computed, ref } from 'vue';
import { useLocalization } from '@/composables/useLocalization';
import { DigimonStatusType } from '@/models';
import type { Attributes, Equipments, Resistances, EnrichedDigievolution, EnrichedAttributes, EnrichedResistances, EnrichedAttributeResistance } from '@/models';
import { DigimonStatusCalculator } from '@/logic/DigimonStatusCalculator';
import DigimonAttributeResistance from '@/components/digimon/DigimonAttributeResistance.vue';
import DigimonTooltip from '@/components/digimon/DigimonTooltip.vue';

const props = defineProps<{
  attributes: Attributes;
  resistances: Resistances;
  equipments: Equipments;
  activeDigievolution: EnrichedDigievolution | null;
}>();

const { t } = useLocalization();

const buildEnrichedVal = (
  key: DigimonStatusType,
  source: any,
  digiSource: any
): EnrichedAttributeResistance => {
  const val = source[key];
  const fromDigimon = (val && typeof val === 'object') ? (val.fromDigimon ?? 0) : Number(val ?? 0);
  const fromEquipaments = DigimonStatusCalculator.calculateBonusFromRawEquipments(key, props.equipments);
  const fromDigievolution = digiSource ? Number(digiSource[key] ?? 0) : 0;
  return {
    fromDigimon,
    fromEquipaments,
    fromDigievolution,
    sumBetweenDigimonAndEquipaments: fromDigimon + fromEquipaments,
  };
};

const enrichedAttributes = computed<EnrichedAttributes>(() => {
  return {
    strength: buildEnrichedVal(DigimonStatusType.strength, props.attributes, props.activeDigievolution?.attributes),
    defense: buildEnrichedVal(DigimonStatusType.defense, props.attributes, props.activeDigievolution?.attributes),
    spirit: buildEnrichedVal(DigimonStatusType.spirit, props.attributes, props.activeDigievolution?.attributes),
    wisdom: buildEnrichedVal(DigimonStatusType.wisdom, props.attributes, props.activeDigievolution?.attributes),
    speed: buildEnrichedVal(DigimonStatusType.speed, props.attributes, props.activeDigievolution?.attributes),
    charisma: buildEnrichedVal(DigimonStatusType.charisma, props.attributes, props.activeDigievolution?.attributes),
  };
});

const enrichedResistances = computed<EnrichedResistances>(() => {
  return {
    fire: buildEnrichedVal(DigimonStatusType.fire, props.resistances, props.activeDigievolution?.resistances),
    water: buildEnrichedVal(DigimonStatusType.water, props.resistances, props.activeDigievolution?.resistances),
    ice: buildEnrichedVal(DigimonStatusType.ice, props.resistances, props.activeDigievolution?.resistances),
    wind: buildEnrichedVal(DigimonStatusType.wind, props.resistances, props.activeDigievolution?.resistances),
    thunder: buildEnrichedVal(DigimonStatusType.thunder, props.resistances, props.activeDigievolution?.resistances),
    machine: buildEnrichedVal(DigimonStatusType.machine, props.resistances, props.activeDigievolution?.resistances),
    dark: buildEnrichedVal(DigimonStatusType.dark, props.resistances, props.activeDigievolution?.resistances),
  };
});

const computedAttributes = computed(() => {
  const attrs = enrichedAttributes.value;
  return [
    { label: t('attributes.strength'), status: attrs.strength, icon: '👊', color: 'text-[#fcd883]', key: 'strength' },
    { label: t('attributes.defense'), status: attrs.defense, icon: '🛡️', color: 'text-gray-400', key: 'defense' },
    { label: t('attributes.spirit'), status: attrs.spirit, icon: '🧙‍♂️', color: 'text-pink-300', key: 'spirit' },
    { label: t('attributes.wisdom'), status: attrs.wisdom, icon: '📖', color: 'text-yellow-600', key: 'wisdom' },
    { label: t('attributes.speed'), status: attrs.speed, icon: '🏃', color: 'text-green-400', key: 'speed' },
    { label: t('attributes.charisma'), status: attrs.charisma, icon: '✨', color: 'text-yellow-300', key: 'charisma' },
  ];
});

const computedResistances = computed(() => {
  const res = enrichedResistances.value;
  return [
    { label: t('resistances.fire'), status: res.fire, icon: '🔥', color: 'text-orange-500', key: 'fire' },
    { label: t('resistances.water'), status: res.water, icon: '💧', color: 'text-blue-400', key: 'water' },
    { label: t('resistances.ice'), status: res.ice, icon: '🧊', color: 'text-cyan-200', key: 'ice' },
    { label: t('resistances.wind'), status: res.wind, icon: '🍃', color: 'text-gray-100', key: 'wind' },
    { label: t('resistances.thunder'), status: res.thunder, icon: '⚡', color: 'text-[#ffffcc]', key: 'thunder' },
    { label: t('resistances.machine'), status: res.machine, icon: '⚙️', color: 'text-gray-500', key: 'machine' },
    { label: t('resistances.dark'), status: res.dark, icon: '🌑', color: 'text-purple-500', key: 'dark' },
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
          <DigimonAttributeResistance 
            v-for="attr in computedAttributes" 
            :key="attr.key"
            :enrichedAttributeResistance="attr.status"
            :property-key="attr.key"
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
          <DigimonAttributeResistance 
            v-for="res in computedResistances" 
            :key="res.key"
            :enrichedAttributeResistance="res.status"
            :property-key="res.key"
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
