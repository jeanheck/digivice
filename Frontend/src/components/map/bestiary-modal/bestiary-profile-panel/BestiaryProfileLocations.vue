<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

const props = defineProps<{
  enemy: EnemyViewModel;
}>();

const { t } = useI18n();

const locationLabels = computed(() => {
  const locations = props.enemy.locations ?? [];
  const uniqueLocationIds: string[] = [];

  for (const location of locations) {
    if (!uniqueLocationIds.includes(location.locationId)) {
      uniqueLocationIds.push(location.locationId);
    }
  }

  return uniqueLocationIds.map((locationId) => {
    return t(`location.${locationId}`);
  });
});
</script>

<template>
  <div
    class="h-full min-h-32 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner text-sm flex flex-col gap-1.5 min-w-0"
  >
    <span class="text-[9px] text-gray-500 uppercase font-bold tracking-wider">
      {{ $t("enemy.locations") }}
    </span>
    <div class="flex flex-col gap-1">
      <span
        v-for="label in locationLabels"
        :key="label"
        class="text-gray-200 text-[10px] leading-tight"
      >
        {{ label }}
      </span>
    </div>
  </div>
</template>
