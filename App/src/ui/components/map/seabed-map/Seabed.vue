<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { SeabedDirection } from "@/shared/types/seabed-direction.type";
import { SeabedPresenter } from "@/presentation/presenters/map/seabed.presenter";

const SeabedDirectionMonoIcon = {
  topLeft: "\u2B09",
  top: "\u2B06",
  topRight: "\u2B08",
} as const satisfies Record<SeabedDirection, string>;

const props = defineProps<{
  seabedRoute: number | null;
  mapVariant: number | null;
  locationId: string;
}>();

const { t } = useI18n();

const routeLocation = computed(() => {
  return SeabedPresenter.getRouteLocation(props.seabedRoute, props.locationId, props.mapVariant);
});

const emerge = computed(() => {
  return routeLocation.value?.emerge ?? [];
});

const leftLocations = computed(() => {
  return routeLocation.value?.left ?? [];
});

const rightLocations = computed(() => {
  return routeLocation.value?.right ?? [];
});

const hasEmerge = computed(() => emerge.value.length > 0);
const hasLeft = computed(() => leftLocations.value.length > 0);
const hasRight = computed(() => rightLocations.value.length > 0);

function getEmergeEmoji(on: SeabedDirection): string {
  return SeabedDirectionMonoIcon[on];
}

function getLocationName(locationId: string): string {
  return t(`map.${locationId}`);
}
</script>

<template>
  <div v-if="hasEmerge || hasLeft || hasRight" class="w-full flex justify-center shrink-0 px-0.5">
    <div class="map-info-panel flex flex-col gap-5">
      <div v-if="hasEmerge" class="flex items-start justify-center gap-3 w-full">
        <div
          v-for="(point, index) in emerge"
          :key="`${point.location}-${index}`"
          class="flex flex-1 flex-col items-center gap-0.5 min-w-0"
        >
          <span
            class="font-emoji text-teal-500 inline-flex leading-none text-[1.1rem] -translate-y-0.5"
            >{{ getEmergeEmoji(point.on) }}</span
          >
          <span class="text-[9px] text-teal-100 text-center leading-tight">{{
            getLocationName(point.location)
          }}</span>
        </div>
      </div>

      <div v-if="hasLeft" class="flex items-center gap-1.5 w-full">
        <span
          class="font-emoji text-teal-500 inline-flex leading-none text-[1.1rem] -translate-y-0.5"
          >&#x2B05;</span
        >
        <div class="flex flex-col text-[9px] text-teal-100 text-left leading-tight min-w-0">
          <span v-for="(locationId, index) in leftLocations" :key="index">{{
            getLocationName(locationId)
          }}</span>
        </div>
      </div>

      <div v-if="hasRight" class="flex items-center justify-end gap-1.5 w-full">
        <div class="flex flex-col text-[9px] text-teal-100 text-right leading-tight min-w-0">
          <span v-for="(locationId, index) in rightLocations" :key="index">{{
            getLocationName(locationId)
          }}</span>
        </div>
        <span
          class="font-emoji text-teal-500 inline-flex leading-none text-[1.1rem] -translate-y-0.5"
          >&#x27A1;</span
        >
      </div>
    </div>
  </div>
</template>
