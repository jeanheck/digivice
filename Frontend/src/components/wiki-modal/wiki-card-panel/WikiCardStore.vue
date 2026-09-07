<script setup lang="ts">
import type { WikiCardStoreViewModel } from "@/viewmodels/wiki-modal/wiki-card-store.viewmodel";

defineProps<{
  store: WikiCardStoreViewModel;
}>();

const emit = defineEmits<{
  (e: "select", storeId: string): void;
}>();

const handleSelect = (storeId: string): void => {
  emit("select", storeId);
};
</script>

<template>
  <button
    type="button"
    class="flex items-center gap-2 px-2.5 py-2 rounded text-left transition-colors cursor-pointer bg-blue-900/30 hover:bg-blue-900/50 border border-blue-800/60"
    @click="handleSelect(store.storeId)"
  >
    <span class="min-w-0">
      <span class="block text-xs font-bold text-blue-200 tracking-wide">
        {{ $t(store.labelKey) }}
      </span>
    </span>
  </button>
</template>
