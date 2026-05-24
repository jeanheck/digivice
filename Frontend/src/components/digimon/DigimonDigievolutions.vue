<script setup lang="ts">
import { ref, computed } from 'vue';
import type { DigievolutionSlot, EnrichedDigievolution } from '../../models';
import { DigievolutionRepository } from '../../repositories/digievolution-repository';
import DigievolutionTechniquesModal from './digievolution/DigievolutionTechniquesModal.vue';

const props = defineProps<{
  digievolutionsSlots: DigievolutionSlot[];
  activeDigievolutionId: number | null;
}>();

const modalOpen = ref(false);
const selectedDigievolution = ref<EnrichedDigievolution | null>(null);

const enrichedDigievolutions = computed(() => {
  return props.digievolutionsSlots.map((slot) => {
    if (!slot.digievolution || slot.digievolutionId === null) {
      return null;
    }

    return DigievolutionRepository.getEnrichedDigievolution(slot.digievolutionId);
  });
});

function openSkills(digievolution: EnrichedDigievolution) {
    selectedDigievolution.value = digievolution;
    modalOpen.value = true;
}

function closeSkills() {
    modalOpen.value = false;
    selectedDigievolution.value = null;
}
</script>

<template>
  <div class="flex flex-col gap-[2px] w-full">
    <div 
      v-for="(enrichedDigievolution, index) in enrichedDigievolutions" 
      :key="index"
      class="evo-row relative flex w-full h-[28px] bg-[#000a2b] text-white overflow-hidden dw3-beveled"
      :class="enrichedDigievolution ? 'cursor-pointer hover:brightness-125 transition-[filter]' : 'cursor-default'"
      @click="enrichedDigievolution ? openSkills(enrichedDigievolution) : null"
    >
      <div class="absolute inset-0 bg-[#0077ff] pointer-events-none dw3-beveled"></div>
      <div class="absolute inset-[1.5px] bg-[#000a2b] pointer-events-none dw3-beveled"></div>

      <template v-if="enrichedDigievolution">
        <div 
          class="relative z-10 flex-1 flex items-center px-4 font-bold text-sm tracking-wider"
          :class="enrichedDigievolution.id === activeDigievolutionId ? 'bg-gradient-to-b from-[#ffcc00] to-[#ff6600] text-transparent bg-clip-text shadow-text-dark' : 'shadow-text'">
          {{ enrichedDigievolution.name }}
        </div>

        <div class="relative z-10 w-[2px] h-full bg-[#0077ff] -skew-x-[30deg] ml-2"></div>

        <div class="relative z-10 w-[45px] flex items-center justify-center pl-2 font-bold text-sm mr-2">
          <span class="bg-gradient-to-b from-[#ffcc00] to-[#ff6600] text-transparent bg-clip-text shadow-text-dark">
            {{ enrichedDigievolution.level }}
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

  <DigievolutionTechniquesModal
    :is-open="modalOpen"
    :digievolution-name="selectedDigievolution?.name ?? ''"
    :techniques="selectedDigievolution?.techniques ?? []"
    @close="closeSkills"
  />
</template>
