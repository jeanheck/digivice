<script setup lang="ts">
import { computed } from "vue";
import Tooltip from "@/components/tooltip/Tooltip.vue";
import type { TooltipPlacement } from "@/composables/use-tooltip-position";
import { useI18n } from "vue-i18n";

const props = withDefaults(
  defineProps<{
    show: boolean;
    x: number;
    y: number;
    title: string;
    base: number;
    equip: number;
    total: number;
    battleDelta?: number;
    maxWidth?: number;
    placement?: TooltipPlacement;
  }>(),
  {
    battleDelta: 0,
    maxWidth: 250,
    placement: "below",
  },
);

const { t } = useI18n();

const hasBattleDelta = computed(() => {
  return props.battleDelta !== 0;
});

const isBuff = computed(() => {
  return props.battleDelta > 0;
});

const deltaMagnitude = computed(() => {
  return Math.abs(props.battleDelta);
});

const modifierColorClass = computed(() => {
  if (isBuff.value) {
    return "text-green-400";
  }

  return "text-red-400";
});

const modifierLabel = computed(() => {
  if (isBuff.value) {
    return t("enemy.buff");
  }

  return t("enemy.debuff");
});
</script>

<template>
  <Tooltip :show="show" :x="x" :y="y" :title="title" :max-width="maxWidth" :placement="placement">
    <div class="flex flex-col w-full min-w-42.5">
      <div
        class="text-white text-base font-bold text-center mb-2 tracking-wider shadow-text whitespace-nowrap"
      >
        {{ total }}
        <span class="text-[10px] text-gray-400 tracking-normal ml-1">
          (<span class="text-white">{{ base }}</span> +
          <span class="text-[#0077ff] font-bold">{{ equip }}</span>
          <template v-if="hasBattleDelta">
            <template v-if="isBuff"> + </template>
            <template v-else> − </template>
            <span class="font-bold" :class="modifierColorClass">{{ deltaMagnitude }}</span>
          </template>)
        </span>
      </div>

      <div class="flex flex-col gap-0.5">
        <div class="flex justify-between text-xs items-center">
          <span class="text-white">{{ t("digimon.baseDigimon") }}</span>
        </div>
        <div class="flex justify-between text-xs items-center">
          <span class="text-[#0077ff] font-bold">{{ t("digimon.equipments") }}</span>
        </div>
        <div v-if="hasBattleDelta" class="flex justify-between text-xs items-center">
          <span class="font-bold" :class="modifierColorClass">{{ modifierLabel }}</span>
        </div>
      </div>
    </div>
  </Tooltip>
</template>
