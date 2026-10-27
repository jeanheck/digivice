<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { ImageCatalog } from "@/catalogs/image.catalog";
import type { CardAlbumItemViewModel } from "@/viewmodels/card-album/card-album.viewmodel";

const props = defineProps<{
  card: CardAlbumItemViewModel | null;
  slotIndex?: number;
}>();

const emit = defineEmits<{
  (e: "select", cardId: string): void;
}>();

const { t, te } = useI18n();

const cardName = computed(() => {
  if (!props.card) {
    return "";
  }
  return t(props.card.nameKey);
});

const cardImageUrl = computed(() => {
  if (!props.card) {
    return null;
  }
  return ImageCatalog.getCardImageUrl(props.card.imageName);
});

const formattedIndex = computed(() => {
  if (!props.card) {
    return props.slotIndex ? `#${String(props.slotIndex).padStart(3, "0")}` : "";
  }
  return `#${String(props.card.cardIndex).padStart(3, "0")}`;
});

const cardTypeLabel = computed(() => {
  if (!props.card) {
    return "";
  }
  if (props.card.type === "action") {
    return te("cardType.action") ? t("cardType.action") : "Ação";
  }
  const key = `cardType.${props.card.type}`;
  return te(key) ? t(key) : props.card.type;
});

const typeColorBorderClass = computed(() => {
  if (!props.card) {
    return "border-blue-900/40";
  }

  switch (props.card.type) {
    case "action":
      return "border-purple-600/40 text-purple-300";
    case "white":
      return "border-slate-300/40 text-slate-200";
    case "blue":
      return "border-cyan-500/40 text-cyan-300";
    case "green":
      return "border-emerald-500/40 text-emerald-300";
    case "red":
      return "border-rose-500/40 text-rose-300";
    case "black":
      return "border-indigo-600/40 text-indigo-300";
    case "brown":
      return "border-amber-600/40 text-amber-300";
    default:
      return "border-blue-800/40 text-blue-300";
  }
});

const typeBadgeClass = computed(() => {
  if (!props.card) {
    return "bg-blue-950/60 border-blue-800/50 text-blue-300";
  }

  switch (props.card.type) {
    case "action":
      return "bg-purple-950/80 border-purple-500/70 text-purple-200";
    case "white":
      return "bg-slate-900/80 border-slate-300/60 text-slate-200";
    case "blue":
      return "bg-blue-950/80 border-cyan-500/70 text-cyan-200";
    case "green":
      return "bg-emerald-950/80 border-emerald-500/70 text-emerald-200";
    case "red":
      return "bg-rose-950/80 border-rose-500/70 text-rose-200";
    case "black":
      return "bg-indigo-950/80 border-indigo-500/70 text-indigo-200";
    case "brown":
      return "bg-amber-950/80 border-amber-600/70 text-amber-200";
    default:
      return "bg-blue-950/80 border-blue-700/60 text-blue-200";
  }
});

const handleClick = () => {
  if (props.card) {
    emit("select", props.card.cardId);
  }
};
</script>

<template>
  <!-- Empty Binder Slot -->
  <div
    v-if="!card"
    class="flex flex-col items-center justify-center p-3 rounded-lg border border-dashed border-blue-900/40 bg-[#000d24]/30 text-blue-800/40 select-none min-h-[195px] h-full"
  >
    <span class="text-xs font-bold text-blue-800/60 mb-1">
      {{ formattedIndex }}
    </span>
    <span class="text-xs text-blue-900/50 font-medium">
      {{ $t("cardAlbum.emptySlot") }}
    </span>
  </div>

  <!-- Interactive Card Slot -->
  <button
    v-else
    type="button"
    class="group relative flex flex-col items-center justify-between p-2.5 rounded-lg border transition-all duration-200 cursor-pointer select-none overflow-hidden text-left h-full min-h-[195px]"
    :class="[
      card.isOwned
        ? 'bg-[#001026]/90 border-blue-800/50 hover:bg-[#001636] hover:border-cyan-400 hover:shadow-[0_0_15px_rgba(0,180,255,0.35)]'
        : 'bg-[#000a18]/70 border-blue-950/60 hover:bg-[#00122e] hover:border-blue-400/70 hover:shadow-[0_0_12px_rgba(0,140,255,0.25)]',
      typeColorBorderClass,
    ]"
    @click="handleClick"
  >
    <!-- Card Header: Index, Type Badge, and Quantity Badge -->
    <div class="flex items-center justify-between w-full shrink-0 gap-1">
      <!-- Card Number (same font size as CardAlbumModal text) -->
      <span class="text-xs font-bold text-cyan-400 tracking-tight shrink-0">
        {{ formattedIndex }}
      </span>

      <!-- Card Type Tag (placed at the top) -->
      <span
        class="text-[9px] uppercase font-bold px-1.5 py-0.5 rounded border leading-none shrink-0"
        :class="typeBadgeClass"
      >
        {{ cardTypeLabel }}
      </span>

      <!-- Owned / Unowned Badge -->
      <span
        v-if="card.isOwned"
        class="px-1.5 py-0.5 rounded text-[10px] font-black bg-emerald-950/90 text-emerald-400 border border-emerald-500/70 shadow-xs shrink-0"
      >
        x{{ card.quantityOwned }}
      </span>
      <span
        v-else
        class="px-1.5 py-0.5 rounded text-[10px] font-semibold bg-gray-900/80 text-gray-500 border border-gray-800 group-hover:text-gray-300 group-hover:border-gray-600 transition-colors shrink-0"
      >
        x0
      </span>
    </div>

    <!-- Card Artwork Container (proportional 40:46) -->
    <div class="relative my-1 flex items-center justify-center h-28 w-full overflow-hidden">
      <img
        v-if="cardImageUrl"
        :src="cardImageUrl"
        :alt="cardName"
        class="h-28 w-auto aspect-[40/46] max-w-full object-contain rendering-pixelated transition-all duration-300 transform group-hover:scale-105 shrink-0"
        :class="
          card.isOwned
            ? 'filter-none'
            : 'grayscale contrast-90 brightness-75 opacity-65 group-hover:grayscale-0 group-hover:contrast-100 group-hover:brightness-100 group-hover:opacity-100'
        "
      />
      <div
        v-else
        class="w-full h-full flex items-center justify-center text-xl opacity-20 select-none"
      >
        🎴
      </div>
    </div>

    <!-- Card Footer: Full Name & Points (Name is completely visible without truncation) -->
    <div class="w-full flex flex-col items-center gap-0.5 shrink-0 text-center">
      <span
        class="w-full text-xs font-bold text-blue-100 leading-snug text-center break-words group-hover:text-cyan-200 transition-colors"
        :title="cardName"
      >
        {{ cardName }}
      </span>

      <div v-if="card.points" class="text-[9px] font-mono text-gray-400">
        AP <span class="font-bold text-amber-300">{{ card.points.ap }}</span>
        / HP <span class="font-bold text-emerald-300">{{ card.points.hp }}</span>
      </div>
    </div>
  </button>
</template>
