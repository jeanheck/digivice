<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import MainPanelMenu from "@/ui-next/components/main-panel/MainPanelMenu.vue";
import Party from "@/ui-next/components/party/Party.vue";
import MapPanel from "@/ui-next/components/map/MapPanel.vue";
import { useMainPanelSection } from "@/ui-next/composables/use-main-panel-section";

const { t } = useI18n();
const { activeSection, showPrevious, showNext } = useMainPanelSection();

const sectionLabel = computed(() => {
  return t(`mainPanel.sections.${activeSection.value}`);
});
</script>

<template>
  <aside class="h-full flex flex-col bg-surface border-l-2 border-frame">
    <MainPanelMenu :label="sectionLabel" @previous="showPrevious" @next="showNext" />
    <section class="min-h-0 overflow-y-auto">
      <Party v-if="activeSection === 'party'" />
    </section>
    <div class="flex-1 flex flex-col px-2 pb-4 bg-surface-active">
      <MapPanel class="flex-1" />
    </div>
  </aside>
</template>
