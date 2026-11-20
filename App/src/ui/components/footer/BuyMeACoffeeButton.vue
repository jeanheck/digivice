<script setup lang="ts">
import { ref } from "vue";
import BuyMeACoffeeModal from "@/ui/components/buy-me-a-coffee-modal/BuyMeACoffeeModal.vue";

const emit = defineEmits<{
  (e: "show-tooltip", event: MouseEvent): void;
  (e: "move-tooltip", event: MouseEvent): void;
  (e: "hide-tooltip"): void;
}>();

const isBuyMeACoffeeModalOpen = ref(false);

function onClick(): void {
  isBuyMeACoffeeModalOpen.value = true;
}

function closeBuyMeACoffeeModal(): void {
  isBuyMeACoffeeModalOpen.value = false;
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
    ☕
  </button>

  <BuyMeACoffeeModal :is-open="isBuyMeACoffeeModalOpen" @close="closeBuyMeACoffeeModal" />
</template>
