<script setup lang="ts">
import { computed } from "vue";
import { DigievolutionDvxpSegmentsPresenter } from "@/presenters/digievolution/digievolution-dvxp-segments.presenter";

const props = defineProps<{
  isActiveDigievolution: boolean;
  dvxp: number;
}>();

const segmentCount = 10;

const filledSegmentCount = computed(() => {
  return DigievolutionDvxpSegmentsPresenter.getFilledSegmentCount(props.dvxp);
});
function getSegmentClass(isFilled: boolean): string {
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
      v-for="segmentIndex in segmentCount"
      :key="segmentIndex"
      class="flex-1 min-w-0 transition-colors duration-500"
      :class="getSegmentClass(segmentIndex <= filledSegmentCount)"
    ></div>
  </div>
</template>