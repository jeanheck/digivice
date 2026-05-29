<script setup lang="ts">
import { computed, ref, watch } from "vue";
import Modal from "@/components/modal/Modal.vue";
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

const hasRequisites = computed(() => {
  return props.questViewModel?.requisites && props.questViewModel.requisites.length > 0;
});

const selectedStep = ref<StepViewModel | null>(null);
const currentLocationIndex = ref(0);

const selectStep = (step: StepViewModel) => {
  selectedStep.value = step;
  currentLocationIndex.value = 0;
};

const currentLocation = computed(() => {
  if (!selectedStep.value?.zoomedLocations || selectedStep.value.zoomedLocations.length === 0) {
    return null;
  }
  return selectedStep.value.zoomedLocations[currentLocationIndex.value];
});

watch(
  () => [props.isOpen, props.questViewModel] as const,
  ([isOpen, questViewModel]) => {
    if (isOpen && questViewModel?.currentStep) {
      selectStep(questViewModel.currentStep);
    } else {
      selectedStep.value = null;
      currentLocationIndex.value = 0;
    }
  }
);

function formatTooltipText(text: string) {
  if (!text) {
    return "";
  }
  const words = text.split(" ");
  const lines: string[] = [];
  let currentLine = "";

  for (const word of words) {
    if (currentLine === "") {
      currentLine = word;
    } else if (currentLine.length > 10) {
      lines.push(currentLine);
      currentLine = word;
    } else {
      currentLine += " " + word;
    }
  }
  if (currentLine) {
    lines.push(currentLine);
  }
  return lines.join("<br/>");
}
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

        <div v-if="hasRequisites" class="flex flex-col gap-2">
          <h3 class="text-xs text-amber-500 font-bold uppercase tracking-wider mb-1 border-b border-amber-900/40 pb-1">
            {{ $t("journal.prerequisites") }}
          </h3>
          <div
            v-for="requisiteViewModel in questViewModel!.requisites"
            :key="requisiteViewModel.id"
            class="flex items-start gap-3 p-2 rounded transition-colors"
            :class="requisiteViewModel.isDone ? 'bg-green-900/10 border border-green-800/30' : 'bg-red-900/10 border border-red-800/30'"
          >
            <div
              class="mt-0.5 shrink-0 w-5 h-5 rounded border-2 flex items-center justify-center transition-colors shadow-inner"
              :class="requisiteViewModel.isDone ? 'bg-green-500/20 border-green-500 text-green-400 shadow-[0_0_8px_rgba(0,255,0,0.3)]' : 'bg-red-500/10 border-red-500/60 text-red-400'"
            >
              <span v-if="requisiteViewModel.isDone" class="text-xs">✔</span>
              <span v-else class="text-xs">✘</span>
            </div>
            <p
              class="text-sm flex-1 leading-snug transition-colors"
              :class="requisiteViewModel.isDone ? 'text-gray-400 line-through decoration-green-900' : 'text-red-300'"
            >
              {{ t(`${questViewModel!.id}.requisites.${requisiteViewModel.id}`) }}
            </p>
          </div>
        </div>

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
              <div v-if="stepViewModel.requisites && stepViewModel.requisites.length > 0" class="mt-1.5 ml-1 flex flex-col gap-1">
                <div
                  v-for="(requisiteViewModel, prerequisiteIndex) in stepViewModel.requisites"
                  :key="prerequisiteIndex"
                  class="flex items-center gap-2 text-xs"
                >
                  <span :class="requisiteViewModel.isDone ? 'text-green-400' : 'text-gray-500'">{{ requisiteViewModel.isDone ? "✔" : "○" }}</span>
                  <span :class="requisiteViewModel.isDone ? 'text-gray-400 line-through' : 'text-gray-400'">
                    {{ t(`${questViewModel!.id}.steps.${stepViewModel.number}.requisites.${requisiteViewModel.id}`) }}
                  </span>
                </div>
              </div>
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
          <div
            v-if="selectedStep.location"
            class="relative w-full min-h-0 flex-1 aspect-4/3 bg-[#00051a] border border-cyan-800/50 rounded overflow-hidden shadow-[0_0_15px_rgba(0,170,255,0.1)] group"
          >
            <img :src="asukaMapUrl" class="w-full h-full object-cover opacity-60 mix-blend-screen saturate-50 group-hover:saturate-100 transition-all duration-500" />
            <div class="absolute inset-0 bg-blue-900/10 z-0 pointer-events-none"></div>

            <div
              v-if="selectedStep.coordinates"
              class="absolute w-8 h-8 -translate-x-1/2 -translate-y-1/2 z-10 flex items-center justify-center pointer-events-none"
              :style="{ left: selectedStep.coordinates.x + '%', top: selectedStep.coordinates.y + '%' }"
            >
              <div class="absolute inset-0 rounded-full border border-cyan-400 animate-ping opacity-90"></div>
              <div class="w-2.5 h-2.5 rounded-full bg-cyan-400 shadow-[0_0_10px_rgba(0,255,255,1)]"></div>
              <div
                class="absolute left-1/2 -translate-x-1/2 text-[10px] font-cyber text-cyan-100 drop-shadow bg-cyan-950/95 px-3 py-1 rounded border border-cyan-700/80 max-w-37.5 leading-tight text-center z-20 shadow-[0_0_10px_rgba(0,0,0,0.5)]"
                :class="selectedStep.coordinates.y < 20 ? 'top-6.5' : 'bottom-6.5'"
                v-html="formatTooltipText(t(`location.${selectedStep.location}`))"
              ></div>
            </div>
          </div>

          <div
            v-if="currentLocation"
            class="relative flex w-full min-h-0 flex-1 flex-col aspect-4/3 bg-[#00051a] border border-cyan-800/50 rounded overflow-hidden shadow-[0_0_15px_rgba(0,170,255,0.1)] group"
          >
            <div class="relative flex-1 w-full bg-black/50 overflow-hidden">
              <img
                :src="QuestDetailsModalPresenter.getLocalMapUrl(currentLocation.location) ?? undefined"
                class="absolute inset-0 w-full h-full object-cover opacity-70 group-hover:opacity-100 transition-opacity duration-500"
              />

              <div
                v-if="currentLocation.coordinates"
                class="absolute w-6 h-6 -translate-x-1/2 -translate-y-1/2 z-10 flex items-center justify-center pointer-events-none transition-all duration-300"
                :style="{ left: currentLocation.coordinates.x + '%', top: currentLocation.coordinates.y + '%' }"
              >
                <div class="absolute inset-0 rounded-full border border-cyan-400 animate-ping opacity-90"></div>
                <div class="w-2 h-2 rounded-full bg-cyan-400 shadow-[0_0_8px_rgba(0,255,255,1)]"></div>
                <div
                  class="absolute left-1/2 -translate-x-1/2 text-[9px] font-cyber text-cyan-100 drop-shadow bg-cyan-950/95 px-2 py-0.5 rounded border border-cyan-700/80 max-w-30 leading-tight text-center z-20 shadow-[0_0_10px_rgba(0,0,0,0.5)]"
                  :class="currentLocation.coordinates.y < 25 ? 'top-6.25' : 'bottom-6.25'"
                  v-html="formatTooltipText(t(`${questViewModel!.id}.steps.${selectedStep.number}.locations.${currentLocationIndex}.locationTarget`))"
                ></div>
              </div>
            </div>

            <div
              v-if="selectedStep.zoomedLocations && selectedStep.zoomedLocations.length > 1"
              class="absolute bottom-3 left-0 right-0 flex items-center justify-center gap-3 z-20"
            >
              <button
                class="w-7 h-7 rounded bg-black/80 border border-cyan-800 flex items-center justify-center text-cyan-400 hover:bg-cyan-900/80 hover:border-cyan-400 hover:text-white transition-all font-bold text-sm shadow-[0_0_10px_rgba(0,170,255,0.2)]"
                @click.prevent="currentLocationIndex = (currentLocationIndex - 1 + selectedStep.zoomedLocations.length) % selectedStep.zoomedLocations.length"
              >
                &lt;
              </button>

              <div class="flex gap-2 px-3 py-1.5 bg-black/80 rounded border border-cyan-900/80 shadow-[0_0_10px_rgba(0,170,255,0.2)]">
                <div
                  v-for="(_, locationDotIndex) in selectedStep.zoomedLocations"
                  :key="locationDotIndex"
                  class="w-2 h-2 rounded-full transition-all cursor-pointer"
                  :class="Number(locationDotIndex) === currentLocationIndex ? 'bg-cyan-400 shadow-[0_0_8px_rgba(0,255,255,1)] scale-110' : 'bg-cyan-900 hover:bg-cyan-600'"
                  @click.prevent="currentLocationIndex = Number(locationDotIndex)"
                ></div>
              </div>

              <button
                class="w-7 h-7 rounded bg-black/80 border border-cyan-800 flex items-center justify-center text-cyan-400 hover:bg-cyan-900/80 hover:border-cyan-400 hover:text-white transition-all font-bold text-sm shadow-[0_0_10px_rgba(0,170,255,0.2)]"
                @click.prevent="currentLocationIndex = (currentLocationIndex + 1) % selectedStep.zoomedLocations.length"
              >
                &gt;
              </button>
            </div>
          </div>
        </template>
      </div>
    </div>
  </Modal>
</template>
