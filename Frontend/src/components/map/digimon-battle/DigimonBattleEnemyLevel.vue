<script setup lang="ts">
import { useI18n } from "vue-i18n";

defineProps<{
  level: number | null;
}>();

const { t } = useI18n();
</script>

<template>
  <span class="text-[10px] font-bold text-gray-300 shrink-0 justify-self-center cursor-default">
    {{ t("digimon.lv") }}.{{ level }}
  </span>
</template>
