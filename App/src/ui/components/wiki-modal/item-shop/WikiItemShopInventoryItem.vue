<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { WikiItemShopItemViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-item-shop-item.viewmodel";

const props = defineProps<{
  item: WikiItemShopItemViewModel;
}>();

const emit = defineEmits<{
  (e: "select", consumableItemId: string): void;
}>();

const { t, te } = useI18n();

const translateOptional = (key: string): string => {
  if (!te(key)) {
    return "";
  }

  const translated = t(key);
  if (translated === key) {
    return "";
  }

  return translated;
};

const note = computed(() => {
  return translateOptional(props.item.noteKey);
});

const additionalInformation = computed(() => {
  return translateOptional(props.item.additionalInformationKey);
});

const handleSelect = (): void => {
  emit("select", props.item.consumableItemId);
};
</script>

<template>
  <button
    type="button"
    class="flex items-start gap-3 w-full px-3 py-2 rounded text-left transition-colors cursor-pointer bg-accent/15 hover:bg-accent/25 border border-frame/50"
    @click="handleSelect"
  >
    <img
      v-if="item.imageUrl"
      :src="item.imageUrl"
      :alt="$t(item.nameKey)"
      class="w-[25px] h-[27px] shrink-0 object-contain rendering-pixelated"
    />
    <span class="flex flex-col gap-1 flex-1 min-w-0">
      <span class="flex items-start justify-between gap-2">
        <span class="text-xs font-bold text-ink tracking-wide truncate">
          {{ $t(item.nameKey) }}
        </span>
        <span class="flex items-baseline gap-1 shrink-0 text-[10px] font-bold whitespace-nowrap">
          <span class="text-amber-400 in-[[data-theme=seabed]]:text-amber-700">{{ item.price }}</span>
          <span class="text-ink">{{ $t("enemy.bits") }}</span>
        </span>
      </span>
      <span v-if="note !== ''" class="text-[8px] text-ink-muted leading-tight line-clamp-2">
        {{ note }}
      </span>
      <span
        v-if="additionalInformation !== ''"
        class="text-[8px] text-ink/70 italic leading-tight"
      >
        {{ additionalInformation }}
      </span>
    </span>
  </button>
</template>
