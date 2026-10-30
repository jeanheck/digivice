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
  return ImageCatalog.getDigimonImageUrl(name) ?? ImageCatalog.getDigimonImageUrl(`${name}-healthy`);
});
</script>

<template>
  <button
    type="button"
    class="flex items-center px-1 py-0.5 border-t-2 border-l-2 last:border-r-2 border-frame text-[15px] font-bold text-foreground"
    :class="
      isActive
        ? 'flex-1 min-w-0 gap-2 bg-surface-active -mb-0.5 pb-1 cursor-default'
        : 'shrink-0 gap-1 bg-surface cursor-pointer'
    "
    :title="name"
    :aria-label="name"
    :aria-pressed="isActive"
    @click="emit('click')"
  >
    <img v-if="imageUrl" :src="imageUrl" :alt="name" class="h-7.5 w-auto" />
    <span v-if="isActive">{{ name }}</span>
    <span class="ml-auto">{{ level }}</span>
  </button>
</template>
