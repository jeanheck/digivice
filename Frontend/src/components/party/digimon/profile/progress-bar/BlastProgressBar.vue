<script setup lang="ts">
import { computed } from "vue";
import ProgressBar from "@/components/party/digimon/profile/progress-bar/ProgressBar.vue";
import { BlastProgressBarPresenter } from "@/presenters/party/digimon/profile/progress-bar/blast-progress-bar.presenter";

const props = defineProps<{
  blast: number;
}>();

const emit = defineEmits<{
  showTooltip: [event: MouseEvent];
  moveTooltip: [event: MouseEvent];
  hideTooltip: [];
}>();

const percentage = computed(() => {
  return BlastProgressBarPresenter.calculateProgressPercentage(props.blast);
});

const fillExtraClass = computed(() => {
  return BlastProgressBarPresenter.getFillEffectClass(percentage.value);
});

const trackExtraClass = computed(() => {
  return BlastProgressBarPresenter.getTrackEffectClass(percentage.value);
});
</script>

<template>
  <ProgressBar
    :current-value="blast"
    :max-value="BlastProgressBarPresenter.MAX_BLAST"
    :progressPercentage="percentage"
    bar-color-class="bg-yellow-200"
    transition-duration-class="duration-300"
    :fill-extra-class="fillExtraClass"
    :track-extra-class="trackExtraClass"
    @show-tooltip="emit('showTooltip', $event)"
    @move-tooltip="emit('moveTooltip', $event)"
    @hide-tooltip="emit('hideTooltip')"
  />
</template>
