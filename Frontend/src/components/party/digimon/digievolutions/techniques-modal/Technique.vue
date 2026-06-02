<script setup lang="ts">
import { computed } from "vue";
import { TechniquePresenter } from "@/presenters/technique.presenter";
import type { DigievolutionTechniqueViewModel } from "@/viewmodels/digievolution/digievolution-technique.viewmodel";
import { IconConstant } from "@/constants/icon.constant";
import type { Constant } from "@/constants/constant";

const props = defineProps<{
  technique: DigievolutionTechniqueViewModel;
  digievolutionLevel: number;
  isSignature: boolean;
}>();

const techniqueViewModel = computed(() => {
  return TechniquePresenter.getTechnique(props.technique, props.isSignature, props.digievolutionLevel);
});

const icon = computed(() => {
  return IconConstant[techniqueViewModel.value.type as Constant];
});
</script>

<template>
  <div
    class="relative rounded px-3 py-2 flex items-start gap-3 border transition-all text-xs"
    :class="{
      'bg-yellow-950/30 border-yellow-500/60 shadow-[0_0_8px_rgba(234,179,8,0.2)]': techniqueViewModel.isSignature,
      'bg-[#001a33]/80 border-[#0055ff]/40': !techniqueViewModel.isSignature && techniqueViewModel.isUnlocked,
      'bg-[#000e1f]/50 border-[#0033aa]/20 opacity-50': !techniqueViewModel.isUnlocked,
    }"
  >
    <span v-if="techniqueViewModel.isSignature" class="absolute top-1 right-2 text-[10px] text-yellow-400 font-bold tracking-widest">⭐</span>

    <span class="text-base leading-none mt-px shrink-0">
      {{ icon }}
    </span>

    <div class="flex-1 min-w-0">
      <div class="flex items-center gap-1 mb-0.5">
        <span
          class="font-bold tracking-wide"
          :class="techniqueViewModel.isSignature ? 'text-yellow-300' : techniqueViewModel.isUnlocked ? 'text-white' : 'text-white/40'"
        >
          {{ $t(`technique.${techniqueViewModel.id}.name`) }}
        </span>
        <span v-if="!techniqueViewModel.isUnlocked" class="text-[10px] text-red-400/80 ml-1">{{ $t(`digievolution.lv`) }}.{{ techniqueViewModel.learnLevel }}</span>
        <span v-else class="text-[10px] text-green-400/80 ml-1">✓</span>
      </div>

      <p class="text-white/50 text-[11px] leading-snug">{{ $t(`technique.${techniqueViewModel.id}.description`) }}</p>

      <div class="flex gap-3 mt-1 text-[10px]">
        <span>
          {{ $t("digievolution.element") }}: {{ $t(`stat.${techniqueViewModel.element}`) }}
        </span>|
        <span class="text-blue-300/70">{{ $t(`digievolution.mpCost`) }}: {{ techniqueViewModel.mp }}</span>
      </div>
    </div>
  </div>
</template>
