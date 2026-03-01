<script setup lang="ts">
import { useGameStore } from '../../stores/useGameStore'
import { computed } from 'vue'

const store = useGameStore()
const items = computed(() => store.gameState?.importantItems || {})

const milestones = [
  { key: 'TreeBoots', name: 'Tree Boots', icon: '🥾' },
  { key: 'FishingPole', name: 'Fishing Pole', icon: '🎣' },
  { key: 'ElDoradoId', name: 'El Dorado Id', icon: '💳' },
  { key: 'FolderBag', name: 'Folder Bag', icon: '📂' }
]

const getStyle = (key: string) => {
  return items.value[key] === true 
    ? 'opacity-100 drop-shadow-[0_0_6px_rgba(255,255,0,0.8)] border-[#d4af37] bg-[#1a2b4c]' 
    : 'opacity-40 grayscale border-[#445] bg-[#001122]'
}
</script>

<template>
  <aside class="w-full h-full bg-[#000e3f] rounded-md shadow-lg border-2 border-[#0033aa] p-3 flex flex-col items-center">
    <div class="w-full mb-3 flex items-center justify-center border-b border-[#0033aa]/50 pb-1">
      <h3 class="font-bold tracking-widest text-[#0077ff] text-shadow-sm uppercase text-sm">Milestones</h3>
    </div>
    
    <div class="flex flex-wrap gap-3 justify-center">
      <div 
        v-for="milestone in milestones" 
        :key="milestone.key"
        :title="milestone.name"
        class="w-[50px] h-[50px] flex items-center justify-center rounded border-2 transition-all duration-300 cursor-help"
        :class="getStyle(milestone.key)"
      >
        <span class="text-2xl filter drop-shadow">{{ milestone.icon }}</span>
      </div>
    </div>
  </aside>
</template>

<style scoped>
.text-shadow-sm {
  text-shadow: 1px 1px 0 #000;
}
</style>
