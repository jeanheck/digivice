<script setup lang="ts">
import { ref, computed } from "vue";
import DigievolutionTechniquesModal from "@/components/modal/digievolution-techniques-modal/DigievolutionTechniquesModal.vue";
import { DigimonDigievolutionPresenter } from "@/presenters/digimon-digievolution.presenter";

const props = defineProps<{
  digievolutionId: number;
  digievolutionLevel: number;
  activeDigievolutionId: number | null;
}>();

const digievolutionName = computed(() => {
  return DigimonDigievolutionPresenter.getDigievolutionNameById(props.digievolutionId);
});

const modalOpen = ref(false);

function openSkills(): void {
  modalOpen.value = true;
}
function closeSkills(): void {
  modalOpen.value = false;
}
</script>

<template>
  <div
    class="evo-row relative flex w-full h-7 bg-[#000a2b] text-white overflow-hidden dw3-beveled cursor-pointer hover:brightness-125 transition-[filter]"
    @click="openSkills"
  >
    <div class="absolute inset-0 bg-[#0077ff] pointer-events-none dw3-beveled"></div>
    <div class="absolute inset-[1.5px] bg-[#000a2b] pointer-events-none dw3-beveled"></div>

    <div
      class="relative z-10 flex-1 flex items-center px-4 font-bold text-sm tracking-wider"
      :class="digievolutionId === activeDigievolutionId ? 'bg-linear-to-b from-[#ffcc00] to-[#ff6600] text-transparent bg-clip-text shadow-text-dark' : 'shadow-text'"
    >
      {{ digievolutionName }}
    </div>

    <div class="relative z-10 w-0.5 h-full bg-[#0077ff] -skew-x-30 ml-2"></div>

    <div class="relative z-10 w-11.25 flex items-center justify-center pl-2 font-bold text-sm mr-2">
      <span class="bg-linear-to-b from-[#ffcc00] to-[#ff6600] text-transparent bg-clip-text shadow-text-dark">
        {{ digievolutionLevel }}
      </span>
    </div>
  </div>

  <DigievolutionTechniquesModal
    :is-open="modalOpen"
    :digievolution-id="digievolutionId"
    :digievolution-name="digievolutionName"
    @close="closeSkills"
  />
</template>
