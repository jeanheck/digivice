<script setup lang="ts">
import { useI18n } from "vue-i18n";
import Tooltip from "@/ui/components/tooltip/Tooltip.vue";
import type { TooltipPlacement } from "@/ui/composables/use-tooltip-position";
import type { AvailableDnaFusionViewModel } from "@/presentation/viewmodels/dna-digievolution/available-dna-fusion.viewmodel";

withDefaults(
  defineProps<{
    show: boolean;
    x: number;
    y: number;
    fusions: AvailableDnaFusionViewModel[];
    maxWidth?: number;
    placement?: TooltipPlacement;
  }>(),
  {
    maxWidth: 460,
    placement: "below",
  },
);

const { t } = useI18n();
</script>

<template>
  <Tooltip
    :show="show && fusions.length > 0"
    :x="x"
    :y="y"
    :title="t('dnaDigievolution.availableTitle')"
    :max-width="maxWidth"
    :placement="placement"
  >
    <div class="flex flex-col w-full">
      <div
        v-for="(fusion, fusionIndex) in fusions"
        :key="fusion.dnaDigievolution.id"
        class="flex flex-col gap-2"
        :class="{ 'mt-2 pt-2 border-t border-[#0066cc]/50': fusionIndex > 0 }"
      >
        <div class="flex items-baseline justify-between gap-3">
          <span class="text-dw3-dna shadow-text-dark font-bold tracking-wider text-xs">
            {{ fusion.dnaDigievolution.name }}
          </span>
          <span class="text-[10px] text-blue-300 tracking-wider whitespace-nowrap">
            {{ t("digimon.mp") }}: {{ fusion.dnaDigievolution.mp }}
          </span>
        </div>

        <div class="flex flex-col gap-0.5">
          <span
            v-for="partner in fusion.partners"
            :key="`${partner.digimonName}-${partner.digievolutionName}`"
            class="text-gray-200 text-[11px] leading-tight"
          >
            {{
              t("dnaDigievolution.tagWith", {
                digievolution: partner.digievolutionName,
                digimon: partner.digimonName,
              })
            }}
          </span>
        </div>

        <div class="flex items-center gap-4 text-[10px] uppercase tracking-wider text-white">
          <span class="flex items-center gap-1.5">
            <img
              v-if="fusion.dnaDigievolution.typeImageUrl"
              :src="fusion.dnaDigievolution.typeImageUrl"
              :alt="t(fusion.dnaDigievolution.typeKey)"
              class="w-[19px] h-5 object-contain rendering-pixelated"
            />
            {{ t(fusion.dnaDigievolution.typeKey) }}
          </span>
          <span
            v-if="fusion.dnaDigievolution.elementKey !== null"
            class="flex items-center gap-1.5"
          >
            <img
              v-if="fusion.dnaDigievolution.elementImageUrl"
              :src="fusion.dnaDigievolution.elementImageUrl"
              :alt="t(fusion.dnaDigievolution.elementKey)"
              class="w-5 h-5 object-contain rendering-pixelated"
            />
            {{ t(fusion.dnaDigievolution.elementKey) }}
          </span>
        </div>

        <p
          v-if="fusion.dnaDigievolution.effectKey !== null"
          class="text-[10px] leading-tight text-white/85"
        >
          {{ t(fusion.dnaDigievolution.effectKey) }}
        </p>
      </div>
    </div>
  </Tooltip>
</template>
