<script setup lang="ts">
import ZoomedLocationMap from "./ZoomedLocationMap.vue";
import type { ZoomedLocationMapViewModel } from "@/viewmodels/quest/zoomed-location-map.viewmodel";
import type { StepViewModel } from "@/viewmodels/quest/step.viewmodel";

defineProps<{
  selectedStep: StepViewModel | null;
  worldMapLocations: ZoomedLocationMapViewModel[];
  localMapLocations: ZoomedLocationMapViewModel[];
}>();
</script>

<template>
  <div class="flex w-full min-h-0 shrink-0 flex-col gap-4 overflow-hidden lg:w-112.5 lg:border-l lg:border-[#0055ff]/30 lg:pl-6">
    <div
      v-if="!selectedStep"
      class="flex-1 flex flex-col items-center justify-center border border-cyan-900/40 bg-[#000a1a] rounded min-h-100"
    >
      <span class="text-cyan-500/50 text-sm tracking-widest text-center px-8 animate-pulse whitespace-pre-line">
        {{ $t("journal.clickStep") }}
      </span>
    </div>

    <div
      v-else-if="!selectedStep.location && (!selectedStep.zoomedLocations || selectedStep.zoomedLocations.length === 0)"
      class="flex-1 flex flex-col items-center justify-center border border-red-900/40 bg-[#1a0000] rounded min-h-100"
    >
      <span class="text-red-500/50 text-sm tracking-widest text-center px-8 whitespace-pre-line">
        {{ $t("journal.noSignal") }}
      </span>
    </div>

    <template v-else>
      <ZoomedLocationMap
        v-if="worldMapLocations.length > 0"
        map-variant="world"
        :locations="worldMapLocations"
      />

      <ZoomedLocationMap
        v-if="localMapLocations.length > 0"
        map-variant="local"
        :locations="localMapLocations"
      />
    </template>
  </div>
</template>
