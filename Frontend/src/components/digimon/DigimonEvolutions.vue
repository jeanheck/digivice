<script setup lang="ts">
import { ref } from 'vue'

// Mock das três digievoluções disponíveis
const evolutions = ref([
  { id: 1, name: 'Beelzemon', level: 76 },
  { id: 2, name: 'GrapLeomon', level: 99 },
  { id: 3, name: 'Marsmon', level: 99 }
])
</script>

<template>
  <div class="flex flex-col gap-[2px] w-full">
    <div 
      v-for="evo in evolutions" 
      :key="evo.id"
      class="evo-row relative flex w-full h-[28px] bg-[#000a2b] text-white overflow-hidden"
    >
      <!-- Borda externa brilhante simuluada via clip-path background -->
      <div class="absolute inset-0 bg-[#0077ff] pointer-events-none evo-border"></div>
      
      <!-- Fundo interno escuro (1 pixel menor que a borda) -->
      <div class="absolute inset-[1.5px] bg-[#000a2b] pointer-events-none evo-inner"></div>

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
    </div>
  </div>
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
