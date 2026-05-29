<script setup lang="ts">
import Requisites from "./Requisites.vue";
import type { StepViewModel } from "@/viewmodels/quest/step.viewmodel";

defineProps<{
  steps: StepViewModel[];
  questId: string;
  selectedStepNumber: string | null;
}>();

const emit = defineEmits<{
  select: [step: StepViewModel];
}>();

const onSelectStep = (step: StepViewModel) => {
  emit("select", step);
};
</script>

<template>
  <div class="flex flex-col gap-2">
    <h3 class="text-xs text-blue-500 font-bold uppercase tracking-wider mb-1 border-b border-blue-900/40 pb-1">
      {{ $t("journal.missionSteps") }}
    </h3>

    <div
      v-for="stepViewModel in steps"
      :key="stepViewModel.number"
      class="flex items-start gap-3 p-2 rounded transition-all cursor-pointer group"
      :class="[
        stepViewModel.isDone ? 'bg-green-900/10 border border-green-800/30' : 'bg-white/5 border border-white/10',
        selectedStepNumber === stepViewModel.number ? 'ring-1 ring-cyan-500 shadow-[0_0_10px_rgba(0,255,255,0.2)] bg-[#001a33]' : '',
      ]"
      @click="onSelectStep(stepViewModel)"
    >
      <div
        class="mt-0.5 shrink-0 w-5 h-5 rounded border-2 flex items-center justify-center transition-colors shadow-inner"
        :class="stepViewModel.isDone ? 'bg-green-500/20 border-green-500 text-green-400 shadow-[0_0_8px_rgba(0,255,0,0.3)]' : 'bg-black/50 border-gray-600'"
      >
        <span v-if="stepViewModel.isDone" class="text-xs">✔</span>
      </div>

      <div class="flex-1">
        <p
          class="text-sm leading-snug transition-colors"
          :class="stepViewModel.isDone ? 'text-gray-400 line-through decoration-green-900' : 'text-gray-200'"
        >
          {{ $t(`${questId}.steps.${stepViewModel.number}.description`) }}
        </p>
        <Requisites
          :items="stepViewModel.requisites"
          :translation-key-prefix="`${questId}.steps.${stepViewModel.number}.requisites`"
          variant="step"
        />
      </div>
    </div>

    <div v-if="steps.length === 0" class="text-center p-3 opacity-50">
      <span class="text-gray-500 text-sm italic">{{ $t("journal.noSteps") }}</span>
    </div>
  </div>
</template>
