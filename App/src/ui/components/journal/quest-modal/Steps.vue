<script setup lang="ts">
import Requisites from "./Requisites.vue";
import type { StepViewModel } from "@/presentation/viewmodels/quest/step.viewmodel";

withDefaults(
  defineProps<{
    steps: StepViewModel[];
    questId: string;
    selectedStepNumber: string | null;
    showTitle?: boolean;
  }>(),
  {
    showTitle: true,
  },
);

const emit = defineEmits<{
  select: [step: StepViewModel];
}>();

const onSelectStep = (step: StepViewModel) => {
  emit("select", step);
};
</script>

<template>
  <div class="flex flex-col gap-2">
    <h3
      v-if="showTitle"
      class="text-xs text-accent font-bold uppercase tracking-wider mb-1 text-center"
    >
      {{ $t("journal.missionSteps") }}
    </h3>

    <div
      v-for="stepViewModel in steps"
      :key="stepViewModel.number"
      class="flex items-start gap-3 p-2 rounded border transition-all cursor-pointer group"
      :class="
        stepViewModel.isDone
          ? 'bg-success-surface border-success-muted/30'
          : selectedStepNumber === stepViewModel.number
            ? 'bg-highlight-surface border-highlight-border shadow-[0_0_10px_color-mix(in_srgb,var(--color-highlight)_20%,transparent)]'
            : 'bg-accent/5 border-frame/30'
      "
      @click="onSelectStep(stepViewModel)"
    >
      <div
        class="shrink-0 w-5 h-5 rounded border-2 flex items-center justify-center transition-colors shadow-inner"
        :class="
          stepViewModel.isDone
            ? 'bg-success-surface border-success text-success'
            : 'bg-surface-deep border-frame/50'
        "
      >
        <span v-if="stepViewModel.isDone" class="text-xs">✔</span>
      </div>

      <div class="flex-1">
        <p
          class="relative top-px text-xs transition-colors"
          :class="
            stepViewModel.isDone
              ? 'text-ink-muted line-through decoration-green-900'
              : 'text-ink'
          "
        >
          {{ stepViewModel.number }} -
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
