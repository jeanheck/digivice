<script setup lang="ts">
import { computed } from "vue";
import { DigievolutionDvexpPresenter } from "@/presenters/digievolution/digievolution-dvexp.presenter";

const props = defineProps<{
  isActiveDigievolution: boolean;
  dvexp: number;
}>();

const dvexp = computed(() => {
  return DigievolutionDvexpPresenter.getCalculatedDvexp(props.dvexp);
});
function getDvexpClass(isFilled: boolean): string {
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
      v-for="i in DigievolutionDvexpPresenter.MAX_DVEXP_BY_LEVEL"
      :key="i"
      class="flex-1 min-w-0 transition-colors duration-500"
      :class="getDvexpClass(i <= dvexp)"
    ></div>
  </div>
</template>
