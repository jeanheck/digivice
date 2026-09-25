<script setup lang="ts">
import { computed } from "vue";
import ProgressBar from "@/components/party/digimon/profile/progress-bar/ProgressBar.vue";
import { MpProgressBarPresenter } from "@/presenters/party/digimon/profile/progress-bar/mp-progress-bar.presenter";
import type { Vital } from "@/models";

const props = defineProps<{
  mp: Vital;
}>();

const emit = defineEmits<{
  showTooltip: [event: MouseEvent];
  moveTooltip: [event: MouseEvent];
  hideTooltip: [];
}>();

const percentage = computed(() => {
  return MpProgressBarPresenter.getCalculatedProgressPercentage(props.mp.current, props.mp.max);
});

const barColorClass = computed(() => {
  return percentage.value <= 30 ? "bg-yellow-400" : "bg-blue-600";
});
</script>

<template>
  <ProgressBar
    :current-value="mp.current"
    :max-value="mp.max"
    :progressPercentage="percentage"
    :bar-color-class="barColorClass"
    transition-duration-class="duration-300"
    @show-tooltip="emit('showTooltip', $event)"
    @move-tooltip="emit('moveTooltip', $event)"
    @hide-tooltip="emit('hideTooltip')"
  />
</template>
