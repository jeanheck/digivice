<script setup lang="ts">
import Dock from "@/components/seabed/Dock.vue";
import mobiusDesertMapUrl from "@/assets/maps/Mobius Desert.webp";
import type { CoordinatesViewModel } from "@/viewmodels/quest/coordinates.viewmodel";

const coordinates: CoordinatesViewModel | null = null;
</script>

<template>
  <div class="flex h-full w-full items-center justify-center">
    <Dock :image-url="mobiusDesertMapUrl" :coordinates="coordinates" />
  </div>
</template>
