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
      class="cursor-pointer hover:bg-accent/30 transition-colors text-[10px] px-2 py-1.5 bg-accent/15 text-accent-bright border border-frame/50 rounded flex items-center"
      @click="emit('select-digievolution-id', digievolutionLink.id)"
    >
      {{ digievolutionLink.name }}
    </button>
  </div>
</template>
