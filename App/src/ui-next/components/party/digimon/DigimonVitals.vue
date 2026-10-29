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
  <div class="relative text-white dw3-beveled">
    <div class="absolute inset-0 bg-progress-frame dw3-beveled pointer-events-none"></div>
    <div class="absolute inset-[1.5px] bg-surface dw3-beveled pointer-events-none"></div>

    <div class="relative z-10 px-2 py-1.5 flex flex-col gap-1">
      <div class="flex items-center gap-2">
        <ExperienceProgressBar
          class="flex-1 min-w-0"
          :digimon-id="digimon.digimonId"
          :level="digimon.level"
          :experience="digimon.experience"
        />
        <div
          class="h-6 px-2 shrink-0 bg-track border-2 border-progress-frame rounded flex items-center gap-1 font-bold font-chakra-petch text-white leading-none"
        >
          <span class="text-[10px] text-gray-300">{{ $t("digimon.lv") }}</span>
          <span class="text-[14px]">{{ digimon.level }}</span>
        </div>
      </div>
      <HpProgressBar :hp="digimon.hp" />
      <MpProgressBar :mp="digimon.mp" />
      <div class="grid grid-cols-4 gap-2">
        <DigievolutionsButton />
        <TrainingPoints :tp="digimon.tp" />
        <BlastProgressBar class="col-span-2" :blast="digimon.blast" />
      </div>
    </div>
  </div>
</template>
