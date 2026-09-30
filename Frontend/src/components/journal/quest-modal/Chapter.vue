<script setup lang="ts">
import Steps from "./Steps.vue";
import type { ChapterViewModel } from "@/viewmodels/quest/chapter.viewmodel";
import type { StepViewModel } from "@/viewmodels/quest/step.viewmodel";

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
          ? 'bg-[#001a33] border-cyan-500/60 hover:border-cyan-400'
          : 'bg-white/5 border-blue-900/50 hover:border-blue-700'
      "
      @click="emit('toggle', chapter.number)"
    >
      <h3
        class="text-xs font-bold uppercase tracking-wider"
        :class="chapter.isCurrent ? 'text-cyan-300' : 'text-blue-500'"
      >
        {{ $t("journal.chapter", { number: chapter.number }) }} -
        {{ $t(`${questId}.chapters.${chapter.number}.name`) }}
      </h3>

      <div class="flex items-center gap-2 shrink-0">
        <span v-if="chapter.isDone" class="text-green-400 text-xs">✔</span>
        <span class="text-gray-400 text-xs font-medium">
          {{ chapter.doneCount }}/{{ chapter.totalCount }}
        </span>
        <span
          class="text-xs text-blue-500 transform transition-transform duration-300"
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
