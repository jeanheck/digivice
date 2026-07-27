<script setup lang="ts">
import { computed } from "vue";
import ProgressBar from "@/components/party/digimon/profile/progress-bar/ProgressBar.vue";
import { ExperienceProgressBarPresenter } from "@/presenters/party/digimon/profile/progress-bar/experience-progress-bar.presenter";

const props = defineProps<{
  digimonId: number;
  level: number;
  experience: number;
}>();

const emit = defineEmits<{
  showTooltip: [event: MouseEvent];
  moveTooltip: [event: MouseEvent];
  hideTooltip: [];
}>();

const progressBarViewModel = computed(() => {
  return ExperienceProgressBarPresenter.getCalculatedExperienceValues(
    props.digimonId,
    props.level,
    props.experience,
  );
});
</script>

<template>
  <ProgressBar
    :current-value="experience"
    :max-value="progressBarViewModel.maxValue"
    :progressPercentage="progressBarViewModel.percentage"
    bar-color-class="bg-linear-to-r from-orange-600 to-yellow-500"
    transition-duration-class="duration-500"
    @show-tooltip="emit('showTooltip', $event)"
    @move-tooltip="emit('moveTooltip', $event)"
    @hide-tooltip="emit('hideTooltip')"
  />
</template>
