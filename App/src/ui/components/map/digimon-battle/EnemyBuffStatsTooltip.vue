<script setup lang="ts">
import { computed } from "vue";
import Tooltip from "@/ui/components/tooltip/Tooltip.vue";
import type { TooltipPlacement } from "@/ui/composables/use-tooltip-position";
import { useI18n } from "vue-i18n";

const props = withDefaults(
  defineProps<{
    show: boolean;
    x: number;
    y: number;
    title: string;
    base: number;
    delta: number;
    total: number;
    placement?: TooltipPlacement;
  }>(),
  {
    placement: "below",
  },
);

const { t } = useI18n();

const isBuff = computed(() => {
  return props.delta > 0;
});

const deltaMagnitude = computed(() => {
  return Math.abs(props.delta);
});

const modifierColorClass = computed(() => {
  if (isBuff.value) {
    return "text-green-400";
  }

  return "text-red-400";
});
</script>

<template>
  <Tooltip :show="show" :x="x" :y="y" :title="title" size="tiny" :placement="placement">
    <div class="flex flex-col w-full">
      <div
        class="text-white text-[10px] font-bold text-center mb-1 tracking-wider shadow-text whitespace-nowrap"
      >
        {{ total }}
        <span class="text-[8px] text-gray-400 tracking-normal ml-1">
          (<span class="text-white">{{ base }}</span>
          <template v-if="isBuff"> + </template>
          <template v-else> − </template>
          <span class="font-bold" :class="modifierColorClass">{{ deltaMagnitude }}</span
          >)
        </span>
      </div>

      <div class="flex flex-col gap-0.5">
        <div class="flex justify-between text-[9px] items-center">
          <span class="text-white">{{ t("digimon.baseDigimon") }}</span>
        </div>
        <div class="flex justify-between text-[9px] items-center">
          <span class="font-bold" :class="modifierColorClass">{{ t("enemy.techniques") }}</span>
        </div>
      </div>
    </div>
  </Tooltip>
</template>
