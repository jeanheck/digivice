<script setup lang="ts">
import Steps from "./Steps.vue";
import type { ChapterViewModel } from "@/presentation/viewmodels/quest/chapter.viewmodel";
import type { StepViewModel } from "@/presentation/viewmodels/quest/step.viewmodel";

defineProps<{
  chapter: ChapterViewModel;
  questId: string;
  selectedStepNumber: string | null;
  isExpanded: boolean;
}>();

const emit = defineEmits<{
  toggle: [chapterNumber: number];
  select: [step: StepViewModel];
}>();
</script>

<template>
  <section class="flex flex-col gap-2">
    <div
      class="flex items-center justify-between gap-3 p-2 rounded border cursor-pointer transition-colors"
      :class="
        chapter.isCurrent
          ? 'bg-[#001a33] border-cyan-500/60 hover:border-cyan-400 in-[[data-theme=seabed]]:bg-yellow-50 in-[[data-theme=seabed]]:border-yellow-500/60 in-[[data-theme=seabed]]:hover:border-yellow-400'
          : 'bg-white/5 border-blue-900/50 hover:border-blue-700 in-[[data-theme=seabed]]:bg-teal-800/10 in-[[data-theme=seabed]]:border-teal-700/40 in-[[data-theme=seabed]]:hover:border-teal-600'
      "
      @click="emit('toggle', chapter.number)"
    >
      <h3
        class="text-xs font-bold uppercase tracking-wider"
        :class="
          chapter.isCurrent
            ? 'text-cyan-300 in-[[data-theme=seabed]]:text-yellow-700'
            : 'text-blue-500 in-[[data-theme=seabed]]:text-teal-700'
        "
      >
        {{ $t("journal.chapter", { number: chapter.number }) }} -
        {{ $t(`${questId}.chapters.${chapter.number}.name`) }}
      </h3>

      <div class="flex items-center gap-2 shrink-0">
        <span v-if="chapter.isDone" class="text-green-400 text-xs">✔</span>
        <span class="text-gray-400 in-[[data-theme=seabed]]:text-teal-700 text-xs font-medium">
          {{ chapter.doneCount }}/{{ chapter.totalCount }}
        </span>
        <span
          class="text-xs text-blue-500 in-[[data-theme=seabed]]:text-teal-700 transform transition-transform duration-300"
          :class="{ 'rotate-180': isExpanded }"
        >
          ▼
        </span>
      </div>
    </div>

    <Steps
      v-show="isExpanded"
      :steps="chapter.steps"
      :quest-id="questId"
      :selected-step-number="selectedStepNumber"
      :show-title="false"
      @select="(step) => emit('select', step)"
    />
  </section>
</template>
