<script setup lang="ts">
defineProps<{
  enemyImageUrl: string | null;
  enemyName: string;
}>();
</script>

<template>
  <div
    class="h-full bg-[#000a1a] in-[[data-theme=seabed]]:bg-[var(--color-surface-deep)] border border-blue-900/50 in-[[data-theme=seabed]]:border-[var(--color-frame)]/50 rounded flex items-center justify-center shadow-inner relative overflow-hidden"
  >
    <div
      class="absolute inset-0 opacity-10 bg-linear-to-br from-blue-500/20 in-[[data-theme=seabed]]:from-[var(--color-accent)]/25 to-transparent pointer-events-none"
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
