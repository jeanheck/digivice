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
      class="hover:scale-110 transition-transform cursor-pointer grayscale hover:grayscale-0 flex items-center justify-center rounded-sm overflow-hidden"
      :class="{ 'grayscale-0 ring-2 ring-orange-500 ring-offset-1 ring-offset-[#000a2b]': locale === 'en-US' }"
      title="English"
    >
      <img src="https://flagcdn.com/w20/us.png" srcset="https://flagcdn.com/w40/us.png 2x" width="20" alt="US" class="block">
    </button>
    <button 
      @click="setLocale('pt-BR')" 
      class="hover:scale-110 transition-transform cursor-pointer grayscale hover:grayscale-0 flex items-center justify-center rounded-sm overflow-hidden"
      :class="{ 'grayscale-0 ring-2 ring-orange-500 ring-offset-1 ring-offset-[#000a2b]': locale === 'pt-BR' }"
      title="Português"
    >
      <img src="https://flagcdn.com/w20/br.png" srcset="https://flagcdn.com/w40/br.png 2x" width="20" alt="BR" class="block">
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
