<script setup lang="ts">
import { computed, ref } from "vue";
import Tooltip from "@/ui/components/tooltip/Tooltip.vue";
import { useI18n } from "vue-i18n";
import {
  useTooltipPosition,
  type TooltipHorizontalAlign,
} from "@/ui/composables/use-tooltip-position";
import type { TechniqueViewModel } from "@/presentation/viewmodels/digievolution/technique.viewmodel";
import type { TechniqueType } from "@/shared/types/technique-type.type";
import { ImageCatalog } from "@/ui/catalogs/image.catalog";

const props = withDefaults(
  defineProps<{
    technique: TechniqueViewModel;
    tooltipAlign?: TooltipHorizontalAlign;
  }>(),
  {
    tooltipAlign: "right",
  },
);

const { t } = useI18n();

function getElementLabel(element: string): string {
  if (element.toLowerCase() === "none") {
    return t("digievolution.neutral");
  }

  return t(`stat.${element.toLowerCase()}`);
}

const tooltipPlacement = "below" as const;
const tooltipPosition = useTooltipPosition();
const { show: tooltipShow, x: tooltipX, y: tooltipY, showAt, move, hide } = tooltipPosition;
const tooltipTitle = ref("");

const showTooltip = (event: MouseEvent, title: string) => {
  tooltipTitle.value = title;
  showAt(event, { placement: tooltipPlacement, align: props.tooltipAlign });
};

const showTypeTooltip = (event: MouseEvent, type: TechniqueType) => {
  showTooltip(event, t(`techniqueTypes.${type}`));
};

const showBadgeTooltip = (event: MouseEvent, i18nKey: string) => {
  showTooltip(event, t(i18nKey));
};

const hideTooltip = () => {
  hide();
};

const moveTooltip = (event: MouseEvent) => {
  move(event, tooltipPlacement);
};

const iconUrl = computed(() => {
  return ImageCatalog.getTechniqueTypeImageUrl(props.technique.type);
});

const TechniqueElementColorClass: Record<string, string> = {
  fire: "text-orange-400 in-[[data-theme=seabed]]:text-orange-600",
  water: "text-blue-400 in-[[data-theme=seabed]]:text-blue-600",
  ice: "text-cyan-300 in-[[data-theme=seabed]]:text-cyan-600",
  wind: "text-gray-300 in-[[data-theme=seabed]]:text-gray-600",
  thunder: "text-yellow-300 in-[[data-theme=seabed]]:text-yellow-600",
  dark: "text-purple-400 in-[[data-theme=seabed]]:text-purple-600",
  machine: "text-gray-400 in-[[data-theme=seabed]]:text-gray-600",
  none: "text-ink-muted",
};

const UnlockedTechniqueElementColorClass: Record<string, string> = {
  fire: "text-orange-300 in-[[data-theme=seabed]]:text-orange-600",
  water: "text-blue-300 in-[[data-theme=seabed]]:text-blue-600",
  ice: "text-cyan-200 in-[[data-theme=seabed]]:text-cyan-600",
  wind: "text-gray-200 in-[[data-theme=seabed]]:text-gray-600",
  thunder: "text-yellow-200 in-[[data-theme=seabed]]:text-yellow-600",
  dark: "text-purple-300 in-[[data-theme=seabed]]:text-purple-600",
  machine: "text-gray-300 in-[[data-theme=seabed]]:text-gray-600",
  none: "text-ink-muted/70",
};

function getTechniqueElementColorClass(element: string): string {
  const normalizedElement = element.toLowerCase();
  const colorMap = props.technique.isUnlocked
    ? UnlockedTechniqueElementColorClass
    : TechniqueElementColorClass;

  return colorMap[normalizedElement] ?? (props.technique.isUnlocked ? "text-ink/80" : "text-ink-muted");
}
</script>

<template>
  <div
    class="relative rounded px-3 py-2 flex items-start gap-3 border transition-all text-xs"
    :class="{
      'bg-surface-deep/50 border-frame/25 opacity-50': !technique.isUnlocked,
      'bg-yellow-950/40 border-yellow-400/70 shadow-[0_0_10px_rgba(234,179,8,0.3)] in-[[data-theme=seabed]]:bg-yellow-100/80 in-[[data-theme=seabed]]:border-yellow-500/70 in-[[data-theme=seabed]]:shadow-[0_0_10px_rgba(234,179,8,0.2)]':
        technique.isSignature && technique.isUnlocked,
      'bg-panel/80 border-accent': !technique.isSignature && technique.isUnlocked,
    }"
  >
    <div class="absolute top-1 right-2 flex flex-col items-end gap-0.5 font-bold tracking-widest">
      <span
        class="cursor-help whitespace-nowrap flex items-center gap-0.5 text-[9px]"
        @mouseenter="showBadgeTooltip($event, 'digievolution.levelToLearn')"
        @mousemove="moveTooltip"
        @mouseleave="hideTooltip"
      >
        <span
          class="text-orange-200 leading-none inline-flex text-[10px] translate-y-px text-outline-black"
          >{{ technique.learnAt }}</span
        >
        <span class="font-emoji-color leading-none inline-flex text-[14px]">📜</span>
      </span>

      <span
        v-if="technique.loadAt !== null"
        class="cursor-help whitespace-nowrap flex items-center gap-0.5 text-[9px]"
        @mouseenter="showBadgeTooltip($event, 'digievolution.levelToLoad')"
        @mousemove="moveTooltip"
        @mouseleave="hideTooltip"
      >
        <span
          class="text-amber-300 leading-none inline-flex text-[10px] translate-y-px text-outline-black"
          >{{ technique.loadAt }}</span
        >
        <span class="font-emoji-color leading-none inline-flex text-[14px]">⏳</span>
      </span>

      <span
        v-else-if="technique.isSignature"
        class="cursor-help font-emoji-color text-[14px] leading-none"
        @mouseenter="showBadgeTooltip($event, 'digievolution.signatureTechnique')"
        @mousemove="moveTooltip"
        @mouseleave="hideTooltip"
        >⭐</span
      >
    </div>

    <span
      class="leading-none mt-px shrink-0 cursor-help tooltip-anchor"
      @mouseenter="showTypeTooltip($event, technique.type)"
      @mousemove="moveTooltip"
      @mouseleave="hideTooltip"
    >
      <img
        v-if="iconUrl"
        :src="iconUrl"
        :alt="t(`techniqueTypes.${technique.type}`)"
        class="w-6.25 h-6.75 object-contain rendering-pixelated"
      />
    </span>

    <div class="flex-1 min-w-0">
      <div class="mb-0.5">
        <span
          class="font-bold tracking-wide"
          :class="
            technique.isUnlocked
              ? technique.isSignature
                ? 'text-yellow-200 in-[[data-theme=seabed]]:text-yellow-700'
                : 'text-ink'
              : technique.isSignature
                ? 'text-yellow-300 in-[[data-theme=seabed]]:text-yellow-700'
                : 'text-ink-muted'
          "
        >
          {{ t(`technique.${technique.id}.name`) }}
        </span>
      </div>

      <div class="flex gap-3 mt-1 text-[9px] uppercase tracking-wider">
        <span :class="getTechniqueElementColorClass(technique.element)">
          {{ getElementLabel(technique.element) }}
        </span>
        <span
          :class="
            technique.isUnlocked ? 'text-accent-bright' : 'text-accent-bright/70'
          "
          >{{ t("digievolution.mpCost") }}: {{ technique.mp }}</span
        >
      </div>

      <p
        class="text-[10px] leading-tight min-h-5.5"
        :class="
          technique.isUnlocked ? 'text-ink/85' : 'text-ink-muted/55'
        "
      >
        {{ t(`technique.${technique.id}.description`) }}
      </p>
    </div>

    <Tooltip
      :show="tooltipShow"
      :x="tooltipX"
      :y="tooltipY"
      :title="tooltipTitle"
      placement="below"
      :horizontal-align="tooltipAlign"
    />
  </div>
</template>
