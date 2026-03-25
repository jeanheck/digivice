<script setup lang="ts">
import { useI18n } from 'vue-i18n';

const { locale } = useI18n();

const setLocale = (newLocale: string) => {
  locale.value = newLocale;
  localStorage.setItem('user-locale', newLocale);
};
</script>

<template>
  <div class="flex items-center gap-2">
    <button 
      @click="setLocale('en-US')" 
      class="hover:scale-110 transition-transform cursor-pointer grayscale hover:grayscale-0"
      :class="{ 'grayscale-0 border-b-2 border-orange-500': locale === 'en-US' }"
      title="English"
    >
      <span class="text-xl">🇺🇸</span>
    </button>
    <button 
      @click="setLocale('pt-BR')" 
      class="hover:scale-110 transition-transform cursor-pointer grayscale hover:grayscale-0"
      :class="{ 'grayscale-0 border-b-2 border-orange-500': locale === 'pt-BR' }"
      title="Português"
    >
      <span class="text-xl">🇧🇷</span>
    </button>
  </div>
</template>

<style scoped>
button {
  background: none;
  border: none;
  padding: 2px 4px;
  line-height: 1;
}
</style>
