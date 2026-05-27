<script setup lang="ts">
import MapImage from "./MapImage.vue";
import MapEnemies from "./MapEnemies.vue";
import { computed } from "vue";
import { MapPresenter } from "@/presenters/map.presenter.ts";

const props = defineProps<{
  locationId: string | null;
}>();

const locationViewModel = computed(() => {
  if(!props.locationId) {
    return null;
  }
  return MapPresenter.getLocationById(props.locationId);
});
</script>

<template>
  <aside class="w-full h-full bg-[#000e3f] rounded-md shadow-lg border-2 border-[#0033aa] p-3 flex flex-col pt-0 mb-1 overflow-hidden">
    <!-- Header banner -->
    <div class="w-full flex items-center justify-center border-b border-[#0033aa]/50 bg-[#000e3f] sticky top-0 py-2 z-10">
      <h3 class="font-bold tracking-widest text-[#0077ff] text-shadow-sm uppercase text-sm">{{ $t("map.map") }}</h3>
    </div>

    <div class="flex-1 flex flex-col mt-2 h-full">
      <MapImage :location-id="locationId" :location="locationViewModel" />
      <MapEnemies :enemies-ids="locationViewModel?.enemies ?? []" />
    </div>
  </aside>
</template>
