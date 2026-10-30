<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";

const { direction } = defineProps<{
  direction: "previous" | "next";
}>();

const emit = defineEmits<{
  click: [];
}>();

const { t } = useI18n();

const label = computed(() => {
  return direction === "previous" ? t("mainPanel.previousSection") : t("mainPanel.nextSection");
});

const icon = computed(() => {
  return direction === "previous" ? "\u{2B05}" : "\u{27A1}";
});
</script>

<template>
  <button
    type="button"
    class="bg-transparent border-0 px-1 py-0.5 leading-none hover:scale-110 transition-transform cursor-pointer"
    :title="label"
    :aria-label="label"
    @click="emit('click')"
  >
    <span class="inline-flex leading-none text-[25px] -translate-y-0.5 font-emoji text-foreground">{{ icon }}</span>
  </button>
</template>
