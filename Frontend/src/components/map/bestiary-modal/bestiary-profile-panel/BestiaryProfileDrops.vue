<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

const props = defineProps<{
  enemy: EnemyViewModel;
}>();

const emit = defineEmits<{
  (e: "open-drops", dropId: string): void;
}>();

const { t } = useI18n();

const resolveDropIds = (enemy: EnemyViewModel): string[] => {
  if (enemy.dropsByLocation !== undefined) {
    const flattenedDropIds = Object.values(enemy.dropsByLocation).flat();
    return [...new Set(flattenedDropIds)];
  }

  return enemy.drops ?? [];
};

const dropIds = computed(() => {
  return resolveDropIds(props.enemy);
});

const isDropInteractive = computed(() => {
  const ids = dropIds.value;
  if (ids.length === 0) {
    return false;
  }

  if (ids.length === 1 && ids[0] === "variousBooster") {
    return false;
  }

  return true;
});

const dropItems = computed(() => {
  const ids = dropIds.value;
  if (ids.length === 0) {
    return [{ id: "none", label: t("drops.none") }];
  }

  const guaranteedSuffix = props.enemy.boss ? ` ${t("enemy.guaranteedDropSuffix")}` : "";

  return ids.map((dropId) => {
    return {
      id: dropId,
      label: `${t(`drops.${dropId}`)}${guaranteedSuffix}`,
    };
  });
});

const handleDropClick = (dropId: string): void => {
  if (!isDropInteractive.value) {
    return;
  }

  emit("open-drops", dropId);
};
</script>

<template>
  <div
    class="relative flex-1 min-h-0 overflow-hidden bg-[#000a1a] border border-blue-900/50 rounded shadow-inner"
  >
    <h4
      class="absolute top-4 left-0 right-0 z-10 text-[10px] uppercase font-bold tracking-widest text-blue-500 text-center pointer-events-none"
    >
      {{ $t("enemy.drops") }}
    </h4>

    <div class="absolute inset-0 flex items-center justify-center px-4">
      <div class="flex flex-col items-center gap-1">
        <template v-for="item in dropItems" :key="item.id">
          <button
            v-if="isDropInteractive"
            type="button"
            class="font-bold text-gray-300 text-xs 2xl:text-sm text-center cursor-pointer transition-colors hover:text-amber-300"
            @click="handleDropClick(item.id)"
          >
            {{ item.label }}
          </button>
          <span
            v-else
            class="font-bold text-gray-300 text-xs 2xl:text-sm text-center"
          >
            {{ item.label }}
          </span>
        </template>
      </div>
    </div>

    <p
      v-if="isDropInteractive"
      class="absolute bottom-4 left-0 right-0 flex w-full justify-center text-[9px] text-gray-500 pointer-events-none"
    >
      {{ $t("enemy.viewDropDetails") }}
    </p>
  </div>
</template>
