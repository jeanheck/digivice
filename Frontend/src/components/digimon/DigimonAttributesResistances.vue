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

const props = defineProps<{
  attributes: Attributes;
  resistances: Resistances;
  equipments: Equipments;
  activeDigievolution: EnrichedDigievolution | null;
}>();

const { t } = useLocalization();
const tooltipPosition = useTooltipPosition();
const { x: tooltipX, y: tooltipY, showAt, move, hide } = tooltipPosition;

type TooltipVariant = "none" | "default" | "math";
const activeVariant = ref<TooltipVariant>("none");

const defaultTooltipContent = ref({ title: "", text: "" });
const mathTooltipContent = ref({ title: "", base: 0, equip: 0, total: 0 });

const createEnrichedAttributeResistance = (
  key: AttributeType | ResistanceType,
  source: any,
  digiSource: any
): EnrichedAttributeResistance => {
  const val = source[key];

  const fromDigimon = (val && typeof val === "object") ? (val.fromDigimon ?? 0) : Number(val ?? 0);
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
    strength: createEnrichedAttributeResistance(AttributeType.Strength, props.attributes, props.activeDigievolution?.attributes),
    defense: createEnrichedAttributeResistance(AttributeType.Defense, props.attributes, props.activeDigievolution?.attributes),
    spirit: createEnrichedAttributeResistance(AttributeType.Spirit, props.attributes, props.activeDigievolution?.attributes),
    wisdom: createEnrichedAttributeResistance(AttributeType.Wisdom, props.attributes, props.activeDigievolution?.attributes),
    speed: createEnrichedAttributeResistance(AttributeType.Speed, props.attributes, props.activeDigievolution?.attributes),
    charisma: createEnrichedAttributeResistance(AttributeType.Charisma, props.attributes, props.activeDigievolution?.attributes),
  };
});

const enrichedResistances = computed<EnrichedResistances>(() => {
  return {
    fire: createEnrichedAttributeResistance(ResistanceType.Fire, props.resistances, props.activeDigievolution?.resistances),
    water: createEnrichedAttributeResistance(ResistanceType.Water, props.resistances, props.activeDigievolution?.resistances),
    ice: createEnrichedAttributeResistance(ResistanceType.Ice, props.resistances, props.activeDigievolution?.resistances),
    wind: createEnrichedAttributeResistance(ResistanceType.Wind, props.resistances, props.activeDigievolution?.resistances),
    thunder: createEnrichedAttributeResistance(ResistanceType.Thunder, props.resistances, props.activeDigievolution?.resistances),
    machine: createEnrichedAttributeResistance(ResistanceType.Machine, props.resistances, props.activeDigievolution?.resistances),
    dark: createEnrichedAttributeResistance(ResistanceType.Dark, props.resistances, props.activeDigievolution?.resistances),
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
            v-for="(attr, key) in enrichedAttributes" 
            :key="key"
            :enrichedAttributeResistance="attr"
            :property-key="key"
            @showIconTooltip="showAttributeIconTooltip"
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
