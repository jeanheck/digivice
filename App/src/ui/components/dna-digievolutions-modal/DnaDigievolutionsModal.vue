<script setup lang="ts">
import Modal from "@/ui/components/modal/Modal.vue";

defineProps<{
  isOpen: boolean;
}>();

const emit = defineEmits<{
  (e: "close"): void;
}>();

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
  </Modal>
</template>
