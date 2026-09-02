<script setup lang="ts">
import { computed } from "vue";
import WikiLocationMapMarker from "@/components/wiki-modal/wiki-locations-panel/WikiLocationMapMarker.vue";
import { useMapFrame } from "@/composables/use-map-frame";
import type { WikiLocationMapMarkerViewModel } from "@/viewmodels/wiki-modal/wiki-location-map-marker.viewmodel";

const props = defineProps<{
  imageUrl: string | null;
  width: number;
  markers: WikiLocationMapMarkerViewModel[];
}>();

const emit = defineEmits<{
  (e: "open-npc", npcId: string): void;
  (e: "open-enemy", enemyId: string): void;
}>();

const imageUrl = computed(() => {
  return props.imageUrl;
});

const frameWidth = computed(() => {
  return props.width;
});

const { mapImageFrameStyle, onImageLoad } = useMapFrame(imageUrl, frameWidth);

const frameStyle = computed(() => {
  return {
    width: `${props.width}px`,
  };
});

const handleMarkerSelect = (marker: WikiLocationMapMarkerViewModel): void => {
  if (marker.kind === "boss") {
    emit("open-enemy", marker.id);
    return;
  }

  emit("open-npc", marker.id);
};
</script>

<template>
  <div
    v-if="imageUrl"
    class="relative shrink-0 min-h-0 bg-[#00051a] border border-cyan-800/50 rounded shadow-[0_0_15px_rgba(0,170,255,0.1)] overflow-hidden"
    :style="frameStyle"
  >
    <div class="relative" :style="mapImageFrameStyle">
      <img
        :src="imageUrl"
        class="block w-full h-full"
        @load="onImageLoad"
      />

      <WikiLocationMapMarker
        v-for="marker in markers"
        :key="`${marker.kind}-${marker.id}`"
        :marker="marker"
        @select="handleMarkerSelect(marker)"
      />
    </div>
  </div>
</template>
