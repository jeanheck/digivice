<script setup lang="ts">
import Tooltip from "@/components/tooltip/Tooltip.vue";
import { useLocalization } from "@/composables/useLocalization";

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
    }>(),
    {
        maxWidth: 250
    }
);

const { t } = useLocalization();
</script>

<template>
  <Tooltip
    :show="show"
    :x="x"
    :y="y"
    :title="title"
    :max-width="maxWidth"
    placement="below"
  >
    <div class="flex flex-col w-full min-w-[170px]">
      <div class="text-white text-base font-bold text-center mb-2 tracking-wider shadow-text whitespace-nowrap">
        {{ total }}
        <span class="text-[10px] text-gray-400 tracking-normal ml-1">
          (<span class="text-white">{{ base }}</span> + <span class="text-[#0077ff] font-bold">{{ equip }}</span>)
        </span>
      </div>

      <div class="flex flex-col gap-[2px]">
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
