<template>
  <div class="p-4 rounded border relative overflow-hidden shrink-0 border-gray-700/40 bg-[#0a0a1a] opacity-70">
    <div class="relative z-10 space-y-2">
      <div class="flex items-start justify-between gap-2">
        <h3 class="font-bold text-sm leading-snug text-gray-500">
          {{ $t("auction.currentIdleTitle") }}
        </h3>

        <span class="text-xs shrink-0">🔒</span>
      </div>

      <p class="text-[11px] leading-relaxed text-gray-400">
        {{ $t("auction.currentIdleSubtitle") }}
      </p>
    </div>
  </div>
</template>
