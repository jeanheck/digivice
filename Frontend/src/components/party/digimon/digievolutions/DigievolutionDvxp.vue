<script setup lang="ts">
import { computed } from "vue";
import { DigievolutionDvxpPresenter } from "@/presenters/digievolution/digievolution-dvxp.presenter";

const props = defineProps<{
  isActiveDigievolution: boolean;
  dvxp: number;
}>();

const dvxp = computed(() => {
  return DigievolutionDvxpPresenter.getCalculatedDvxp(props.dvxp);
});
function getDvxpClass(isFilled: boolean): string {
  if (props.isActiveDigievolution) {
    if (isFilled) {
      return "bg-yellow-400";
    }

    return "bg-orange-600/35";
  }

  if (isFilled) {
    return "bg-blue-400";
  }

  return "bg-blue-800/35";
}
</script>

<template>
  <div class="flex w-full gap-1 h-1" role="presentation">
    <div
      v-for="i in DigievolutionDvxpPresenter.MAX_DVXP_BY_LEVEL"
      :key="i"
      class="flex-1 min-w-0 transition-colors duration-500"
      :class="getDvxpClass(i <= dvxp)"
    ></div>
  </div>
</template>