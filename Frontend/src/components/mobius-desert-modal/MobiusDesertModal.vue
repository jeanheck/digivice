<script setup lang="ts">
import { computed, ref, watch } from "vue";
import Modal from "@/components/modal/Modal.vue";
import MobiusDesertAreas from "@/components/mobius-desert-modal/MobiusDesertAreas.vue";
import MobiusDesertAreaDetails from "@/components/mobius-desert-modal/MobiusDesertAreaDetails.vue";
import type { DesertAreaViewModel } from "@/viewmodels/desert/desert-area.viewmodel";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

const props = defineProps<{
  isOpen: boolean;
  location: LocationViewModel | null;
}>();

const emit = defineEmits<{
  (e: "close"): void;
}>();

const isModalOpen = computed(() => {
  return props.isOpen;
});

const selectedArea = ref<DesertAreaViewModel | null>(null);

watch(
  () => props.isOpen,
  (isOpen) => {
    if (isOpen) {
      selectedArea.value = null;
    }
  },
);

function onSelectArea(area: DesertAreaViewModel): void {
  selectedArea.value = area;
}

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
        <MobiusDesertAreas @select-area="onSelectArea" />
      </div>
      <div class="h-full min-h-0 w-1/2">
        <MobiusDesertAreaDetails :selected-area="selectedArea" />
      </div>
    </div>
  </Modal>
</template>
