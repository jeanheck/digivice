<script setup lang="ts">
import { computed, ref, watch } from "vue";
import type { Digimon } from "@/models";
import Modal from "@/components/modal/Modal.vue";
import DigievolutionFamilyTree from "./DigievolutionFamilyTree.vue";
import DigievolutionDetailPanel from "./DigievolutionDetailPanel.vue";
import DigievolutionsModalSearchBar from "./DigievolutionsModalSearchBar.vue";
import { DigievolutionGridModalPresenter } from "@/presenters/digievolution-grid-modal.presenter.ts";
import type { DigimonDigievolutionRequirementViewModel } from "@/viewmodels/digimon/digimon-digievolution-requirement.viewmodel.ts";

const props = defineProps<{
  isOpen: boolean;
  digimon: Digimon;
  digimonId: number;
}>();

const emit = defineEmits<{
  (e: "close"): void;
}>();

const isModalOpen = computed(() => {
  return props.isOpen && props.digimon !== null && props.digimon !== undefined;
});

const handleClose = () => {
  emit("close");
};

const digimonName = computed(() => {
  return DigievolutionGridModalPresenter.getNameById(props.digimonId);
});

const selectedEvolution = ref<DigimonDigievolutionRequirementViewModel[] | null>(null);
const selectedEvolutionName = ref<string | undefined>(undefined);

watch(() => props.isOpen, (open) => {
  if (open && props.digimon) {
    selectedEvolution.value = null;
    selectedEvolutionName.value = undefined;
  }
});

const handleSelectNode = (digievolutionName: string) => {
  const requirements = DigievolutionGridModalPresenter.getDigievolutionRequirements(props.digimonId, digievolutionName);

  if (requirements) {
    selectedEvolution.value = requirements;
    selectedEvolutionName.value = digievolutionName;
  } else if (digievolutionName === digimonName.value) {
    selectedEvolution.value = null;
    selectedEvolutionName.value = undefined;
  }
};

const derivativeParameter = computed(() => {
  return DigievolutionGridModalPresenter.getDigievolutionsById(props.digimonId);
});

const allEvolutions = computed(() => {
  const digimonDigievolutionTable = DigievolutionGridModalPresenter.getDigievolutionsById(props.digimonId);
  return Object.keys(digimonDigievolutionTable) as string[];
});
</script>

<template>
  <Modal
    :is-open="isModalOpen"
    max-width="max-w-450"
    max-height="h-[92vh] max-h-250"
    panel-class="w-[98vw]"
    @close="handleClose"
  >
    <template #header>
      <div class="flex items-center gap-6 flex-1 min-w-0">
        <h2 class="text-white font-bold tracking-widest drop-shadow flex items-center gap-2 whitespace-nowrap shrink-0">
          {{ $t("digievolution.title", { name: digimonName }) }}
        </h2>

        <DigievolutionsModalSearchBar
          :all-evolutions="allEvolutions"
          @select="handleSelectNode"
        />
      </div>
    </template>

    <div class="flex flex-1 overflow-hidden min-h-0">
      <div class="w-[75%] h-full border-r border-[#0055ff]/30 relative">
        <DigievolutionFamilyTree
          :rookie-name="digimonName"
          :digimon="digimon"
          :digimon-id="digimonId"
          :selected-node-name="selectedEvolutionName"
          @select-node="handleSelectNode"
        />
      </div>

      <div class="w-[25%] h-full bg-[#000a1a]/60 overflow-y-auto custom-scroll flex flex-col">
        <div v-if="selectedEvolution" class="flex-1 flex flex-col p-1">
          <DigievolutionDetailPanel
            :evolution="selectedEvolution"
            :evolution-name="selectedEvolutionName"
            :all-evolutions="allEvolutions"
            :derivative-parameter="derivativeParameter"
            @select-evolution="handleSelectNode"
          />
        </div>

        <div v-else class="flex-1 flex flex-col items-center justify-center p-12 text-center">
          <p class="text-[10px] text-blue-300/40 font-cyber leading-relaxed max-w-50">
            {{ $t("digievolution.selectNode") }}
          </p>
        </div>
      </div>
    </div>
  </Modal>
</template>
