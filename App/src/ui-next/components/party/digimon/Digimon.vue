<script setup lang="ts">
import ExperienceProgressBar from "@/ui-next/components/party/progress-bar/ExperienceProgressBar.vue";
import HpProgressBar from "@/ui-next/components/party/progress-bar/HpProgressBar.vue";
import MpProgressBar from "@/ui-next/components/party/progress-bar/MpProgressBar.vue";
import BlastProgressBar from "@/ui-next/components/party/progress-bar/BlastProgressBar.vue";
import DigievolutionsButton from "@/ui-next/components/party/DigievolutionsButton.vue";
import TrainingPoints from "@/ui-next/components/party/TrainingPoints.vue";
import PartyDigievolutions from "@/ui-next/components/party/digievolutions/PartyDigievolutions.vue";
import PartyStats from "@/ui-next/components/party/stats/PartyStats.vue";
import PartyEquipments from "@/ui-next/components/party/equipments/PartyEquipments.vue";
import type { PartyDigimonMock } from "@/ui-next/mocks/party.mock";

const { digimon } = defineProps<{
  digimon: PartyDigimonMock;
}>();
</script>

<template>
  <div class="flex-1 px-2 pt-2 pb-2 flex flex-col gap-1">
    <ExperienceProgressBar
      :digimon-id="digimon.digimonId"
      :level="digimon.level"
      :experience="digimon.experience"
    />
    <HpProgressBar :hp="digimon.hp" />
    <MpProgressBar :mp="digimon.mp" />
    <div class="grid grid-cols-4 gap-2">
      <DigievolutionsButton />
      <TrainingPoints :tp="digimon.tp" />
      <BlastProgressBar class="col-span-2" :blast="digimon.blast" />
    </div>
    <PartyDigievolutions
      class="mt-2"
      :digievolutions="digimon.digievolutions"
      :active-digievolution-id="digimon.activeDigievolutionId"
    />
    <div class="mt-2 flex items-stretch gap-1">
      <PartyStats class="flex-1 min-w-0" :stats="digimon.stats" />
      <PartyEquipments :equipments="digimon.equipments" />
    </div>
  </div>
</template>
