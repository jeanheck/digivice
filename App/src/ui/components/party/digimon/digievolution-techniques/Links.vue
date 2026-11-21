<script setup lang="ts">
import type { LinkViewModel } from "@/presentation/viewmodels/digievolution/link.viewmodel";

defineProps<{
  digievolutionLinks: LinkViewModel[];
}>();

const emit = defineEmits<{
  (e: "select-digievolution-id", digievolutionId: number): void;
}>();
</script>

<template>
  <div class="flex flex-wrap gap-2">
    <button
      v-for="digievolutionLink in digievolutionLinks"
      :key="digievolutionLink.id"
      class="cursor-pointer hover:bg-indigo-700/50 in-[[data-theme=seabed]]:hover:bg-teal-700/30 transition-colors text-[10px] px-2 py-1.5 bg-indigo-950/30 in-[[data-theme=seabed]]:bg-teal-800/15 text-indigo-200 in-[[data-theme=seabed]]:text-teal-800 border border-indigo-900/40 in-[[data-theme=seabed]]:border-teal-700/50 rounded flex items-center"
      @click="emit('select-digievolution-id', digievolutionLink.id)"
    >
      {{ digievolutionLink.name }}
    </button>
  </div>
</template>
