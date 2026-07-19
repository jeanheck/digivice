<script setup lang="ts">
import { useI18n } from "vue-i18n";

const { t } = useI18n();
</script>

<template>
  <div
    class="p-2 rounded border cursor-pointer transition-all duration-200 group relative overflow-hidden border-gray-700/40 bg-[#0a0a1a] opacity-50 hover:opacity-70"
  >
    <div class="flex items-center justify-between mb-1 relative z-10">
      <span class="font-bold truncate transition-colors text-xs text-gray-500">
        {{ t("auction.cardIdleTitle") }}
      </span>

      <span class="text-xs shrink-0 ml-2">🔒</span>
    </div>

    <p class="text-gray-400 text-[10px] leading-tight line-clamp-2 relative z-10 opacity-50">
      {{ t("auction.cardIdleSubtitle") }}
    </p>
  </div>
</template>
