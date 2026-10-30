<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import MainPanelMenu from "@/ui-next/components/main-panel/MainPanelMenu.vue";
import Party from "@/ui-next/components/party/Party.vue";
import MapPanel from "@/ui-next/components/map/MapPanel.vue";
import SeabedMap from "@/ui-next/components/map/seabed/SeabedMap.vue";
import MobiusDesertMap from "@/ui-next/components/map/mobius-desert/MobiusDesertMap.vue";
import CardBattleMap from "@/ui-next/components/map/card-battle/CardBattleMap.vue";
import DigimonBattleMap from "@/ui-next/components/map/digimon-battle/DigimonBattleMap.vue";
import { useMainPanelSection } from "@/ui-next/composables/use-main-panel-section";
import { useMapPreview } from "@/ui-next/composables/use-map-preview";
import { SeabedLongestNamesMapMock, SeabedMapMock } from "@/ui-next/mocks/map.mock";

const { t } = useI18n();
const { activeSection, showPrevious, showNext } = useMainPanelSection();
const { activeMap } = useMapPreview();

const sectionLabel = computed(() => {
  return t(`mainPanel.sections.${activeSection.value}`);
});
</script>

<template>
  <aside class="h-full flex flex-col bg-surface border-l-2 border-frame">
    <MainPanelMenu v-show="false" :label="sectionLabel" @previous="showPrevious" @next="showNext" />
    <section class="min-h-0 overflow-y-auto">
      <Party v-if="activeSection === 'party'" />
    </section>
    <div class="flex-1 flex flex-col px-2 pb-2 bg-surface-active">
      <MapPanel v-if="activeMap === 'location'" class="flex-1" />
      <SeabedMap v-else-if="activeMap === 'seabed'" class="flex-1" :mock="SeabedMapMock" />
      <SeabedMap
        v-else-if="activeMap === 'seabedLongestNames'"
        class="flex-1"
        :mock="SeabedLongestNamesMapMock"
      />
      <MobiusDesertMap v-else-if="activeMap === 'mobiusDesert'" class="flex-1" />
      <CardBattleMap v-else-if="activeMap === 'cardBattle'" class="flex-1" />
      <DigimonBattleMap v-else class="flex-1" />
    </div>
  </aside>
</template>
