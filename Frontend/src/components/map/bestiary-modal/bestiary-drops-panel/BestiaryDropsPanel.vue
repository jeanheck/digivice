<script setup lang="ts">
import { computed, ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

const props = defineProps<{
  enemy: EnemyViewModel;
  initialDropId: string | null;
}>();

const emit = defineEmits<{
  (e: "back"): void;
}>();

const { t } = useI18n();

const dropIds = computed(() => {
  const ids = [...new Set((props.enemy.drops ?? []).map((drop) => drop.id))];
  if (ids.length === 1 && ids[0] === "variousBooster") {
    return [] as string[];
  }

  return ids;
});

const selectedDropId = ref<string | null>(null);

watch(
  [dropIds, () => props.initialDropId],
  ([ids, initialDropId]) => {
    if (initialDropId !== null && ids.includes(initialDropId)) {
      selectedDropId.value = initialDropId;
      return;
    }

    selectedDropId.value = ids[0] ?? null;
  },
  { immediate: true },
);

const dropListItems = computed(() => {
  return dropIds.value.map((dropId) => {
    return {
      id: dropId,
      label: t(`drops.${dropId}`),
    };
  });
});
</script>

<template>
  <div class="p-4 flex flex-col gap-3 h-full min-h-0 overflow-hidden">
    <div class="relative flex shrink-0 items-center min-h-6">
      <button
        type="button"
        class="shrink-0 inline-flex items-center gap-1 text-[10px] 2xl:text-xs font-bold uppercase tracking-widest text-gray-400 hover:text-blue-300 transition-colors cursor-pointer"
        @click="emit('back')"
      >
        <span class="text-[1.2rem] -translate-y-0.5" aria-hidden="true">⬅️</span>
        {{ $t("enemy.back") }}
      </button>

      <div class="absolute inset-0 flex items-center justify-center pointer-events-none">
        <div class="flex items-center justify-center gap-2 pointer-events-auto">
          <button
            v-for="item in dropListItems"
            :key="item.id"
            type="button"
            class="shrink-0 px-2.5 py-1.5 rounded text-[10px] 2xl:text-xs font-bold tracking-wide transition-colors cursor-pointer"
            :class="
              selectedDropId === item.id
                ? 'bg-amber-900/40 text-amber-300 border border-amber-300'
                : 'text-gray-300 hover:bg-blue-900/20 border border-transparent'
            "
            @click="selectedDropId = item.id"
          >
            {{ item.label }}
          </button>
        </div>
      </div>
    </div>

    <section
      class="flex-1 w-full min-h-0 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner"
    >
      <!-- Drop detail placeholder (type-specific components later) -->
    </section>
  </div>
</template>
