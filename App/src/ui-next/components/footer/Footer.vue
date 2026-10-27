<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { useEmulatorToggle } from "@/ui-next/composables/use-emulator-fit";
import { useServerThemeToggle } from "@/ui-next/composables/use-server-theme";
import FooterIconButton from "@/ui-next/components/footer/FooterIconButton.vue";
const { t } = useI18n();
const { isFitted, isFitting, toggle } = useEmulatorToggle();
const { isAmaterasu, toggle: toggleServerTheme } = useServerThemeToggle();

const fitLabel = computed(() => {
  return isFitted.value ? t("emulatorSlot.releaseButton") : t("emulatorSlot.fitButton");
});
</script>

<template>
  <footer class="w-full h-12 px-4 flex items-center bg-surface border-t-2 border-frame">
    <div class="ml-auto flex items-center gap-3">
      <FooterIconButton
        icon="⛶"
        :label="fitLabel"
        :is-active="isFitted"
        :is-disabled="isFitting"
        @click="toggle"
      />
      <FooterIconButton
        icon="🌸"
        :label="t('serverTheme.amaterasuButton')"
        :is-active="isAmaterasu"
        @click="toggleServerTheme"
      />
    </div>
  </footer>
</template>
