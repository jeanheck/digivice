<script setup lang="ts">
import { ref } from "vue";
import type { DigievolutionSlot } from "@/models";
import Digievolution from "./Digievolution.vue";
import TechniquesModal from "./techniques-modal/TechniquesModal.vue";
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
      :active-digievolution-id="activeDigievolutionId"
      @open-techniques="openTechniques"
    />

    <TechniquesModal
      v-if="selectedDigievolution"
      :is-open="isTechniquesModalOpen"
      :digievolution-id="selectedDigievolution.id"
      :digievolution-name="selectedDigievolution.name"
      :digievolution-level="selectedDigievolution.level"
      @close="closeTechniques"
    />
  </div>
</template>
