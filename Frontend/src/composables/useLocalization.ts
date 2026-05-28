import { useI18n } from 'vue-i18n';

export function useLocalization() {
  const { locale, t } = useI18n();

  const getLocalized = (obj: any) => {
    if (!obj) return '';
    if (typeof obj === 'string') return obj;

    // Suporte para o padrão { "PT-BR": "...", "EN-US": "..." }
    const currentLocale = locale.value.toUpperCase();
    return obj[currentLocale] || obj['EN-US'] || '';
  };

  return {
    locale,
    t,
    getLocalized,
  };
}