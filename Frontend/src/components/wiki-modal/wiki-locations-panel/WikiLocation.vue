<script setup lang="ts">
import { computed, ref } from "vue";
import { useI18n } from "vue-i18n";
import { splitLocationLabel } from "@/presenters/helper/location-label.helper";
import type { WikiLocationViewModel } from "@/viewmodels/wiki-modal/wiki-location.viewmodel";

const props = withDefaults(
  defineProps<{
    location: WikiLocationViewModel;
    isSelected: boolean;
    variant?: "chip" | "list";
  }>(),
  {
    variant: "chip",
  },
);

defineEmits<{
  select: [];
}>();

const { t } = useI18n();

const rootButton = ref<HTMLButtonElement | null>(null);

const locationLabel = computed(() => {
  return t(props.location.labelKey);
});

const locationLabelLines = computed(() => {
  return splitLocationLabel(locationLabel.value);
});

const isListVariant = computed(() => {
  return props.variant === "list";
});

defineExpose({
  rootButton,
});
</script>

<template>
  <button
    ref="rootButton"
    type="button"
    class="text-left px-2.5 py-1.5 rounded border transition-colors cursor-pointer"
    :class="[
      isListVariant ? 'w-full shrink-0' : 'shrink-0',
      isSelected
        ? 'bg-blue-900/40 border-[#0033aa] text-blue-100'
        : 'border-transparent text-white hover:bg-blue-900/40',
    ]"
    @click="$emit('select')"
  >
    <span
      v-if="isListVariant"
      class="grid grid-cols-[minmax(0,1fr)_auto] items-start gap-x-1 w-full"
    >
      <span class="min-w-0 text-[10px] font-bold tracking-wide leading-tight text-left">
        <span
          v-for="(line, lineIndex) in locationLabelLines"
          :key="lineIndex"
          class="block"
        >
          {{ line }}
        </span>
      </span>
      <span class="shrink-0 self-center flex gap-0.5">
        <span
          v-for="source in location.sources"
          :key="source.ariaLabelKey"
          class="text-[12px] 2xl:text-[14px] leading-none"
          :aria-label="$t(source.ariaLabelKey)"
        >
          <span class="inline-flex leading-none text-[1.2rem] -translate-y-1">{{ source.icon }}</span>
        </span>
      </span>
    </span>

    <span
      v-else
      class="inline-flex items-center gap-1.5 text-[10px] font-bold tracking-wide whitespace-nowrap"
    >
      <span>{{ locationLabel }}</span>
      <span
        v-for="source in location.sources"
        :key="source.ariaLabelKey"
        class="text-[12px] 2xl:text-[14px] leading-none"
        :aria-label="$t(source.ariaLabelKey)"
      >
        <span class="inline-flex leading-none text-[1.2rem] -translate-y-1">{{ source.icon }}</span>
      </span>
    </span>
  </button>
</template>
