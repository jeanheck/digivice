<script setup lang="ts">
defineProps<{
  enemyImageUrl: string | null;
  enemyName: string;
}>();
</script>

<template>
  <div
    class="h-full bg-[#000a1a] border border-blue-900/50 rounded flex items-center justify-center shadow-inner relative overflow-hidden"
  >
    <div
      class="absolute inset-0 opacity-10 bg-linear-to-br from-blue-500/20 to-transparent pointer-events-none"
    ></div>
    <img
      v-if="enemyImageUrl"
      :src="enemyImageUrl"
      class="w-full h-full object-contain drop-shadow-[0_0_15px_rgba(0,170,255,0.4)]"
      :alt="enemyName"
    />
    <div v-else class="text-4xl opacity-30 select-none">❓</div>
  </div>
</template>
