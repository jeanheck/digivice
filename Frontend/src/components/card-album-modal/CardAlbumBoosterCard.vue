<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { CardAlbumBoosterPresenter } from "@/presenters/card-album/card-album-booster.presenter";
import type { CardAlbumBoosterItemViewModel } from "@/viewmodels/card-album/card-album.viewmodel";

const props = defineProps<{
  booster: CardAlbumBoosterItemViewModel;
}>();

const emit = defineEmits<{
  (e: "select", boosterId: number): void;
}>();

const { t } = useI18n();

const theme = computed(() => {
  return CardAlbumBoosterPresenter.getBoosterCardTheme(props.booster.series);
});

const seriesLabel = computed(() => {
  if (props.booster.series === "R") {
    return t("cardAlbum.seriesR");
  }
  if (props.booster.series === "A") {
    return t("cardAlbum.seriesA");
  }
  return t("cardAlbum.seriesB");
});

const handleClick = () => {
  emit("select", props.booster.id);
};
</script>

<template>
  <button
    type="button"
    class="group relative flex flex-col justify-between p-3 rounded-xl border text-left transition-all duration-300 cursor-pointer select-none min-h-[245px] h-full"
    :class="[
      theme.wrapperClass,
      booster.isComplete ? 'ring-2 ring-emerald-400/80 shadow-[0_0_15px_rgba(52,211,153,0.3)]' : ''
    ]"
    @click="handleClick"
  >
    <!-- Card Header: Code (#01a/#1b/#R-01), Series Badge, and Completion Status Badge -->
    <div class="flex items-center justify-between w-full shrink-0 gap-1">
      <!-- Booster Code tag -->
      <span
        class="text-xs font-bold font-mono tracking-tight shrink-0"
        :class="theme.codeClass"
      >
        #{{ booster.code }}
      </span>

      <!-- Series Badge -->
      <span
        class="text-[9px] font-mono font-black uppercase px-2 py-0.5 rounded border tracking-wider shadow-xs shrink-0"
        :class="theme.badgeClass"
      >
        {{ seriesLabel }}
      </span>

      <!-- Completion Status Badge -->
      <span
        class="px-2 py-0.5 rounded text-[10px] font-mono font-bold border shrink-0 transition-colors"
        :class="
          booster.isComplete
            ? 'bg-emerald-950/90 text-emerald-300 border-emerald-500/80 shadow-xs'
            : 'bg-black/60 text-gray-300 border-gray-700/80'
        "
      >
        {{ booster.ownedCards }}/{{ booster.totalCards }}
      </span>
    </div>

    <!-- Mini Trading Card Artwork (Proportional 40:46 collectible card format, NOT a package/encomenda) -->
    <div class="relative my-2 flex items-center justify-center h-28 w-full shrink-0">
      <div
        class="h-28 w-auto aspect-[40/46] rounded-md border-2 flex flex-col items-center justify-between p-1.5 transition-all duration-300 transform group-hover:scale-105 group-hover:shadow-lg select-none relative overflow-hidden"
        :class="theme.innerCardClass"
      >
        <!-- Card Frame Top Accent -->
        <div class="flex items-center justify-between w-full text-[8px] font-mono font-bold tracking-widest opacity-80 shrink-0">
          <span>{{ booster.code }}</span>
          <span>{{ booster.series === 'R' ? '★★★' : booster.series === 'A' ? '★' : '★★' }}</span>
        </div>

        <!-- Central Metallic Card Crest -->
        <div class="relative flex flex-col items-center justify-center my-auto">
          <div
            class="w-11 h-11 rounded-full border-2 flex items-center justify-center shadow-md relative"
            :class="theme.crestClass"
          >
            <span class="text-xl font-black font-mono tracking-tighter drop-shadow">
              {{ booster.series }}
            </span>
          </div>
          <span class="text-[9px] font-mono font-black uppercase tracking-widest mt-1 opacity-90 drop-shadow-xs">
            {{ booster.series === 'R' ? 'RARE' : `SÉRIE ${booster.series}` }}
          </span>
        </div>

        <!-- Card Frame Bottom Accent -->
        <div class="w-full text-center text-[7px] font-mono uppercase tracking-widest opacity-70 shrink-0 border-t border-current/20 pt-0.5">
          DIGIMON TCG
        </div>
      </div>
    </div>

    <!-- Booster Full Name (never truncated, completely readable) -->
    <h3 class="w-full text-xs font-bold text-center leading-snug break-words px-1 text-white group-hover:text-cyan-200 transition-colors my-1">
      {{ $t(booster.nameKey) }}
    </h3>

    <!-- Card Bottom: Progress Bar & Fully Visible Progress Text -->
    <div class="w-full flex flex-col gap-1.5 shrink-0 pt-2 border-t border-white/10 mt-auto">
      <!-- Full Progress Text (Always 100% visible, whether complete or incomplete) -->
      <div class="flex items-center justify-between w-full text-xs font-mono leading-tight">
        <div class="flex items-center gap-1 font-bold">
          <span :class="booster.isComplete ? 'text-emerald-300' : 'text-gray-200'">
            {{ booster.ownedCards }} / {{ booster.totalCards }}
          </span>
          <span
            v-if="booster.isComplete"
            class="text-emerald-400 font-extrabold text-[11px]"
          >
            ({{ $t('cardAlbum.complete').toLowerCase() }})
          </span>
        </div>
        <span
          class="font-extrabold font-mono shrink-0 pl-1 text-xs"
          :class="booster.isComplete ? 'text-emerald-400' : theme.percentageClass"
        >
          {{ booster.percentage }}%
        </span>
      </div>

      <!-- Progress Bar -->
      <div class="w-full h-2 bg-black/70 border border-white/20 rounded-full overflow-hidden p-0.5 shadow-inner">
        <div
          class="h-full rounded-full transition-all duration-300"
          :class="booster.isComplete ? 'bg-linear-to-r from-emerald-500 via-teal-400 to-emerald-300' : theme.progressClass"
          :style="{ width: `${booster.percentage}%` }"
        />
      </div>
    </div>
  </button>
</template>
