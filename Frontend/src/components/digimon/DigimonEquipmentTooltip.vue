<script setup lang="ts">
import { useLocalization } from '../../composables/useLocalization'
import { EquipamentsAttributesOperationType, type Equipament } from '../../models'

defineProps<{
  activeTooltip: { show: boolean, item: Equipament | null, x: number, y: number }
}>()

const { t, getLocalized } = useLocalization()
</script>

<template>
  <Teleport to="body">
    <Transition name="fade">
      <div 
        v-if="activeTooltip.show && activeTooltip.item"
        class="fixed z-[9999] pointer-events-none p-3 max-w-[250px] bg-[#001133ee] border-[2px] border-[#0066cc] rounded-sm shadow-[0_4px_12px_rgba(0,0,0,0.8)] flex flex-col gap-1 backdrop-blur-sm"
        :style="{ top: `${activeTooltip.y}px`, left: `${activeTooltip.x}px` }"
      >
        <div class="flex flex-col gap-1 w-full min-w-[170px]">
           <div class="font-bold text-yellow-300 text-sm border-b border-[#0066cc]/50 pb-1 mb-1 shadow-text-dark text-center uppercase tracking-wider">
              {{ getLocalized(activeTooltip.item.name) }}
           </div>
           
           <div v-if="activeTooltip.item.typeDescription" class="text-blue-300 text-[10px] tracking-widest uppercase mb-1 text-center font-bold">
              {{ getLocalized(activeTooltip.item.typeDescription) }}
           </div>

           <div v-if="activeTooltip.item.attributes && activeTooltip.item.attributes.length > 0" class="flex flex-col gap-[2px] mb-1">
               <div v-for="attr in activeTooltip.item.attributes" :key="attr.attribute" class="flex justify-between text-xs items-center bg-[#002266]/40 px-1 rounded-sm">
                  <span class="text-gray-200">{{ t('equipamentsAttributes.' + attr.attribute) }}</span>
                  <span :class="attr.type === EquipamentsAttributesOperationType.Addition ? 'text-green-400' : 'text-red-400'" class="font-bold tracking-wider">
                    {{ attr.type === EquipamentsAttributesOperationType.Addition ? '+' : '-' }}{{ attr.value }}
                  </span>
               </div>
           </div>

           <div class="mt-1 pt-1 border-t border-[#0033aa]/50 flex flex-col gap-1">
              <span class="text-gray-400 text-[9px] uppercase tracking-widest leading-none">{{ $t('digimon.equipableBy') }}</span>
              <span class="text-gray-200 text-[11px] leading-tight">
                <template v-if="!activeTooltip.item.equipableDigimon || activeTooltip.item.equipableDigimon.length === 0">
                  {{ $t('digimon.states.none') }}
                </template>
                <template v-else-if="activeTooltip.item.equipableDigimon.length === 8">
                  <span class="text-purple-300 font-bold uppercase tracking-wider text-[10px]">{{ $t('digimon.allDigimon') }}</span>
                </template>
                <template v-else>
                  {{ activeTooltip.item.equipableDigimon.join(', ') }}
                </template>
              </span>
           </div>

           <div v-if="activeTooltip.item.note" class="mt-1 pt-1 border-t border-[#0033aa]/50 text-gray-400 text-[10px] italic leading-tight">
              "{{ getLocalized(activeTooltip.item.note) }}"
           </div>
        </div>
      </div>
    </Transition>
  </Teleport>
</template>
