<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { Constant } from "@/constants/constant";
import { IconConstant } from "@/constants/icon.constant";
import type { SeabedConstant } from "@/constants/seabed.constant";
import { SeabedPresenter } from "@/presenters/map/seabed.presenter";

const props = defineProps<{
  seabedRoute: number;
  mapVariant: number;
  locationId: string | null;
}>();

const { t } = useI18n();

const routeLocation = computed(() => {
  if (props.locationId === null) {
    return null;
  }

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

function getEmergeEmoji(on: SeabedConstant): string {
  return IconConstant[on];
}

function getLocationName(locationId: string): string {
  return t(`location.${locationId}`);
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
          <span class="inline-flex leading-none text-[1.1rem] -translate-y-0.5">{{ getEmergeEmoji(point.on) }}</span>
          <span class="text-[9px] text-cyan-100 text-center leading-tight">{{ getLocationName(point.location) }}</span>
        </div>
      </div>

      <div v-if="hasLeft" class="flex items-center gap-1.5 w-full">
        <span class="inline-flex leading-none text-[1.1rem] -translate-y-0.5">⬅️</span>
        <div class="flex flex-col text-[9px] text-cyan-200 text-left leading-tight min-w-0">
          <span v-for="(locationId, index) in leftLocations" :key="index">{{ getLocationName(locationId) }}</span>
        </div>
      </div>

      <div v-if="hasRight" class="flex items-center justify-end gap-1.5 w-full">
        <div class="flex flex-col text-[9px] text-cyan-300 text-right leading-tight min-w-0">
          <span v-for="(locationId, index) in rightLocations" :key="index">{{ getLocationName(locationId) }}</span>
        </div>
        <span class="inline-flex leading-none text-[1.1rem] -translate-y-0.5">➡️</span>
      </div>
    </div>
  </div>
</template>
