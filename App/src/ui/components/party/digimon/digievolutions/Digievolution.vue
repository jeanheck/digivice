<script setup lang="ts">
import { computed } from "vue";
import DigievolutionDvexp from "@/ui/components/party/digimon/digievolutions/DigievolutionDvexp.vue";
import { DigievolutionPresenter } from "@/presentation/presenters/digievolution/digievolution.presenter";
import type { DigievolutionResumedViewModel } from "@/presentation/viewmodels/digievolution/digievolution-resumed.viewmodel";
import { useGameState } from "@/ui/composables/use-game-state";
import { useDnaFusionHighlight } from "@/ui/composables/use-dna-fusion-highlight";

type DnaFusionHighlightMode = "border" | "background" | "both";

const dnaFusionHighlightMode = "both" as DnaFusionHighlightMode;

const props = defineProps<{
  digievolutionId: number | null;
  digievolutionLevel: number | null;
  digievolutionDvexp: number | null;
  activeDigievolutionId: number | null;
  digimonSlotIndex: number;
  digimonId: number;
}>();

const gameState = useGameState();
const dnaFusionHighlight = useDnaFusionHighlight();

const emit = defineEmits<{
  (e: "open-techniques", digievolutionResumedViewModel: DigievolutionResumedViewModel): void;
  (e: "show-tooltip", event: MouseEvent): void;
  (e: "move-tooltip", event: MouseEvent): void;
  (e: "hide-tooltip"): void;
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

const hasAvailableDnaFusion = computed(() => {
  if (isEmpty.value) {
    return false;
  }

  return DigievolutionPresenter.hasAvailableDnaFusion(
    gameState.value.party,
    props.digimonSlotIndex,
    props.digievolutionId!,
  );
});

const isDnaFusionHighlighted = computed(() => {
  if (isEmpty.value) {
    return false;
  }

  return dnaFusionHighlight.isHighlighted(props.digimonId, props.digievolutionId!);
});

const showsDnaFusionHighlightBorder = computed(() => {
  return isDnaFusionHighlighted.value && dnaFusionHighlightMode !== "background";
});

const showsDnaFusionHighlightBackground = computed(() => {
  return isDnaFusionHighlighted.value && dnaFusionHighlightMode !== "border";
});

function onMouseEnter(event: MouseEvent): void {
  if (!hasAvailableDnaFusion.value) {
    return;
  }

  emit("show-tooltip", event);
}

function onMouseMove(event: MouseEvent): void {
  if (!hasAvailableDnaFusion.value) {
    return;
  }

  emit("move-tooltip", event);
}

function onMouseLeave(): void {
  emit("hide-tooltip");
}

function openTechniques(): void {
  emit("open-techniques", {
    id: props.digievolutionId!,
    name: digievolutionName.value,
    level: props.digievolutionLevel!,
  });
}
</script>

<template>
  <div
    class="dw3-panel evo-row flex min-h-9 text-white dw3-beveled"
    @mouseenter="onMouseEnter"
    @mousemove="onMouseMove"
    @mouseleave="onMouseLeave"
  >
    <div class="dw3-panel-border dw3-beveled">
      <div v-if="showsDnaFusionHighlightBorder" class="dna-fusion-highlight-border"></div>
    </div>
    <div class="dw3-panel-inner dw3-beveled">
      <div v-if="showsDnaFusionHighlightBackground" class="dna-fusion-highlight-background"></div>
      <div v-if="hasAvailableDnaFusion" class="absolute inset-y-0 left-0 w-1 bg-red-400"></div>
    </div>

    <template v-if="isEmpty">
      <div class="dw3-panel-content flex flex-1 flex-col w-full py-1 hd:py-2">
        <div
          class="absolute inset-0 flex items-center justify-center px-4 font-bold text-[10px] fhdscaled:text-sm tracking-wider text-white/80 shadow-text cursor-default"
        >
          {{ $t("digimon.states.empty") }}
        </div>

        <div class="flex-1 min-h-0"></div>

        <div class="w-full shrink-0 px-4 pt-0.5">
          <div class="h-[15px] hd:h-[20px]"></div>
        </div>
      </div>
    </template>

    <div
      v-else
      class="dw3-panel-content flex flex-1 flex-col w-full py-1 hd:py-2 cursor-pointer hover:brightness-125 transition-[filter]"
      @click="openTechniques"
    >
      <div class="flex flex-1 items-center w-full min-h-0">
        <div
          class="flex-1 min-w-0 px-4 font-bold text-[12px] fhdscaled:text-sm tracking-wider truncate leading-none"
          :class="isActiveDigievolution ? 'text-dw3-gold shadow-text-dark' : 'shadow-text'"
        >
          {{ digievolutionName }}
        </div>
        <div
          class="w-11.25 shrink-0 flex items-center justify-end pl-2 pr-0.5 font-bold text-[12px] fhdscaled:text-sm mr-2"
        >
          <span :class="isActiveDigievolution ? 'text-dw3-gold shadow-text-dark' : 'shadow-text'">
            {{ digievolutionLevel }}
          </span>
        </div>
      </div>

      <div class="w-full shrink-0 pl-3.5 pr-2 pt-0.5">
        <DigievolutionDvexp
          :is-active-digievolution="isActiveDigievolution"
          :digievolution-id="digievolutionId!"
          :level="digievolutionLevel!"
          :dvexp="digievolutionDvexp!"
        />
      </div>
    </div>
  </div>
</template>
