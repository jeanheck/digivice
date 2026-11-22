<script setup lang="ts">
import type { DnaDigievolutionFusionMemberViewModel } from "@/presentation/viewmodels/dna-digievolution/dna-digievolution-fusion-member.viewmodel";

defineProps<{
  member: DnaDigievolutionFusionMemberViewModel;
}>();
</script>

<template>
  <div
    class="flex flex-1 min-w-0 items-center justify-center h-8 px-3 rounded-md border-2 border-cyan-400/80 bg-cyan-900/30 shadow-[0_0_10px_rgba(0,255,255,0.25)] in-[[data-theme=seabed]]:border-[var(--color-accent)]/80 in-[[data-theme=seabed]]:bg-[var(--color-accent)]/15 in-[[data-theme=seabed]]:shadow-none"
  >
    <span
      class="text-[10px] font-bold tracking-wider truncate text-white in-[[data-theme=seabed]]:text-[var(--color-accent)]"
    >
      {{ member.name }}
    </span>
  </div>
</template>
