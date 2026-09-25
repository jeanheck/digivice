<script setup lang="ts">
import WikiDroppedBySource from "@/components/wiki-modal/wiki-drops-panel/WikiDroppedBySource.vue";
import type { DropSourceKind } from "@/viewmodels/drop/drop-source.viewmodel";
import type { WikiDroppedBySourceViewModel } from "@/viewmodels/wiki-modal/wiki-dropped-by-source.viewmodel";

defineProps<{
  sources: WikiDroppedBySourceViewModel[];
  sectionLabelKey: string;
  emptyLabelKey: string;
}>();

const emit = defineEmits<{
  (e: "open-source", payload: { kind: DropSourceKind; sourceId: string }): void;
}>();

const handleSelect = (source: WikiDroppedBySourceViewModel): void => {
  emit("open-source", {
    kind: source.kind,
    sourceId: source.sourceId,
  });
};
</script>

<template>
  <section
    class="shrink-0 w-full bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner"
  >
    <h4 class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-3">
      {{ $t(sectionLabelKey) }}
    </h4>

    <p
      v-if="sources.length === 0"
      class="text-xs text-gray-400 italic"
    >
      {{ $t(emptyLabelKey) }}
    </p>
    <div
      v-else
      class="flex flex-wrap gap-2"
    >
      <WikiDroppedBySource
        v-for="source in sources"
        :key="`${source.kind}-${source.sourceId}-${source.locationId ?? ''}`"
        :source="source"
        @select="handleSelect(source)"
      />
    </div>
  </section>
</template>
