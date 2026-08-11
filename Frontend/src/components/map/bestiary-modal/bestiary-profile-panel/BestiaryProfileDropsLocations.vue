<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

const props = defineProps<{
  enemy: EnemyViewModel;
}>();

const { t, te } = useI18n();

const hasDropsByLocation = computed(() => {
  const dropsByLocation = props.enemy.dropsByLocation;
  if (dropsByLocation === undefined) {
    return false;
  }

  return Object.keys(dropsByLocation).length > 0;
});

const resolveAreaLabel = (areaKey: string): string => {
  const sectorKey = `sectors.${areaKey}`;
  if (te(sectorKey)) {
    return t(sectorKey);
  }

  return t(`location.${areaKey}`);
};

const dropByLocationLines = computed(() => {
  const dropsByLocation = props.enemy.dropsByLocation;
  if (dropsByLocation === undefined) {
    return [] as string[];
  }

  return Object.entries(dropsByLocation).map(([areaKey, dropIds]) => {
    const areaLabel = resolveAreaLabel(areaKey);
    const dropLabels = dropIds
      .map((dropId) => {
        return t(`drops.${dropId}`);
      })
      .join(", ");

    return `${areaLabel}: ${dropLabels}`;
  });
});
</script>

<template>
  <div v-if="hasDropsByLocation" class="flex-1 min-w-0 flex flex-col gap-1.5">
    <span class="text-[9px] text-gray-500 uppercase font-bold tracking-wider">
      {{ $t("enemy.dropsByLocation") }}
    </span>
    <div class="flex flex-col gap-1">
      <span
        v-for="line in dropByLocationLines"
        :key="line"
        class="text-gray-200 text-xs leading-tight"
      >
        {{ line }}
      </span>
    </div>
  </div>
</template>
