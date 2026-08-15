<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { WikiModalPresenter } from "@/presenters/map/wiki-modal.presenter";

const props = defineProps<{
  cardId: string;
}>();

const emit = defineEmits<{
  (e: "open-drop", dropKey: string): void;
}>();

const { t, te } = useI18n();

const cardNote = computed(() => {
  const noteKey = `cards.${props.cardId}.note`;
  if (!te(noteKey)) {
    return "";
  }

  return t(noteKey);
});

const boosterSources = computed(() => {
  return WikiModalPresenter.getCardBoosterSources(props.cardId);
});

const boosterLabel = (boosterId: number): string => {
  return t(`boosters.${boosterId}.name`);
};

const handleOpenDrop = (dropKey: string): void => {
  emit("open-drop", dropKey);
};
</script>

<template>
  <div class="p-4 flex flex-col gap-4 h-full min-h-0 overflow-hidden">
    <section
      class="flex-1 w-full min-h-0 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-col"
    >
      <h4 class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-2">
        {{ t("enemy.cardDetails") }}
      </h4>
      <p
        v-if="cardNote !== ''"
        class="text-xs text-gray-400 italic"
      >
        {{ cardNote }}
      </p>
    </section>

    <section
      class="shrink-0 w-full bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner"
    >
      <h4 class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-3">
        {{ t("enemy.obtainedFrom") }}
      </h4>

      <p
        v-if="boosterSources.length === 0"
        class="text-xs text-gray-400 italic"
      >
        {{ t("enemy.obtainedFromNone") }}
      </p>
      <div
        v-else
        class="flex flex-wrap gap-2"
      >
        <button
          v-for="source in boosterSources"
          :key="source.dropKey"
          type="button"
          class="flex items-center gap-2 px-2.5 py-2 rounded text-left transition-colors cursor-pointer bg-blue-900/30 hover:bg-blue-900/50 border border-blue-800/60"
          @click="handleOpenDrop(source.dropKey)"
        >
          <span class="min-w-0">
            <span class="block text-xs font-bold text-blue-200 tracking-wide">
              {{ boosterLabel(source.boosterId) }}
            </span>
          </span>
        </button>
      </div>
    </section>
  </div>
</template>
