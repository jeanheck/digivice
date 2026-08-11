<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { DropRaw } from "@/repositories/tables/raws/enemy/drop.raw";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

const props = defineProps<{
  enemy: EnemyViewModel;
}>();

const emit = defineEmits<{
  (e: "open-drops"): void;
}>();

const { t } = useI18n();

const hasDrop = computed(() => {
  return props.enemy.drop !== undefined;
});

const dropLabel = computed(() => {
  const drop = props.enemy.drop;
  if (drop === undefined) {
    return t("drops.none");
  }

  if (typeof drop === "string") {
    return t(`drops.${drop}`);
  }

  const dropIds = drop.flatMap((sectorDrop: DropRaw) => {
    return sectorDrop.sectorDrops;
  });
  const uniqueDropIds = [...new Set(dropIds)];

  return uniqueDropIds
    .map((dropId) => {
      return t(`drops.${dropId}`);
    })
    .join(", ");
});

const showGuaranteedDropNote = computed(() => {
  return props.enemy.boss && hasDrop.value;
});

const handleDropClick = (): void => {
  if (!hasDrop.value) {
    return;
  }

  emit("open-drops");
};
</script>

<template>
  <div
    class="flex-1 min-h-0 overflow-hidden bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-col"
    :class="
      hasDrop
        ? 'cursor-pointer transition-colors hover:border-blue-500/50 hover:bg-[#00122a]'
        : ''
    "
    :role="hasDrop ? 'button' : undefined"
    :tabindex="hasDrop ? 0 : undefined"
    @click="handleDropClick"
    @keydown.enter="handleDropClick"
  >
    <h4
      class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-2 border-b border-blue-900/30 pb-1 w-full text-center shrink-0"
    >
      {{ $t("enemy.drops") }}
    </h4>

    <div class="flex-1 min-h-0 flex items-center justify-center">
      <span class="font-bold text-gray-300 text-xs 2xl:text-sm text-center">
        {{ dropLabel }}
      </span>
    </div>

    <p
      v-if="showGuaranteedDropNote"
      class="shrink-0 mt-2 text-[9px] text-gray-500 text-center italic"
    >
      {{ $t("enemy.guaranteedDrop") }}
    </p>
  </div>
</template>
