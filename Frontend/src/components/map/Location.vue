<script setup lang="ts">
import { computed } from "vue";
import { ImageCatalog } from "@/catalogs/image.catalog.ts";
import { useI18n } from "vue-i18n";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

const props = defineProps<{
  location: LocationViewModel | null;
}>();

const { t } = useI18n();

const locationName = computed(() => {
  return props.location?.id ? t(`location.${props.location.id}`) : t("map.unknownZone");
});

const locationImage = computed(() => {
  return ImageCatalog.getMapImageUrl(props.location?.image ?? null);
});
</script>

<template>
  <div
    class="flex-3 relative w-full rounded border-2 border-[#0044aa]/50 bg-black bg-opacity-60 flex items-center justify-center overflow-hidden shadow-inner"
    :class="{ 'bg-grid-pattern': !locationImage }"
  >
    <img
      v-if="locationImage"
      :src="locationImage"
      class="absolute inset-0 w-full h-full object-cover opacity-60 mix-blend-lighten pointer-events-none"
    />

    <div class="relative z-10 px-3 py-2 bg-black/60 border border-[#00aaff]/40 rounded backdrop-blur-sm max-w-[90%] text-center">
      <h4 class="text-xs sm:text-sm font-bold text-white tracking-widest uppercase drop-shadow-[0_0_5px_rgba(0,170,255,0.8)] leading-tight">
        {{ locationName }}
      </h4>
    </div>

    <div class="absolute top-1 left-1 w-3 h-3 border-t-2 border-l-2 border-[#00aaff]/60"></div>
    <div class="absolute bottom-1 right-1 w-3 h-3 border-b-2 border-r-2 border-[#00aaff]/60"></div>
    <div class="absolute top-1 right-1 w-3 h-3 border-t-2 border-r-2 border-[#00aaff]/60"></div>
    <div class="absolute bottom-1 left-1 w-3 h-3 border-b-2 border-l-2 border-[#00aaff]/60"></div>
  </div>
</template>

<style scoped>
.bg-grid-pattern {
  background-image:
    linear-gradient(to right, #0055ff 1px, transparent 1px),
    linear-gradient(to bottom, #0055ff 1px, transparent 1px);
  background-size: 15px 15px;
}
</style>
