<script setup lang="ts">
import { computed, ref } from 'vue'
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

const isHovered = ref(false)
const tooltipStyle = ref({ top: '0px', left: '0px' })

const showTooltip = (event: MouseEvent) => {
  const el = event.currentTarget as HTMLElement
  const rect = el.getBoundingClientRect()
  
  // Predict tooltip width (approximate 280px max for safe margin)
  const estimatedTooltipWidth = 280
  
  if (rect.right + estimatedTooltipWidth > window.innerWidth) {
    // Show on top-left diagonal if right edge is getting clipped
    tooltipStyle.value = {
      top: `${rect.top - 20}px`,
      left: `${rect.left - estimatedTooltipWidth + 20}px`
    }
  } else {
    // Diagonal Top-Right relative to the hovered node boundaries
    tooltipStyle.value = {
      top: `${rect.top - 20}px`,
      left: `${rect.right - 40}px`
    }
  }
  isHovered.value = true
}

const hideTooltip = () => {
  isHovered.value = false
}
</script>

<template>
  <!-- Outer Wrapper that spans the entire Subtree Height -->
  <div class="flex relative whitespace-nowrap">
    
    <!-- Vertical Spine & Inlet Line Wrapper (Left Side) -->
    <div v-if="!isRoot" class="relative w-[32px] shrink-0">
      
      <!-- Vertical Spine Segment (Top Half) -->
      <div v-if="!isFirstChild"
           class="absolute left-[16px] w-[3px] shadow-[0_1px_3px_black] transition-colors duration-500 top-[-2px] h-[44px]"
           :class="isTopHalfIlluminated ? 'bg-[#00ffff] shadow-[0_0_10px_cyan] z-10' : 'bg-[#d4af37] z-0'">
      </div>

      <!-- Vertical Spine Segment (Bottom Half) -->
      <div v-if="!isLastChild"
           class="absolute left-[16px] w-[3px] shadow-[0_1px_3px_black] transition-colors duration-500 top-[39px] bottom-[-2px]"
           :class="isBottomHalfIlluminated ? 'bg-[#00ffff] shadow-[0_0_10px_cyan] z-10' : 'bg-[#d4af37] z-0'">
      </div>

      <!-- Horizontal Bridge Line (First Child Only, connects Parent Outlet to Spine) -->
      <div v-if="isFirstChild" 
           class="absolute left-[0px] w-[16px] h-[3px] shadow-[0_1px_3px_black] top-[39px] transition-colors duration-500"
           :class="isTopHalfIlluminated ? 'bg-[#00ffff] shadow-[0_0_10px_cyan] z-10' : 'bg-[#d4af37] z-0'"></div>

      <!-- Horizontal Inlet Line (From Spine to Node) -->
      <div class="absolute left-[16px] right-0 h-[3px] shadow-[0_1px_3px_black] top-[39px] transition-colors duration-500"
           :class="isInletIlluminated ? 'bg-[#00ffff] shadow-[0_0_10px_cyan] z-10' : 'bg-[#d4af37] z-0'"></div>
    </div>

    <!-- Content (Row + Children) -->
    <div class="flex items-start">
      
      <!-- Node Row (Fixed height 80px to guarantee alignment without subpixels) -->
      <!-- Only give padding-left to Root so it looks balanced without lines -->
      <div class="flex items-center h-[80px] relative" :class="{ 'pl-4': isRoot }">
         
         <!-- NODE WRAPPER (Handles hover triggers, sizing, and sibling positioning) -->
         <div class="relative w-[300px] shrink-0 z-10 hover:z-[70] transition-all duration-500 cursor-pointer"
              @mouseenter="showTooltip"
              @mouseleave="hideTooltip"
              @click="onNodeClick"
              :class="{ 'opacity-30 grayscale': isPathIlluminated && !isNodeInPath }">
           
           <!-- NODE BOX (Applies opacity if locked without affecting tooltip) -->
           <div 
             class="flex items-center p-1.5 bg-[#000a2b] border-[3px] rounded transition-all duration-300 shadow-[0_0_8px_black] w-full h-full"
             :class="[
               isNodeInPath
                 ? 'border-cyan-300 shadow-[0_0_12px_cyan]'
                 : isUnlocked 
                   ? 'border-yellow-600 hover:border-yellow-300' 
                   : 'border-slate-800 hover:border-slate-500 opacity-60'
             ]"
           >
             <DigimonIcon 
               :digimon-name="node.name" 
               class="w-12 h-12 transition-all duration-300"
               :class="{ 
                 'grayscale brightness-50': !isUnlocked && !isNodeInPath,
                 'brightness-125 scale-110 drop-shadow-[0_0_8px_cyan]': isNodeInPath
               }"
             />
             
             <!-- Node Name -->
             <div class="ml-3 flex-1">
               <span class="font-bold text-[0.7rem] tracking-wider text-white break-words">{{ node.name }}</span>
             </div>
           </div>
           
           <!-- Teleported Tooltip (Breaks out of modal clipping) -->
           <Teleport to="body">
             <div v-if="isHovered" 
                  class="fixed w-max p-2 bg-[#000a2b] border-2 rounded text-xs text-white z-[99999] shadow-[0_0_12px_black] pointer-events-none"
                  :class="isUnlocked ? 'border-[#0077ff]' : 'border-slate-600'"
                  :style="tooltipStyle">
               <p class="font-bold mb-1 border-b pb-1 flex items-center gap-2" :class="isUnlocked ? 'text-[#ffcc00] border-[#0077ff]' : 'text-slate-400 border-slate-700'">
                 {{ node.name }} <span v-if="!isUnlocked" class="text-red-400 ml-1">(Locked)</span>
               </p>
               <ul class="text-[0.7rem] text-gray-300 space-y-0.5" v-if="node.requirements.length > 0">
                 <li v-for="(req, idx) in node.requirements" :key="idx" class="flex items-center gap-1">
                   <span class="inline-block w-1.5 h-1.5 rounded-full bg-slate-500"></span>
                   <span v-if="req.Type === 'DigimonLevel'">Lv. {{ req.Value }} (Rookie)</span>
                   <span v-else-if="req.Type === 'DigievolutionLevel'">Lv. {{ req.Value }} ({{ req.Digievolution }})</span>
                   <span v-else-if="req.Type === 'Attribute'">{{ req.Attribute }} &ge; {{ req.Value }}</span>
                 </li>
               </ul>
               <p v-else class="text-[0.6rem] text-gray-500 italic">Base</p>
             </div>
           </Teleport>
         </div>
         
         <!-- Outlet Line (Positioned perfectly at y=39 within the 80px row) -->
         <div v-if="hasChildren" class="h-full flex items-start">
           <div class="w-[32px] h-[3px] shadow-[0_1px_3px_black] shrink-0 mt-[39px] transition-colors duration-500"
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
