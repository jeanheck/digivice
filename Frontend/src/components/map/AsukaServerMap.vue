<script setup lang="ts">
import Location from "./Location.vue";
import Enemies from "./Enemies.vue";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

defineProps<{
  location: LocationViewModel | null;
}>();

const emit = defineEmits<{
  (e: "open-enemy-modal", enemyId: string): void;
}>();
</script>

<template>
  <div class="relative z-10 flex flex-col flex-1 min-h-0 pt-1">
    <div class="flex flex-col items-center gap-2 shrink-0">
      <Location :location="location" />
      <Enemies :location="location" @open-enemy-modal="emit('open-enemy-modal', $event)" />
    </div>

    <div class="flex-1 min-h-0" />
  </div>
</template>
