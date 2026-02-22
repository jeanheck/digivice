<script setup lang="ts">
// Props for player stats
defineProps<{
  playerName: string
  bits: number
}>()
</script>

<template>
  <footer class="w-full bg-[#8B4513] text-[#FFE4B5] p-3 rounded-md shadow-lg border border-[#5c2e0c] flex items-center gap-12 px-6">
    <div class="font-bold text-lg">
      <span class="opacity-80 text-sm mr-2 font-normal">Tamer:</span>
      {{ playerName }}
    </div>
    
    <div class="font-bold text-lg">
      <span class="opacity-80 text-sm mr-2 font-normal">Bits:</span>
      {{ bits }}
    </div>
  </footer>
</template>
