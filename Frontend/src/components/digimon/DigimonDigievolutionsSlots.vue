<script setup lang="ts">
import { ref } from "vue";
import type { DigievolutionSlot } from "@/models";
import DigimonDigievolutionSlot from "@/components/digimon/DigimonDigievolutionSlot.vue";
import DigievolutionTechniquesModal from "@/components/modal/digievolution-techniques-modal/DigievolutionTechniquesModal.vue";

defineProps<{
  slots: DigievolutionSlot[];
  activeDigievolutionId: number | null;
}>();

interface SelectedDigievolution {
  id: number;
  name: string;
  level: number;
}

const isTechniquesModalOpen = ref(false);
const selectedDigievolution = ref<SelectedDigievolution | null>(null);

function openTechniques(payload: SelectedDigievolution): void {
  selectedDigievolution.value = payload;
  isTechniquesModalOpen.value = true;
}

function closeTechniques(): void {
  isTechniquesModalOpen.value = false;
}
</script>

<template>
  <div class="flex flex-col gap-0.5 w-full">
    <template v-for="slot in slots" :key="slot.index">
      <DigimonDigievolutionSlot
        v-if="slot.digievolutionId && slot.digievolution"
        variant="filled"
        :digievolution-id="slot.digievolutionId"
        :digievolution-level="slot.digievolution.level"
        :active-digievolution-id="activeDigievolutionId"
        @open-techniques="openTechniques"
      />
      <DigimonDigievolutionSlot v-else variant="empty" />
    </template>

    <DigievolutionTechniquesModal
      v-if="selectedDigievolution"
      :is-open="isTechniquesModalOpen"
      :digievolution-id="selectedDigievolution.id"
      :digievolution-name="selectedDigievolution.name"
      :digievolution-level="selectedDigievolution.level"
      @close="closeTechniques"
    />
  </div>
</template>
