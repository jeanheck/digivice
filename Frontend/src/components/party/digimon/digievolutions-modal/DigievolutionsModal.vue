<script setup lang="ts">
import { computed, ref, watch } from "vue";
import type { Digimon } from "@/models";
import Modal from "@/components/modal/Modal.vue";
import Tree from "./Tree.vue";
import DigievolutionsModalDigievolutionDetails from "./details/DigievolutionsModalDigievolutionDetails.vue";
import SearchBar from "./SearchBar.vue";
import { DigievolutionsModalPresenter } from "@/presenters/digievolutions-modal.presenter";

const props = defineProps<{
  isOpen: boolean;
  digimon: Digimon;
  digimonId: number;
}>();

const emit = defineEmits<{
  (e: "close"): void;
}>();

const isModalOpen = computed(() => {
  return props.isOpen;
});

const handleClose = () => {
  emit("close");
};

const digimonName = computed(() => {
  return DigievolutionsModalPresenter.getNameById(props.digimonId);
});

const selectedDigievolutionId = ref<number | undefined>(undefined);

watch(() => props.isOpen, (open) => {
  if (open && props.digimon) {
    selectedDigievolutionId.value = undefined;
  }
});

const handleSelectDigievolutionById = (digievolutionId: number) => {
  selectedDigievolutionId.value = digievolutionId;
};

const allDigievolutions = DigievolutionsModalPresenter.getAllDigievolutions();
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

        <SearchBar
          :all-digievolutions="allDigievolutions"
          :selected-digievolution-id="selectedDigievolutionId"
          @select-digievolution-id="handleSelectDigievolutionById"
        />
      </div>
    </template>

    <div class="flex flex-1 overflow-hidden min-h-0">
      <div class="w-[75%] h-full border-r border-[#0055ff]/30 relative">
        <Tree
          :digimon-name="digimonName"
          :digimon="digimon"
          :digimon-id="digimonId"
          :selected-digievolution-id="selectedDigievolutionId"
          @select-digievolution-id="handleSelectDigievolutionById"
        />
      </div>

      <div class="w-[25%] h-full bg-[#000a1a]/60 overflow-y-auto custom-scroll flex flex-col">
        <div v-if="selectedDigievolutionId !== undefined" class="flex-1 flex flex-col p-1">
          <DigievolutionsModalDigievolutionDetails
            :digimon-id="digimonId"
            :digievolution-id="selectedDigievolutionId"
            @select-digievolution-id="handleSelectDigievolutionById"
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
