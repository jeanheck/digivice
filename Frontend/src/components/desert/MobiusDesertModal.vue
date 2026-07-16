<script setup lang="ts">
import { computed } from "vue";
import Modal from "@/components/modal/Modal.vue";
import MobiusDesertMap from "@/components/desert/MobiusDesertMap.vue";
import MobiusDesertArea from "@/components/desert/MobiusDesertArea.vue";

const props = defineProps<{
  isOpen: boolean;
}>();

const emit = defineEmits<{
  (e: "close"): void;
}>();

const isModalOpen = computed(() => {
  return props.isOpen;
});

function closeModal(): void {
  emit("close");
}
</script>

<template>
  <Modal
    :is-open="isModalOpen"
    max-width="max-w-[1300px]"
    max-height="h-[650px] max-h-[650px]"
    panel-class="w-[1300px]"
    @close="closeModal"
  >
    <template #header>
      <h2 class="text-white font-bold tracking-widest drop-shadow whitespace-nowrap shrink-0">
        {{ $t("map.mobiusDesert") }}
      </h2>
    </template>

    <div class="flex flex-1 min-h-0 h-full w-full">
      <div class="h-full min-h-0 w-1/2">
        <MobiusDesertMap />
      </div>
      <div class="h-full min-h-0 w-1/2">
        <MobiusDesertArea />
      </div>
    </div>
  </Modal>
</template>
