<script setup lang="ts">
import DnaDigievolutionFusionMember from "@/ui/components/dna-digievolutions-modal/DnaDigievolutionFusionMember.vue";
import type { DnaDigievolutionFusionMemberViewModel } from "@/presentation/viewmodels/dna-digievolution/dna-digievolution-fusion-member.viewmodel";

defineProps<{
  fusion: [DnaDigievolutionFusionMemberViewModel, DnaDigievolutionFusionMemberViewModel];
}>();
</script>

<template>
  <div class="flex items-center gap-2 w-full">
    <DnaDigievolutionFusionMember :member="fusion[0]" />
    <span class="text-dw3-gold shadow-text-dark text-lg font-bold leading-none shrink-0">+</span>
    <DnaDigievolutionFusionMember :member="fusion[1]" />
  </div>
</template>
