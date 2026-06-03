<script setup lang="ts">
import { computed } from "vue";
import type { QuestCardVariant, QuestViewModel } from "@/viewmodels/quest/quest.viewmodel";

const props = defineProps<{
  quest: QuestViewModel;
  displayMode: "main" | "side";
  stepNumberAccentClass?: string;
  questTitleAccentClass?: string;
  questTitleHoverAccentClass?: string;
}>();

const emit = defineEmits<{
  click: [quest: QuestViewModel];
}>();

const QUEST_CARD_VARIANT_CLASSES: Record<QuestCardVariant, string> = {
  locked: "border-gray-700/40 bg-[#0a0a1a] opacity-50 hover:opacity-70",
  done: "border-green-800/50 bg-green-900/20 hover:bg-green-900/40",
  new: "border-white/60 bg-[#001a2a] hover:bg-[#002a3a] hover:border-white",
  active: "border-[#0033aa]/0 bg-[#001122] hover:bg-[#002244] hover:border-[#0055ff]",
};

const isMainDisplayMode = computed(() => {
  return props.displayMode === "main";
});

const cardClass = computed(() => {
  if (isMainDisplayMode.value) {
    return "border-gray-600 bg-gray-800/50 hover:bg-gray-700/60";
  }

  return QUEST_CARD_VARIANT_CLASSES[props.quest.cardVariant];
});

const titleClass = computed(() => {
  if (isMainDisplayMode.value) {
    return "text-yellow-300 group-hover:text-yellow-400";
  }

  if (props.quest.isLocked) {
    return "text-gray-500";
  }

  if (props.quest.isDone) {
    return "text-gray-400 line-through";
  }

  const accentTitleClass = props.questTitleAccentClass ?? "text-blue-300";
  const accentTitleHoverClass = props.questTitleHoverAccentClass ?? "group-hover:text-blue-400";

  return `${accentTitleClass} ${accentTitleHoverClass}`;
});

const stepNumberClass = computed(() => {
  if (isMainDisplayMode.value) {
    return "text-yellow-400 font-bold mr-1";
  }

  const accentStepNumberClass = props.stepNumberAccentClass ?? "text-blue-400";

  return `${accentStepNumberClass} font-bold mr-1`;
});

const titleSizeClass = computed(() => {
  if (isMainDisplayMode.value) {
    return "text-xs";
  }

  return "text-[10px]";
});

const doneIconClass = computed(() => {
  if (isMainDisplayMode.value) {
    return "text-green-400 text-xs";
  }

  return "text-green-500 text-xs drop-shadow shrink-0 ml-2";
});

const onClick = () => {
  emit("click", props.quest);
};
</script>

<template>
  <div
    class="p-2 rounded border cursor-pointer transition-all duration-200 group relative overflow-hidden"
    :class="[
      cardClass,
      isMainDisplayMode ? 'transition-colors' : '',
    ]"
    @click="onClick"
  >
    <div
      v-if="!isMainDisplayMode && quest.isDone"
      class="absolute inset-0 bg-green-500/15 pointer-events-none"
    />

    <div class="flex items-center justify-between mb-1 relative z-10">
      <span
        class="font-bold truncate transition-colors"
        :class="[titleSizeClass, titleClass]"
      >
        {{ $t(`${quest.id}.title`) }}
      </span>

      <span v-if="quest.isDone" :class="doneIconClass">✔</span>
      <span v-else-if="!isMainDisplayMode && quest.isLocked" class="text-xs shrink-0 ml-2">🔒</span>
      <span v-else-if="!isMainDisplayMode && quest.isNew" class="text-xs shrink-0 ml-2">❕</span>
    </div>

    <p
      class="text-gray-400 text-[10px] leading-tight line-clamp-1 relative z-10"
      :class="{ 'opacity-50': !isMainDisplayMode && (quest.isDone || quest.isLocked) }"
    >
      <template v-if="quest.isDone">
        {{ $t("journal.questCompleted") }}
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
