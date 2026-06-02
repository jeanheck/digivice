<script setup lang="ts">
import { computed, ref, watch } from "vue";
import Modal from "@/components/modal/Modal.vue";
import StepPanel from "./StepPanel.vue";
import Steps from "./Steps.vue";
import Requisites from "./Requisites.vue";
import type { QuestViewModel } from "@/viewmodels/quest/quest.viewmodel";
import type { StepViewModel } from "@/viewmodels/quest/step.viewmodel";
import { QuestModalPresenter } from "@/presenters/journal/quest-modal.presenter.ts";

const props = defineProps<{
  questViewModel: QuestViewModel | null;
  isOpen: boolean;
}>();

const emit = defineEmits<{
  (e: "close"): void;
}>();

const isModalOpen = computed(() => {
  return props.isOpen && props.questViewModel !== null;
});

const closeModal = () => {
  emit("close");
};

const selectedStep = ref<StepViewModel | null>(null);

const selectStep = (step: StepViewModel) => {
  selectedStep.value = step;
};

const worldMapLocations = computed(() => {
  return QuestModalPresenter.getWorldMapLocations(selectedStep.value);
});

const localMapLocations = computed(() => {
  return QuestModalPresenter.getLocalMapLocations(selectedStep.value, props.questViewModel?.id ?? null);
});

watch(
  () => [props.isOpen, props.questViewModel] as const,
  ([isOpen, questViewModel]) => {
    if (isOpen && questViewModel?.currentStep) {
      selectStep(questViewModel.currentStep);
    } else {
      selectedStep.value = null;
    }
  }
);
</script>

<template>
  <Modal
    :is-open="isModalOpen"
    max-width="max-w-[clamp(64rem,50vw,90rem)]"
    max-height="max-h-[min(90vh,1000px)]"
    @close="closeModal"
  >
    <template #header>
      <h2 class="text-white font-bold tracking-widest drop-shadow">
        {{ $t(`${questViewModel!.id}.title`) }}
      </h2>
    </template>

    <div class="flex min-h-0 flex-1 flex-col gap-6 overflow-hidden p-4 lg:flex-row">
      <div class="flex-1 flex flex-col gap-4 overflow-y-auto custom-scroll pr-2">
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
