<script setup lang="ts">
import type { DnaDigievolutionFusionMemberViewModel } from "@/presentation/viewmodels/dna-digievolution/dna-digievolution-fusion-member.viewmodel";

defineProps<{
  member: DnaDigievolutionFusionMemberViewModel;
}>();
</script>

<template>
  <div
    class="flex flex-1 min-w-0 items-center justify-center h-8 px-3 rounded-md border-2 border-cyan-400/80 bg-cyan-900/30 shadow-[0_0_10px_rgba(0,255,255,0.25)]"
  >
    <span class="text-[10px] font-bold tracking-wider truncate shadow-text">
      {{ member.name }}
    </span>
  </div>
</template>
