<script setup lang="ts">
import { computed } from "vue";
import { IconConstant } from "@/constants/icon.constant";
import { useI18n } from "vue-i18n";
import { Constant } from "@/constants/constant";
import type { StatViewModel } from "@/viewmodels/digimon/stat.viewmodel";

const props = defineProps<{
  statViewModel: StatViewModel;
  stat: string;
}>();

const emit = defineEmits<{
  (e: "showIconTooltip", event: MouseEvent, title: string, propertyKey: Constant): void;
  (e: "showMathTooltip", event: MouseEvent, title: string, base: number, equip: number, digi: number, total: number): void;
  (e: "moveTooltip", event: MouseEvent): void;
  (e: "hideTooltip"): void;
}>();

const { t } = useI18n();

const label = computed(() => {
  return t(`stat.${props.stat}`);
});

const statKey = computed(() => props.stat as Constant);

const icon = computed(() => {
  return IconConstant[statKey.value];
});

</script>

<template>
  <div class="flex items-center gap-2">
    <div
      class="flex items-center w-5 justify-center cursor-help select-none z-20 tooltip-anchor relative"
      @mouseenter="(event) => emit('showIconTooltip', event, label, statKey)"
      @mousemove="(event) => emit('moveTooltip', event)"
      @mouseleave="emit('hideTooltip')"
    >
      <span class="text-base font-emoji drop-shadow-[0_0_2px_rgba(255,255,255,0.7)] -translate-y-1">{{ icon }}</span>
    </div>

    <div
      class="font-bold tracking-widest cursor-help flex items-center"
      @mouseenter="(event) => emit('showMathTooltip', event, label, statViewModel.fromDigimon, statViewModel.fromEquipaments, statViewModel.fromDigievolution, statViewModel.sumBetweenDigimonAndEquipaments)"
      @mousemove="(event) => emit('moveTooltip', event)"
      @mouseleave="emit('hideTooltip')"
    >
      <span class="shadow-text">{{ statViewModel.sumBetweenDigimonAndEquipaments }}</span>
      <span
        v-if="statViewModel.fromDigievolution > 0"
        class="ml-2 font-bold text-dw3-gold shadow-text-dark tracking-normal"
      >+{{ statViewModel.fromDigievolution }}</span>
    </div>
  </div>
</template>
