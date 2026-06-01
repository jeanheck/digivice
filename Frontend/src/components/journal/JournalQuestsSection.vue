<script setup lang="ts">
import { ref } from "vue";

const props = withDefaults(defineProps<{
  title: string;
  titleClass: string;
  borderClass: string;
  chevronClass?: string;
  headerHoverClass?: string;
  defaultExpanded?: boolean;
}>(), {
  chevronClass: "text-blue-500",
  headerHoverClass: "hover:bg-blue-900/30",
  defaultExpanded: false,
});

const isExpanded = ref(props.defaultExpanded);

const toggle = () => {
  isExpanded.value = !isExpanded.value;
};
</script>

<template>
  <section>
    <div
      class="flex items-center justify-between mb-2 border-b pb-1 cursor-pointer transition-colors p-1 -mx-1 rounded"
      :class="[borderClass, headerHoverClass]"
      @click="toggle"
    >
      <h4 class="text-xs font-bold uppercase tracking-wide" :class="titleClass">
        {{ title }}
      </h4>
      <span
        class="text-xs transform transition-transform duration-300"
        :class="[chevronClass, { 'rotate-180': isExpanded }]"
      >
        ▼
      </span>
    </div>

    <div v-show="isExpanded" class="space-y-2">
      <slot />
    </div>
  </section>
</template>
