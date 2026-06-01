<script setup lang="ts">
import { computed } from "vue";
import { DigimonDigievolutionPresenter } from "@/presenters/digimon-digievolution.presenter";

const props = defineProps<{
  variant: "empty" | "filled";
  digievolutionId?: number;
  digievolutionLevel?: number;
  activeDigievolutionId?: number | null;
}>();

const emit = defineEmits<{
  (e: "open-techniques", payload: { id: number; name: string; level: number }): void;
}>();

const digievolutionName = computed(() => {
  if (props.variant !== "filled" || props.digievolutionId === undefined) {
    return "";
  }

  return DigimonDigievolutionPresenter.getDigievolutionNameById(props.digievolutionId);
});

const isActiveDigievolution = computed(() => {
  if (props.variant !== "filled" || props.digievolutionId === undefined) {
    return false;
  }

  return props.digievolutionId === props.activeDigievolutionId;
});

function handleRowClick(): void {
  if (props.variant !== "filled" || props.digievolutionId === undefined || props.digievolutionLevel === undefined) {
    return;
  }

  emit("open-techniques", {
    id: props.digievolutionId,
    name: digievolutionName.value,
    level: props.digievolutionLevel,
  });
}
</script>

<template>
  <div
    class="evo-row relative flex w-full h-7 bg-[#000a2b] text-white overflow-hidden dw3-beveled"
    :class="variant === 'filled' ? 'cursor-pointer hover:brightness-125 transition-[filter]' : 'cursor-default'"
    @click="handleRowClick"
  >
    <div class="absolute inset-0 bg-[#0077ff] pointer-events-none dw3-beveled"></div>
    <div class="absolute inset-[1.5px] bg-[#000a2b] pointer-events-none dw3-beveled"></div>

    <template v-if="variant === 'empty'">
      <div class="relative z-10 flex-1 flex items-center px-4 font-bold text-sm tracking-wider text-white/80 shadow-text">
        {{ $t("digimon.states.empty") }}
      </div>
    </template>

    <template v-else>
      <div class="relative z-10 flex-1 flex items-center px-4 font-bold text-sm tracking-wider"
        :class="isActiveDigievolution ? 'bg-linear-to-b from-[#ffcc00] to-[#ff6600] text-transparent bg-clip-text shadow-text-dark' : 'shadow-text'"
      >
        {{ digievolutionName }}
      </div>

      <div class="relative z-10 w-0.5 h-full bg-[#0077ff] -skew-x-30 ml-2"></div>

      <div class="relative z-10 w-11.25 flex items-center justify-center pl-2 font-bold text-sm mr-2">
        <span class="bg-linear-to-b from-[#ffcc00] to-[#ff6600] text-transparent bg-clip-text shadow-text-dark">
          {{ digievolutionLevel }}
        </span>
      </div>
    </template>
  </div>
</template>
