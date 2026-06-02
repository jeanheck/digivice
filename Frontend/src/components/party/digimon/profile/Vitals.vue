<script setup lang="ts">
import { computed } from "vue";
import ProgressBar from "@/components/party/digimon/profile/ProgressBar.vue";
import type { Vitals } from "@/models";
import { ProgressBarConstant } from "@/constants/progress-bar.constant";
import { MathUtils } from "@/utils/MathUtils";

const props = defineProps<{
  vitals: Vitals;
}>();

const hpPercentage = computed(() => {
  return MathUtils.calculatePercentage(props.vitals.currentHP, props.vitals.maxHP);
});

const mpPercentage = computed(() => {
  return MathUtils.calculatePercentage(props.vitals.currentMP, props.vitals.maxMP);
});
</script>

<template>
  <div class="flex flex-col gap-2 mt-2">
    <ProgressBar
      :variant="ProgressBarConstant.hp"
      :current-value="vitals.currentHP"
      :max-value="vitals.maxHP"
      :percentage="hpPercentage"
    />
    <ProgressBar
      :variant="ProgressBarConstant.mp"
      :current-value="vitals.currentMP"
      :max-value="vitals.maxMP"
      :percentage="mpPercentage"
    />
  </div>
</template>
