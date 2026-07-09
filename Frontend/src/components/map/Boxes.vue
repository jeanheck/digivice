<script setup lang="ts">
const emit = defineEmits<{
  click: [];
}>();

function onClick(): void {
  emit("click");
}
</script>

<template>
  <button type="button" class="map-info-panel map-info-panel-action" @click="onClick">
    <span class="inline-flex leading-none text-[1.2rem] -translate-y-0.5">🎁</span>
  </button>
</template>
