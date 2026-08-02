<script setup lang="ts">
import { useI18n } from "vue-i18n";
import usFlag from "@/assets/icons/flags/us.png";
import usFlag2x from "@/assets/icons/flags/us@2x.png";
import brFlag from "@/assets/icons/flags/br.png";
import brFlag2x from "@/assets/icons/flags/br@2x.png";

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
      <img :src="usFlag" :srcset="`${usFlag2x} 2x`" width="20" alt="US" class="block" />
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
      <img :src="brFlag" :srcset="`${brFlag2x} 2x`" width="20" alt="BR" class="block" />
    </button>
  </div>
</template>
