<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { WikiDropConsumableItemPresenter } from "@/presenters/map/wiki-modal/wiki-drop-consumable-item.presenter";

const props = defineProps<{
  consumableItemId: number;
}>();

const { t, te } = useI18n();

const consumableItem = computed(() => {
  return WikiDropConsumableItemPresenter.getViewModel(props.consumableItemId);
});

const note = computed(() => {
  if (consumableItem.value === null || !te(consumableItem.value.noteKey)) {
    return "";
  }

  const translatedNote = t(consumableItem.value.noteKey);
  if (translatedNote === "" || translatedNote === consumableItem.value.noteKey) {
    return "";
  }

  return translatedNote;
});

const additionalInformation = computed(() => {
  if (consumableItem.value === null || !te(consumableItem.value.additionalInformationKey)) {
    return "";
  }

  const translatedAdditionalInformation = t(consumableItem.value.additionalInformationKey);
  if (
    translatedAdditionalInformation === "" ||
    translatedAdditionalInformation === consumableItem.value.additionalInformationKey
  ) {
    return "";
  }

  return translatedAdditionalInformation;
});
</script>

<template>
  <div v-if="consumableItem" class="flex flex-col gap-2 min-h-0 overflow-y-auto custom-scroll">
    <h5 class="text-sm font-bold text-blue-200 tracking-wide">
      {{ $t(consumableItem.nameKey) }}
    </h5>

    <div class="flex flex-col gap-1 text-xs">
      <div class="flex justify-between gap-4">
        <span class="text-gray-400 uppercase tracking-widest text-[10px]">
          {{ $t("enemy.resaleValue") }}
        </span>
        <span class="text-cyan-300 font-bold">{{ consumableItem.resaleValue }}</span>
      </div>
      <div class="flex justify-between gap-4">
        <span class="text-gray-400 uppercase tracking-widest text-[10px]">
          {{ $t("enemy.soldInStore") }}
        </span>
        <span class="text-gray-200">
          {{ consumableItem.soldInStore ? $t("enemy.soldInStoreYes") : $t("enemy.soldInStoreNo") }}
        </span>
      </div>
    </div>

    <p
      v-if="note !== ''"
      class="text-gray-400 text-[10px] italic leading-tight"
    >
      {{ note }}
    </p>
    <p
      v-if="additionalInformation !== ''"
      class="text-gray-300 text-[10px] leading-tight"
    >
      {{ additionalInformation }}
    </p>
  </div>
</template>
