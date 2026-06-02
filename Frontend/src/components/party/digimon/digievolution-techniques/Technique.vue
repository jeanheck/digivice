<script setup lang="ts">
import { computed, ref } from "vue";
import Tooltip from "@/components/tooltip/Tooltip.vue";
import { useI18n } from "vue-i18n";
import { useTooltipPosition } from "@/composables/use-tooltip-position";
import type { TechniqueViewModel } from "@/viewmodels/digievolution/technique.viewmodel";
import type { Constant } from "@/constants/constant";
import { IconConstant } from "@/constants/icon.constant";

const props = defineProps<{
  technique: TechniqueViewModel;
}>();

const { t } = useI18n();

function getElementLabel(element: string): string {
  if (element.toLowerCase() === "none") {
    return t("digievolution.neutral");
  }

  return t(`stat.${element.toLowerCase()}`);
}

const tooltipPlacement = "below" as const;
const tooltipPosition = useTooltipPosition(150);
const { show: tooltipShow, x: tooltipX, y: tooltipY, showAt, move, hide } = tooltipPosition;
const tooltipTitle = ref("");

const showTypeTooltip = (event: MouseEvent, type: string) => {
  const normalizedType = (type || "physical").toLowerCase();
  tooltipTitle.value = t(`techniqueTypes.${normalizedType}`);
  showAt(event, { maxWidth: 150, placement: tooltipPlacement });
};

const hideTypeTooltip = () => {
  hide();
};

const moveTypeTooltip = (event: MouseEvent) => {
  move(event, tooltipPlacement);
};

const icon = computed(() => {
  return IconConstant[props.technique.type as Constant];
});

const TechniqueElementColorClass: Record<string, string> = {
    fire: "text-orange-400",
    water: "text-blue-400",
    ice: "text-cyan-300",
    wind: "text-gray-300",
    thunder: "text-yellow-300",
    dark: "text-purple-400",
    machine: "text-gray-400",
    none: "text-white/60",
};

function getTechniqueElementColorClass(element: string): string {
    const normalizedElement = element.toLowerCase();

    return TechniqueElementColorClass[normalizedElement] ?? "text-white/60";
}
</script>

<template>
  <div
    class="relative rounded px-3 py-2 flex items-start gap-3 border transition-all text-xs"
    :class="{
      'bg-[#000e1f]/50 border-[#0033aa]/20 opacity-50': !technique.isUnlocked,
      'bg-yellow-950/30 border-yellow-500/60 shadow-[0_0_8px_rgba(234,179,8,0.2)]': technique.isSignature && technique.isUnlocked,
      'bg-[#001a33]/60 border-[#0055ff]/40': !technique.isSignature && technique.isUnlocked,
    }"
  >
    <span
      v-if="technique.isSignature"
      class="absolute top-1 right-2 text-[10px] text-yellow-400 font-bold tracking-widest"
    >
      ⭐
    </span>

    <span
      class="text-base leading-none mt-px shrink-0 cursor-help tooltip-anchor"
      @mouseenter="showTypeTooltip($event, technique.type)"
      @mousemove="moveTypeTooltip"
      @mouseleave="hideTypeTooltip"
    >
      {{ icon }}
    </span>

    <div class="flex-1 min-w-0">
      <div class="flex items-center gap-1 mb-0.5">
        <span
          class="font-bold tracking-wide"
          :class="technique.isSignature ? 'text-yellow-300' : 'text-white'"
        >
          {{ t(`technique.${technique.id}.name`) }}
        </span>
        <span class="text-[10px] text-cyan-400/80 ml-1">{{ t("digievolution.lv") }}.{{ technique.learnLevel }}</span>
      </div>

      <p class="text-white/50 text-[10px] leading-snug">{{ t(`technique.${technique.id}.description`) }}</p>

      <div class="flex gap-3 mt-1 text-[9px] uppercase tracking-wider">
        <span :class="getTechniqueElementColorClass(technique.element)">
          {{ getElementLabel(technique.element) }}
        </span>
        <span class="text-blue-300/70">{{ t("digievolution.mpCost") }}: {{ technique.mp }}</span>
      </div>
    </div>

    <Tooltip
      :show="tooltipShow"
      :x="tooltipX"
      :y="tooltipY"
      :title="tooltipTitle"
      :max-width="150"
      placement="below"
    />
  </div>
</template>
