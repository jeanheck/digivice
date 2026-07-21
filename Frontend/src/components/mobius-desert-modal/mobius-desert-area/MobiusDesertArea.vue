<script setup lang="ts">
import { computed } from "vue";
import type { DesertAreaTypeViewModel } from "@/viewmodels/desert/desert-area-type.viewmodel";
import MirageTowerArea from "@/components/mobius-desert-modal/mobius-desert-area/MirageTowerArea.vue";
import NoiseDesertSArea from "@/components/mobius-desert-modal/mobius-desert-area/NoiseDesertSArea.vue";
import NormalArea from "@/components/mobius-desert-modal/mobius-desert-area/NormalArea.vue";
import BorderArea from "@/components/mobius-desert-modal/mobius-desert-area/BorderArea.vue";

const props = defineProps<{
  hasRightConnection: boolean;
  hasBottomConnection: boolean;
  label: string;
  type: DesertAreaTypeViewModel;
  note?: string;
  rightNeighborType: DesertAreaTypeViewModel | null;
  bottomNeighborType: DesertAreaTypeViewModel | null;
  clickable: boolean;
}>();

const emit = defineEmits<{
  (e: "click"): void;
}>();

const areaComponentByType = {
  mirageTower: MirageTowerArea,
  noiseDesertS: NoiseDesertSArea,
  normal: NormalArea,
  border: BorderArea,
} as const;

const areaComponent = computed(() => {
  return areaComponentByType[props.type];
});

function onClick(): void {
  emit("click");
}
</script>

<template>
  <component
    :is="areaComponent"
    :has-right-connection="hasRightConnection"
    :has-bottom-connection="hasBottomConnection"
    :label="label"
    :note="note"
    :right-neighbor-type="rightNeighborType"
    :bottom-neighbor-type="bottomNeighborType"
    :clickable="clickable"
    @click="onClick"
  />
</template>
