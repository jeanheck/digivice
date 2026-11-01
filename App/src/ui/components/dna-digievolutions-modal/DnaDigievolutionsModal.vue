<script setup lang="ts">
import { computed } from "vue";
import Modal from "@/ui/components/modal/Modal.vue";
import DnaDigievolutionCard from "@/ui/components/dna-digievolutions-modal/DnaDigievolutionCard.vue";
import { DnaDigievolutionsModalPresenter } from "@/presentation/presenters/dna-digievolutions-modal/dna-digievolutions-modal.presenter";

defineProps<{
  isOpen: boolean;
}>();

const emit = defineEmits<{
  (e: "close"): void;
}>();

const dnaDigievolutions = computed(() => {
  return DnaDigievolutionsModalPresenter.getViewModels();
});

const closeModal = () => {
  emit("close");
};
</script>

<template>
  <Modal
    :is-open="isOpen"
    max-width="max-w-[1300px]"
    max-height="h-[650px] max-h-[650px]"
    panel-class="w-[1300px]"
    @close="closeModal"
  >
    <template #header>
      <h2 class="text-white font-bold tracking-widest drop-shadow whitespace-nowrap shrink-0">
        {{ $t("dnaDigievolution.title") }}
      </h2>
    </template>

    <div class="flex-1 min-h-0 overflow-y-auto custom-scroll p-4">
      <p class="text-sm text-white/90 leading-relaxed mb-4">
        {{ $t("dnaDigievolution.description") }}
      </p>

      <div class="grid grid-cols-2 gap-4">
        <DnaDigievolutionCard
          v-for="dnaDigievolution in dnaDigievolutions"
          :key="dnaDigievolution.id"
          :dna-digievolution="dnaDigievolution"
        />
      </div>
    </div>
  </Modal>
</template>
