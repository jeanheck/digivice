<script setup lang="ts">
import { computed } from "vue";
import ProgressBar from "@/components/party/digimon/profile/progress-bar/ProgressBar.vue";
import { HpProgressBarPresenter } from "@/presenters/party/digimon/profile/progress-bar/hp-progress-bar.presenter";
import type { Vital } from "@/models";

const props = defineProps<{
  hp: Vital;
}>();

const emit = defineEmits<{
  showTooltip: [event: MouseEvent];
  moveTooltip: [event: MouseEvent];
  hideTooltip: [];
}>();

const progressPercentage = computed(() => {
  return HpProgressBarPresenter.getCalculatedProgressPercentage(props.hp.current, props.hp.max);
});

const barColorClass = computed(() => {
  return progressPercentage.value <= 30 ? "bg-red-500" : "bg-green-500";
});
</script>

<template>
  <ProgressBar
    :current-value="hp.current"
    :max-value="hp.max"
    :progress-percentage="progressPercentage"
    :bar-color-class="barColorClass"
    transition-duration-class="duration-300"
    @show-tooltip="emit('showTooltip', $event)"
    @move-tooltip="emit('moveTooltip', $event)"
    @hide-tooltip="emit('hideTooltip')"
  />
</template>
