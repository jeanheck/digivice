<script setup lang="ts">
import { computed, ref } from "vue";
import { useI18n } from "vue-i18n";
import MapModal from "@/components/map/map-modal/MapModal.vue";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

const props = defineProps<{
  location: LocationViewModel | null;
}>();

const { t } = useI18n();

const locationName = computed(() => {
  return props.location?.id ? t(`location.${props.location.id}`) : t("map.unknownZone");
});

const isMapModalOpen = ref(false);

const openMapModal = () => {
  isMapModalOpen.value = true;
};

const closeMapModal = () => {
  isMapModalOpen.value = false;
};
</script>

<template>
  <div class="w-full flex justify-center shrink-0">
    <button
      type="button"
      class="map-info-panel-fit map-info-panel-action text-center"
      @click="openMapModal"
    >
      <h4 class="text-xs sm:text-sm font-bold text-white tracking-widest uppercase drop-shadow-[0_0_5px_rgba(0,170,255,0.8)] leading-tight">
        {{ locationName }}
      </h4>
    </button>

    <MapModal
      :is-open="isMapModalOpen"
      :title="locationName"
      @close="closeMapModal"
    />
  </div>
</template>
