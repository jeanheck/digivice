<script setup lang="ts">
import { computed, ref } from "vue";
import LanguageSelector from "@/ui/components/footer/LanguageSelector.vue";
import DnaDigievolutionsButton from "@/ui/components/footer/DnaDigievolutionsButton.vue";
import SeabedButton from "@/ui/components/footer/SeabedButton.vue";
import MobiusDesertButton from "@/ui/components/footer/MobiusDesertButton.vue";
import BuyMeACoffeeButton from "@/ui/components/footer/BuyMeACoffeeButton.vue";
import DescriptiveTooltip from "@/ui/components/tooltip/DescriptiveTooltip.vue";
import Tooltip from "@/ui/components/tooltip/Tooltip.vue";
import { useI18n } from "vue-i18n";
import { useTooltipPosition } from "@/ui/composables/use-tooltip-position";
import { useGameState } from "@/ui/composables/use-game-state";
import { FooterPresenter } from "@/presentation/presenters/footer/footer.presenter";

const gameState = useGameState();
const { t } = useI18n();

const playerBits = computed(() => {
  return gameState.value.player.bits;
});

const tooltipPlacement = "above" as const;
const { show: tooltipShow, x: tooltipX, y: tooltipY, showAt, move, hide } = useTooltipPosition();
const tooltipTitle = ref("");
const tooltipText = ref("");

function showTitleTooltip(event: MouseEvent, title: string): void {
  tooltipTitle.value = title;
  tooltipText.value = "";
  showAt(event, { placement: tooltipPlacement, align: "left" });
}

function showDescriptiveTooltip(event: MouseEvent, title: string, text: string): void {
  tooltipTitle.value = title;
  tooltipText.value = text;
  showAt(event, { placement: tooltipPlacement });
}

function moveTooltip(event: MouseEvent): void {
  move(event, tooltipPlacement);
}

function hideTooltip(): void {
  hide();
  tooltipTitle.value = "";
  tooltipText.value = "";
}

const groupCharisma = computed(() => {
  return FooterPresenter.getPartyCharisma(gameState.value.party);
});

const groupPartyLevel = computed(() => {
  return FooterPresenter.getPartyLevel(gameState.value.party);
});
</script>

<template>
  <footer
    class="dw3-frame [--dw3-frame-fill-color:var(--color-surface)] w-full text-white p-2 flex items-center gap-12 px-6"
  >
    <div class="font-bold text-ms flex items-baseline cursor-default">
      <span class="opacity-80 text-[0.7rem] mr-2 font-normal text-frame tracking-wider uppercase"
        >{{ $t("player.bits") }}:</span
      >
      <span class="text-white shadow-text">{{ playerBits }}</span>
    </div>

    <div
      class="font-bold text-ms flex items-baseline cursor-help"
      @mouseenter="
        showDescriptiveTooltip($event, t('party.groupCharisma'), t('party.groupCharismaWarning'))
      "
      @mousemove="moveTooltip"
      @mouseleave="hideTooltip"
    >
      <span class="opacity-80 text-[0.7rem] mr-2 font-normal text-frame tracking-wider uppercase"
        >{{ $t("party.groupCharisma") }}:</span
      >
      <span class="text-white shadow-text">{{ groupCharisma }}</span>
    </div>

    <div
      class="font-bold text-ms flex items-baseline cursor-help"
      @mouseenter="
        showDescriptiveTooltip($event, t('party.groupLevel'), t('party.groupLevelExplanation'))
      "
      @mousemove="moveTooltip"
      @mouseleave="hideTooltip"
    >
      <span class="opacity-80 text-[0.7rem] mr-2 font-normal text-frame tracking-wider uppercase"
        >{{ $t("party.groupLevel") }}:</span
      >
      <span class="text-white shadow-text">{{ groupPartyLevel }}</span>
    </div>

    <div
      class="absolute right-4 top-1/2 -translate-y-1/2 flex items-center gap-3 text-sm opacity-80"
    >
      <div class="flex items-center gap-3 border-r border-blue-900 pr-3">
        <SeabedButton
          @show-tooltip="(event) => showTitleTooltip(event, t('seabed.title'))"
          @move-tooltip="moveTooltip"
          @hide-tooltip="hideTooltip"
        />
        <MobiusDesertButton
          @show-tooltip="(event) => showTitleTooltip(event, t('mobiusDesert.title'))"
          @move-tooltip="moveTooltip"
          @hide-tooltip="hideTooltip"
        />
      </div>

      <div class="flex items-center gap-3 border-r border-blue-900 pr-3">
        <DnaDigievolutionsButton
          @show-tooltip="(event) => showTitleTooltip(event, t('dnaDigievolution.title'))"
          @move-tooltip="moveTooltip"
          @hide-tooltip="hideTooltip"
        />
      </div>

      <div class="flex items-center gap-3 border-r border-blue-900 pr-3">
        <LanguageSelector
          @show-tooltip="showTitleTooltip"
          @move-tooltip="moveTooltip"
          @hide-tooltip="hideTooltip"
        />
      </div>

      <div class="flex items-center">
        <BuyMeACoffeeButton
          @show-tooltip="(event) => showTitleTooltip(event, t('buyMeACoffee.title'))"
          @move-tooltip="moveTooltip"
          @hide-tooltip="hideTooltip"
        />
      </div>
    </div>
  </footer>

  <Tooltip
    :show="tooltipShow && tooltipText === ''"
    :x="tooltipX"
    :y="tooltipY"
    :title="tooltipTitle"
    placement="above"
    horizontal-align="left"
  />

  <DescriptiveTooltip
    :show="tooltipShow && tooltipText !== ''"
    :x="tooltipX"
    :y="tooltipY"
    :title="tooltipTitle"
    :text="tooltipText"
    placement="above"
  />
</template>
