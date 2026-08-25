<script setup lang="ts">
import WikiProfileImage from "@/components/wiki-modal/wiki-profile-panel/WikiProfileImage.vue";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

defineProps<{
  enemy: EnemyViewModel;
  imageUrl: string | null;
}>();
</script>

<template>
  <div
    class="h-full min-w-0 bg-[#000a1a] border border-blue-900/50 rounded p-2 shadow-inner flex flex-col gap-2"
  >
    <WikiProfileImage
      class="!h-auto w-full aspect-square shrink-0"
      :enemy-image-url="imageUrl"
      :enemy-name="enemy.name"
    />

    <p class="text-center text-xs font-bold text-white tracking-wide truncate">
      {{ enemy.name }}
    </p>

    <div class="flex flex-col gap-1.5">
      <div class="flex items-center justify-between text-[10px] gap-1">
        <span class="font-bold text-blue-500 tracking-wider uppercase shrink-0">
          {{ $t("enemy.specie") }}:
        </span>
        <span class="font-bold text-gray-300 capitalize text-right truncate">
          {{ $t(`species.${enemy.species}`) }}
        </span>
      </div>

      <div class="flex items-center justify-between text-[10px] gap-1">
        <span class="font-bold text-blue-500 tracking-wider uppercase shrink-0">
          {{ $t("enemy.level") }}:
        </span>
        <span class="font-bold text-gray-300">{{ enemy.level }}</span>
      </div>

      <div class="flex items-center justify-between text-[10px] gap-1">
        <span class="font-bold text-blue-500 tracking-wider uppercase shrink-0">HP:</span>
        <span class="font-bold text-white">{{ enemy.hp }}</span>
      </div>
    </div>
  </div>
</template>
