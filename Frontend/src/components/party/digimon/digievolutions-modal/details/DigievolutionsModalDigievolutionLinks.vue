<script setup lang="ts">
import type { DigievolutionsModalDigievolutionLinkViewModel } from "@/viewmodels/digievolution/digievolutions-modal-digievolution-link.viewmodel";

defineProps<{
  digievolutionLinks: DigievolutionsModalDigievolutionLinkViewModel[];
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
      class="cursor-pointer hover:bg-indigo-700/50 transition-colors text-[10px] px-2 py-1.5 bg-indigo-950/30 text-indigo-200 border border-indigo-900/40 rounded font-cyber flex items-center"
      @click="emit('select-digievolution-id', digievolutionLink.id)"
    >
      {{ digievolutionLink.name }}
    </button>
  </div>
</template>
