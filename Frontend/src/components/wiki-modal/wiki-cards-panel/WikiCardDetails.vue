<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";

const props = defineProps<{
  noteKey: string;
}>();

const { t, te } = useI18n();

const cardNote = computed(() => {
  if (!te(props.noteKey)) {
    return "";
  }

  return t(props.noteKey);
});
</script>

<template>
  <section
    class="flex-1 w-full min-h-0 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-col"
  >
    <h4 class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-2">
      {{ $t("enemy.cardDetails") }}
    </h4>
    <p
      v-if="cardNote !== ''"
      class="text-xs text-gray-400 italic"
    >
      {{ cardNote }}
    </p>
  </section>
</template>
