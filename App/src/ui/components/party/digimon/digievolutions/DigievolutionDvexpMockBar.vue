<script setup lang="ts">
import { computed } from "vue";

// Temporary mock to preview a DVEXP progress bar with text; values ignore the real RAM state.
const DvexpMockBySlotIndex: Record<number, { current: number; max: number }> = {
  1: { current: 3, max: 10 },
  2: { current: 37, max: 50 },
  3: { current: 50, max: 50 },
};

const props = defineProps<{
  slotIndex: number;
  isActiveDigievolution: boolean;
}>();

const dvexpMock = computed(() => {
  return DvexpMockBySlotIndex[props.slotIndex] ?? DvexpMockBySlotIndex[1]!;
});

const progressPercentage = computed(() => {
  return (dvexpMock.value.current / dvexpMock.value.max) * 100;
});
</script>

<template>
  <div
    class="relative w-full h-[15px] hd:h-[20px] rounded overflow-hidden shadow-inner flex items-center justify-center border-2 border-[#00154a]"
    :class="isActiveDigievolution ? 'bg-orange-600/35' : 'bg-blue-800/35'"
  >
    <div
      class="absolute left-0 top-0 h-full transition-all duration-500"
      :class="isActiveDigievolution ? 'bg-linear-to-r from-orange-600 to-yellow-500' : 'bg-blue-400'"
      :style="{ width: `${progressPercentage}%` }"
    ></div>

    <span
      class="relative z-10 text-[10px] hd:text-[12px] font-bold font-chakra-petch text-white text-outline-black px-1 tracking-wider"
    >
      {{ `${dvexpMock.current} / ${dvexpMock.max}` }}
    </span>
  </div>
</template>
