<script setup lang="ts">
import { computed } from 'vue'
import { EvolutionGraph } from '../../logic/EvolutionGraph'
import type { GraphNode } from '../../logic/EvolutionGraph'
import DigimonIcon from '../ui/DigimonIcon.vue'
import type { Digimon } from '../../types/backend'

const props = defineProps<{
  digimon: Digimon
  node: GraphNode
  isRoot?: boolean
  isFirstChild?: boolean
  isLastChild?: boolean
}>()

const hasChildren = computed(() => props.node.children && props.node.children.length > 0)

const isUnlocked = computed(() => {
  return EvolutionGraph.checkRequirements(props.digimon, props.node)
})

</script>

<template>
  <div class="flex flex-col relative w-max" :class="{'mt-4': !isRoot}">
    
    <!-- Top Row: Lines + Node -->
    <div class="flex items-center relative z-10 group min-h-[60px]">
      
      <!-- Lines Container for this Node -->
      <!-- Positioned relative to the row, sitting exactly to the left of the Node Box -->
      <div v-if="!isRoot" class="absolute left-[-32px] w-[32px] top-0 bottom-0 pointer-events-none">
         
         <!-- Horizontal line (exact center of THIS ROW) -->
         <div class="absolute left-0 right-[4px] h-[3px] bg-[#d4af37] top-1/2 -translate-y-1/2 shadow-[0_1px_3px_black]"></div>

         <!-- Vertical Line Snippet -->
         <!-- It must span from the top to connect to previous sibling, and down to connect to the next -->
         <div class="absolute left-0 w-[3px] bg-[#d4af37] shadow-[0_1px_3px_black] top-[-16px]"
             :class="isLastChild ? 'bottom-1/2' : 'bottom-[-16px]'"
         ></div>
      </div>

      <!-- Node Box -->
      <div 
        class="flex items-center p-1.5 bg-[#000a2b] border-[3px] rounded cursor-help transition-all shadow-[0_0_8px_black]"
        :class="[
          isUnlocked 
            ? 'border-yellow-600 hover:border-yellow-300' 
            : 'border-slate-800 hover:border-slate-500 opacity-60'
        ]"
      >
        
        <DigimonIcon 
          :digimon-name="node.name" 
          class="w-12 h-12 transition-all"
          :class="{ 'grayscale brightness-50': !isUnlocked }"
        />
        
        <!-- Tooltip Placeholder for Phase 4 -->
        <div class="absolute bottom-full left-1/2 transform -translate-x-1/2 mb-2 w-max px-3 py-1.5 bg-black bg-opacity-95 border rounded text-xs text-white opacity-0 group-hover:opacity-100 transition-opacity pointer-events-none z-50 shadow-lg"
             :class="isUnlocked ? 'border-yellow-600' : 'border-slate-600'"
        >
          <p class="font-bold mb-1 border-b pb-1" :class="isUnlocked ? 'text-yellow-400 border-yellow-800' : 'text-slate-400 border-slate-700'">
            {{ node.name }} <span v-if="!isUnlocked" class="text-red-400 ml-1">(Locked)</span>
          </p>
          <ul class="text-[0.7rem] text-gray-300 space-y-0.5" v-if="node.requirements.length > 0">
            <li v-for="(req, idx) in node.requirements" :key="idx" class="flex items-center gap-1">
              <span class="inline-block w-1 h-1 rounded-full bg-slate-500"></span>
              <span v-if="req.Type === 'DigimonLevel'">Lv. {{ req.Value }} (Rookie)</span>
              <span v-else-if="req.Type === 'DigievolutionLevel'">Lv. {{ req.Value }} ({{ req.Digievolution }})</span>
              <span v-else-if="req.Type === 'Attribute'">{{ req.Attribute }} {{ req.Value }}</span>
            </li>
          </ul>
          <p v-else class="text-[0.6rem] text-gray-500 italic">Base</p>
        </div>
      </div>
    </div>

    <!-- Children Container -->
    <!-- Indented to the right so its vertical spine aligns with the left edge of children's incoming block -->
    <div v-if="hasChildren" class="flex flex-col ml-[36px] relative">
       <DigievolutionTreeNode 
        v-for="(child, index) in node.children" 
        :key="child.id" 
        :digimon="digimon"
        :node="child"
        :is-root="false"
        :is-first-child="index === 0"
        :is-last-child="index === node.children.length - 1"
      />
    </div>

  </div>
</template>

<script lang="ts">
export default {
  name: 'DigievolutionTreeNode'
}
</script>
