<script setup lang="ts">
import { computed } from "vue";
import { STAT_ICONS } from "@/constants/stat-icons";
import { useLocalization } from "@/composables/useLocalization";
import { Stat } from "@/models/stat";
import type { StatViewModel } from "@/viewmodels/digimon/stat.viewmodel";

const props = defineProps<{
  statViewModel: StatViewModel;
  propertyKey: string;
}>();

const emit = defineEmits<{
  (e: "showIconTooltip", event: MouseEvent, title: string, propertyKey: Stat): void;
  (e: "showMathTooltip", event: MouseEvent, title: string, base: number, equip: number, digi: number, total: number): void;
  (e: "moveTooltip", event: MouseEvent): void;
  (e: "hideTooltip"): void;
}>();

const { t } = useLocalization();

const label = computed(() => {
  return t(`stat.${props.propertyKey}`);
});

const stat = computed(() => props.propertyKey as Stat);

const icon = computed(() => {
  return STAT_ICONS[stat.value];
});

</script>

<template>
  <div class="flex items-center gap-2">
    <div
      class="flex items-center w-5 justify-center cursor-help select-none z-20 tooltip-anchor relative"
      @mouseenter="(event) => emit('showIconTooltip', event, label, stat)"
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
        class="ml-2 font-bold bg-linear-to-b from-[#ffcc00] to-[#ff6600] text-transparent bg-clip-text shadow-text-dark tracking-normal"
      >+{{ statViewModel.fromDigievolution }}</span>
    </div>
  </div>
</template>
