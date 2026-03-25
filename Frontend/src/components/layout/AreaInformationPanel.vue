<script setup lang="ts">
import { useGameStore } from '../../stores/useGameStore'
import { computed, ref } from 'vue'
import locationsData from '../../data/static/Locations.json'
import enemiesData from '../../data/static/EnemiesTable.json'
import EnemyDetailsModal from './EnemyDetailsModal.vue'
import { useLocalization } from '../../composables/useLocalization'

const store = useGameStore()
const { t, getLocalized } = useLocalization()
const locations = locationsData as Record<string, any>

const currentLocationStr = computed(() => {
    return store.gameState?.player?.mapId
})

const currentLocation = computed(() => {
    const loc = currentLocationStr.value
    if (!loc || loc === 'Unknown') return t('area.unknownArea') || 'Unknown Area'
    
    return locations[loc] ? getLocalized(locations[loc].Name) : `${t('area.unknownZone') || 'Unknown Zone'} ${loc}`
})

const areaEnemies = computed(() => {
    const rawLoc = currentLocationStr.value
    if (!rawLoc || rawLoc === 'Unknown') return []
    
    const locObj = locations[rawLoc]
    // Use EN-US name as the stable key for filtering in EnemiesTable
    const mapNameEn = locObj && locObj.Name ? locObj.Name['EN-US'] : null
    
    if (!mapNameEn) return [];

    return (enemiesData as any[]).filter((enemy: any) => {
        if (!enemy.Location) return false;
        
        // Handle both localized object and legacy string if any
        const enemyLocEn = typeof enemy.Location === 'object' ? enemy.Location['EN-US'] : enemy.Location;
        return enemyLocEn === mapNameEn;
    })
})

const currentMapImage = computed(() => {
    const rawLoc = currentLocationStr.value
    if (!rawLoc || rawLoc === 'Unknown') return null
    try {
        const locEntry = locations[rawLoc]
        const imageFile = locEntry?.Image || rawLoc
        return new URL(`../../assets/maps/${imageFile}.webp`, import.meta.url).href
    } catch {
        return null
    }
})

const isEnemyModalOpen = ref(false)
const selectedEnemy = ref<any | null>(null)

const openEnemyDetails = (enemy: any) => {
    selectedEnemy.value = enemy
    isEnemyModalOpen.value = true
}

const closeEnemyDetails = () => {
    isEnemyModalOpen.value = false
    selectedEnemy.value = null
}
</script>

<template>
  <aside class="w-full h-full bg-[#000e3f] rounded-md shadow-lg border-2 border-[#0033aa] p-3 flex flex-col pt-0 mb-1 overflow-hidden">
    <!-- Header banner -->
    <div class="w-full flex items-center justify-center border-b border-[#0033aa]/50 bg-[#000e3f] sticky top-0 py-2 z-10">
      <h3 class="font-bold tracking-widest text-[#0077ff] text-shadow-sm uppercase text-sm">{{ $t('area.title') }}</h3>
    </div>
    
    <div class="flex-1 flex flex-col mt-2 h-full">
        <!-- 75% Space: Image/Text representation -->
        <div class="flex-[3] relative w-full rounded border-2 border-[#0044aa]/50 bg-black bg-opacity-60 flex items-center justify-center overflow-hidden shadow-inner">
            
            <!-- Animated grid background -->
            <div class="absolute inset-0 bg-grid-pattern opacity-10 pointer-events-none animate-pan-bg"></div>

            <!-- Dynamic BG Map Image -->
            <img v-if="currentMapImage" :src="currentMapImage" class="absolute inset-0 w-full h-full object-cover opacity-60 mix-blend-lighten pointer-events-none" />

            <!-- Scanline overlay -->
            <div class="absolute inset-0 bg-[linear-gradient(transparent_50%,rgba(0,0,0,0.5)_50%)] bg-[length:100%_4px] pointer-events-none opacity-40"></div>
            
            <!-- Text overlay -->
            <div class="relative z-10 px-3 py-2 bg-black/60 border border-[#00aaff]/40 rounded backdrop-blur-sm max-w-[90%] text-center">
                <h4 class="text-xs sm:text-sm font-bold text-white tracking-widest uppercase drop-shadow-[0_0_5px_rgba(0,170,255,0.8)] leading-tight">
                    {{ currentLocation }}
                </h4>
            </div>

            <!-- Cyberpunk brackets -->
            <div class="absolute top-1 left-1 w-3 h-3 border-t-2 border-l-2 border-[#00aaff]/60"></div>
            <div class="absolute bottom-1 right-1 w-3 h-3 border-b-2 border-r-2 border-[#00aaff]/60"></div>
            <div class="absolute top-1 right-1 w-3 h-3 border-t-2 border-r-2 border-[#00aaff]/60"></div>
            <div class="absolute bottom-1 left-1 w-3 h-3 border-b-2 border-l-2 border-[#00aaff]/60"></div>
        </div>

        <!-- Area Enemies -->
        <div class="flex-1 w-full mt-2 pt-2 border-t border-[#0033aa]/50 flex flex-col justify-center items-center">
             <h4 class="text-[9px] uppercase font-bold tracking-[0.2em] text-[#00aaff] mb-1">{{ $t('area.enemies') }}</h4>
             
             <div v-if="areaEnemies.length === 0" class="text-xs text-[#00aaff] opacity-50 italic">
                {{ $t('area.safeZone') || 'Safe Zone' }}
             </div>
             <div v-else class="flex flex-wrap items-center justify-center gap-x-3 gap-y-1">
                <button 
                   v-for="enemy in areaEnemies" 
                   :key="enemy.Id" 
                   @click="openEnemyDetails(enemy)"
                   class="font-bold text-sm tracking-wide text-[#9e3737] hover:text-[#b24848] drop-shadow-[0_0_2px_rgba(158,55,55,0.8)] transition-all flex items-center justify-center focus:outline-none focus:ring-1 focus:ring-[#9e3737] rounded px-1 cursor-pointer"
                >
                   {{ getLocalized(enemy.Name) }}
                </button>
             </div>
        </div>
    </div>
    
    <EnemyDetailsModal :is-open="isEnemyModalOpen" :enemy="selectedEnemy" @close="closeEnemyDetails" />
  </aside>
</template>

<style scoped>
.bg-grid-pattern {
  background-image: 
    linear-gradient(to right, #0055ff 1px, transparent 1px),
    linear-gradient(to bottom, #0055ff 1px, transparent 1px);
  background-size: 15px 15px;
}

.animate-pan-bg {
    animation: panbg 20s linear infinite;
}

@keyframes panbg {
    0% { background-position: 0 0; }
    100% { background-position: 30px 30px; }
}
</style>
