<script setup lang="ts">
import type { EnemyResumedViewModel } from "@/presentation/viewmodels/enemy/enemy-resumed.viewmodel";

const { icon, enemies } = defineProps<{
  icon: string;
  enemies: EnemyResumedViewModel[];
}>();
</script>

<template>
  <div class="max-w-full flex items-center gap-1.5">
    <span class="w-5 shrink-0 flex justify-center text-[14px] leading-none font-emoji text-white">
      {{ icon }}
    </span>
    <div
      class="min-w-0 flex flex-wrap items-center gap-x-3 gap-y-1 text-[8px] leading-none font-bold tracking-wide"
    >
      <span v-for="enemy in enemies" :key="enemy.id" class="text-red-400 shadow-text">
        {{ enemy.name }}
      </span>
    </div>
  </div>
</template>
