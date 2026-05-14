<script setup lang="ts">
import { ref } from 'vue';
import type { Digievolution } from '../../models';
import DigievolutionTechniquesModal from './DigievolutionTechniquesModal.vue';

const props = defineProps<{
  digievolutions: (Digievolution | null)[];
  activeDigievolutionId: number | null;
}>();

// Skills modal state
const modalOpen = ref(false);
const selectedDigievolutionName = ref<string | null>(null);
const selectedLevel = ref(0);

function openSkills(digievolution: Digievolution | null) {
    if (!digievolution) {
        return;
    }
    selectedDigievolutionName.value = digievolution.name;
    selectedLevel.value = digievolution.level;
    modalOpen.value = true;
}

function closeSkills() {
    modalOpen.value = false;
    selectedDigievolutionName.value = null;
    selectedLevel.value = 0;
}
</script>

<template>
  <div class="flex flex-col gap-[2px] w-full">
    <div 
      v-for="(digievolution, index) in digievolutions" 
      :key="index"
      class="evo-row relative flex w-full h-[28px] bg-[#000a2b] text-white overflow-hidden dw3-beveled"
      :class="digievolution ? 'cursor-pointer hover:brightness-125 transition-[filter]' : 'cursor-default'"
      @click="openSkills(digievolution)"
    >
      <!-- External glowing border simulated via clip-path background -->
      <div class="absolute inset-0 bg-[#0077ff] pointer-events-none dw3-beveled"></div>
      
      <!-- Internal dark background (1.5px smaller than the border) -->
      <div class="absolute inset-[1.5px] bg-[#000a2b] pointer-events-none dw3-beveled"></div>

      <!-- Render texts only if the digievolution slot has a value -->
      <template v-if="digievolution">
        <!-- Content (Name) -->
        <div 
          class="relative z-10 flex-1 flex items-center px-4 font-bold text-sm tracking-wider"
          :class="digievolution.id === activeDigievolutionId ? 'bg-gradient-to-b from-[#ffcc00] to-[#ff6600] text-transparent bg-clip-text shadow-text-dark' : 'shadow-text'">
          {{ digievolution.name }}
        </div>

        <!-- Slanted Divider -->
        <div class="relative z-10 w-[2px] h-full bg-[#0077ff] -skew-x-[30deg] ml-2"></div>

        <!-- Content (Level) -->
        <div class="relative z-10 w-[45px] flex items-center justify-center pl-2 font-bold text-sm mr-2">
          <span class="bg-gradient-to-b from-[#ffcc00] to-[#ff6600] text-transparent bg-clip-text shadow-text-dark">
            {{ digievolution.level }}
          </span>
        </div>
      </template>
      
      <template v-else>
        <div class="relative z-10 flex-1 flex items-center px-4 font-bold text-sm tracking-wider text-white/80 shadow-text">
          {{ $t('digimon.states.empty') }}
        </div>
      </template>
    </div>
  </div>

  <!-- Skills Modal -->
  <DigievolutionTechniquesModal
    :is-open="modalOpen"
    :digivolution-name="selectedDigievolutionName"
    :current-level="selectedLevel"
    @close="closeSkills"
  />
</template>
