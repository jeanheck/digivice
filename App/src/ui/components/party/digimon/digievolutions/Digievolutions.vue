<script setup lang="ts">
import { computed, onBeforeUnmount, ref } from "vue";
import Modal from "@/ui/components/modal/Modal.vue";
import DigievolutionTechniques from "@/ui/components/party/digimon/digievolution-techniques/DigievolutionTechniques.vue";
import type { DigievolutionSlot } from "@/state/models";
import Digievolution from "./Digievolution.vue";
import DnaFusionTooltip from "./DnaFusionTooltip.vue";
import type { DigievolutionResumedViewModel } from "@/presentation/viewmodels/digievolution/digievolution-resumed.viewmodel";
import { DigievolutionPresenter } from "@/presentation/presenters/digievolution/digievolution.presenter";
import { useGameState } from "@/ui/composables/use-game-state";
import { useTooltipPosition } from "@/ui/composables/use-tooltip-position";
import { useDnaFusionHighlight } from "@/ui/composables/use-dna-fusion-highlight";

const props = defineProps<{
  slots: DigievolutionSlot[];
  activeDigievolutionId: number | null;
  digimonSlotIndex: number;
  digimonId: number;
}>();

const gameState = useGameState();
const dnaFusionHighlight = useDnaFusionHighlight();

const isTechniquesModalOpen = ref(false);
const selectedDigievolution = ref<DigievolutionResumedViewModel | null>(null);

function openTechniques(digievolutionResumedViewModel: DigievolutionResumedViewModel): void {
  selectedDigievolution.value = digievolutionResumedViewModel;
  isTechniquesModalOpen.value = true;
}

function closeTechniques(): void {
  isTechniquesModalOpen.value = false;
}

const tooltipPlacement = "below" as const;
const tooltipMaxWidth = 460;
const tooltipPosition = useTooltipPosition(tooltipMaxWidth);
const { show: tooltipShow, x: tooltipX, y: tooltipY, showAt, move, hide } = tooltipPosition;
const hoveredDigievolutionId = ref<number | null>(null);

const availableDnaFusions = computed(() => {
  if (hoveredDigievolutionId.value === null) {
    return [];
  }

  return DigievolutionPresenter.getAvailableDnaFusions(
    gameState.value.party,
    props.digimonSlotIndex,
    hoveredDigievolutionId.value,
  );
});

function showTooltip(event: MouseEvent, digievolutionId: number | null): void {
  hoveredDigievolutionId.value = digievolutionId;
  showAt(event, { maxWidth: tooltipMaxWidth, placement: tooltipPlacement });
  highlightDnaFusion(digievolutionId);
}

function moveTooltip(event: MouseEvent): void {
  move(event, tooltipPlacement);
}

function hideTooltip(): void {
  hide();
  hoveredDigievolutionId.value = null;
  dnaFusionHighlight.clear();
}

function highlightDnaFusion(digievolutionId: number | null): void {
  if (digievolutionId === null) {
    return;
  }

  const partners = availableDnaFusions.value.flatMap((fusion) => {
    return fusion.partners.map((partner) => {
      return { digimonId: partner.digimonId, digievolutionId: partner.digievolutionId };
    });
  });

  dnaFusionHighlight.highlight([{ digimonId: props.digimonId, digievolutionId }, ...partners]);
}

onBeforeUnmount(() => {
  if (hoveredDigievolutionId.value !== null) {
    dnaFusionHighlight.clear();
  }
});
</script>

<template>
  <div class="flex flex-col gap-0.5 w-full">
    <Digievolution
      v-for="slot in slots"
      :key="slot.index"
      :digievolution-id="slot.digievolutionId"
      :digievolution-level="slot.digievolution?.level ?? null"
      :digievolution-dvexp="slot.digievolution?.dvexp ?? null"
      :active-digievolution-id="activeDigievolutionId"
      :digimon-slot-index="digimonSlotIndex"
      :digimon-id="digimonId"
      @open-techniques="openTechniques"
      @show-tooltip="showTooltip($event, slot.digievolutionId)"
      @move-tooltip="moveTooltip"
      @hide-tooltip="hideTooltip"
    />

    <DnaFusionTooltip
      :show="tooltipShow"
      :x="tooltipX"
      :y="tooltipY"
      :fusions="availableDnaFusions"
      :max-width="tooltipMaxWidth"
      :placement="tooltipPlacement"
    />

    <Modal
      v-if="selectedDigievolution"
      :is-open="isTechniquesModalOpen"
      max-width="max-w-lg"
      max-height="h-[700px] max-h-[92vh]"
      :show-footer-bar="false"
      @close="closeTechniques"
    >
      <div class="flex flex-1 min-h-0 flex-col overflow-hidden">
        <DigievolutionTechniques
          :digievolution-id="selectedDigievolution.id"
          :digievolution-level="selectedDigievolution.level"
        />
      </div>
    </Modal>
  </div>
</template>
