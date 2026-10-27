<script setup lang="ts">
import { computed } from "vue";
import { ImageCatalog } from "@/ui/catalogs/image.catalog";

const { name, level, isActive } = defineProps<{
  name: string;
  level: number;
  isActive: boolean;
}>();

const emit = defineEmits<{
  click: [];
}>();

const imageUrl = computed(() => {
  return ImageCatalog.getDigimonImageUrl(`${name}-healthy`);
});
</script>

<template>
  <button
    type="button"
    class="flex items-center gap-2 px-2 py-1 border-t-2 border-l-2 border-frame first:border-l-0 text-[12px] text-white"
    :class="
      isActive
        ? 'flex-1 min-w-0 bg-surface-active -mb-0.5 pb-1.5 cursor-default'
        : 'shrink-0 bg-surface cursor-pointer'
    "
    :title="name"
    :aria-label="name"
    :aria-pressed="isActive"
    @click="emit('click')"
  >
    <img v-if="imageUrl" :src="imageUrl" :alt="name" class="w-7.5 h-7.5 object-cover" />
    <span v-if="isActive">{{ name }}</span>
    <span>{{ level }}</span>
  </button>
</template>
