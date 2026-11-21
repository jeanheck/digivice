<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { MapService } from "@/presentation/services/map.service";

const props = withDefaults(
  defineProps<{
    locationId: string;
    titleOverride?: string | null;
    isSafeZone?: boolean;
    titleColorClass?: string | null;
  }>(),
  {
    titleOverride: null,
    isSafeZone: false,
    titleColorClass: null,
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

  return t(`map.${props.locationId}`);
});

const isClickable = computed(() => {
  return MapService.getWorldLocation(props.locationId) !== undefined;
});

const titleClass = computed(() => {
  const colorClass =
    props.titleColorClass ?? (props.isSafeZone ? "text-sky-300" : "text-gray-200");
  const glowClass =
    props.titleColorClass !== null
      ? "drop-shadow-[0_0_5px_rgba(45,212,191,0.85)]"
      : "drop-shadow-[0_0_5px_rgba(0,170,255,0.8)]";
  return `text-sm font-bold ${colorClass} tracking-widest uppercase ${glowClass} leading-tight`;
});

const clickableHoverClass = computed(() => {
  if (props.titleColorClass !== null) {
    return "hover:brightness-125";
  }

  return props.isSafeZone ? "hover:text-sky-200" : "hover:text-gray-100";
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
