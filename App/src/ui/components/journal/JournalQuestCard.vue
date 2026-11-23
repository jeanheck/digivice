<script setup lang="ts">
import { computed, inject } from "vue";
import type { QuestCardVariant, QuestViewModel } from "@/presentation/viewmodels/quest/quest.viewmodel";
import {
  getJournalSectionPalette,
  journalSectionPaletteKey,
} from "@/ui/components/journal/journal-section-palette";

const props = defineProps<{
  quest: QuestViewModel;
  displayMode: "main" | "side";
}>();

const emit = defineEmits<{
  click: [questId: string];
}>();

const QuestCardVariantClasses: Record<QuestCardVariant, string> = {
  locked: "border-frame/50 bg-surface-deep opacity-50 hover:opacity-70",
  unavailable: "border-frame/50 bg-surface-deep opacity-50 hover:opacity-70",
  done: "border-success-muted/50 bg-success-surface hover:bg-success-surface",
  new: "border-accent/70 bg-surface hover:bg-surface-deep hover:border-accent",
  active: "border-frame/40 bg-surface hover:bg-surface-deep hover:border-accent",
};

const isMainDisplayMode = computed(() => {
  return props.displayMode === "main";
});

const sectionPalette = inject(journalSectionPaletteKey, null);

const sideQuestPalette = computed(() => {
  if (sectionPalette !== null) {
    return sectionPalette.value;
  }

  return getJournalSectionPalette("cyan");
});

const cardClass = computed(() => {
  if (isMainDisplayMode.value) {
    return "border-gray-600 bg-gray-800/50 hover:bg-gray-700/60";
  }

  return QuestCardVariantClasses[props.quest.cardVariant];
});

const titleClass = computed(() => {
  if (isMainDisplayMode.value) {
    return "text-yellow-300 group-hover:text-yellow-400";
  }

  if (props.quest.isLocked || props.quest.isUnavailable) {
    return "text-ink-muted/55";
  }

  if (props.quest.isDone) {
    return "text-success line-through decoration-success";
  }

  const accentTitleClass = sideQuestPalette.value.questTitleClass;
  const accentTitleHoverClass = sideQuestPalette.value.questTitleHoverClass;

  return `${accentTitleClass} ${accentTitleHoverClass}`;
});

const stepNumberClass = computed(() => {
  if (isMainDisplayMode.value) {
    return "text-yellow-400 font-bold mr-1";
  }

  return `${sideQuestPalette.value.stepNumberClass} font-bold mr-1`;
});

const titleSizeClass = computed(() => {
  if (isMainDisplayMode.value) {
    return "text-xs";
  }

  return "text-[10px]";
});

const doneIconClass = computed(() => {
  if (isMainDisplayMode.value) {
    return "text-success text-xs";
  }

  return "text-success text-xs drop-shadow shrink-0 ml-2";
});

const onClick = () => {
  if (props.quest.isUnavailable) {
    return;
  }

  emit("click", props.quest.id);
};
</script>

<template>
  <div
    class="p-2 rounded border transition-all duration-200 group relative overflow-hidden"
    :class="[
      cardClass,
      quest.isUnavailable ? 'cursor-default' : 'cursor-pointer',
      isMainDisplayMode ? 'transition-colors' : '',
    ]"
    @click="onClick"
  >
    <div
      v-if="!isMainDisplayMode && quest.isDone"
      class="absolute inset-0 bg-success-surface pointer-events-none"
    />

    <div class="flex items-center justify-between mb-1 relative z-10">
      <span class="font-bold truncate transition-colors" :class="[titleSizeClass, titleClass]">
        <template v-if="isMainDisplayMode">
          <template v-if="quest.isDone">
            {{ $t(`${quest.id}.title`) }}
          </template>
          <template v-else-if="quest.currentChapter">
            {{ $t("journal.chapter", { number: quest.currentChapter.number }) }}
          </template>
        </template>
        <template v-else>
          {{ $t(`${quest.id}.title`) }}
          <template v-if="quest.currentChapter && !quest.isDone">
            - {{ $t("journal.chapter", { number: quest.currentChapter.number }) }}
          </template>
        </template>
      </span>

      <span v-if="quest.isDone" :class="doneIconClass">✔</span>
      <span
        v-else-if="!isMainDisplayMode && quest.isUnavailable"
        class="font-emoji-color text-xs shrink-0 ml-2"
        >🚫</span
      >
      <span
        v-else-if="!isMainDisplayMode && quest.isLocked"
        class="font-emoji-color text-xs shrink-0 ml-2"
        >🔒</span
      >
      <span
        v-else-if="!isMainDisplayMode && quest.isNew"
        class="font-emoji text-highlight-bright text-xs shrink-0 ml-2"
        >❕</span
      >
    </div>

    <p
      class="text-[10px] leading-tight line-clamp-1 relative z-10"
      :class="{
        'text-highlight': quest.isDone && isMainDisplayMode,
        'text-success': quest.isDone && !isMainDisplayMode,
        'text-ink-muted/70': !quest.isDone,
        'opacity-50': !isMainDisplayMode && (quest.isDone || quest.isLocked || quest.isUnavailable),
      }"
    >
      <template v-if="quest.isDone">
        {{ $t("journal.questCompleted") }}
      </template>
      <template v-else-if="!isMainDisplayMode && quest.isUnavailable">
        {{ $t("journal.questUnavailableStarter") }}
      </template>
      <template v-else-if="!isMainDisplayMode && quest.isLocked">
        {{ $t(`${quest.id}.description`) }}
      </template>
      <template v-else-if="quest.currentStep">
        <span :class="stepNumberClass">[{{ quest.currentStep.number }}]</span>
        {{ $t(`${quest.id}.steps.${quest.currentStep.number}.description`) }}
      </template>
      <template v-else>
        {{ $t(`${quest.id}.description`) }}
      </template>
    </p>
  </div>
</template>
