<script setup lang="ts">
import { ref } from "vue";
import DnaDigievolutionsModal from "@/ui/components/dna-digievolutions-modal/DnaDigievolutionsModal.vue";

const emit = defineEmits<{
  (e: "show-tooltip", event: MouseEvent): void;
  (e: "move-tooltip", event: MouseEvent): void;
  (e: "hide-tooltip"): void;
}>();

const isDnaDigievolutionsModalOpen = ref(false);

function onClick(): void {
  isDnaDigievolutionsModalOpen.value = true;
}

function closeDnaDigievolutionsModal(): void {
  isDnaDigievolutionsModalOpen.value = false;
}

function onMouseEnter(event: MouseEvent): void {
  emit("show-tooltip", event);
}

function onMouseMove(event: MouseEvent): void {
  emit("move-tooltip", event);
}

function onMouseLeave(): void {
  emit("hide-tooltip");
}
</script>

<template>
  <button
    type="button"
    class="inline-flex items-center justify-center leading-none text-[1.2rem] -translate-y-px cursor-pointer hover:opacity-100 opacity-90 transition-opacity"
    @click="onClick"
    @mouseenter="onMouseEnter"
    @mousemove="onMouseMove"
    @mouseleave="onMouseLeave"
  >
    <span class="font-emoji-color leading-none">&#x1F9EC;</span>
  </button>

  <DnaDigievolutionsModal
    :is-open="isDnaDigievolutionsModalOpen"
    @close="closeDnaDigievolutionsModal"
  />
</template>
