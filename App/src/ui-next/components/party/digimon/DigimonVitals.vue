<script setup lang="ts">
import ExperienceProgressBar from "@/ui-next/components/party/progress-bar/ExperienceProgressBar.vue";
import HpProgressBar from "@/ui-next/components/party/progress-bar/HpProgressBar.vue";
import MpProgressBar from "@/ui-next/components/party/progress-bar/MpProgressBar.vue";
import BlastProgressBar from "@/ui-next/components/party/progress-bar/BlastProgressBar.vue";
import DigievolutionsButton from "@/ui-next/components/party/DigievolutionsButton.vue";
import TrainingPoints from "@/ui-next/components/party/TrainingPoints.vue";
import type { PartyDigimonMock } from "@/ui-next/mocks/party.mock";

const { digimon } = defineProps<{
  digimon: PartyDigimonMock;
}>();
</script>

<template>
  <div class="relative text-foreground dw3-beveled">
    <div class="absolute inset-0 bg-progress-frame dw3-beveled pointer-events-none"></div>
    <div class="absolute inset-[1.5px] bg-surface-card dw3-beveled pointer-events-none"></div>

    <div class="relative z-10 px-2 py-1.5 grid grid-cols-4 gap-x-2 gap-y-1">
      <HpProgressBar class="col-span-3" :hp="digimon.hp" />
      <DigievolutionsButton />
      <MpProgressBar class="col-span-3" :mp="digimon.mp" />
      <TrainingPoints :tp="digimon.tp" />
      <BlastProgressBar class="col-span-2" :blast="digimon.blast" />
      <ExperienceProgressBar
        class="col-span-2"
        :digimon-id="digimon.digimonId"
        :level="digimon.level"
        :experience="digimon.experience"
      />
    </div>
  </div>
</template>
