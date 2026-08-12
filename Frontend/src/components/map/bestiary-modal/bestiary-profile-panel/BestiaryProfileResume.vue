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
    class="h-full bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-col justify-start gap-2.5 min-h-0"
  >
    <div class="flex items-center justify-between text-xs">
      <span class="font-bold text-blue-500 tracking-wider uppercase"
        >{{ $t("enemy.specie") }}:</span
      >
      <span class="font-bold text-gray-300 capitalize">{{
        $t(`species.${enemy.species}`)
      }}</span>
    </div>

    <div class="flex items-center justify-between text-xs">
      <span class="font-bold text-blue-500 tracking-wider uppercase"
        >{{ $t("enemy.level") }}:</span
      >
      <span class="font-bold text-gray-300">{{ enemy.level }}</span>
    </div>

    <div class="flex items-center justify-between text-xs">
      <span class="font-bold text-blue-500 tracking-wider uppercase">HP:</span>
      <span class="font-bold text-white">{{ enemy.hp }}</span>
    </div>

    <div class="flex items-center justify-between text-xs">
      <span class="font-bold text-blue-500 tracking-wider uppercase">
        {{ $t("enemy.baseExp") }}:
      </span>
      <span class="font-bold text-gray-300">{{ enemy.exp }}</span>
    </div>

    <div class="flex items-center justify-between text-xs">
      <span class="font-bold text-blue-500 tracking-wider uppercase"
        >{{ $t("enemy.baseBits") }}:</span
      >
      <span class="font-bold text-gray-300">{{ enemy.bits }}</span>
    </div>

    <div class="border-t border-blue-900/50 pt-2.5 flex flex-col gap-1 min-h-0 text-xs">
      <span class="font-bold text-blue-500 tracking-wider uppercase">{{
        $t("enemy.foundIn")
      }}:</span>
      <div class="flex flex-col gap-1 min-h-0 overflow-y-auto custom-scroll">
        <span
          v-for="label in locationLabels"
          :key="label"
          class="font-bold text-gray-300"
        >
          {{ label }}
        </span>
      </div>
    </div>
  </div>
</template>
