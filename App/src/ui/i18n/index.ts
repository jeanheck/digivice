import { watch } from "vue";
import { createI18n } from "vue-i18n";
import enUS from "./en-US";
import ptBR from "./pt-BR";
import esES from "./es-ES";

function getSystemLocale(): string {
  const browserLocale = navigator.language;
  if (browserLocale.startsWith("pt")) {
    return "pt-BR";
  }
  if (browserLocale.startsWith("es")) {
    return "es-ES";
  }
  return "en-US";
}

// Saved choice first, then the system language.
const savedLocale = localStorage.getItem("user-locale");
const defaultLocale = savedLocale || getSystemLocale();

const i18n = createI18n({
  legacy: false,
  locale: defaultLocale,
  fallbackLocale: "en-US",
  messages: {
    "en-US": enUS,
    "pt-BR": ptBR,
    "es-ES": esES,
  },
});

watch(
  i18n.global.locale,
  (locale) => {
    document.documentElement.lang = locale;
  },
  { immediate: true },
);

export default i18n;
