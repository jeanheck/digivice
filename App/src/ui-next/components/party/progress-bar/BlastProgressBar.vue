<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import ProgressBar from "@/ui-next/components/party/progress-bar/ProgressBar.vue";
import { BlastProgressBarPresenter } from "@/presentation/presenters/party/digimon/profile/progress-bar/blast-progress-bar.presenter";

const { blast } = defineProps<{
  blast: number;
}>();

const { t } = useI18n();

const progressPercentage = computed(() => {
  return BlastProgressBarPresenter.calculateProgressPercentage(blast);
});

const fillExtraClass = computed(() => {
  return BlastProgressBarPresenter.getFillEffectClass(progressPercentage.value);
});

const trackExtraClass = computed(() => {
  return BlastProgressBarPresenter.getTrackEffectClass(progressPercentage.value);
});
</script>

<template>
  <ProgressBar
    :current-value="blast"
    :max-value="BlastProgressBarPresenter.MaxBlast"
    :progress-percentage="progressPercentage"
    bar-color-class="bg-yellow-200"
    transition-duration-class="duration-300"
    :fill-extra-class="fillExtraClass"
    :track-extra-class="trackExtraClass"
    :label="t('digimon.blast')"
  />
</template>
