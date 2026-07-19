<script setup lang="ts">
import { computed } from "vue";
import ProgressBar from "@/components/party/digimon/profile/progress-bar/ProgressBar.vue";
import { BlastGaugeProgressBarPresenter } from "@/presenters/party/digimon/profile/progress-bar/blast-gauge-progress-bar.presenter";

const props = defineProps<{
  blastGauge: number;
}>();

const emit = defineEmits<{
  showTooltip: [event: MouseEvent];
  moveTooltip: [event: MouseEvent];
  hideTooltip: [];
}>();

const percentage = computed(() => {
  return BlastGaugeProgressBarPresenter.calculateProgressPercentage(props.blastGauge);
});

const fillExtraClass = computed(() => {
  return BlastGaugeProgressBarPresenter.getFillEffectClass(percentage.value);
});

const trackExtraClass = computed(() => {
  return BlastGaugeProgressBarPresenter.getTrackEffectClass(percentage.value);
});
</script>

<template>
  <ProgressBar
    :current-value="blastGauge"
    :max-value="BlastGaugeProgressBarPresenter.MAX_BLAST_GAUGE"
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
