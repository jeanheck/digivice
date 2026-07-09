<script setup lang="ts">
import { computed, provide, ref } from "vue";
import {
    getJournalSectionPalette,
    journalSectionPaletteKey,
    type JournalSectionAccentColor,
} from "@/components/journal/journal-section-palette";

const props = withDefaults(defineProps<{
  title: string;
  accentColor: JournalSectionAccentColor;
  defaultExpanded?: boolean;
}>(), {
  defaultExpanded: false,
});

const palette = computed(() => {
  return getJournalSectionPalette(props.accentColor);
});

provide(journalSectionPaletteKey, palette);

const isExpanded = ref(props.defaultExpanded);

const toggle = () => {
  isExpanded.value = !isExpanded.value;
};
</script>

<template>
  <section>
    <div
      class="flex items-center justify-between mb-2 border-b pb-1 cursor-pointer transition-colors p-1 -mx-1 rounded"
      :class="[palette.sectionBorderClass, palette.sectionHeaderHoverClass]"
      @click="toggle"
    >
      <h4 class="text-xs font-bold tracking-wide" :class="palette.sectionTitleClass">
        {{ title }}
      </h4>
      <span
        class="text-xs transform transition-transform duration-300"
        :class="[palette.sectionChevronClass, { 'rotate-180': isExpanded }]"
      >
        ▼
      </span>
    </div>

    <div v-show="isExpanded" class="space-y-2">
      <slot />
    </div>
  </section>
</template>
