<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";

const props = defineProps<{
  effectiveIcons: string[];
  notEffectiveIcons: string[];
}>();

const { t } = useI18n();

const hasAffinities = computed(() => {
  return props.effectiveIcons.length > 0 || props.notEffectiveIcons.length > 0;
});
</script>

<template>
  <div
    v-if="hasAffinities"
    class="absolute top-2 right-2 z-5 flex flex-col items-end gap-1.25 pointer-events-none"
  >
    <div v-if="effectiveIcons.length > 0" class="flex items-center gap-1 leading-tight">
      <span class="leading-none inline-flex -translate-y-0.5 text-[13px] 2xl:text-base drop-shadow-[0_1px_2px_rgba(0,0,0,1)]">
        {{ effectiveIcons.join("") }}
      </span>
      <span
        class="text-[9px] 2xl:text-sm font-bold tracking-wide text-green-400 text-outline-black-glow"
      >
        {{ t("digimonBattle.effective") }}
      </span>
    </div>
    <div v-if="notEffectiveIcons.length > 0" class="flex items-center gap-1 leading-tight">
      <span class="leading-none inline-flex -translate-y-0.5 text-[13px] 2xl:text-base drop-shadow-[0_1px_2px_rgba(0,0,0,1)]">
        {{ notEffectiveIcons.join("") }}
      </span>
      <span
        class="text-[9px] 2xl:text-sm font-bold tracking-wide text-red-400 text-outline-black-glow"
      >
        {{ t("digimonBattle.notEffective") }}
      </span>
    </div>
  </div>
</template>
