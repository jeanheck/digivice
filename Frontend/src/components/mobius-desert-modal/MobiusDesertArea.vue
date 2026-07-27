<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { DesertNeighborHelper } from "@/presenters/helper/desert-neighbor.helper";
import type { DesertAreaTypeViewModel } from "@/viewmodels/desert/desert-area-type.viewmodel";

const props = defineProps<{
  hasRightConnection: boolean;
  hasBottomConnection: boolean;
  label: string;
  type: DesertAreaTypeViewModel;
  note?: string;
  rightNeighborType: DesertAreaTypeViewModel | null;
  bottomNeighborType: DesertAreaTypeViewModel | null;
  clickable: boolean;
  isPlayerLocation: boolean;
}>();

const emit = defineEmits<{
  (e: "click"): void;
}>();

const { t } = useI18n();

function onClick(): void {
  if (!props.clickable) {
    return;
  }

  emit("click");
}

const backgroundClassByType: Record<DesertAreaTypeViewModel, string> = {
  noiseDesertS: "bg-green-300",
  mirageTower: "bg-cyan-300",
  normal: "bg-[#e0db8e]",
  border: "bg-gray-700",
};

const textClassByType: Record<DesertAreaTypeViewModel, string> = {
  noiseDesertS: "text-[11px] text-blue-800",
  mirageTower: "text-[11px] text-blue-800",
  normal: "text-[20px] text-blue-800",
  border: "text-[20px] text-gray-500",
};

const translatedLabelKeys = new Set(["noiseDesertS", "mirageTower"]);

function getConnectionColorClasse(
  sourceType: DesertAreaTypeViewModel,
  targetType: DesertAreaTypeViewModel | null
): string {
  if (sourceType === "border" || targetType === "border") {
    return "bg-gray-500";
  }

  if (sourceType === "noiseDesertS" || targetType === "noiseDesertS") {
    return "bg-green-500";
  }

  if (sourceType === "mirageTower" || targetType === "mirageTower") {
    return "bg-cyan-400";
  }

  return "bg-cyan-500";
}

const backgroundClass = computed(() => {
  if (props.isPlayerLocation) {
    return "bg-rose-200";
  }

  return backgroundClassByType[props.type];
});

const textClass = computed(() => {
  if (props.isPlayerLocation) {
    return "text-[20px] text-rose-400";
  }

  if (props.type === "border" && translatedLabelKeys.has(props.label)) {
    return "text-[11px] text-gray-500";
  }

  return textClassByType[props.type];
});

const displayLabel = computed(() => {
  const neighborName = DesertNeighborHelper.resolveNeighborName(props.label);

  if (neighborName.kind === "i18n") {
    return t(neighborName.key);
  }

  return neighborName.value;
});

const rightConnectionClass = computed(() => {
  return getConnectionColorClasse(props.type, props.rightNeighborType);
});

const bottomConnectionClass = computed(() => {
  return getConnectionColorClasse(props.type, props.bottomNeighborType);
});
</script>

<template>
  <div
    class="relative flex w-16 h-16 items-center justify-center"
    :class="[backgroundClass, clickable ? 'cursor-pointer hover:brightness-110' : 'cursor-default']"
    @click="onClick"
  >
    <span
      v-if="displayLabel"
      class="relative z-10 overflow-hidden px-0.0 text-center font-['Exo_2'] font-bold leading-tight"
      :class="textClass"
    >
      {{ displayLabel }}
    </span>
    <span
      v-if="note"
      class="absolute bottom-0 z-10 w-full px-0.5 text-center font-['Exo_2'] text-[14px] font-bold leading-tight text-blue-800"
    >
      {{ note }}
    </span>
    <span
      v-if="hasRightConnection"
      class="absolute left-full top-1/2 h-0.75 w-8.75 -translate-y-1/2"
      :class="rightConnectionClass"
    />
    <span
      v-if="hasBottomConnection"
      class="absolute left-1/2 top-full h-8.75 w-0.75 -translate-x-1/2"
      :class="bottomConnectionClass"
    />
  </div>
</template>
