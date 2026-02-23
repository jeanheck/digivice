<script setup lang="ts">
import DigimonHeaderInfo from './DigimonHeaderInfo.vue'
import DigimonEvolutions from './DigimonEvolutions.vue'
import DigimonDetails from './DigimonDetails.vue'
import type { Digimon } from '../../types/backend'

defineProps<{
  digimon: Digimon
}>()
</script>

<template>
  <div class="flex flex-col h-full w-full bg-[#000e3f] p-4 rounded-md shadow-lg border-2 border-[#0033aa] gap-4">
    <DigimonHeaderInfo :digimon="digimon" />
    
    <DigimonEvolutions />

    <DigimonDetails />
    
    <!-- Empty space for future body content (Equipaments) -->
    <div class="flex-1 rounded border-2 border-dashed border-[#0033aa] bg-[#00154a]/50 flex items-center justify-center min-h-[50px]">
      <span class="text-[#0055ff] font-medium opacity-70">Reserved for Equipments</span>
    </div>
  </div>
</template>
