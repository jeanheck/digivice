<script setup lang="ts">
import DnaDigievolutionFusion from "@/ui/components/dna-digievolutions-modal/DnaDigievolutionFusion.vue";
import type { DnaDigievolutionViewModel } from "@/presentation/viewmodels/dna-digievolution/dna-digievolution.viewmodel";

defineProps<{
  dnaDigievolution: DnaDigievolutionViewModel;
}>();
</script>

<template>
  <div class="dw3-panel dw3-beveled text-white">
    <div class="dw3-panel-border dw3-beveled"></div>
    <div class="dw3-panel-inner dw3-beveled"></div>

    <div class="dw3-panel-content flex gap-4 w-full p-3">
      <div
        class="w-28 h-28 shrink-0 bg-[#000a1a] border border-blue-900/50 rounded flex items-center justify-center overflow-hidden"
      >
        <img
          v-if="dnaDigievolution.imageUrl"
          :src="dnaDigievolution.imageUrl"
          :alt="dnaDigievolution.name"
          class="w-full h-full object-cover object-top"
        />
        <span v-else class="text-cyan-500/50 text-2xl font-bold">?</span>
      </div>

      <div class="flex flex-1 min-w-0 flex-col gap-2">
        <div class="flex items-baseline justify-between gap-3">
          <span class="text-dw3-gold shadow-text-dark font-bold tracking-wider truncate">
            {{ dnaDigievolution.name }}
          </span>
          <span class="text-[10px] text-blue-300 tracking-wider whitespace-nowrap">
            {{ $t("digimon.mp") }}: {{ dnaDigievolution.mp }}
          </span>
        </div>

        <div class="flex flex-col gap-1">
          <template v-for="(fusion, fusionIndex) in dnaDigievolution.fusions" :key="fusionIndex">
            <span
              v-if="fusionIndex > 0"
              class="text-[9px] text-white/60 uppercase tracking-widest text-center"
            >
              {{ $t("dnaDigievolution.or") }}
            </span>
            <DnaDigievolutionFusion :fusion="fusion" />
          </template>
        </div>

        <div
          class="flex items-center gap-4 pt-2 border-t border-[#0066cc]/40 text-[12px] uppercase tracking-wider"
        >
          <span class="flex items-center gap-1.5">
            <img
              v-if="dnaDigievolution.typeImageUrl"
              :src="dnaDigievolution.typeImageUrl"
              :alt="$t(dnaDigievolution.typeKey)"
              class="w-[19px] h-5 object-contain rendering-pixelated"
            />
            {{ $t(dnaDigievolution.typeKey) }}
          </span>
          <span v-if="dnaDigievolution.elementKey !== null" class="flex items-center gap-1.5">
            <img
              v-if="dnaDigievolution.elementImageUrl"
              :src="dnaDigievolution.elementImageUrl"
              :alt="$t(dnaDigievolution.elementKey)"
              class="w-5 h-5 object-contain rendering-pixelated"
            />
            {{ $t(dnaDigievolution.elementKey) }}
          </span>
        </div>

        <p v-if="dnaDigievolution.effectKey !== null" class="text-[10px] leading-tight text-white/85">
          {{ $t(dnaDigievolution.effectKey) }}
        </p>
      </div>
    </div>
  </div>
</template>
