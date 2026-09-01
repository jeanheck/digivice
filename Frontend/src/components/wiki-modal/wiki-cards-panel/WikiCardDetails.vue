<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { ImageCatalog } from "@/catalogs/image.catalog";
import type { WikiCardDetailsViewModel } from "@/viewmodels/wiki-modal/wiki-card-details.viewmodel";

const props = defineProps<{
  card: WikiCardDetailsViewModel;
}>();

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
    class="flex-1 w-full min-h-0 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-col items-center text-center gap-4 overflow-y-auto custom-scroll"
  >
    <img
      v-if="cardImageUrl"
      :src="cardImageUrl"
      :alt="cardName"
      class="h-40 object-contain rendering-pixelated"
    />

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
