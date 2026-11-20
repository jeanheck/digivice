<script setup lang="ts">
import { computed } from "vue";
import type { HealthStatus } from "@/state/models";

const props = defineProps<{
  healthStatus: HealthStatus;
  lastErrorCode: string | null;
  lastErrorDetail: string | null;
  lastChangedAt: Date | null;
}>();

const healthColorClass = computed(() => {
  if (props.healthStatus === "Healthy") {
    return "text-green-400";
  }

  if (props.healthStatus === "Loading") {
    return "text-yellow-300";
  }

  return "text-red-400";
});

const lastChangedLabel = computed(() => props.lastChangedAt?.toLocaleTimeString() ?? "-");
</script>

<template>
  <section class="flex flex-col gap-0.5">
    <div class="text-cyan-300 font-bold">
      DIGIVICE DEBUG MONITOR <span class="text-gray-500">(F2)</span>
    </div>
    <div>
      <span class="text-gray-400">Health: </span>
      <span :class="healthColorClass">{{ props.healthStatus }}</span>
      <span v-if="props.lastErrorCode !== null" class="text-red-400">
        ({{ props.lastErrorCode }})</span
      >
    </div>
    <div v-if="props.lastErrorDetail !== null" class="text-red-300/80 wrap-break-word">
      {{ props.lastErrorDetail }}
    </div>
    <div><span class="text-gray-400">Last change: </span>{{ lastChangedLabel }}</div>
  </section>
</template>
