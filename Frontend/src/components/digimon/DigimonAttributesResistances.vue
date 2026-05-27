<script setup lang="ts">
import { computed, ref } from "vue";
import { useLocalization } from "@/composables/useLocalization";
import { AttributeType, ResistanceType } from "@/models";
import type { Attributes, Equipments, Resistances, EnrichedDigievolution, EnrichedAttributes, EnrichedResistances, EnrichedAttributeResistance } from "@/models";
import { DigimonStatusCalculator } from "@/logic/DigimonStatusCalculator";
import DigimonAttributeResistance from "@/components/digimon/DigimonAttributeResistance.vue";
import DefaultTooltip from "@/components/tooltip/DefaultTooltip.vue";
import DigimonTooltip from "@/components/tooltip/DigimonTooltip.vue";
import { useTooltipPosition } from "@/composables/use-tooltip-position";
import { DigimonAttributesResistancesPresenter } from "@/presenters/digimon-attributes-resistances.presenter";
import type { AttributeResistanceViewModel } from "@/viewmodels/attribute-resistance.viewmodel";
import type { AttributesViewModel } from "@/viewmodels/attributes.viewmodel";
import type { ResistancesViewModel } from "@/viewmodels/resistances.viewmodel";

const props = defineProps<{
  attributes: Attributes;
  resistances: Resistances;
  equipments: Equipments;
  activeDigievolutionId: number | null;
}>();

const { t } = useLocalization();
const tooltipPosition = useTooltipPosition();
const { x: tooltipX, y: tooltipY, showAt, move, hide } = tooltipPosition;

type TooltipVariant = "none" | "default" | "math";
const activeVariant = ref<TooltipVariant>("none");

const defaultTooltipContent = ref({ title: "", text: "" });
const mathTooltipContent = ref({ title: "", base: 0, equip: 0, total: 0 });

const activeDigievolution = computed(() => {
  return props.activeDigievolutionId ? DigimonAttributesResistancesPresenter.getDigievolutionById(props.activeDigievolutionId!) : null;
});

const createEnrichedAttributeResistance = (type: AttributeType | ResistanceType, fromDigimon: number, fromDigievolution: number): AttributeResistanceViewModel => {
  const fromEquipaments = DigimonStatusCalculator.calculateBonusFromRawEquipments(type as AttributeType | ResistanceType, props.equipments);

  return {
    fromDigimon,
    fromEquipaments,
    fromDigievolution,
    sumBetweenDigimonAndEquipaments: fromDigimon + fromEquipaments,
  };
};

const attributesViewModel = computed<AttributesViewModel>(() => {
  return {
    strength: createEnrichedAttributeResistance(AttributeType.strength, props.attributes.strength, activeDigievolution.value?.attributes?.strength ?? 0),
    defense: createEnrichedAttributeResistance(AttributeType.defense, props.attributes.defense, activeDigievolution.value?.attributes?.defense ?? 0),
    spirit: createEnrichedAttributeResistance(AttributeType.spirit, props.attributes.spirit, activeDigievolution.value?.attributes?.spirit ?? 0),
    wisdom: createEnrichedAttributeResistance(AttributeType.wisdom, props.attributes.wisdom, activeDigievolution.value?.attributes?.wisdom ?? 0),
    speed: createEnrichedAttributeResistance(AttributeType.speed, props.attributes.speed, activeDigievolution.value?.attributes?.speed ?? 0),
    charisma: createEnrichedAttributeResistance(AttributeType.charisma, props.attributes.charisma, activeDigievolution.value?.attributes?.charisma ?? 0),
  };
});

const resistancesViewModel = computed<ResistancesViewModel>(() => {
  return {
    fire: createEnrichedAttributeResistance(ResistanceType.fire, props.resistances.fire, activeDigievolution.value?.resistances?.fire ?? 0),
    water: createEnrichedAttributeResistance(ResistanceType.water, props.resistances.water, activeDigievolution.value?.resistances?.water ?? 0),
    ice: createEnrichedAttributeResistance(ResistanceType.ice, props.resistances.ice, activeDigievolution.value?.resistances?.ice ?? 0),
    wind: createEnrichedAttributeResistance(ResistanceType.wind, props.resistances.wind, activeDigievolution.value?.resistances?.wind ?? 0),
    thunder: createEnrichedAttributeResistance(ResistanceType.thunder, props.resistances.thunder, activeDigievolution.value?.resistances?.thunder ?? 0),
    machine: createEnrichedAttributeResistance(ResistanceType.machine, props.resistances.machine, activeDigievolution.value?.resistances?.machine ?? 0),
    dark: createEnrichedAttributeResistance(ResistanceType.dark, props.resistances.dark, activeDigievolution.value?.resistances?.dark ?? 0),
  };
});

const showIconTooltip = (event: MouseEvent, title: string, text: string) => {
  if (!text) {
    return;
  }

  defaultTooltipContent.value = { title, text };
  activeVariant.value = "default";
  showAt(event);
};

const showAttributeIconTooltip = (event: MouseEvent, title: string, propertyKey: string) => {
  showIconTooltip(event, title, t(`attribute.${propertyKey}-explanation`));
};

const showResistanceIconTooltip = (event: MouseEvent, title: string, propertyKey: string) => {
  showIconTooltip(event, title, t(`element.${propertyKey}-explanation`));
};

const showMathTooltip = (
  event: MouseEvent,
  title: string,
  base: number,
  equip: number,
  _digi: number,
  total: number
) => {
  mathTooltipContent.value = { title, base, equip, total };
  activeVariant.value = "math";
  showAt(event);
};

const hideTooltip = () => {
  activeVariant.value = "none";
  hide();
};

const moveTooltip = (event: MouseEvent) => {
  move(event);
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
            v-for="(attributeViewModel, key) in attributesViewModel" 
            :key="key"
            :enrichedAttributeResistance="attributeViewModel"
            :property-key="key"
            @showIconTooltip="showAttributeIconTooltip"
            @showMathTooltip="showMathTooltip"
            @moveTooltip="moveTooltip"
            @hideTooltip="hideTooltip"
          />
        </div>

        <div class="flex flex-col gap-1 w-24">
          <DigimonAttributeResistance 
            v-for="(resistanceViewModel, key) in resistancesViewModel" 
            :key="key"
            :enrichedAttributeResistance="resistanceViewModel"
            :property-key="key"
            @showIconTooltip="showResistanceIconTooltip"
            @showMathTooltip="showMathTooltip"
            @moveTooltip="moveTooltip"
            @hideTooltip="hideTooltip"
          />
        </div>
      </div>
    </div>

    <DefaultTooltip
      :show="activeVariant === 'default'"
      :x="tooltipX"
      :y="tooltipY"
      :title="defaultTooltipContent.title"
      :text="defaultTooltipContent.text"
    />

    <DigimonTooltip
      :show="activeVariant === 'math'"
      :x="tooltipX"
      :y="tooltipY"
      :title="mathTooltipContent.title"
      :base="mathTooltipContent.base"
      :equip="mathTooltipContent.equip"
      :total="mathTooltipContent.total"
    />
  </div>
</template>
