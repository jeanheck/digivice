<script setup lang="ts">
import type { DnaDigievolutionFusionMemberViewModel } from "@/presentation/viewmodels/dna-digievolution/dna-digievolution-fusion-member.viewmodel";

defineProps<{
  member: DnaDigievolutionFusionMemberViewModel;
}>();
</script>

<template>
  <div
    class="flex flex-1 min-w-0 items-center justify-center h-8 px-3 rounded-md border-2 border-accent/80 bg-accent/15 shadow-[0_0_10px_color-mix(in_srgb,var(--color-accent)_25%,transparent)]"
  >
    <span
      class="text-[10px] font-bold tracking-wider truncate text-accent-bright"
    >
      {{ member.name }}
    </span>
  </div>
</template>
