<script setup lang="ts">
defineProps<{
  dock: boolean;
}>();

const emit = defineEmits<{
  click: [];
}>();

function onClick(): void {
  emit("click");
}
</script>

<template>
  <button type="button" class="map-info-panel map-info-panel-action" @click="onClick">
    <span
      class="inline-flex leading-none text-[1.2rem] -translate-y-0.5"
      :class="{ 'filter-[grayscale(100%)_sepia(100%)_saturate(400%)_hue-rotate(5deg)_brightness(1.1)]': !dock }"
    >
      ⚓
    </span>
  </button>
</template>
