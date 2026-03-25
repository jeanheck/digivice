import { createI18n } from 'vue-i18n';
import enUS from './locales/en-US.json';
import ptBR from './locales/pt-BR.json';

// Tenta detectar o idioma do navegador ou usa o que estiver no localStorage
const savedLocale = localStorage.getItem('user-locale');
const browserLocale = navigator.language;
const defaultLocale = savedLocale || (browserLocale.startsWith('pt') ? 'pt-BR' : 'en-US');

const i18n = createI18n({
  legacy: false, // Usar Composition API
  locale: defaultLocale,
  fallbackLocale: 'en-US',
  messages: {
    'en-US': enUS,
    'pt-BR': ptBR
  }
});

export default i18n;
