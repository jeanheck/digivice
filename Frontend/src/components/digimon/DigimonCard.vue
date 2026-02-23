<script setup lang="ts">
import DigimonHeaderInfo from './DigimonHeaderInfo.vue'
import DigimonEvolutions from './DigimonEvolutions.vue'
import DigimonDetails from './DigimonDetails.vue'
import DigimonEquipments from './DigimonEquipments.vue'
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
    
    <DigimonEquipments />
  </div>
</template>
