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
  activePath?: string[]
  isPathBelow?: boolean
}>()

const emit = defineEmits<{
  (e: 'node-click', name: string): void
}>()

const hasChildren = computed(() => props.node.children && props.node.children.length > 0)

const isUnlocked = computed(() => {
  return EvolutionGraph.checkRequirements(props.digimon, props.node)
})

const isPathIlluminated = computed(() => props.activePath && props.activePath.length > 0)
const isNodeInPath = computed(() => isPathIlluminated.value && props.activePath!.includes(props.node.name))

const activeIndex = computed(() => {
  if (!props.node.children || !isPathIlluminated.value) return -1
  return props.node.children.findIndex(c => props.activePath!.includes(c.name))
})

const isTopHalfIlluminated = computed(() => isNodeInPath.value || props.isPathBelow)
const isBottomHalfIlluminated = computed(() => props.isPathBelow)
const isInletIlluminated = computed(() => isNodeInPath.value)
const isOutletIlluminated = computed(() => activeIndex.value !== -1)

const onNodeClick = () => {
  emit('node-click', props.node.name)
}
</script>

<template>
  <!-- Outer Wrapper that spans the entire Subtree Height -->
  <div class="flex relative whitespace-nowrap">
    
    <!-- Vertical Spine & Inlet Line Wrapper (Left Side) -->
    <div v-if="!isRoot" class="relative w-[32px] shrink-0">
      
      <!-- Vertical Spine Segment (Top Half) -->
      <!-- 'h-[46px]' overlaps the connector at 42px. -->
      <div v-if="!isFirstChild"
           class="absolute left-[16px] w-[3px] shadow-[0_1px_3px_black] transition-colors duration-500 top-[-2px] h-[46px]"
           :class="isTopHalfIlluminated ? 'bg-[#00ffff] shadow-[0_0_10px_cyan] z-10' : 'bg-[#d4af37] z-0'">
      </div>

      <!-- Vertical Spine Segment (Bottom Half) -->
      <div v-if="!isLastChild"
           class="absolute left-[16px] w-[3px] shadow-[0_1px_3px_black] transition-colors duration-500 top-[42px] bottom-[-2px]"
           :class="isBottomHalfIlluminated ? 'bg-[#00ffff] shadow-[0_0_10px_cyan] z-10' : 'bg-[#d4af37] z-0'">
      </div>

      <!-- Horizontal Bridge Line (First Child Only, connects Parent Outlet to Spine) -->
      <div v-if="isFirstChild" 
           class="absolute left-[0px] w-[16px] h-[3px] shadow-[0_1px_3px_black] top-[42px] transition-colors duration-500"
           :class="isTopHalfIlluminated ? 'bg-[#00ffff] shadow-[0_0_10px_cyan] z-10' : 'bg-[#d4af37] z-0'"></div>

      <!-- Horizontal Inlet Line (From Spine to Node) -->
      <div class="absolute left-[16px] right-0 h-[3px] shadow-[0_1px_3px_black] top-[42px] transition-colors duration-500"
           :class="isInletIlluminated ? 'bg-[#00ffff] shadow-[0_0_10px_cyan] z-10' : 'bg-[#d4af37] z-0'"></div>
    </div>

    <!-- Content (Row + Children) -->
    <div class="flex items-start">
      
      <!-- Node Row -->
      <!-- Min-height ensures the node can grow down with text, while pt-[7px] anchors the box so the lines perfectly hit the icon center -->
      <div class="flex items-start min-h-[80px] relative pt-[7px] pb-[7px]" :class="{ 'pl-4': isRoot }">
         
         <!-- NODE WRAPPER -->
         <div class="relative w-[350px] shrink-0 z-10 transition-all duration-500 cursor-default"
              @click="onNodeClick"
              :class="{ 'opacity-30 grayscale': isPathIlluminated && !isNodeInPath }">
           
           <!-- NODE BOX -->
           <div 
             class="flex items-start p-2 bg-[#000a2b] border-[3px] rounded transition-all duration-300 shadow-[0_0_8px_black] w-full h-[110px] overflow-hidden"
             :class="[
               isNodeInPath
                 ? 'border-cyan-300 shadow-[0_0_12px_cyan]'
                 : isUnlocked 
                   ? 'border-yellow-600 hover:border-yellow-300' 
                   : 'border-slate-800 opacity-60'
             ]"
           >
             <DigimonIcon 
               :digimon-name="node.name" 
               class="w-12 h-12 transition-all duration-300 shrink-0"
               :class="{ 
                 'grayscale brightness-50': !isUnlocked && !isNodeInPath,
                 'brightness-125 scale-110 drop-shadow-[0_0_8px_cyan]': isNodeInPath
               }"
             />
             
             <!-- Text Info Container -->
             <div class="ml-3 flex-1 flex flex-col pt-0.5">
               <span class="font-bold text-[0.8rem] tracking-wider truncate border-b pb-1" :class="isUnlocked ? 'text-[#ffcc00] border-yellow-900/50' : 'text-slate-400 border-slate-700/50'">{{ node.name }}</span>
               
               <div class="h-[18px] flex items-center mt-1">
                 <span v-if="!isUnlocked" class="text-[0.6rem] uppercase tracking-widest text-[#ff3333] border border-[#ff3333] px-1 rounded-sm bg-red-900/10 shadow-[0_0_4px_rgba(255,0,0,0.5)]">Locked</span>
               </div>
               
               <ul class="text-[0.65rem] text-gray-300 space-y-0.5 mt-0.5" v-if="node.requirements.length > 0">
                 <li v-for="(req, idx) in node.requirements" :key="idx" class="flex items-center gap-1.5">
                   <span class="inline-block w-1 h-1 rounded-full bg-slate-500 shrink-0"></span>
                   <span v-if="req.Type === 'DigimonLevel'" class="truncate">Lv. {{ req.Value }} (Rookie)</span>
                   <span v-else-if="req.Type === 'DigievolutionLevel'" class="truncate">Lv. {{ req.Value }} ({{ req.Digievolution }})</span>
                   <span v-else-if="req.Type === 'Attribute'" class="truncate">{{ req.Attribute }} &ge; {{ req.Value }}</span>
                 </li>
               </ul>
               <p v-else class="text-[0.65rem] text-gray-500 italic mt-0.5">Base Evolution</p>
             </div>
           </div>
         </div>
         
         <!-- Outlet Line (Positioned perfectly at y=42px relative to the top of the Row, requiring mt-35px within pt-7px) -->
         <div v-if="hasChildren" class="h-full flex items-start">
           <div class="w-[32px] h-[3px] shadow-[0_1px_3px_black] shrink-0 mt-[35px] transition-colors duration-500"
                :class="isOutletIlluminated ? 'bg-[#00ffff] shadow-[0_0_10px_cyan] z-10' : 'bg-[#d4af37] z-0'"></div>
         </div>
      </div>

      <!-- Children Container -->
      <div v-if="hasChildren" class="flex flex-col">
         <DigievolutionTreeNode 
          v-for="(child, index) in node.children" 
          :key="child.id" 
          :digimon="digimon"
          :node="child"
          :is-root="false"
          :is-first-child="index === 0"
          :is-last-child="index === node.children.length - 1"
          :active-path="activePath"
          :is-path-below="activeIndex > index"
          @node-click="(name) => emit('node-click', name)"
        />
      </div>

    </div>
  </div>
</template>

<script lang="ts">
export default {
  name: 'DigievolutionTreeNode'
}
</script>
