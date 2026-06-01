<script setup lang="ts">
import { computed, ref } from "vue";
import { useLocalization } from "@/composables/useLocalization";
import type { Digimon } from "@/models";
import { Stat } from "@/models/stat";
import DigimonStat from "./DigimonStat.vue";
import DefaultTooltip from "@/components/tooltip/DefaultTooltip.vue";
import DigimonStatsTooltip from "./DigimonStatsTooltip.vue";
import { useTooltipPosition } from "@/composables/use-tooltip-position";
import { DigimonStatsPresenter } from "@/presenters/digimon-stats.presenter";

const props = defineProps<{
  digimon: Digimon;
}>();

const { t } = useLocalization();
const tooltipPlacement = "below" as const;
const tooltipPosition = useTooltipPosition();
const { x: tooltipX, y: tooltipY, showAt, move, hide } = tooltipPosition;

type TooltipVariant = "none" | "default" | "math";
const activeVariant = ref<TooltipVariant>("none");

const defaultTooltipContent = ref({ title: "", text: "" });
const mathTooltipContent = ref({ title: "", base: 0, equip: 0, total: 0 });

const statsViewModel = computed(() => {
  return DigimonStatsPresenter.getStatsViewModel(props.digimon);
});

const showIconTooltip = (event: MouseEvent, title: string, text: string) => {
  if (!text) {
    return;
  }

  defaultTooltipContent.value = { title, text };
  activeVariant.value = "default";
  showAt(event, { placement: tooltipPlacement });
};

const showStatIconTooltip = (event: MouseEvent, title: string, propertyKey: Stat) => {
  showIconTooltip(event, title, t(`stat.${propertyKey}-explanation`));
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
  showAt(event, { placement: tooltipPlacement });
};

const hideTooltip = () => {
  activeVariant.value = "none";
  hide();
};

const moveTooltip = (event: MouseEvent) => {
  move(event, tooltipPlacement);
};
</script>

<template>
  <div class="relative overflow-hidden flex flex-col w-full bg-[#000a2b]">
    <div class="absolute inset-0 bg-[#0077ff] pointer-events-none dw3-beveled"></div>
    <div class="absolute inset-[1.5px] bg-[#000a2b] pointer-events-none dw3-beveled"></div>

    <div class="relative z-10 details-panel flex justify-center w-full p-4 text-white text-sm">
      <div class="flex gap-20 -ml-16">
        <div class="flex flex-col gap-1 w-24">
          <DigimonStat
            v-for="(statViewModel, key) in statsViewModel.attributes"
            :key="key"
            :statViewModel="statViewModel"
            :property-key="key"
            @show-icon-tooltip="showStatIconTooltip"
            @show-math-tooltip="showMathTooltip"
            @move-tooltip="moveTooltip"
            @hide-tooltip="hideTooltip"
          />
        </div>

        <div class="flex flex-col gap-1 w-24">
          <DigimonStat
            v-for="(statViewModel, key) in statsViewModel.resistances"
            :key="key"
            :statViewModel="statViewModel"
            :property-key="key"
            @show-icon-tooltip="showStatIconTooltip"
            @show-math-tooltip="showMathTooltip"
            @move-tooltip="moveTooltip"
            @hide-tooltip="hideTooltip"
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
      placement="below"
    />

    <DigimonStatsTooltip
      :show="activeVariant === 'math'"
      :x="tooltipX"
      :y="tooltipY"
      :title="mathTooltipContent.title"
      :base="mathTooltipContent.base"
      :equip="mathTooltipContent.equip"
      :total="mathTooltipContent.total"
      placement="below"
    />
  </div>
</template>
