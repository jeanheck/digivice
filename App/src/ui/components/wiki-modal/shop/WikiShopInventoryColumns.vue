<script setup lang="ts" generic="Item">
import { computed } from "vue";

const props = defineProps<{
  items: Item[];
  itemKey: (item: Item) => string;
}>();

defineSlots<{
  item(props: { item: Item }): unknown;
}>();

const leftColumnSize = computed(() => {
  return Math.ceil(props.items.length / 2);
});

const leftColumnItems = computed(() => {
  return props.items.slice(0, leftColumnSize.value);
});

const rightColumnItems = computed(() => {
  return props.items.slice(leftColumnSize.value);
});
</script>

<template>
  <div class="flex flex-col flex-1 min-h-0 overflow-hidden">
    <div class="flex-1 min-h-0 overflow-y-auto overflow-x-hidden custom-scroll">
      <div class="grid grid-cols-2 divide-x divide-blue-900/50 w-full">
        <div class="flex flex-col gap-2 pr-4">
          <template v-for="item in leftColumnItems" :key="itemKey(item)">
            <slot name="item" :item="item" />
          </template>
        </div>
        <div class="flex flex-col gap-2 pl-4">
          <template v-for="item in rightColumnItems" :key="itemKey(item)">
            <slot name="item" :item="item" />
          </template>
        </div>
      </div>
    </div>
  </div>
</template>
