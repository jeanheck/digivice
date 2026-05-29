<script setup lang="ts">
import type { RequisiteViewModel } from "@/viewmodels/quest/requisite.viewmodel";

withDefaults(defineProps<{
  items: RequisiteViewModel[];
  translationKeyPrefix: string;
  variant?: "quest" | "step";
  titleKey?: string;
}>(), {
  variant: "quest",
  titleKey: "",
});
</script>

<template>
  <div
    v-if="items.length > 0"
    class="flex flex-col gap-2"
    :class="{ 'mt-1.5 ml-1 gap-1': variant === 'step' }"
  >
    <h3
      v-if="variant === 'quest' && titleKey"
      class="text-xs text-amber-500 font-bold uppercase tracking-wider mb-1 border-b border-amber-900/40 pb-1"
    >
      {{ $t(titleKey) }}
    </h3>

    <div
      v-for="requisiteViewModel in items"
      :key="requisiteViewModel.id"
      class="flex transition-colors"
      :class="
        variant === 'quest'
          ? ['items-start gap-3 p-2 rounded', requisiteViewModel.isDone ? 'bg-green-900/10 border border-green-800/30' : 'bg-red-900/10 border border-red-800/30']
          : 'items-center gap-2 text-xs'
      "
    >
      <div
        v-if="variant === 'quest'"
        class="mt-0.5 shrink-0 w-5 h-5 rounded border-2 flex items-center justify-center transition-colors shadow-inner"
        :class="requisiteViewModel.isDone ? 'bg-green-500/20 border-green-500 text-green-400 shadow-[0_0_8px_rgba(0,255,0,0.3)]' : 'bg-red-500/10 border-red-500/60 text-red-400'"
      >
        <span v-if="requisiteViewModel.isDone" class="text-xs">✔</span>
        <span v-else class="text-xs">✘</span>
      </div>

      <span
        v-else
        :class="requisiteViewModel.isDone ? 'text-green-400' : 'text-gray-500'"
      >
        {{ requisiteViewModel.isDone ? "✔" : "○" }}
      </span>

      <p
        v-if="variant === 'quest'"
        class="flex-1 text-sm leading-snug transition-colors"
        :class="requisiteViewModel.isDone ? 'text-gray-400 line-through decoration-green-900' : 'text-red-300'"
      >
        {{ $t(`${translationKeyPrefix}.${requisiteViewModel.id}`) }}
      </p>
      <span
        v-else
        class="transition-colors"
        :class="requisiteViewModel.isDone ? 'text-gray-400 line-through' : 'text-gray-400'"
      >
        {{ $t(`${translationKeyPrefix}.${requisiteViewModel.id}`) }}
      </span>
    </div>
  </div>
</template>
