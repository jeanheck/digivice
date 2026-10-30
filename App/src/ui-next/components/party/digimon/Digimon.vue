<script setup lang="ts">
import DigimonVitals from "@/ui-next/components/party/digimon/DigimonVitals.vue";
import PartyDigievolutions from "@/ui-next/components/party/digievolutions/PartyDigievolutions.vue";
import PartyStats from "@/ui-next/components/party/stats/PartyStats.vue";
import PartyEquipments from "@/ui-next/components/party/equipments/PartyEquipments.vue";
import PartyEquipmentList from "@/ui-next/components/party/equipments/PartyEquipmentList.vue";
import type { PartyDigimonMock } from "@/ui-next/mocks/party.mock";

const { digimon } = defineProps<{
  digimon: PartyDigimonMock;
}>();

const IsEquipmentIconsVisible = false;
</script>

<template>
  <div class="flex-1 px-2 pt-2 pb-2 flex flex-col gap-2 bg-surface-active">
    <DigimonVitals :digimon="digimon" />
    <PartyDigievolutions
      :digievolutions="digimon.digievolutions"
      :active-digievolution-id="digimon.activeDigievolutionId"
    />
    <div class="flex items-stretch gap-1">
      <PartyStats class="flex-1 min-w-0" :stats="digimon.stats" />
      <PartyEquipments v-if="IsEquipmentIconsVisible" :equipments="digimon.equipments" />
    </div>
    <PartyEquipmentList :equipments="digimon.equipments" />
  </div>
</template>
