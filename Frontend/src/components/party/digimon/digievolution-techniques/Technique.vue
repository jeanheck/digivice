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

const UnlockedTechniqueElementColorClass: Record<string, string> = {
    fire: "text-orange-300",
    water: "text-blue-300",
    ice: "text-cyan-200",
    wind: "text-gray-200",
    thunder: "text-yellow-200",
    dark: "text-purple-300",
    machine: "text-gray-300",
    none: "text-white/40",
};

function getTechniqueElementColorClass(element: string): string {
    const normalizedElement = element.toLowerCase();
    const colorMap = props.technique.isUnlocked
        ? UnlockedTechniqueElementColorClass
        : TechniqueElementColorClass;

    return colorMap[normalizedElement] ?? (props.technique.isUnlocked ? "text-white/80" : "text-white/60");
}
</script>

<template>
  <div
    class="relative rounded px-3 py-2 flex items-start gap-3 border transition-all text-xs"
    :class="{
      'bg-[#000e1f]/50 border-[#0033aa]/20 opacity-50': !technique.isUnlocked,
      'bg-yellow-950/40 border-yellow-400/70 shadow-[0_0_10px_rgba(234,179,8,0.3)]': technique.isSignature && technique.isUnlocked,
      'bg-[#001a33]/80 border-[#0077ff]/55': !technique.isSignature && technique.isUnlocked,
    }"
  >
    <span
      v-if="technique.isSignature"
      class="absolute top-1 right-2 text-[10px] font-bold tracking-widest"
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
      <div class="mb-0.5">
        <span
          class="font-bold tracking-wide"
          :class="
            technique.isUnlocked
              ? technique.isSignature ? 'text-yellow-200' : 'text-white'
              : technique.isSignature ? 'text-yellow-300' : 'text-white'
          "
        >
          {{ t(`technique.${technique.id}.name`) }}
        </span>
      </div>

      <div
        class="flex gap-3 mb-0.5 text-[10px]"
        :class="technique.isUnlocked ? 'text-cyan-500' : 'text-cyan-400/80'"
      >
        <span>{{ t("digievolution.learnAt") }}: {{ t("digievolution.lv") }}.{{ technique.learnLevel }}</span>
        <span v-if="technique.loadedLevel !== null">| {{ t("digievolution.loadAt") }}: {{ t("digievolution.lv") }}.{{ technique.loadedLevel }}</span>
      </div>

      <div class="flex gap-3 mt-1 text-[9px] uppercase tracking-wider">
        <span :class="getTechniqueElementColorClass(technique.element)">
          {{ getElementLabel(technique.element) }}
        </span>
        <span :class="technique.isUnlocked ? 'text-blue-300' : 'text-blue-300/70'">{{ t("digievolution.mpCost") }}: {{ technique.mp }}</span>
      </div>

      <p
        class="text-[10px] leading-tight min-h-5.5"
        :class="technique.isUnlocked ? 'text-white/85' : 'text-white/50'"
      >{{ t(`technique.${technique.id}.description`) }}</p>
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
