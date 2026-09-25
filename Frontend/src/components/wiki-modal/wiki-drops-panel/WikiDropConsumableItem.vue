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
  <div
    v-if="consumableItem"
    class="flex flex-col min-h-0 overflow-y-auto custom-scroll text-xs text-center gap-4"
  >
    <div class="flex flex-col gap-1">
      <p class="bg-[#002266]/40 px-2 py-1 rounded-sm text-blue-500 uppercase font-bold">
        {{ $t("enemy.resaleValueLabel") }}
      </p>
      <p>
        <span class="text-gray-100 uppercase font-bold">{{ consumableItem.resaleValue }}&nbsp;</span>
        <span class="text-blue-500 uppercase font-bold">{{ $t("player.bits") }}</span>
      </p>
    </div>

    <div class="flex flex-col gap-1">
      <p class="bg-[#002266]/40 px-2 py-1 rounded-sm text-blue-500 uppercase font-bold">
        {{ $t("enemy.soldInStore") }}
      </p>
      <p
        class="uppercase font-bold"
        :class="consumableItem.soldInStore ? 'text-green-400' : 'text-red-400'"
      >
        {{ consumableItem.soldInStore ? $t("enemy.soldInStoreYes") : $t("enemy.soldInStoreNo") }}
      </p>
    </div>

    <div
      v-if="note !== '' || additionalInformation !== ''"
      class="flex flex-col gap-4"
    >
      <p
        v-if="note !== ''"
        class="text-violet-400 font-bold"
      >
        {{ note }}
      </p>
      <p
        v-if="additionalInformation !== ''"
        class="text-gray-400 italic leading-tight"
      >
        {{ additionalInformation }}
      </p>
    </div>
  </div>
</template>
