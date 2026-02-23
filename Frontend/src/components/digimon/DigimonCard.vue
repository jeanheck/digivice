<script setup lang="ts">
import DigimonHeaderInfo from './DigimonHeaderInfo.vue'
import type { Digimon } from '../../types/backend'

defineProps<{
  digimon: Digimon
}>()
</script>

<template>
  <div class="flex flex-col h-full w-full bg-[#000e3f] p-4 rounded-md shadow-lg border-2 border-[#0033aa]">
    <DigimonHeaderInfo :digimon="digimon" />
    
    <!-- Empty space for future body content -->
    <div class="flex-1 mt-4 rounded border-2 border-dashed border-[#0033aa] bg-[#00154a]/50 flex items-center justify-center">
      <span class="text-[#0055ff] font-medium opacity-70">Reserved for Details</span>
    </div>
  </div>
</template>
