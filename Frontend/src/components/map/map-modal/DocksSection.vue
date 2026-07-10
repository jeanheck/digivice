<script setup lang="ts">
import { computed } from "vue";
import asukaMapUrl from "@/assets/AsukaMap.webp";
import Dock from "@/components/map/map-modal/Dock.vue";
import Docks from "@/components/map/map-modal/Docks.vue";
import { DocksSectionPresenter } from "@/presenters/map/docks-section.presenter";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

const props = defineProps<{
  location: LocationViewModel | null;
}>();

const dockViewModel = computed(() => {
  return DocksSectionPresenter.getDock(props.location);
});
</script>

<template>
  <div class="flex flex-1 min-h-0 w-full gap-4 p-4 overflow-y-auto overflow-x-hidden">
    <div class="flex flex-1 min-w-0 min-h-0 max-h-full items-center justify-center">
      <Docks :image-url="asukaMapUrl" />
    </div>
    <div class="flex flex-1 min-w-0 min-h-0 max-h-full items-center justify-center">
      <Dock
        :image-url="dockViewModel?.imageUrl ?? null"
        :coordinates="dockViewModel?.coordinates ?? null"
      />
    </div>
  </div>
</template>
