<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { ImageCatalog } from "@/catalogs/image.catalog";
import type { CardViewModel } from "@/viewmodels/card/card.viewmodel";

const props = withDefaults(
  defineProps<{
    card: CardViewModel;
    quantityOwned?: number;
  }>(),
  {
    quantityOwned: 0,
  },
);

const { t, te } = useI18n();

const isActionCard = computed(() => {
  return props.card.type === "action";
});

const cardName = computed(() => {
  return t(props.card.nameKey);
});

const cardImageUrl = computed(() => {
  return ImageCatalog.getCardImageUrl(props.card.imageName);
});

const cardNote = computed(() => {
  if (!te(props.card.noteKey)) {
    return "";
  }

  const translatedNote = t(props.card.noteKey);
  if (translatedNote === "" || translatedNote === props.card.noteKey) {
    return "";
  }

  return translatedNote;
});
</script>

<template>
  <section
    class="flex-1 w-full min-h-0 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-col items-center text-center gap-3 overflow-y-auto custom-scroll"
  >
    <div class="relative">
      <img
        v-if="cardImageUrl"
        :src="cardImageUrl"
        :alt="cardName"
        class="h-40 object-contain rendering-pixelated"
      />
    </div>

    <div class="flex flex-col items-center gap-1">
      <h3 class="text-sm font-bold text-blue-200">
        {{ cardName }}
      </h3>
      <div
        v-if="quantityOwned > 0"
        class="inline-flex items-center gap-1 px-2.5 py-0.5 rounded text-xs font-extrabold bg-emerald-950/60 text-emerald-400 border border-emerald-500/50"
      >
        <span>x{{ quantityOwned }}</span>
      </div>
      <div
        v-else
        class="inline-flex items-center gap-1 px-2.5 py-0.5 rounded text-xs font-medium bg-gray-900/60 text-gray-500 border border-gray-800"
      >
        <span>x0</span>
      </div>
    </div>

    <template v-if="isActionCard">
      <p
        v-if="cardNote !== ''"
        class="text-xs text-gray-400 italic"
      >
        {{ cardNote }}
      </p>
    </template>
    <template v-else>
      <p
        v-if="card.points"
        class="text-gray-100 uppercase font-bold"
      >
        AP {{ card.points.ap }} / HP {{ card.points.hp }}
      </p>
      <p class="text-gray-100 uppercase font-bold">
        {{ $t(`cardType.${card.type}`) }}
      </p>
    </template>
  </section>
</template>
