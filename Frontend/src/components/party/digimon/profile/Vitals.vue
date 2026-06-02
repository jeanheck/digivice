<script setup lang="ts">
import { computed } from "vue";
import ProgressBar from "@/components/party/digimon/profile/ProgressBar.vue";
import type { Vitals } from "@/models";
import { ProgressBarConstant } from "@/constants/progress-bar.constant";
import { VitalsPresenter } from "@/presenters/vitals.presenter";

const props = defineProps<{
  vitals: Vitals;
}>();

const vitalsViewModel = computed(() => {
  return VitalsPresenter.getViewModel(props.vitals);
});
</script>

<template>
  <div class="flex flex-col gap-2 mt-2">
    <ProgressBar
      :variant="ProgressBarConstant.hp"
      :current-value="vitals.currentHP"
      :max-value="vitals.maxHP"
      :percentage="vitalsViewModel.hpPercentage"
    />
    <ProgressBar
      :variant="ProgressBarConstant.mp"
      :current-value="vitals.currentMP"
      :max-value="vitals.maxMP"
      :percentage="vitalsViewModel.mpPercentage"
    />
  </div>
</template>
