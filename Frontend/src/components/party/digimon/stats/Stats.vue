<script setup lang="ts">
import { computed, ref } from "vue";
import { useI18n } from "vue-i18n";
import type { Digimon } from "@/models";
import { Constant } from "@/constants/constant.ts";
import Stat from "./Stat.vue";
import DefaultTooltip from "@/components/tooltip/DefaultTooltip.vue";
import StatsTooltip from "./StatsTooltip.vue";
import { useTooltipPosition } from "@/composables/use-tooltip-position";
import { StatsPresenter } from "@/presenters/stats.presenter.ts";

const props = defineProps<{
  digimon: Digimon;
}>();

const { t } = useI18n();
const tooltipPlacement = "below" as const;
const tooltipPosition = useTooltipPosition();
const { x: tooltipX, y: tooltipY, showAt, move, hide } = tooltipPosition;

type TooltipVariant = "none" | "default" | "math";
const activeVariant = ref<TooltipVariant>("none");

const defaultTooltipContent = ref({ title: "", text: "" });
const mathTooltipContent = ref({ title: "", base: 0, equip: 0, total: 0 });

const statsViewModel = computed(() => {
  return StatsPresenter.getStatsViewModel(props.digimon);
});

const showIconTooltip = (event: MouseEvent, title: string, text: string) => {
  if (!text) {
    return;
  }

  defaultTooltipContent.value = { title, text };
  activeVariant.value = "default";
  showAt(event, { placement: tooltipPlacement });
};

const showStatIconTooltip = (event: MouseEvent, title: string, propertyKey: Constant) => {
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
  <div class="dw3-panel flex flex-col">
    <div class="dw3-panel-border dw3-beveled"></div>
    <div class="dw3-panel-inner dw3-beveled"></div>

    <div class="dw3-panel-content details-panel flex justify-center w-full p-4 text-white text-sm">
      <div class="flex gap-20 -ml-16">
        <div class="flex flex-col gap-1 w-24">
          <Stat
            v-for="(statViewModel, key) in statsViewModel.attributes"
            :key="key"
            :stat-view-model="statViewModel"
            :stat="key"
            @show-icon-tooltip="showStatIconTooltip"
            @show-math-tooltip="showMathTooltip"
            @move-tooltip="moveTooltip"
            @hide-tooltip="hideTooltip"
          />
        </div>

        <div class="flex flex-col gap-1 w-24">
          <Stat
            v-for="(statViewModel, key) in statsViewModel.resistances"
            :key="key"
            :stat-view-model="statViewModel"
            :stat="key"
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

    <StatsTooltip
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
