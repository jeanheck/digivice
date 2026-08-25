<script setup lang="ts">
defineProps<{
  npcId: string;
  battleIndex: number;
}>();
</script>

<template>
  <div class="flex-1 min-h-0 flex items-center justify-center">
    <span class="text-xs text-gray-400 font-bold tracking-wide">
      {{ $t("npc.battle.digimonPlaceholder") }}
    </span>
  </div>
</template>
