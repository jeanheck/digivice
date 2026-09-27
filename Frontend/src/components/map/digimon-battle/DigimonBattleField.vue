<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { FieldPresenter } from "@/presenters/map/field.presenter";

const props = defineProps<{
  battleFieldId: number;
}>();

const { t } = useI18n();

const fieldViewModel = computed(() => {
  return FieldPresenter.getFieldViewModel(props.battleFieldId);
});
</script>

<template>
  <div class="absolute bottom-2 left-2 z-5 flex flex-col gap-0.5 pointer-events-none">
    <span
      class="text-[10px] 2xl:text-sm font-bold tracking-wide text-white text-outline-black-glow leading-tight"
    >
      {{ t(`field.${fieldViewModel.name}`) }}
    </span>
    <span
      v-if="fieldViewModel.strengthen"
      class="text-[10px] 2xl:text-sm font-bold tracking-wide text-green-400 text-outline-black-glow leading-tight"
    >
      {{ t("field.strengthen", { element: t(`stat.${fieldViewModel.strengthen}`) }) }}
    </span>
    <span
      v-if="fieldViewModel.weaken"
      class="text-[10px] 2xl:text-sm font-bold tracking-wide text-red-400 text-outline-black-glow leading-tight"
    >
      {{ t("field.weaken", { element: t(`stat.${fieldViewModel.weaken}`) }) }}
    </span>
  </div>
</template>
