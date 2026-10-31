<script setup lang="ts">
import { computed, ref } from "vue";
import LanguageSelector from "@/ui/components/footer/LanguageSelector.vue";
import SeabedButton from "@/ui/components/footer/SeabedButton.vue";
import MobiusDesertButton from "@/ui/components/footer/MobiusDesertButton.vue";
import BuyMeACoffeeButton from "@/ui/components/footer/BuyMeACoffeeButton.vue";
import DefaultTooltip from "@/ui/components/tooltip/DefaultTooltip.vue";
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

const tooltipPosition = useTooltipPosition(300);
const { show: tooltipShow, x: tooltipX, y: tooltipY, showAt, move, hide } = tooltipPosition;
const tooltipTitle = ref("");
const tooltipText = ref("");
const isTitleOnlyTooltip = ref(false);

const showSeabedTooltip = (event: MouseEvent) => {
  isTitleOnlyTooltip.value = true;
  tooltipTitle.value = t("seabed.title");
  tooltipText.value = "";
  showAt(event, { maxWidth: 300, placement: "above", align: "left" });
};

const moveSeabedTooltip = (event: MouseEvent) => {
  move(event, "above");
};

const hideSeabedTooltip = () => {
  hide();
  isTitleOnlyTooltip.value = false;
};

const showMobiusDesertTooltip = (event: MouseEvent) => {
  isTitleOnlyTooltip.value = true;
  tooltipTitle.value = t("mobiusDesert.title");
  tooltipText.value = "";
  showAt(event, { maxWidth: 300, placement: "above", align: "left" });
};

const moveMobiusDesertTooltip = (event: MouseEvent) => {
  move(event, "above");
};

const hideMobiusDesertTooltip = () => {
  hide();
  isTitleOnlyTooltip.value = false;
};

const showBuyMeACoffeeTooltip = (event: MouseEvent) => {
  isTitleOnlyTooltip.value = true;
  tooltipTitle.value = t("buyMeACoffee.title");
  tooltipText.value = "";
  showAt(event, { maxWidth: 300, placement: "above", align: "left" });
};

const moveBuyMeACoffeeTooltip = (event: MouseEvent) => {
  move(event, "above");
};

const hideBuyMeACoffeeTooltip = () => {
  hide();
  isTitleOnlyTooltip.value = false;
};

const showGroupCharismaTooltip = (event: MouseEvent) => {
  isTitleOnlyTooltip.value = false;
  tooltipTitle.value = t("party.groupCharisma");
  tooltipText.value = t("party.groupCharismaWarning");
  showAt(event, { maxWidth: 300, placement: "above" });
};

const moveGroupCharismaTooltip = (event: MouseEvent) => {
  move(event, "above");
};

const hideGroupCharismaTooltip = () => {
  hide();
};

const showGroupLevelTooltip = (event: MouseEvent) => {
  isTitleOnlyTooltip.value = false;
  tooltipTitle.value = t("party.groupLevel");
  tooltipText.value = t("party.groupLevelExplanation");
  showAt(event, { maxWidth: 300, placement: "above" });
};

const moveGroupLevelTooltip = (event: MouseEvent) => {
  move(event, "above");
};

const hideGroupLevelTooltip = () => {
  hide();
};

const groupCharisma = computed(() => {
  return FooterPresenter.getPartyCharisma(gameState.value.party);
});

const groupPartyLevel = computed(() => {
  return FooterPresenter.getPartyLevel(gameState.value.party);
});
</script>

<template>
  <footer
    class="dw3-frame [--dw3-frame-fill-color:#000a2b] w-full text-white p-2 flex items-center gap-12 px-6"
  >
    <div class="font-bold text-ms flex items-baseline">
      <span class="opacity-80 text-[0.7rem] mr-2 font-normal text-blue-300 tracking-wider uppercase"
        >{{ $t("player.bits") }}:</span
      >
      <span class="text-white">{{ playerBits }}</span>
    </div>

    <div
      class="font-bold text-ms flex items-baseline cursor-help"
      @mouseenter="showGroupCharismaTooltip"
      @mousemove="moveGroupCharismaTooltip"
      @mouseleave="hideGroupCharismaTooltip"
    >
      <span class="opacity-80 text-[0.7rem] mr-2 font-normal text-blue-300 tracking-wider uppercase"
        >{{ $t("party.groupCharisma") }}:</span
      >
      <span class="text-white">{{ groupCharisma }}</span>
    </div>

    <div
      class="font-bold text-ms flex items-baseline cursor-help"
      @mouseenter="showGroupLevelTooltip"
      @mousemove="moveGroupLevelTooltip"
      @mouseleave="hideGroupLevelTooltip"
    >
      <span class="opacity-80 text-[0.7rem] mr-2 font-normal text-blue-300 tracking-wider uppercase"
        >{{ $t("party.groupLevel") }}:</span
      >
      <span class="text-white">{{ groupPartyLevel }}</span>
    </div>

    <div
      class="absolute right-4 top-1/2 -translate-y-1/2 flex items-center gap-4 text-sm opacity-80"
    >
      <div class="flex items-center gap-3 border-r border-blue-900 pr-4 mr-2">
        <SeabedButton
          @show-tooltip="showSeabedTooltip"
          @move-tooltip="moveSeabedTooltip"
          @hide-tooltip="hideSeabedTooltip"
        />
        <MobiusDesertButton
          @show-tooltip="showMobiusDesertTooltip"
          @move-tooltip="moveMobiusDesertTooltip"
          @hide-tooltip="hideMobiusDesertTooltip"
        />
      </div>

      <div class="flex items-center gap-3 border-r border-blue-900 pr-4 mr-2">
        <LanguageSelector />
      </div>

      <div class="flex items-center">
        <BuyMeACoffeeButton
          @show-tooltip="showBuyMeACoffeeTooltip"
          @move-tooltip="moveBuyMeACoffeeTooltip"
          @hide-tooltip="hideBuyMeACoffeeTooltip"
        />
      </div>
    </div>
  </footer>

  <Tooltip
    :show="tooltipShow && isTitleOnlyTooltip"
    :x="tooltipX"
    :y="tooltipY"
    :title="tooltipTitle"
    :max-width="300"
    placement="above"
    horizontal-align="left"
  />

  <DefaultTooltip
    :show="tooltipShow && !isTitleOnlyTooltip"
    :x="tooltipX"
    :y="tooltipY"
    :title="tooltipTitle"
    :text="tooltipText"
    placement="above"
  />
</template>
