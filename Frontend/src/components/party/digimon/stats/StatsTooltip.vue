<script setup lang="ts">
import Tooltip from "@/components/tooltip/Tooltip.vue";
import type { TooltipPlacement } from "@/composables/use-tooltip-position";
import { useI18n } from "vue-i18n";

withDefaults(
    defineProps<{
        show: boolean;
        x: number;
        y: number;
        title: string;
        base: number;
        equip: number;
        total: number;
        maxWidth?: number;
        placement?: TooltipPlacement;
    }>(),
    {
        maxWidth: 250,
        placement: "below"
    }
);

const { t } = useI18n();
</script>

<template>
  <Tooltip
    :show="show"
    :x="x"
    :y="y"
    :title="title"
    :max-width="maxWidth"
    :placement="placement"
  >
    <div class="flex flex-col w-full min-w-42.5">
      <div class="text-white text-base font-bold text-center mb-2 tracking-wider shadow-text whitespace-nowrap">
        {{ total }}
        <span class="text-[10px] text-gray-400 tracking-normal ml-1">
          (<span class="text-white">{{ base }}</span> + <span class="text-[#0077ff] font-bold">{{ equip }}</span>)
        </span>
      </div>

      <div class="flex flex-col gap-0.5">
        <div class="flex justify-between text-xs items-center">
          <span class="text-white">{{ t("digimon.baseDigimon") }}</span>
        </div>
        <div class="flex justify-between text-xs items-center">
          <span class="text-[#0077ff] font-bold">{{ t("digimon.equipments") }}</span>
        </div>
      </div>
    </div>
  </Tooltip>
</template>
