<script setup lang="ts">
import { computed, ref } from 'vue'
import type { Digimon } from '../../types/backend'
import DigievolutionTechniquesModal from './DigievolutionTechniquesModal.vue'
import digievolutionData from '../../data/static/Digievolution.json'

const props = defineProps<{
  digimon: Digimon
}>()

const digiIdMap = new Map(
  digievolutionData.digievolutions
    .filter(d => d.id !== null)
    .map(d => [d.id!, d.name])
)

function getEvolutionName(id: number) {
  return digiIdMap.get(id) ?? `Unknown (${id})`
}

const evolutions = computed(() => {
  const evos = props.digimon.equippedDigievolutions || [null, null, null]
  return [0, 1, 2].map(i => {
    const evo = evos[i]
    if (!evo) return null
    return {
      name: getEvolutionName(evo.id),
      level: evo.level
    }
  })
})

// Skills modal state
const modalOpen = ref(false)
const selectedDigi = ref<string | null>(null)
const selectedLevel = ref(0)

function openSkills(evo: { name: string; level: number } | null) {
  if (!evo) return
  selectedDigi.value = evo.name
  selectedLevel.value = evo.level
  modalOpen.value = true
}

function closeSkills() {
  modalOpen.value = false
  selectedDigi.value = null
}
</script>

<template>
  <div class="flex flex-col gap-[2px] w-full">
    <div 
      v-for="(evo, index) in evolutions" 
      :key="index"
      class="evo-row relative flex w-full h-[28px] bg-[#000a2b] text-white overflow-hidden"
      :class="evo ? 'cursor-pointer hover:brightness-125 transition-[filter]' : 'cursor-default'"
      @click="openSkills(evo)"
    >
      <!-- Borda externa brilhante simuluada via clip-path background -->
      <div class="absolute inset-0 bg-[#0077ff] pointer-events-none evo-border"></div>
      
      <!-- Fundo interno escuro (1 pixel menor que a borda) -->
      <div class="absolute inset-[1.5px] bg-[#000a2b] pointer-events-none evo-inner"></div>

      <!-- Renderiza os textos somente se o slot de digievolução tiver valor -->
      <template v-if="evo">
        <!-- Conteúdo (Nome) -->
        <div class="relative z-10 flex-1 flex items-center px-4 font-bold text-sm tracking-wider shadow-text">
          {{ evo.name }}
        </div>

        <!-- Divisor Inclinado -->
        <div class="relative z-10 w-[2px] h-full bg-[#0077ff] -skew-x-[30deg] ml-2"></div>

        <!-- Conteúdo (Level) -->
        <div class="relative z-10 w-[45px] flex items-center justify-center pl-2 font-bold text-sm mr-2">
          <span class="bg-gradient-to-b from-[#ffcc00] to-[#ff6600] text-transparent bg-clip-text shadow-text-dark">
            {{ evo.level }}
          </span>
        </div>
      </template>
    </div>
  </div>

  <!-- Skills Modal (Teleported outside component) -->
  <DigievolutionTechniquesModal
    :is-open="modalOpen"
    :digivolution-name="selectedDigi"
    :current-level="selectedLevel"
    @close="closeSkills"
  />
</template>

<style scoped>
/* O formato chanfrado do Digimon World 3 (cortes nos cantos opostos) */
.evo-border, .evo-inner {
  clip-path: polygon(4px 0, 100% 0, 100% calc(100% - 4px), calc(100% - 4px) 100%, 0 100%, 0 4px);
}

.shadow-text {
  text-shadow: 1px 1px 0 #000;
}

.shadow-text-dark {
  /* Uma sombra sutil pro gradient sobressair */
  filter: drop-shadow(1px 1px 0 rgba(0,0,0,0.8));
}
</style>
