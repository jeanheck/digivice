<script setup lang="ts">
import { useI18n } from "vue-i18n";

const { locale } = useI18n();

const setLocale = (newLocale: string) => {
  locale.value = newLocale;
  localStorage.setItem("user-locale", newLocale);
};
</script>

<template>
  <div class="flex items-center gap-2">
    <button
      @click="setLocale('en-US')"
      class="bg-transparent border-0 px-1 py-0.5 leading-none hover:scale-110 transition-transform cursor-pointer grayscale hover:grayscale-0 flex items-center justify-center rounded-sm overflow-hidden"
      :class="{
        'grayscale-0 ring-2 ring-orange-500 ring-offset-1 ring-offset-[#000a2b]':
          locale === 'en-US',
      }"
      title="English"
    >
      <span class="font-emoji-color text-lg leading-none">🇺🇸</span>
    </button>
    <button
      @click="setLocale('pt-BR')"
      class="bg-transparent border-0 px-1 py-0.5 leading-none hover:scale-110 transition-transform cursor-pointer grayscale hover:grayscale-0 flex items-center justify-center rounded-sm overflow-hidden"
      :class="{
        'grayscale-0 ring-2 ring-orange-500 ring-offset-1 ring-offset-[#000a2b]':
          locale === 'pt-BR',
      }"
      title="Português"
    >
      <span class="font-emoji-color text-lg leading-none">🇧🇷</span>
    </button>
  </div>
</template>
