<script setup lang="ts">
import { computed } from "vue";
import { ImageCatalog } from "@/catalogs/image.catalog";

const props = defineProps<{
  battleFieldId: number;
}>();

const fieldImageUrl = computed(() => {
  return ImageCatalog.getDigimonBattleFieldImageUrl(props.battleFieldId);
});
</script>

<template>
  <div
    v-if="fieldImageUrl"
    class="absolute -left-3 -right-3 -top-1.5 -bottom-1.5 z-0 bg-cover bg-center pointer-events-none"
    :style="{ backgroundImage: `url(${fieldImageUrl})` }"
  />
</template>
