<script setup lang="ts">
import { computed } from "vue";
import type { DesertAreaTypeViewModel } from "@/viewmodels/desert/desert-area-type.viewmodel";
import { getConnectionColorClasse } from "@/components/mobius-desert-modal/mobius-desert-area/helpers/mobius-desert-area.helper";

const props = defineProps<{
  hasRightConnection: boolean;
  hasBottomConnection: boolean;
  label: string;
  note?: string;
  rightNeighborType: DesertAreaTypeViewModel | null;
  bottomNeighborType: DesertAreaTypeViewModel | null;
  clickable: boolean;
}>();

const emit = defineEmits<{
  (e: "click"): void;
}>();

const SOURCE_TYPE: DesertAreaTypeViewModel = "normal";

function onClick(): void {
  if (!props.clickable) {
    return;
  }

  emit("click");
}

const rightConnectionClass = computed(() => {
  return getConnectionColorClasse(SOURCE_TYPE, props.rightNeighborType);
});

const bottomConnectionClass = computed(() => {
  return getConnectionColorClasse(SOURCE_TYPE, props.bottomNeighborType);
});
</script>

<template>
  <div
    class="relative flex w-16 h-16 items-center justify-center bg-[#e0db8e]"
    :class="clickable ? 'cursor-pointer hover:brightness-110' : 'cursor-default'"
    @click="onClick"
  >
    <span
      v-if="label"
      class="relative z-10 overflow-hidden px-0.0 text-center font-['Exo_2'] font-bold leading-tight text-[20px] text-blue-800"
    >
      {{ label }}
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
