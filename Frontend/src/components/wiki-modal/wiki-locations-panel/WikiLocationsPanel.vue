<script setup lang="ts">
import { computed } from "vue";
import MapFrame from "@/components/map-frame/MapFrame.vue";
import { WikiLocationsPanelPresenter } from "@/presenters/map/wiki-modal/wiki-locations-panel.presenter";

const props = defineProps<{
  locationId: string;
}>();

const locationsViewModel = computed(() => {
  return WikiLocationsPanelPresenter.getLocationPanelViewModel(props.locationId);
});
</script>

<template>
  <div class="p-4 flex flex-col h-full min-h-0 overflow-hidden">
    <div class="flex-1 min-h-0 flex gap-4 items-center justify-center overflow-hidden">
      <MapFrame v-if="locationsViewModel.asukaSlides.length > 0" :slides="locationsViewModel.asukaSlides" />

      <MapFrame v-if="locationsViewModel.localSlides.length > 0" :slides="locationsViewModel.localSlides" />
      <div
        v-else
        class="flex items-center justify-center min-h-0 px-8"
      >
        <span class="text-xs text-gray-500 font-bold tracking-wide">
          {{
            locationsViewModel.selectedLocationLabelKey
              ? $t(locationsViewModel.selectedLocationLabelKey)
              : ""
          }}
        </span>
      </div>
    </div>
  </div>
</template>
