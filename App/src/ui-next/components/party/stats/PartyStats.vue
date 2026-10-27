<script setup lang="ts">
import { useI18n } from "vue-i18n";
import PartyStat from "@/ui-next/components/party/stats/PartyStat.vue";
import { DigimonAttributes } from "@/shared/constants/digimon-attribute.constant";
import { DigimonElements } from "@/shared/constants/digimon-element.constant";
import type { DigimonStat } from "@/shared/types/digimon-stat.type";
import type { PartyStatsMock } from "@/ui-next/mocks/party.mock";

const { stats } = defineProps<{
  stats: PartyStatsMock;
}>();

const { t } = useI18n();

// Code points without U+FE0F, otherwise WebView2 falls back to the colored Segoe UI Emoji.
const StatIcons = {
  strength: "\u{1F44A}",
  defense: "\u{1F6E1}",
  spirit: "\u{1F9D9}",
  wisdom: "\u{1F4D6}",
  speed: "\u{1F3C3}",
  charisma: "\u{2728}",
  fire: "\u{1F525}",
  water: "\u{1F4A7}",
  ice: "\u{1F9CA}",
  wind: "\u{1F343}",
  thunder: "\u{26A1}",
  machine: "\u{2699}",
  dark: "\u{1F311}",
} as const satisfies Record<DigimonStat, string>;
</script>

<template>
  <div class="relative text-white dw3-beveled">
    <div class="absolute inset-0 bg-card-frame dw3-beveled pointer-events-none"></div>
    <div class="absolute inset-[1.5px] bg-surface dw3-beveled pointer-events-none"></div>

    <div class="relative z-10 px-2 py-1.5 grid grid-cols-2 gap-x-2">
      <div class="flex flex-col gap-1">
        <PartyStat
          v-for="attribute in DigimonAttributes"
          :key="attribute"
          :icon="StatIcons[attribute]"
          :label="t(`stat.${attribute}`)"
          :stat="stats.attributes[attribute]"
        />
      </div>

      <div class="flex flex-col gap-1">
        <PartyStat
          v-for="element in DigimonElements"
          :key="element"
          :icon="StatIcons[element]"
          :label="t(`stat.${element}`)"
          :stat="stats.elements[element]"
        />
      </div>
    </div>
  </div>
</template>
