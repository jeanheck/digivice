<script setup lang="ts">
import { computed } from "vue";
import { DigievolutionDvexpPresenter } from "@/presentation/presenters/digievolution/digievolution-dvexp.presenter";

const props = defineProps<{
  isActiveDigievolution: boolean;
  digievolutionId: number;
  level: number;
  dvexp: number;
}>();

const progress = computed(() => {
  return DigievolutionDvexpPresenter.getProgress(props.digievolutionId, props.level, props.dvexp);
});

const progressPercentage = computed(() => {
  return (progress.value.current / progress.value.max) * 100;
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
      {{ `${progress.current} / ${progress.max}` }}
    </span>
  </div>
</template>
