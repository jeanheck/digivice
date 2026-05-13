<script setup lang="ts">
import { useGameStore } from '../../stores/useGameStore'
import { computed, ref } from 'vue'
import { useLocalization } from '../../composables/useLocalization'

const store = useGameStore()
const { t } = useLocalization()

const items = computed(() => store.gameState?.importantItems || ({} as import('../../models').ImportantItems))

const milestones = [
  { key: 'treeBoots', nameKey: 'player.treeBoots', icon: '🥾' },
  { key: 'fishingPole', nameKey: 'player.fishingPole', icon: '🎣' },
  { key: 'elDoradoId', nameKey: 'player.elDoradoId', icon: '💳' },
  { key: 'folderBag', nameKey: 'player.folderBag', icon: '📂' }
]

const getStyle = (key: keyof import('../../models').ImportantItems) => {
  return items.value?.[key]?.has === true 
    ? 'opacity-100 drop-shadow-[0_0_6px_rgba(255,255,0,0.8)] border-[#d4af37] bg-[#1a2b4c]' 
    : 'opacity-40 grayscale border-[#445] bg-[#001122]'
}

// Tooltip Logic
const activeTooltip = ref({ show: false, name: '', x: 0, y: 0 })

const showTooltip = (event: MouseEvent, nameKey: string) => {
  const tooltipWidth = 160
  let posX = event.clientX + 15
  if (posX + tooltipWidth > window.innerWidth) {
    posX = event.clientX - tooltipWidth - 10
  }

  activeTooltip.value = {
    show: true,
    name: t(nameKey),
    x: posX,
    y: event.clientY + 15
  }
}

const hideTooltip = () => {
  activeTooltip.value.show = false
}

const moveTooltip = (event: MouseEvent) => {
  if (!activeTooltip.value.show) return

  const tooltipWidth = 160
  let posX = event.clientX + 15
  if (posX + tooltipWidth > window.innerWidth) {
    posX = event.clientX - tooltipWidth - 10
  }

  activeTooltip.value.x = posX
  activeTooltip.value.y = event.clientY + 15
}
</script>

<template>
  <aside class="w-full h-full bg-[#000e3f] rounded-md shadow-lg border-2 border-[#0033aa] p-3 flex flex-col items-center">
    <div class="w-full mb-3 flex items-center justify-center border-b border-[#0033aa]/50 pb-1">
      <h3 class="font-bold tracking-widest text-[#0077ff] text-shadow-sm uppercase text-sm">{{ $t('player.milestones') || 'Milestones' }}</h3>
    </div>
    
    <div class="flex flex-wrap gap-3 justify-center">
      <div 
        v-for="milestone in milestones" 
        :key="milestone.key"
        class="w-[50px] h-[50px] flex items-center justify-center rounded border-2 transition-all duration-300 cursor-help"
        :class="getStyle(milestone.key as keyof import('../../models').ImportantItems)"
        @mouseenter="e => showTooltip(e, milestone.nameKey)"
        @mousemove="moveTooltip"
        @mouseleave="hideTooltip"
      >
        <span class="text-2xl filter drop-shadow">{{ milestone.icon }}</span>
      </div>
    </div>

    <!-- Teleported Tooltip -->
    <Teleport to="body">
      <Transition name="fade">
        <div 
          v-if="activeTooltip.show"
          class="fixed z-[9999] pointer-events-none p-3 max-w-[160px] bg-[#001133ee] border-[2px] border-[#0066cc] rounded-sm shadow-[0_4px_12px_rgba(0,0,0,0.8)] flex flex-col gap-1 backdrop-blur-sm"
          :style="{ top: `${activeTooltip.y}px`, left: `${activeTooltip.x}px` }"
        >
          <div class="font-bold text-yellow-300 text-sm shadow-black text-shadow-sm uppercase tracking-wider text-center">
            {{ activeTooltip.name }}
          </div>
        </div>
      </Transition>
    </Teleport>
  </aside>
</template>

<style scoped>
.text-shadow-sm {
  text-shadow: 1px 1px 0 #000;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.15s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
