<script setup lang="ts">
import { ref } from "vue";
import type { WikiLocationViewModel } from "@/viewmodels/wiki-modal/wiki-location.viewmodel";

defineProps<{
  location: WikiLocationViewModel;
  isSelected: boolean;
}>();

defineEmits<{
  select: [];
}>();

const rootButton = ref<HTMLButtonElement | null>(null);

defineExpose({
  rootButton,
});
</script>

<template>
  <button
    ref="rootButton"
    type="button"
    class="shrink-0 text-left px-2.5 py-1.5 rounded border transition-colors cursor-pointer"
    :class="
      isSelected
        ? 'bg-blue-900/40 border-[#0033aa] text-blue-100'
        : 'border-transparent text-white hover:bg-blue-900/20'
    "
    @click="$emit('select')"
  >
    <span class="inline-flex items-center gap-1.5 text-[11px] font-bold tracking-wide whitespace-nowrap">
      <span>{{ $t(location.labelKey) }}</span>
      <span
        v-for="source in location.sources"
        :key="source.ariaLabelKey"
        class="text-[12px] 2xl:text-[14px] leading-none"
        :aria-label="$t(source.ariaLabelKey)"
      >
        <span class="inline-flex leading-none text-[1.2rem] -translate-y-1">{{ source.icon }}</span>
      </span>
    </span>
  </button>
</template>
