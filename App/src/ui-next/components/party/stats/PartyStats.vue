<script setup lang="ts">
import { useI18n } from "vue-i18n";
import PartyStat from "@/ui-next/components/party/stats/PartyStat.vue";
import { ImageCatalog } from "@/ui/catalogs/image.catalog";
import { DigimonAttributes } from "@/shared/constants/digimon-attribute.constant";
import { DigimonElements } from "@/shared/constants/digimon-element.constant";
import type { DigimonStat } from "@/shared/types/digimon-stat.type";
import type { PartyStatsMock } from "@/ui-next/mocks/party.mock";

const { stats } = defineProps<{
  stats: PartyStatsMock;
}>();

const { t } = useI18n();

const StatImageNames = {
  strength: "Strength",
  defense: "Defense",
  spirit: "Spirit",
  wisdom: "Wisdom",
  speed: "Speed",
  charisma: "Charisma",
  fire: "Fire",
  water: "Water",
  ice: "Ice",
  wind: "Wind",
  thunder: "Thunder",
  machine: "Machine",
  dark: "Dark",
} as const satisfies Record<DigimonStat, string>;
</script>

<template>
  <div class="relative text-foreground dw3-beveled">
    <div class="absolute inset-0 bg-progress-frame dw3-beveled pointer-events-none"></div>
    <div class="absolute inset-[1.5px] bg-surface-card dw3-beveled pointer-events-none"></div>

    <div class="relative z-10 px-2 py-1.5 grid grid-cols-2 gap-x-2">
      <div class="flex flex-col">
        <PartyStat
          v-for="attribute in DigimonAttributes"
          :key="attribute"
          :image-url="ImageCatalog.getStatImageUrl(StatImageNames[attribute])"
          :label="t(`stat.${attribute}`)"
          :stat="stats.attributes[attribute]"
        />
      </div>

      <div class="flex flex-col">
        <PartyStat
          v-for="element in DigimonElements"
          :key="element"
          :image-url="ImageCatalog.getStatImageUrl(StatImageNames[element])"
          :label="t(`stat.${element}`)"
          :stat="stats.elements[element]"
        />
      </div>
    </div>
  </div>
</template>
