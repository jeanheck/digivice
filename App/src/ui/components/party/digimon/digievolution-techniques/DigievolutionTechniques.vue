<script setup lang="ts">
import { computed } from "vue";
import { ImageCatalog } from "@/ui/catalogs/image.catalog";
import Links from "@/ui/components/party/digimon/digievolution-techniques/Links.vue";
import SectionTitle from "@/ui/components/party/digimon/digievolution-techniques/SectionTitle.vue";
import Technique from "@/ui/components/party/digimon/digievolution-techniques/Technique.vue";
import { DigievolutionTechniquesPresenter } from "@/presentation/presenters/digievolution/digievolution-techniques.presenter";
import type { TooltipHorizontalAlign } from "@/ui/composables/use-tooltip-position";

const props = withDefaults(
  defineProps<{
    digievolutionId: number;
    digimonId?: number;
    digievolutionLevel?: number;
    showTreeSections?: boolean;
    tooltipAlign?: TooltipHorizontalAlign;
  }>(),
  {
    tooltipAlign: "right",
  },
);

defineEmits<{
  (e: "select-digievolution-id", digievolutionId: number): void;
}>();

const viewModel = computed(() => {
  return DigievolutionTechniquesPresenter.getViewModel(props.digievolutionId, {
    digimonId: props.digimonId,
    digievolutionLevel: props.digievolutionLevel,
    showTreeSections: props.showTreeSections,
  });
});

const evolutionAvatarUrl = computed(() => {
  return ImageCatalog.getDigievolutionImageUrl(viewModel.value.evolutionName);
});
</script>

<template>
  <div
    class="flex flex-col h-full bg-surface rounded overflow-hidden relative"
  >
    <div
      class="relative flex-none pt-4 px-4 pb-2 flex flex-col items-center justify-center bg-linear-to-b from-surface-deep to-transparent shrink-0"
    >
      <div
        class="relative w-full h-32 bg-linear-to-r from-accent/25 to-surface-deep rounded-lg border border-frame shadow-inner overflow-hidden group"
      >
        <img
          v-if="evolutionAvatarUrl"
          :src="evolutionAvatarUrl"
          class="absolute inset-0 w-full h-full object-cover object-[center_15%] pointer-events-none drop-shadow-[0_0_15px_color-mix(in_srgb,var(--color-accent)_40%,transparent)] transition-opacity duration-500"
          :alt="viewModel.evolutionName"
        />

        <h2
          class="absolute top-3 left-4 text-xl font-bold text-white tracking-widest drop-shadow-[0_2px_4px_rgba(0,0,0,0.9)] z-10"
        >
          {{ viewModel.evolutionName }}
        </h2>
      </div>
    </div>

    <div class="px-4 pb-4 pt-2 flex-1 flex flex-col gap-4 overflow-y-auto custom-scroll">
      <div v-if="showTreeSections && viewModel.requirementDigievolutions.length" class="shrink-0">
        <SectionTitle :label="$t('digievolution.requirementDigievolutions')" />
        <Links
          :digievolution-links="viewModel.requirementDigievolutions"
          @select-digievolution-id="$emit('select-digievolution-id', $event)"
        />
      </div>

      <div class="flex flex-col">
        <div class="flex flex-col gap-1 pr-1">
          <Technique
            v-for="techniqueViewModel in viewModel.techniques"
            :key="techniqueViewModel.id"
            :technique="techniqueViewModel"
            :tooltip-align="tooltipAlign"
          />

          <p
            v-if="!viewModel.techniques.length"
            class="text-ink-muted/60 text-center py-4 text-[10px] italic border border-frame/30 rounded"
          >
            {{ $t("digievolution.noTechData") }}
          </p>
        </div>
      </div>

      <div v-if="showTreeSections && viewModel.derivativeDigievolutions.length" class="shrink-0">
        <SectionTitle :label="$t('digievolution.nextDigievolutions')" />
        <Links
          :digievolution-links="viewModel.derivativeDigievolutions"
          @select-digievolution-id="$emit('select-digievolution-id', $event)"
        />
      </div>
    </div>
  </div>
</template>
