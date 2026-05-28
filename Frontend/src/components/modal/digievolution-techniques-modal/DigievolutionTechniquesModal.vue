<script setup lang="ts">
import { computed } from "vue";
import Modal from "@/components/modal/Modal.vue";
import Technique from "@/components/modal/digievolution-techniques-modal/Technique.vue";
import { DigievolutionTechniquesModalPresenter } from "@/presenters/digievolution-techniques-modal.presenter";

const props = defineProps<{
  isOpen: boolean;
  digievolutionId: number;
  digievolutionName: string;
  digievolutionLevel: number;
}>();

const emit = defineEmits<{
  (e: "close"): void;
}>();

const handleClose = () => {
  emit("close");
};

const digievolutionTechniques = computed(() => {
  return DigievolutionTechniquesModalPresenter.getTechniquesByDigievolutionId(props.digievolutionId);
});

const signatureTechniqueId = computed(() => {
  return DigievolutionTechniquesModalPresenter.getSignatureTechnique(digievolutionTechniques.value);
});
</script>

<template>
  <Modal
    :is-open="isOpen"
    max-width="max-w-md"
    max-height="max-h-[70vh]"
    @close="handleClose"
  >
    <template #header>
      <h2 class="flex items-center gap-2 text-sm font-bold tracking-widest text-white">
        <span class="text-yellow-400">⚡</span>
        {{ digievolutionName }}
      </h2>
    </template>

    <div class="flex min-h-0 flex-1 flex-col gap-0.75 overflow-y-auto p-3 custom-scroll">
      <Technique
        v-for="technique in digievolutionTechniques"
        :key="technique.id"
        :technique="technique"
        :digievolution-level="digievolutionLevel"
        :is-signature="signatureTechniqueId === technique.id"
      />

      <p v-if="digievolutionTechniques.length === 0" class="py-4 text-center text-xs text-white/40">
        {{ $t("digievolution.noTechData") }}
      </p>
    </div>
  </Modal>
</template>
