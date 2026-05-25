<script setup lang="ts">
import ProgressBar from "@/components/digimon/ProgressBar.vue";
import type { Vitals } from "@/models";
import { ProgressBarTypes } from "@/constants/progress-bar-types";

defineProps<{
  vitals: Vitals;
}>();
</script>

<template>
  <div class="flex flex-col gap-2 mt-2">
    <ProgressBar
      :current-value="vitals.currentHP"
      :max-value="vitals.maxHP"
      :type="ProgressBarTypes.HP"
    />
    <ProgressBar
      :current-value="vitals.currentMP"
      :max-value="vitals.maxMP"
      :type="ProgressBarTypes.MP"
    />
  </div>
</template>
