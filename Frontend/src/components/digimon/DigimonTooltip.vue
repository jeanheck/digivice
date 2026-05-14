<script setup lang="ts">
import { useLocalization } from '../../composables/useLocalization';

defineProps<{
  activeTooltip: {
    show: boolean;
    title: string;
    text: string;
    isMath: boolean;
    base: number;
    equip: number;
    digi: number;
    total: number;
    x: number;
    y: number;
  }
}>();

const { t } = useLocalization();
</script>

<template>
  <Teleport to="body">
    <Transition name="fade">
      <div 
        v-if="activeTooltip.show"
        class="fixed z-[9999] pointer-events-none p-3 max-w-[250px] bg-[#001133ee] border-[2px] border-[#0066cc] rounded-sm shadow-[0_4px_12px_rgba(0,0,0,0.8)] flex flex-col gap-1 backdrop-blur-sm"
        :style="{ top: `${activeTooltip.y}px`, left: `${activeTooltip.x}px` }"
      >
        <div v-if="!activeTooltip.isMath">
           <div class="font-bold text-yellow-300 text-sm border-b border-[#0066cc]/50 pb-1 mb-1 shadow-black text-shadow-sm uppercase tracking-wider">
              {{ activeTooltip.title }}
           </div>
           <div class="text-gray-100 text-xs leading-relaxed shadow-black text-shadow-sm">
              {{ activeTooltip.text }}
           </div>
        </div>
        <div v-else class="flex flex-col w-full min-w-[170px]">
           <div class="font-bold text-yellow-300 text-sm border-b border-[#0066cc]/50 pb-1 mb-2 shadow-black text-shadow-sm uppercase tracking-wider text-center">
              {{ activeTooltip.title }}
           </div>
           
           <div class="text-white text-base font-bold text-center mb-2 tracking-wider shadow-text whitespace-nowrap">
              {{ activeTooltip.total }} 
              <span class="text-[10px] text-gray-400 tracking-normal ml-1">(<span class="text-white">{{ activeTooltip.base }}</span> + <span class="text-[#0077ff] font-bold">{{ activeTooltip.equip }}</span>)</span>
           </div>

           <div class="flex flex-col gap-[2px]">
               <div class="flex justify-between text-xs items-center">
                  <span class="text-white">{{ t('digimon.baseDigimon') }}</span>
               </div>
               <div class="flex justify-between text-xs items-center">
                  <span class="text-[#0077ff] font-bold">{{ t('digimon.equipments') }}</span>
               </div>
           </div>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>
