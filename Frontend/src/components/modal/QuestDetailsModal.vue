<script setup lang="ts">
import { computed, ref, watch } from "vue";
import Modal from "@/components/modal/Modal.vue";
import Requisites from "@/components/modal/Requisites.vue";
import ZoomedLocationMap from "@/components/modal/ZoomedLocationMap.vue";
import type { ZoomedLocationMapItem } from "@/components/modal/ZoomedLocationMap.vue";
import asukaMapUrl from "@/assets/AsukaMap.webp";
import { useLocalization } from "@/composables/useLocalization";
import type { QuestViewModel } from "@/viewmodels/quest/quest.viewmodel";
import type { StepViewModel } from "@/viewmodels/quest/step.viewmodel";
import { QuestDetailsModalPresenter } from "@/presenters/quest-details-modal.presenter";

const props = defineProps<{
  questViewModel: QuestViewModel | null;
  isOpen: boolean;
}>();

const { t } = useLocalization();

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

const worldMapLocations = computed((): ZoomedLocationMapItem[] => {
  if (!selectedStep.value?.location || !selectedStep.value.coordinates) {
    return [];
  }

  return [{
    imageUrl: asukaMapUrl,
    coordinates: selectedStep.value.coordinates,
    labelKey: `location.${selectedStep.value.location}`,
  }];
});

const localMapLocations = computed((): ZoomedLocationMapItem[] => {
  if (!selectedStep.value?.zoomedLocations?.length || !props.questViewModel) {
    return [];
  }

  return selectedStep.value.zoomedLocations.map((zoomedLocation, locationIndex) => {
    return {
      imageUrl: QuestDetailsModalPresenter.getLocalMapUrl(zoomedLocation.location),
      coordinates: zoomedLocation.coordinates,
      labelKey: `${props.questViewModel!.id}.steps.${selectedStep.value!.number}.locations.${locationIndex}.locationTarget`,
    };
  });
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
    max-width="max-w-5xl"
    max-height="max-h-[90vh]"
    @close="closeModal"
  >
    <template #header>
      <h2 class="text-white font-bold tracking-widest drop-shadow">
        {{ t(`${questViewModel!.id}.title`) }}
      </h2>
    </template>

    <div class="flex min-h-0 flex-1 flex-col gap-6 overflow-hidden p-4 lg:flex-row">
      <div class="flex-1 flex flex-col gap-4 overflow-y-auto custom-scroll pr-2">
        <div class="bg-[#000a1a] p-3 rounded border border-blue-900/50 shadow-inner">
          <p class="text-gray-300 text-sm leading-relaxed font-medium">
            {{ t(`${questViewModel!.id}.description`) }}
          </p>
        </div>

        <Requisites
          :items="questViewModel!.requisites"
          :translation-key-prefix="`${questViewModel!.id}.requisites`"
          variant="quest"
          title-key="journal.prerequisites"
        />

        <div class="flex flex-col gap-2">
          <h3 class="text-xs text-blue-500 font-bold uppercase tracking-wider mb-1 border-b border-blue-900/40 pb-1">
            {{ $t("journal.missionSteps") }}
          </h3>

          <div
            v-for="stepViewModel in questViewModel!.steps"
            :key="stepViewModel.number"
            class="flex items-start gap-3 p-2 rounded transition-all cursor-pointer group"
            :class="[
              stepViewModel.isDone ? 'bg-green-900/10 border border-green-800/30' : 'bg-white/5 border border-white/10',
              selectedStep?.number === stepViewModel.number ? 'ring-1 ring-cyan-500 shadow-[0_0_10px_rgba(0,255,255,0.2)] bg-[#001a33]' : '',
            ]"
            @click="selectStep(stepViewModel)"
          >
            <div
              class="mt-0.5 shrink-0 w-5 h-5 rounded border-2 flex items-center justify-center transition-colors shadow-inner"
              :class="stepViewModel.isDone ? 'bg-green-500/20 border-green-500 text-green-400 shadow-[0_0_8px_rgba(0,255,0,0.3)]' : 'bg-black/50 border-gray-600'"
            >
              <span v-if="stepViewModel.isDone" class="text-xs">✔</span>
            </div>

            <div class="flex-1">
              <p
                class="text-sm leading-snug transition-colors"
                :class="stepViewModel.isDone ? 'text-gray-400 line-through decoration-green-900' : 'text-gray-200'"
              >
                {{ t(`${questViewModel!.id}.steps.${stepViewModel.number}.description`) }}
              </p>
              <Requisites
                :items="stepViewModel.requisites"
                :translation-key-prefix="`${questViewModel!.id}.steps.${stepViewModel.number}.requisites`"
                variant="step"
              />
            </div>
          </div>

          <div v-if="!questViewModel!.steps || questViewModel!.steps.length === 0" class="text-center p-3 opacity-50">
            <span class="text-gray-500 text-sm italic">{{ $t("journal.noSteps") }}</span>
          </div>
        </div>
      </div>

      <div class="flex w-full min-h-0 shrink-0 flex-col gap-4 overflow-hidden lg:w-112.5 lg:border-l lg:border-[#0055ff]/30 lg:pl-6">
        <div v-if="!selectedStep" class="flex-1 flex flex-col items-center justify-center border border-cyan-900/40 bg-[#000a1a] rounded min-h-100">
          <span class="text-cyan-500/50 font-cyber text-sm tracking-widest text-center px-8 animate-pulse" v-html="$t('journal.clickStep').replace('\n', '<br/>')"></span>
        </div>

        <div
          v-else-if="!selectedStep.location && (!selectedStep.zoomedLocations || selectedStep.zoomedLocations.length === 0)"
          class="flex-1 flex flex-col items-center justify-center border border-red-900/40 bg-[#1a0000] rounded min-h-100"
        >
          <span class="text-red-500/50 font-cyber text-sm tracking-widest text-center px-8" v-html="$t('journal.noSignal').replace('\n', '<br/>')"></span>
        </div>

        <template v-else>
          <ZoomedLocationMap
            v-if="worldMapLocations.length > 0"
            map-variant="world"
            :locations="worldMapLocations"
          />

          <ZoomedLocationMap
            v-if="localMapLocations.length > 0"
            map-variant="local"
            :locations="localMapLocations"
          />
        </template>
      </div>
    </div>
  </Modal>
</template>
