<script setup lang="ts">
import { computed } from "vue";
import { DigievolutionPresenter } from "@/presenters/digievolution.presenter";
import type { DigievolutionResumedViewModel } from "@/viewmodels/digievolution/digievolution-resumed.viewmodel";

const props = defineProps<{
  digievolutionId: number | null;
  digievolutionLevel: number | null;
  activeDigievolutionId: number | null;
}>();

const emit = defineEmits<{
  (e: "open-techniques", digievolutionResumedViewModel: DigievolutionResumedViewModel): void;
}>();

const isEmpty = computed(() => {
  return props.digievolutionId === null;
});

const digievolutionName = computed(() => {
  if (isEmpty.value) {
    return "";
  }

  return DigievolutionPresenter.getNameById(props.digievolutionId!);
});

const isActiveDigievolution = computed(() => {
  if (isEmpty.value) {
    return false;
  }

  return props.digievolutionId === props.activeDigievolutionId;
});

function openTechniques(): void {
  emit("open-techniques", {
    id: props.digievolutionId!,
    name: digievolutionName.value,
    level: props.digievolutionLevel!,
  });
}
</script>

<template>
  <div class="dw3-panel evo-row flex h-7 text-white dw3-beveled">
    <div class="dw3-panel-border dw3-beveled"></div>
    <div class="dw3-panel-inner dw3-beveled"></div>

    <template v-if="isEmpty">
      <div class="dw3-panel-content flex-1 flex items-center px-4 font-bold text-sm tracking-wider text-white/80 shadow-text cursor-default">
        {{ $t("digimon.states.empty") }}
      </div>
    </template>

    <div
      v-else
      class="dw3-panel-content flex flex-1 w-full items-center cursor-pointer hover:brightness-125 transition-[filter]"
      @click="openTechniques"
    >
      <div
        class="flex-1 flex items-center px-4 font-bold text-sm tracking-wider"
        :class="isActiveDigievolution ? 'bg-linear-to-b from-[#ffcc00] to-[#ff6600] text-transparent bg-clip-text shadow-text-dark' : 'shadow-text'"
      >
        {{ digievolutionName }}
      </div>

      <div class="w-0.5 h-full bg-[#0077ff] -skew-x-30 ml-2"></div>

      <div class="w-11.25 flex items-center justify-center pl-2 font-bold text-sm mr-2">
        <span class="bg-linear-to-b from-[#ffcc00] to-[#ff6600] text-transparent bg-clip-text shadow-text-dark">
          {{ digievolutionLevel }}
        </span>
      </div>
    </div>
  </div>
</template>
