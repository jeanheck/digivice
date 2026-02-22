<script setup lang="ts">
import DigimonHeaderInfo from './DigimonHeaderInfo.vue'

// Temporary mock type until we fetch from store
type DigimonData = {
  name: string
  level: number
  currentExp: number
  expForNextLevel: number
  currentHp: number
  maxHp: number
  currentMp: number
  maxMp: number
}

defineProps<{
  digimon: DigimonData
}>()
</script>

<template>
  <div class="flex flex-col h-full w-full bg-[#8b5cf6] p-4 rounded-md shadow-lg border border-purple-700">
    <DigimonHeaderInfo :digimon="digimon" />
    
    <!-- Empty space for future body content -->
    <div class="flex-1 mt-4 rounded border-2 border-dashed border-purple-400 bg-purple-500/20 flex items-center justify-center">
      <span class="text-purple-200 font-medium">Reserved for Details</span>
    </div>
  </div>
</template>
