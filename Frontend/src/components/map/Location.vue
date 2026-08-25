<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { LocationService } from "@/services/location.service";

const props = withDefaults(
  defineProps<{
    locationId: string | null;
    titleOverride?: string | null;
    isSafeZone?: boolean;
  }>(),
  {
    titleOverride: null,
    isSafeZone: false,
  },
);

const emit = defineEmits<{
  (e: "open-location-wiki", locationId: string): void;
}>();

const { t } = useI18n();

const locationName = computed(() => {
  if (props.titleOverride) {
    return props.titleOverride;
  }

  return props.locationId ? t(`location.${props.locationId}`) : t("map.unknownZone");
});

const isClickable = computed(() => {
  if (props.locationId === null) {
    return false;
  }

  return LocationService.getWorldLocation(props.locationId) !== undefined;
});

const titleClass = computed(() => {
  const colorClass = props.isSafeZone ? "text-sky-300" : "text-white";
  return `text-xs sm:text-sm font-bold ${colorClass} tracking-widest uppercase drop-shadow-[0_0_5px_rgba(0,170,255,0.8)] leading-tight`;
});

const clickableHoverClass = computed(() => {
  return props.isSafeZone ? "hover:text-sky-200" : "hover:text-blue-300";
});

const handleClick = () => {
  if (!isClickable.value || props.locationId === null) {
    return;
  }

  emit("open-location-wiki", props.locationId);
};
</script>

<template>
  <div class="w-full flex justify-center shrink-0">
    <div class="map-info-panel-fit text-center">
      <button
        v-if="isClickable"
        type="button"
        :class="[titleClass, 'cursor-pointer transition-colors', clickableHoverClass]"
        @click="handleClick"
      >
        {{ locationName }}
      </button>
      <h4 v-else :class="titleClass">
        {{ locationName }}
      </h4>
    </div>
  </div>
</template>
