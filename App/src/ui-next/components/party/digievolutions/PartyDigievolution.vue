<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import ProgressBar from "@/ui-next/components/party/progress-bar/ProgressBar.vue";
import { DigievolutionPresenter } from "@/presentation/presenters/digievolution/digievolution.presenter";
import type { DigievolutionTier } from "@/shared/types/digievolution-tier.type";
import type { PartyDigievolutionSlotMock } from "@/ui-next/mocks/party.mock";

type TierIndicator = "stars" | "stripe" | "none";

const { digievolution, isActive, tierIndicator } = defineProps<{
  digievolution: PartyDigievolutionSlotMock;
  isActive: boolean;
  tierIndicator: TierIndicator;
}>();

const { t } = useI18n();

const IsDnaIconVisible = false;

const TierStripeClasses = {
  1: "bg-gray-400",
  2: "bg-green-500",
  3: "bg-blue-500",
  4: "bg-purple-500",
  5: "bg-amber-400",
} as const satisfies Record<DigievolutionTier, string>;

const name = computed(() => {
  if (digievolution === null) {
    return "";
  }

  return DigievolutionPresenter.getNameById(digievolution.digievolutionId);
});

const tier = computed(() => {
  if (digievolution === null) {
    return null;
  }

  return DigievolutionPresenter.getTierById(digievolution.digievolutionId);
});
</script>

<template>
  <div class="relative h-14 text-white dw3-beveled">
    <div class="absolute inset-0 bg-progress-frame dw3-beveled pointer-events-none"></div>
    <div class="absolute inset-[1.5px] bg-surface dw3-beveled pointer-events-none">
      <div
        v-if="digievolution?.isDnaActive"
        class="absolute inset-y-0 left-0 w-1 bg-red-500"
      ></div>
      <div
        v-else-if="tier !== null && tierIndicator === 'stripe'"
        class="absolute inset-y-0 left-0 w-1"
        :class="TierStripeClasses[tier]"
      ></div>
    </div>

    <div class="relative z-10 h-full px-2 py-0.5 flex flex-col justify-center gap-[5px]">
      <template v-if="digievolution">
        <div
          class="px-0.5 flex items-center gap-1.5 text-[12px] leading-none"
          :class="isActive ? 'text-dw3-gold shadow-text-dark' : 'shadow-text'"
        >
          <span class="min-w-0 truncate">{{ name }}</span>
          <span
            v-if="tier !== null && tierIndicator === 'stars'"
            class="shrink-0 flex gap-0.5 text-[10px] font-emoji"
          >
            <span v-for="star in tier" :key="star" class="text-yellow-300">
              &#x2B50;
            </span>
          </span>
          <span
            v-if="IsDnaIconVisible && digievolution.isDnaActive"
            class="shrink-0 text-[14px] font-emoji text-red-400"
          >
            &#x1F534;
          </span>
          <span class="ml-auto shrink-0">{{ digievolution.level }}</span>
        </div>

        <ProgressBar
          :current-value="digievolution.dvexpInLevel"
          :max-value="digievolution.dvexpPerLevel"
          :progress-percentage="(digievolution.dvexpInLevel / digievolution.dvexpPerLevel) * 100"
          bar-color-class="bg-blue-400"
          transition-duration-class="duration-500"
          height-class="h-5"
          :label="t('digimon.dvexp')"
        />
      </template>

      <span v-else class="text-center text-[12px] leading-none text-white/60">
        {{ t("digimon.states.empty") }}
      </span>
    </div>
  </div>
</template>
