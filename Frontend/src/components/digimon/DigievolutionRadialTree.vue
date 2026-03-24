<script setup lang="ts">
import { ref, onMounted, computed, watch } from 'vue'
import type { Digimon } from '../../types/backend'
import { EvolutionGraph, type DigievolutionGraph } from '../../logic/EvolutionGraph'
import DigievolutionTreeNode from './DigievolutionTreeNode.vue'

const props = defineProps<{
  rookieName: string
  digimon: Digimon
  selectedNodeName?: string
}>()

const emit = defineEmits<{
  (e: 'select-node', name: string): void
}>()

// --- State ---
const graph = ref<DigievolutionGraph>({ nodes: [], links: [] })
const zoom = ref(1.0)
const panX = ref(0)
const panY = ref(0)
const isDragging = ref(false)
const dragStart = ref({ x: 0, y: 0 })

// --- Initialization ---
const initGraph = () => {
    graph.value = EvolutionGraph.buildGraph(props.rookieName)
    panX.value = 0
    panY.value = 0
}

onMounted(() => {
    initGraph()
})

watch(() => props.rookieName, () => {
    initGraph()
})

// --- Zoom & Pan Logic ---
const handleMouseDown = (e: MouseEvent) => {
    if (e.button !== 0) return
    isDragging.value = true
    dragStart.value = { x: e.clientX - panX.value, y: e.clientY - panY.value }
}

const handleMouseMove = (e: MouseEvent) => {
    if (!isDragging.value) return
    panX.value = e.clientX - dragStart.value.x
    panY.value = e.clientY - dragStart.value.y
}

const handleMouseUp = () => {
    isDragging.value = false
}

const handleWheel = (e: WheelEvent) => {
    e.preventDefault()
    const delta = e.deltaY > 0 ? 0.9 : 1.1
    const newZoom = zoom.value * delta
    zoom.value = Math.max(0.15, Math.min(2.5, newZoom))
}

const containerStyle = computed(() => ({
    transform: `translate(${panX.value}px, ${panY.value}px) scale(${zoom.value})`,
    cursor: isDragging.value ? 'grabbing' : 'grab'
}))

// --- Link Calculations (disabled for now) ---
// const NODE_W = 176
// const NODE_H = 80


</script>

<template>
  <div 
    class="relative w-full h-full overflow-hidden bg-[#040510] interface-grid select-none"
    @mousedown="handleMouseDown"
    @mousemove="handleMouseMove"
    @mouseup="handleMouseUp"
    @mouseleave="handleMouseUp"
    @wheel="handleWheel"
  >
    <!-- The World Container -->
    <div 
        class="absolute left-1/2 top-1/2 origin-center"
        :style="containerStyle"
    >
        <!-- Nodes Layer -->
        <DigievolutionTreeNode
            v-for="node in graph.nodes"
            :key="node.id"
            :node="node"
            :digimon="digimon"
            :is-selected="selectedNodeName === node.name"
            class="pointer-events-auto"
            :style="{ position: 'absolute', left: `${node.x}px`, top: `${node.y}px`, transform: 'translate(-50%, -50%)' }"
            @select="emit('select-node', node.name)"
        />
    </div>

    <!-- UI Overlay (Controls) -->
    <div class="absolute bottom-4 left-4 flex gap-2 z-20">
        <div class="px-2 py-1 bg-black/60 border border-cyan-900/50 rounded text-[10px] text-cyan-400 font-cyber">
            ZOOM: {{ Math.round(zoom * 100) }}%
        </div>
        <button @click="zoom = 1.0; panX = 0; panY = 0" class="px-2 py-1 bg-cyan-900/40 border border-cyan-500/50 rounded text-[10px] text-white font-cyber hover:bg-cyan-500/20 transition-colors">
            RESET VIEW
        </button>
    </div>
  </div>
</template>

<style scoped>
.interface-grid {
    background-image: 
        linear-gradient(rgba(0, 255, 255, 0.04) 1px, transparent 1px),
        linear-gradient(90deg, rgba(0, 255, 255, 0.04) 1px, transparent 1px);
    background-size: 60px 60px;
}
</style>
