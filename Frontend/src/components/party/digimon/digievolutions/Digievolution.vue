<script setup lang="ts">
import { computed } from "vue";
import DigievolutionDvxpSegments from "@/components/party/digimon/digievolutions/DigievolutionDvxpSegments.vue";
import { DigievolutionPresenter } from "@/presenters/digievolution/digievolution.presenter";
import type { DigievolutionResumedViewModel } from "@/viewmodels/digievolution/digievolution-resumed.viewmodel";

const props = defineProps<{
  digievolutionId: number | null;
  digievolutionLevel: number | null;
  digievolutionDvxp: number | null;
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
  <div class="dw3-panel evo-row flex min-h-9 text-white dw3-beveled">
    <div class="dw3-panel-border dw3-beveled"></div>
    <div class="dw3-panel-inner dw3-beveled"></div>

    <template v-if="isEmpty">
      <div class="dw3-panel-content flex flex-1 flex-col w-full py-1">
        <div class="flex flex-1 items-center w-full min-h-0 px-4 font-bold text-sm tracking-wider text-white/80 shadow-text cursor-default">
          {{ $t("digimon.states.empty") }}
        </div>

        <div class="w-full shrink-0 px-4 pt-0.5 h-1" aria-hidden="true"></div>
      </div>
    </template>

    <div
      v-else
      class="dw3-panel-content flex flex-1 flex-col w-full py-1 cursor-pointer hover:brightness-125 transition-[filter]"
      @click="openTechniques"
    >
      <div class="flex flex-1 items-center w-full min-h-0">
        <div
          class="flex-1 min-w-0 px-4 font-bold text-sm tracking-wider truncate leading-none"
          :class="isActiveDigievolution ? 'text-dw3-gold shadow-text-dark' : 'shadow-text'"
        >
          {{ digievolutionName }}
        </div>

        <div class="w-0.5 shrink-0 self-stretch bg-[#0077ff] -skew-x-30"></div>

        <div class="w-11.25 shrink-0 flex items-center justify-center pl-2 font-bold text-sm mr-2">
          <span
            :class="isActiveDigievolution ? 'text-dw3-gold shadow-text-dark' : 'shadow-text'"
          >
            {{ digievolutionLevel }}
          </span>
        </div>
      </div>

      <div class="w-full shrink-0 px-4 pt-0.5">
        <DigievolutionDvxpSegments
          :is-active-digievolution="isActiveDigievolution"
          :dvxp="digievolutionDvxp ?? 0"
        />
      </div>
    </div>
  </div>
</template>
