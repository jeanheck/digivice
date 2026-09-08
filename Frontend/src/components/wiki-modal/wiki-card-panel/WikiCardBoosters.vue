<script setup lang="ts">
import { computed } from "vue";
import WikiCardBooster from "@/components/wiki-modal/wiki-card-panel/WikiCardBooster.vue";
import { WikiCardBoostersPresenter } from "@/presenters/map/wiki-modal/wiki-card-boosters.presenter";

const props = defineProps<{
  cardId: string;
}>();

const emit = defineEmits<{
  (e: "open-drop", dropKey: string): void;
}>();

const boosters = computed(() => {
  return WikiCardBoostersPresenter.getViewModel(props.cardId);
});

const handleSelect = (dropKey: string): void => {
  emit("open-drop", dropKey);
};
</script>

<template>
  <section
    class="shrink-0 w-1/2 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner"
  >
    <h4 class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-3">
      {{ $t("enemy.obtainedFrom") }}
    </h4>

    <p
      v-if="boosters.length === 0"
      class="text-xs text-gray-400 italic"
    >
      {{ $t("enemy.obtainedFromNone") }}
    </p>
    <div
      v-else
      class="flex flex-wrap gap-2"
    >
      <WikiCardBooster
        v-for="booster in boosters"
        :key="booster.dropKey"
        :booster="booster"
        @select="handleSelect(booster.dropKey)"
      />
    </div>
  </section>
</template>
