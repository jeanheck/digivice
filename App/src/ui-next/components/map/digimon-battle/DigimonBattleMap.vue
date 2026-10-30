<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import MapFrame from "@/ui-next/components/map/MapFrame.vue";
import HpProgressBar from "@/ui-next/components/party/progress-bar/HpProgressBar.vue";
import { ImageCatalog } from "@/ui/catalogs/image.catalog";
import { DigimonBattlePresenter } from "@/presentation/presenters/map/digimon-battle.presenter";
import { FieldPresenter } from "@/presentation/presenters/map/field.presenter";
import type { DigimonStatus } from "@/shared/types/digimon-status.type";
import { DigimonBattleMapMock } from "@/ui-next/mocks/map.mock";

const StatusColors = {
  healthy: "#00B6BF",
  injured: "#A3D956",
  debuffed: "#CB9200",
  ko: "#760F08",
} as const satisfies Record<DigimonStatus, string>;

const { t } = useI18n();

const enemy = DigimonBattleMapMock.enemy;

const digimonBattleViewModel = computed(() => {
  return DigimonBattlePresenter.getViewModel(enemy);
});

const fieldImageUrl = computed(() => {
  return ImageCatalog.getFieldImageUrl(DigimonBattleMapMock.fieldId);
});

const juniorImageUrl = ImageCatalog.getJuniorImageUrl();

const fieldViewModel = computed(() => {
  return FieldPresenter.getFieldViewModel(DigimonBattleMapMock.fieldId);
});

const status = computed(() => {
  return DigimonBattlePresenter.getStatus(enemy.condition, digimonBattleViewModel.value.hp);
});

const statusLabel = computed(() => {
  if (status.value !== "debuffed") {
    return t(`digimon.status.${status.value}`);
  }

  return DigimonBattlePresenter.getActiveDebuffs(enemy.condition)
    .map((debuff) => t(`digimon.debuff.${debuff}.affected`))
    .join(", ");
});

const speciesLabel = computed(() => {
  const species = digimonBattleViewModel.value.species;
  return species !== null ? t(`species.${species}`) : "";
});

const affinities = computed(() => {
  return digimonBattleViewModel.value.affinities;
});
</script>

<template>
  <MapFrame>
    <template #background>
      <div
        v-if="fieldImageUrl"
        class="absolute inset-0 bg-cover bg-center"
        :style="{ backgroundImage: `url(${fieldImageUrl})` }"
      ></div>
    </template>

    <div class="flex-1 min-h-0 flex flex-col">
      <header
        class="px-2 py-1.5 grid grid-cols-[1fr_auto_auto_auto] items-center gap-x-2 bg-black/80"
      >
        <HpProgressBar :hp="digimonBattleViewModel.hp" />
        <div
          class="w-6 h-6 rounded border-2 border-progress-frame cursor-help"
          :style="{ backgroundColor: StatusColors[status] }"
          :title="statusLabel"
        ></div>
        <span class="text-[8px] font-bold text-dw3-gold shadow-text-dark">
          {{ t("digimon.lv") }}.{{ digimonBattleViewModel.level }}
        </span>
        <span
          class="text-[16px] leading-none font-emoji text-foreground cursor-help"
          :title="speciesLabel"
        >
          {{ digimonBattleViewModel.speciesEmoji }}
        </span>
      </header>

      <div class="relative flex-1 min-h-0 overflow-hidden">
        <img
          v-if="digimonBattleViewModel.enemyImageUrl"
          :src="digimonBattleViewModel.enemyImageUrl"
          :alt="digimonBattleViewModel.enemyName"
          class="absolute left-1/2 top-1/2 -translate-x-1/2 -translate-y-1/2 z-1 w-[70%] max-h-[90%] object-contain drop-shadow-[0_6px_14px_rgba(0,0,0,1)] pointer-events-none"
        />

        <div
          v-if="affinities.effectiveIcons.length > 0 || affinities.notEffectiveIcons.length > 0"
          class="absolute bottom-2 right-2 z-5 flex flex-col items-end gap-1.5 pointer-events-none"
        >
          <div v-if="affinities.effectiveIcons.length > 0" class="flex items-center gap-1">
            <span class="text-[14px] leading-none">{{ affinities.effectiveIcons.join("") }}</span>
            <span class="text-[8px] font-bold text-green-400 text-outline-black-glow">
              {{ t("digimonBattle.effective") }}
            </span>
          </div>
          <div v-if="affinities.notEffectiveIcons.length > 0" class="flex items-center gap-1">
            <span class="text-[14px] leading-none">{{ affinities.notEffectiveIcons.join("") }}</span>
            <span class="text-[8px] font-bold text-red-400 text-outline-black-glow">
              {{ t("digimonBattle.notEffective") }}
            </span>
          </div>
        </div>

        <div
          v-if="juniorImageUrl"
          class="absolute -bottom-12 -right-6 z-2 w-full h-[110%] overflow-hidden pointer-events-none"
        >
          <img
            :src="juniorImageUrl"
            alt=""
            class="absolute top-0 right-0 w-full max-w-none h-[260%] object-contain object-top drop-shadow-[0_6px_14px_rgba(0,0,0,1)]"
          />
        </div>

        <div
          class="absolute bottom-2 left-2 z-5 flex flex-col gap-1 text-[8px] font-bold leading-tight text-outline-black-glow pointer-events-none"
        >
          <span class="text-foreground">{{ t(`field.${fieldViewModel.name}`) }}</span>
          <span v-if="fieldViewModel.strengthen" class="text-green-400">
            {{ t("field.strengthen", { element: t(`stat.${fieldViewModel.strengthen}`) }) }}
          </span>
          <span v-if="fieldViewModel.weaken" class="text-red-400">
            {{ t("field.weaken", { element: t(`stat.${fieldViewModel.weaken}`) }) }}
          </span>
        </div>
      </div>
    </div>
  </MapFrame>
</template>
