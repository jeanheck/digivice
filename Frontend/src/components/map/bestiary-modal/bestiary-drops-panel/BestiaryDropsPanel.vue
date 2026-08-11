<script setup lang="ts">
import { computed, ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import type { DropRaw } from "@/repositories/tables/raws/enemy/drop.raw";
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
  const drop = props.enemy.drop;
  if (drop === undefined || drop === "variousBooster") {
    return [] as string[];
  }

  if (typeof drop === "string") {
    return [drop];
  }

  const flattenedDropIds = drop.flatMap((sectorDrop: DropRaw) => {
    return sectorDrop.sectorDrops;
  });

  return [...new Set(flattenedDropIds)];
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
    <div class="flex gap-4 shrink-0 items-center min-h-6">
      <div class="w-[20%] shrink-0 flex items-center">
        <button
          type="button"
          class="inline-flex items-center gap-1 text-[10px] 2xl:text-xs font-bold uppercase tracking-widest text-blue-400 hover:text-blue-300 transition-colors cursor-pointer"
          @click="emit('back')"
        >
          <span class="text-[1.2rem] -translate-y-0.5" aria-hidden="true">⬅️</span>
          {{ $t("enemy.back") }}
        </button>
      </div>

      <div class="flex-1 min-w-0 flex items-center justify-center">
        <h3 class="text-[15px] uppercase font-bold tracking-widest text-blue-500">
          {{ $t("enemy.dropDetails") }}
        </h3>
      </div>
    </div>

    <div class="flex gap-4 flex-1 min-h-0 overflow-hidden">
      <aside
        class="w-[20%] shrink-0 flex flex-col min-h-0 bg-[#000a1a] border border-blue-900/50 rounded p-3 shadow-inner"
      >
        <div class="flex-1 min-h-0 overflow-y-auto custom-scroll flex flex-col gap-1">
          <button
            v-for="item in dropListItems"
            :key="item.id"
            type="button"
            class="w-full text-left px-2 py-1.5 rounded text-[10px] 2xl:text-xs font-bold tracking-wide transition-colors cursor-pointer"
            :class="
              selectedDropId === item.id
                ? 'bg-blue-900/40 text-blue-200 border border-blue-500/40'
                : 'text-gray-300 hover:bg-blue-900/20 border border-transparent'
            "
            @click="selectedDropId = item.id"
          >
            {{ item.label }}
          </button>
        </div>
      </aside>

      <section
        class="flex-1 min-w-0 min-h-0 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner"
      >
        <!-- Drop detail placeholder (type-specific components later) -->
      </section>
    </div>
  </div>
</template>
