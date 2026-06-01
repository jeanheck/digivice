<script setup lang="ts">
import { computed } from "vue";
import { ImageCatalog } from "@/catalogs/image.catalog.ts";
import DigievolutionsModalDigievolutionLinks from "./DigievolutionsModalDigievolutionLinks.vue";
import DigievolutionsModalSectionTitle from "./DigievolutionsModalSectionTitle.vue";
import DigievolutionsModalTechnique from "./DigievolutionsModalTechnique.vue";
import { DigievolutionsModalDigievolutionDetailsPresenter } from "@/presenters/digievolutions-modal/digievolutions-modal-digievolution-details.presenter";

const props = defineProps<{
  digimonId: number;
  digievolutionId: number | undefined;
}>();

defineEmits<{
  (e: "select-digievolution-id", digievolutionId: number): void;
}>();

const detailPanelViewModel = computed(() => {
  return DigievolutionsModalDigievolutionDetailsPresenter.getDetailPanelViewModel(
    props.digimonId,
    props.digievolutionId
  );
});

const evolutionAvatarUrl = computed(() => {
  return ImageCatalog.getDigievolutionIconUrl(detailPanelViewModel.value?.evolutionName ?? null);
});
</script>

<template>
  <div class="flex flex-col h-full bg-[#0c0d1b] rounded overflow-hidden relative">
    <div class="relative flex-none pt-4 px-4 pb-2 flex flex-col items-center justify-center bg-linear-to-b from-[#00051a] to-transparent shrink-0">
      <div class="relative w-full h-32 bg-linear-to-r from-cyan-950/40 to-[#001533] rounded-lg border border-cyan-800/50 shadow-inner overflow-hidden group">
        <img
          v-if="evolutionAvatarUrl"
          :src="evolutionAvatarUrl"
          class="absolute inset-0 w-full h-full object-cover object-[center_15%] pointer-events-none drop-shadow-[0_0_15px_rgba(0,170,255,0.4)] transition-opacity duration-500"
          :alt="detailPanelViewModel?.evolutionName"
        />

        <h2 class="absolute top-3 left-4 text-lg sm:text-xl font-bold font-cyber text-white tracking-widest drop-shadow-[0_2px_4px_rgba(0,0,0,0.9)] z-10">
          {{ detailPanelViewModel?.evolutionName }}
        </h2>
      </div>
    </div>

    <div class="px-4 pb-4 pt-2 flex-1 flex flex-col gap-4 overflow-y-auto custom-scroll">
      <div v-if="detailPanelViewModel?.requirementDigievolutions.length" class="shrink-0">
        <DigievolutionsModalSectionTitle
          icon="🧬"
          :label="$t('digievolution.requirementDigievolutions')"
        />
        <DigievolutionsModalDigievolutionLinks
          :digievolution-links="detailPanelViewModel.requirementDigievolutions"
          @select-digievolution-id="$emit('select-digievolution-id', $event)"
        />
      </div>

      <div class="flex flex-col">
        <DigievolutionsModalSectionTitle
          icon="⚔️"
          :label="$t('digievolution.techniques')"
        />

        <div class="flex flex-col gap-1 pr-1">
          <DigievolutionsModalTechnique
            v-for="techniqueViewModel in detailPanelViewModel?.techniques ?? []"
            :key="techniqueViewModel.id"
            :technique="techniqueViewModel"
          />

          <p
            v-if="!detailPanelViewModel?.techniques.length"
            class="text-white/40 text-center py-4 text-[10px] italic font-cyber border border-white/5 rounded"
          >
            {{ $t("digievolution.noTechData") }}
          </p>
        </div>
      </div>

      <div v-if="detailPanelViewModel?.derivativeDigievolutions.length" class="shrink-0">
        <DigievolutionsModalSectionTitle
          icon="🧬"
          :label="$t('digievolution.nextDigievolutions')"
        />
        <DigievolutionsModalDigievolutionLinks
          :digievolution-links="detailPanelViewModel.derivativeDigievolutions"
          @select-digievolution-id="$emit('select-digievolution-id', $event)"
        />
      </div>
    </div>
  </div>
</template>
