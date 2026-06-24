<script setup lang="ts">
import { ref } from "vue";
import Modal from "@/components/modal/Modal.vue";
import DigievolutionTechniques from "@/components/party/digimon/digievolution-techniques/DigievolutionTechniques.vue";
import type { DigievolutionSlot } from "@/models";
import Digievolution from "./Digievolution.vue";
import type { DigievolutionResumedViewModel } from "@/viewmodels/digievolution/digievolution-resumed.viewmodel";

defineProps<{
  slots: DigievolutionSlot[];
  activeDigievolutionId: number | null;
}>();

const isTechniquesModalOpen = ref(false);
const selectedDigievolution = ref<DigievolutionResumedViewModel | null>(null);

function openTechniques(digievolutionResumedViewModel: DigievolutionResumedViewModel): void {
  selectedDigievolution.value = digievolutionResumedViewModel;
  isTechniquesModalOpen.value = true;
}

function closeTechniques(): void {
  isTechniquesModalOpen.value = false;
}
</script>

<template>
  <div class="flex flex-col gap-0.5 w-full">
    <Digievolution
      v-for="slot in slots"
      :key="slot.index"
      :digievolution-id="slot.digievolutionId"
      :digievolution-level="slot.digievolution?.level ?? null"
      :digievolution-dvxp="slot.digievolution?.dvxp ?? null"
      :active-digievolution-id="activeDigievolutionId"
      @open-techniques="openTechniques"
    />

    <Modal
      v-if="selectedDigievolution"
      :is-open="isTechniquesModalOpen"
      max-width="max-w-lg"
      max-height="max-h-[75vh]"
      :show-footer-bar="false"
      @close="closeTechniques"
    >
      <DigievolutionTechniques
        :digievolution-id="selectedDigievolution.id"
        :digievolution-level="selectedDigievolution.level"
      />
    </Modal>
  </div>
</template>
