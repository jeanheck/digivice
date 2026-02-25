<script setup lang="ts">
import { ref, watch } from 'vue'
import Modal from '../ui/Modal.vue'
import type { Digimon } from '../../types/backend'
import { EvolutionGraph, type GraphNode } from '../../logic/EvolutionGraph'
import DigievolutionTreeNode from './DigievolutionTreeNode.vue'

const props = defineProps<{
  isOpen: boolean
  digimon: Digimon
}>()

const emit = defineEmits<{
  (e: 'close'): void
}>()

const treeRoot = ref<GraphNode | null>(null)

watch(() => props.isOpen, (isOpen) => {
  if (isOpen && props.digimon) {
    // Build the tree only when modal opens to save processing
    treeRoot.value = EvolutionGraph.buildTree(props.digimon.basicInfo.name)
  }
})
</script>

<template>
  <Modal 
    :is-open="isOpen" 
    :title="`${digimon.basicInfo.name} Digievolution Tree`"
    @close="emit('close')"
  >
    <div class="flex min-h-[400px] w-full bg-[url('/src/assets/bg-pattern.svg')] lg:p-8 p-2 overflow-x-auto custom-scrollbar items-center">
      <div v-if="treeRoot" class="flex items-center min-w-max">
         <DigievolutionTreeNode :digimon="digimon" :node="treeRoot" :is-root="true" />
      </div>
      <div v-else class="text-white">Carregando árvore...</div>
    </div>
  </Modal>
</template>

