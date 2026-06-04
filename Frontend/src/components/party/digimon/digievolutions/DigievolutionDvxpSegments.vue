<script setup lang="ts">
const props = defineProps<{
  isActiveDigievolution: boolean;
}>();

const segmentCount = 10;
const previewFilledSegmentCount = 7;

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
      :class="getSegmentClass(segmentIndex <= previewFilledSegmentCount)"
    ></div>
  </div>
</template>