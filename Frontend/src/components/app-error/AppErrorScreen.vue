<script setup lang="ts">
import { useI18n } from "vue-i18n";
import type { AppBlockingErrorViewModel } from "@/models/app-blocking-error";

const props = defineProps<{
    error: AppBlockingErrorViewModel;
}>();

const { t } = useI18n();
</script>

<template>
  <div
    class="fixed inset-0 z-200 flex items-center justify-center bg-[#000030]/95 p-6"
    role="alert"
    aria-live="assertive"
  >
    <div class="dw3-panel max-w-lg w-full shadow-text">
      <div class="dw3-panel-border dw3-beveled"></div>
      <div class="dw3-panel-inner dw3-beveled"></div>

      <div class="dw3-panel-content flex flex-col gap-4 p-6 text-center">
        <h1 class="text-sm leading-relaxed shadow-text-dark">
          {{ t(props.error.titleKey) }}
        </h1>

        <p class="text-[0.65rem] leading-relaxed text-blue-200 opacity-90">
          {{ t(props.error.hintKey) }}
        </p>

        <p
          v-if="props.error.detail"
          class="text-[0.55rem] leading-relaxed text-blue-300/80 wrap-break-word font-mono"
        >
          {{ props.error.detail }}
        </p>
      </div>
    </div>
  </div>
</template>
