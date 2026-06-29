<script setup lang="ts">
import { computed, ref, watch } from "vue";
import Modal from "@/components/modal/Modal.vue";
import StepPanel from "./StepPanel.vue";
import Steps from "./Steps.vue";
import Requisites from "./Requisites.vue";
import type { StepViewModel } from "@/viewmodels/quest/step.viewmodel";
import { QuestModalPresenter } from "@/presenters/journal/quest-modal.presenter.ts";
import { useGameStore } from "@/stores/use-game-store";

const props = defineProps<{
  questId: string | null;
  isOpen: boolean;
}>();

const emit = defineEmits<{
  (e: "close"): void;
}>();

const store = useGameStore();

const questViewModel = computed(() => {
  if (props.questId === null) {
    return null;
  }

  const journal = store.currentState?.journal;
  if (journal === null || journal === undefined) {
    return null;
  }

  return QuestModalPresenter.getQuestViewModel(journal, props.questId);
});

const isModalOpen = computed(() => {
  return props.isOpen && questViewModel.value !== null;
});

const closeModal = () => {
  emit("close");
};

const selectedStepNumber = ref<string | null>(null);

const selectedStep = computed(() => {
  if (questViewModel.value === null || selectedStepNumber.value === null) {
    return null;
  }

  return questViewModel.value.steps.find((step) => step.number === selectedStepNumber.value) ?? null;
});

const selectStep = (step: StepViewModel) => {
  selectedStepNumber.value = step.number;
};

const worldMapLocations = computed(() => {
  return QuestModalPresenter.getWorldMapLocations(selectedStep.value);
});

const localMapLocations = computed(() => {
  return QuestModalPresenter.getLocalMapLocations(selectedStep.value, props.questId);
});

const currentStepNumber = computed(() => {
  return questViewModel.value?.currentStep?.number ?? null;
});

watch(
  () => [props.isOpen, props.questId] as const,
  ([isOpen, questId]) => {
    if (!isOpen || questId === null) {
      selectedStepNumber.value = null;
      return;
    }

    selectedStepNumber.value = currentStepNumber.value;
  }
);

watch(currentStepNumber, (nextCurrentStepNumber, previousCurrentStepNumber) => {
  if (!props.isOpen) {
    return;
  }

  if (previousCurrentStepNumber === undefined) {
    return;
  }

  if (selectedStepNumber.value !== previousCurrentStepNumber) {
    return;
  }

  if (nextCurrentStepNumber === previousCurrentStepNumber) {
    return;
  }

  selectedStepNumber.value = nextCurrentStepNumber;
});
</script>

<template>
  <Modal
    :is-open="isModalOpen"
    max-width="max-w-450"
    max-height="h-[92vh] max-h-250"
    panel-class="w-[98vw]"
    @close="closeModal"
  >
    <template #header>
      <h2 class="text-white font-bold tracking-widest drop-shadow">
        {{ $t(`${questViewModel!.id}.title`) }}
      </h2>
    </template>

    <div class="flex min-h-0 flex-1 flex-col gap-6 overflow-hidden p-4 lg:flex-row">
      <div class="flex min-w-0 flex-1 flex-col gap-4 overflow-y-auto custom-scroll lg:flex-[1.4] lg:pr-2">
        <div class="bg-[#000a1a] p-3 rounded border border-blue-900/50 shadow-inner">
          <p class="text-gray-300 text-sm leading-relaxed font-medium">
            {{ $t(`${questViewModel!.id}.description`) }}
          </p>
        </div>

        <Requisites
          :items="questViewModel!.requisites"
          :translation-key-prefix="`${questViewModel!.id}.requisites`"
          variant="quest"
          title-key="journal.prerequisites"
        />

        <Steps
          :steps="questViewModel!.steps"
          :quest-id="questViewModel!.id"
          :selected-step-number="selectedStep?.number ?? null"
          @select="selectStep"
        />
      </div>

      <StepPanel
        :selected-step="selectedStep"
        :world-map-locations="worldMapLocations"
        :local-map-locations="localMapLocations"
      />
    </div>
  </Modal>
</template>
