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
    class="h-full min-w-0 bg-[#000a1a] border border-blue-900/50 rounded p-3 shadow-inner flex flex-col gap-1.5"
  >
    <h4
      class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-2 border-b border-blue-900/30 pb-1 w-full text-center truncate"
    >
      {{ enemy.name }}
    </h4>

    <div class="flex-1 min-h-0 max-h-[70%] flex items-center justify-center">
      <WikiProfileImage
        class="!h-auto w-full max-h-full aspect-square !border-0"
        :enemy-image-url="imageUrl"
        :enemy-name="enemy.name"
      />
    </div>

    <div class="shrink-0 flex flex-col gap-1.5">
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
