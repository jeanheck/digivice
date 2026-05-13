<script setup lang="ts">
import { computed } from 'vue';
import { MathUtils } from '../../utils/MathUtils';
import { ProgressBarTypes } from '../../types/ui';

const props = defineProps<{
  currentValue: number;
  maxValue: number;
  type?: ProgressBarTypes;
  colorClass?: string;
  label?: string;
}>();

const percentage = computed(() => {
    return MathUtils.calculatePercentage(props.currentValue, props.maxValue);
});

const barColorClass = computed(() => {
    // If a specific color class is provided, it takes precedence
    if (props.colorClass) {
        return props.colorClass;
    }

    if (props.type === ProgressBarTypes.HP) {
        return percentage.value <= 30 ? 'bg-red-500' : 'bg-green-500';
    }

    if (props.type === ProgressBarTypes.MP) {
        return percentage.value <= 30 ? 'bg-yellow-400' : 'bg-blue-600';
    }

    return 'bg-blue-500'; // Generic default
});
</script>

<template>
  <div class="relative w-full h-6 bg-[#000e3f] rounded overflow-hidden shadow-inner flex items-center justify-center border-2 border-[#00154a]">
    <div 
      class="absolute left-0 top-0 h-full transition-all duration-300 bg-opacity-90"
      :class="barColorClass"
      :style="{ width: `${percentage}%` }"
    ></div>
    
    <span class="relative z-10 text-[0.6rem] font-bold text-white drop-shadow-md px-1 tracking-wider">
      {{ label || `${currentValue} / ${maxValue}` }}
    </span>
  </div>
</template>
