<script setup lang="ts">
import type { WikiCardStoreViewModel } from "@/viewmodels/wiki-modal/wiki-card-store.viewmodel";

defineProps<{
  store: WikiCardStoreViewModel;
}>();
</script>

<template>
  <button
    type="button"
    class="flex items-center gap-2 px-2.5 py-2 rounded text-left transition-colors cursor-default bg-blue-900/30 border border-blue-800/60"
  >
    <span class="min-w-0">
      <span class="block text-xs font-bold text-blue-200 tracking-wide">
        {{ $t(store.labelKey) }}
      </span>
    </span>
  </button>
</template>
