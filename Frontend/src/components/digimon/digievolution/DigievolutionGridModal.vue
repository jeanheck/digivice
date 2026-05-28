<script setup lang="ts">
import { computed, ref, watch } from "vue";
import type { Digimon } from "@/models";
import Modal from "@/components/modal/Modal.vue";
import DigievolutionFamilyTree from "./DigievolutionFamilyTree.vue";
import DigievolutionDetailPanel from "./DigievolutionDetailPanel.vue";
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

const searchQuery = ref("");
const showDropdown = ref(false);

const filteredEvolutions = computed(() => {
  const query = searchQuery.value.toLowerCase();
  if (!query) {
    return [];
  }
  return allEvolutions.value.filter((evolution) => evolution.toLowerCase().includes(query));
});

const handleSearchSelect = (name: string) => {
  handleSelectNode(name);
  searchQuery.value = "";
  showDropdown.value = false;
};

const handleBlur = () => {
  window.setTimeout(() => {
    showDropdown.value = false;
  }, 200);
};
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

        <div class="relative w-lg min-w-0 flex-1 max-w-lg">
          <input
            type="text"
            v-model="searchQuery"
            :placeholder="$t('digievolution.searchDigimon')"
            class="w-full bg-[#001a33]/60 border border-[#0055ff]/50 rounded px-3 py-1 text-xs text-[#00aaff] placeholder-[#00aaff]/40 outline-none focus:border-[#00aaff] focus:bg-[#002244] font-cyber transition-all"
            @focus="showDropdown = true"
            @blur="handleBlur"
          />
          <div
            v-if="showDropdown && searchQuery && filteredEvolutions.length > 0"
            class="absolute top-full left-0 right-0 mt-1 bg-[#001122] border border-[#0055ff]/50 rounded shadow-[0_4px_12px_rgba(0,119,255,0.2)] max-h-48 overflow-y-auto custom-scroll z-50 flex flex-col"
          >
            <div
              v-for="evolution in filteredEvolutions"
              :key="evolution"
              class="px-3 py-1.5 text-xs text-[#00aaff] hover:bg-[#0033aa] hover:text-white cursor-pointer transition-colors font-cyber border-b last:border-b-0 border-[#0055ff]/20"
              @click.stop="handleSearchSelect(evolution)"
            >
              {{ evolution }}
            </div>
          </div>
        </div>
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
