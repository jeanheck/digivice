<script setup lang="ts">
import { useI18n } from "vue-i18n";
import type { AppHealthyScreenViewModel } from "@/viewmodels/app-healthy/app-healthy-screen.viewmodel";

const props = defineProps<{
  viewModel: AppHealthyScreenViewModel;
}>();

const { t } = useI18n();
</script>

<template>
  <div
    class="fixed inset-0 z-200 flex items-center justify-center bg-[#000030]/95 p-6"
    :role="props.viewModel.kind === 'loading' ? 'status' : 'alert'"
    :aria-live="props.viewModel.kind === 'loading' ? 'polite' : 'assertive'"
  >
    <div class="dw3-panel max-w-lg w-full shadow-text">
      <div class="dw3-panel-border dw3-beveled"></div>
      <div class="dw3-panel-inner dw3-beveled"></div>

      <div class="dw3-panel-content flex flex-col gap-4 p-6 text-center">
        <h1 class="text-sm leading-relaxed shadow-text-dark">
          {{ t(props.viewModel.titleKey) }}
        </h1>

        <p class="text-[0.65rem] leading-relaxed text-blue-200 opacity-90">
          {{ t(props.viewModel.hintKey) }}
        </p>

        <p
          v-if="props.viewModel.detail"
          class="text-[0.55rem] leading-relaxed text-blue-300/80 wrap-break-word font-mono"
        >
          {{ props.viewModel.detail }}
        </p>
      </div>
    </div>
  </div>
</template>
