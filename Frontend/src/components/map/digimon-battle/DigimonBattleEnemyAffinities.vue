<script setup lang="ts">
import { useI18n } from "vue-i18n";

const { t } = useI18n();

const weaknessEmojis = ["🔥", "⚡"];
const resistanceEmojis = ["💧", "🌪️"];
</script>

<template>
  <div
    class="absolute top-2 right-2 z-5 flex flex-col items-end gap-1.25 pointer-events-none"
  >
    <div class="flex items-center gap-1 leading-tight">
      <span class="leading-none inline-flex -translate-y-0.5 text-[12px] 2xl:text-base drop-shadow-[0_1px_2px_rgba(0,0,0,1)]">
        {{ weaknessEmojis.join("") }}
      </span>
      <span
        class="text-[9px] 2xl:text-sm font-bold tracking-wide text-green-400 text-outline-black-glow"
      >
        {{ t("digimonBattle.effective") }}
      </span>
    </div>
    <div class="flex items-center gap-1 leading-tight">
      <span class="leading-none inline-flex -translate-y-0.5 text-[12px] 2xl:text-base drop-shadow-[0_1px_2px_rgba(0,0,0,1)]">
        {{ resistanceEmojis.join("") }}
      </span>
      <span
        class="text-[9px] 2xl:text-sm font-bold tracking-wide text-red-400 text-outline-black-glow"
      >
        {{ t("digimonBattle.notEffective") }}
      </span>
    </div>
  </div>
</template>
