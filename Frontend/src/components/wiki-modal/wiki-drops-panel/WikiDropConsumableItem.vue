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
  <div v-if="consumableItem" class="flex flex-col min-h-0 overflow-y-auto custom-scroll text-xs">
    <p>
      <span class="text-blue-500 uppercase">{{ $t("enemy.resaleValueLabel") }}: </span>
      <span class="text-gray-100">{{ consumableItem.resaleValue }}&nbsp;</span>
      <span class="text-blue-500 uppercase">{{ $t("player.bits") }}</span>
    </p>
    <p class="mt-2">
      <span class="text-blue-500 uppercase">{{ $t("enemy.soldInStore") }}: </span>
      <span class="text-gray-100 uppercase">
        {{ consumableItem.soldInStore ? $t("enemy.soldInStoreYes") : $t("enemy.soldInStoreNo") }}
      </span>
    </p>

    <p
      v-if="note !== ''"
      class="mt-4 text-gray-400 italic leading-tight"
    >
      {{ note }}
    </p>
    <p
      v-if="additionalInformation !== ''"
      class="mt-4 text-violet-500 leading-tight"
    >
      {{ additionalInformation }}
    </p>
  </div>
</template>
