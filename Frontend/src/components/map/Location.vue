<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";

const props = defineProps<{
  locationId: string | null;
  titleOverride?: string | null;
}>();

const { t } = useI18n();

const locationName = computed(() => {
  if (props.titleOverride) {
    return props.titleOverride;
  }

  return props.locationId ? t(`location.${props.locationId}`) : t("map.unknownZone");
});
</script>

<template>
  <div class="w-full flex justify-center shrink-0">
    <div class="map-info-panel-fit text-center">
      <h4 class="text-xs sm:text-sm font-bold text-white tracking-widest uppercase drop-shadow-[0_0_5px_rgba(0,170,255,0.8)] leading-tight">
        {{ locationName }}
      </h4>
    </div>
  </div>
</template>
