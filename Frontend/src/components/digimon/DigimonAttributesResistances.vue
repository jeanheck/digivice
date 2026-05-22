<script setup lang="ts">
import { computed, ref } from 'vue';
import { useLocalization } from '@/composables/useLocalization';
import { AttributeType, ResistanceType } from '@/models';
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

const createEnrichedAttributeResistance = (
  key: AttributeType | ResistanceType,
  source: any,
  digiSource: any
): EnrichedAttributeResistance => {
  const val = source[key];
  const fromDigimon = (val && typeof val === 'object') ? (val.fromDigimon ?? 0) : Number(val ?? 0);
  const fromEquipaments = DigimonStatusCalculator.calculateBonusFromRawEquipments(key as unknown as AttributeType | ResistanceType, props.equipments);
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
    strength: createEnrichedAttributeResistance(AttributeType.strength, props.attributes, props.activeDigievolution?.attributes),
    defense: createEnrichedAttributeResistance(AttributeType.defense, props.attributes, props.activeDigievolution?.attributes),
    spirit: createEnrichedAttributeResistance(AttributeType.spirit, props.attributes, props.activeDigievolution?.attributes),
    wisdom: createEnrichedAttributeResistance(AttributeType.wisdom, props.attributes, props.activeDigievolution?.attributes),
    speed: createEnrichedAttributeResistance(AttributeType.speed, props.attributes, props.activeDigievolution?.attributes),
    charisma: createEnrichedAttributeResistance(AttributeType.charisma, props.attributes, props.activeDigievolution?.attributes),
  };
});

const enrichedResistances = computed<EnrichedResistances>(() => {
  return {
    fire: createEnrichedAttributeResistance(ResistanceType.fire, props.resistances, props.activeDigievolution?.resistances),
    water: createEnrichedAttributeResistance(ResistanceType.water, props.resistances, props.activeDigievolution?.resistances),
    ice: createEnrichedAttributeResistance(ResistanceType.ice, props.resistances, props.activeDigievolution?.resistances),
    wind: createEnrichedAttributeResistance(ResistanceType.wind, props.resistances, props.activeDigievolution?.resistances),
    thunder: createEnrichedAttributeResistance(ResistanceType.thunder, props.resistances, props.activeDigievolution?.resistances),
    machine: createEnrichedAttributeResistance(ResistanceType.machine, props.resistances, props.activeDigievolution?.resistances),
    dark: createEnrichedAttributeResistance(ResistanceType.dark, props.resistances, props.activeDigievolution?.resistances),
  };
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
            v-for="(attr, key) in enrichedAttributes" 
            :key="key"
            :enrichedAttributeResistance="attr"
            :property-key="key"
            @showIconTooltip="showIconTooltip"
            @showMathTooltip="showMathTooltip"
            @moveTooltip="moveTooltip"
            @hideTooltip="hideTooltip"
          />
        </div>

        <div class="flex flex-col gap-1 w-24">
          <DigimonAttributeResistance 
            v-for="(res, key) in enrichedResistances" 
            :key="key"
            :enrichedAttributeResistance="res"
            :property-key="key"
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
