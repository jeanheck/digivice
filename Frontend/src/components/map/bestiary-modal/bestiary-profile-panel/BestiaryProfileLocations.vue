<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

const props = defineProps<{
  enemy: EnemyViewModel;
}>();

const { t } = useI18n();

const hasLocations = computed(() => {
  return (props.enemy.locations?.length ?? 0) > 0;
});

const locationsLabel = computed(() => {
  const locations = props.enemy.locations ?? [];
  const uniqueLocationIds: string[] = [];

  for (const location of locations) {
    if (!uniqueLocationIds.includes(location.locationId)) {
      uniqueLocationIds.push(location.locationId);
    }
  }

  return uniqueLocationIds
    .map((locationId) => {
      return t(`location.${locationId}`);
    })
    .join(", ");
});
</script>

<template>
  <div v-if="hasLocations" class="flex-1 min-w-0 flex flex-col gap-1.5">
    <span class="text-[9px] text-gray-500 uppercase font-bold tracking-wider">
      {{ $t("enemy.locations") }}
    </span>
    <span class="text-gray-200 text-xs leading-tight">
      {{ locationsLabel }}
    </span>
  </div>
</template>
