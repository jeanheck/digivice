<script setup lang="ts">
import { watch, ref, nextTick } from 'vue'
import type { Digimon } from '../../models'
import { EvolutionGraph, type FamilyChain, type EvolutionRequirement } from '../../logic/EvolutionGraph'
import DigievolutionTreeNode from './DigievolutionTreeNode.vue'

const props = defineProps<{
  rookieName: string
  digimon: Digimon
  selectedNodeName?: string
}>()

const emit = defineEmits<{
  (e: 'select-node', name: string): void
}>()

const families = ref<FamilyChain[]>([])

const getRequirements = (name: string): EvolutionRequirement[] => {
    const allEvos = EvolutionGraph.getAllEvolutions(props.rookieName)
    const evo = allEvos.find(e => e.name === name)
    return evo?.requirements || []
}

const getRookieFamily = (): string => {
    return props.rookieName
}

const initFamilies = () => {
    families.value = EvolutionGraph.buildFamilyChains(getRookieFamily())
}

watch(() => props.rookieName, () => initFamilies(), { immediate: true })

// Auto-scroll to selected node when selection changes
watch(() => props.selectedNodeName, (name) => {
    if (!name) return
    nextTick(() => {
        const el = document.querySelector(`[data-node-name="${name}"]`) as HTMLElement | null
        el?.scrollIntoView({ behavior: 'smooth', block: 'nearest', inline: 'nearest' })
    })
})

const hasBranching = (chain: FamilyChain): boolean => {
    return chain.sharedPrefix.length > 0 && chain.branches.length > 1
}
</script>

<template>
  <div class="family-tree-container">
    
    <template v-for="(chain, chainIdx) in families" :key="chain.family">

      <!-- NON-BRANCHING: simple horizontal row -->
      <div v-if="!hasBranching(chain)" class="family-row">
        <template v-for="(branch, bIdx) in chain.branches" :key="bIdx">
          <div class="branch-row">
            <template v-for="(name, nodeIdx) in branch.names" :key="name">
              <DigievolutionTreeNode
                :node="{ name, requirements: getRequirements(name) }"
                :digimon="digimon"
                :is-selected="selectedNodeName === name"
                class="shrink-0"
                :data-node-name="name"
                @select="emit('select-node', name)"
              />
              <div v-if="nodeIdx < branch.names.length - 1" class="connector">
                <div class="connector-line">
                  <div class="connector-arrow"></div>
                </div>
              </div>
            </template>
          </div>
        </template>
      </div>

      <!-- BRANCHING: shared prefix then forking rows -->
      <div v-else class="family-row branching">
        <div class="branch-layout">
          <!-- Shared prefix nodes (e.g., Greymon) -->
          <div class="shared-prefix">
            <template v-for="(name, nodeIdx) in chain.sharedPrefix" :key="name">
              <DigievolutionTreeNode
                :node="{ name, requirements: getRequirements(name) }"
                :digimon="digimon"
                :is-selected="selectedNodeName === name"
                class="shrink-0"
                :data-node-name="name"
                @select="emit('select-node', name)"
              />
              <div v-if="nodeIdx < chain.sharedPrefix.length - 1" class="connector">
                <div class="connector-line">
                  <div class="connector-arrow"></div>
                </div>
              </div>
            </template>
          </div>

          <!-- Fork connector (arrow from prefix to branches) -->
          <div class="fork-connector">
            <div class="fork-line-top"></div>
            <div class="fork-line-bottom"></div>
          </div>

          <!-- Branch suffixes stacked vertically -->
          <div class="branch-suffixes">
            <div v-for="(branch, bIdx) in chain.branches" :key="bIdx" class="branch-row">
              <template v-for="(name, nodeIdx) in branch.names" :key="name">
                <DigievolutionTreeNode
                  :node="{ name, requirements: getRequirements(name) }"
                  :digimon="digimon"
                  :is-selected="selectedNodeName === name"
                  class="shrink-0"
                  :data-node-name="name"
                  @select="emit('select-node', name)"
                />
                <div v-if="nodeIdx < branch.names.length - 1" class="connector">
                  <div class="connector-line">
                    <div class="connector-arrow"></div>
                  </div>
                </div>
              </template>
            </div>
          </div>
        </div>
      </div>

      <!-- HR Separator between families -->
      <div v-if="chainIdx < families.length - 1" class="family-separator"></div>
    </template>

    <div v-if="families.length === 0" class="empty-state">
      No evolution data available
    </div>
  </div>
</template>

<style scoped>
.family-tree-container {
  width: 100%;
  height: 100%;
  overflow: auto;
  background: #001122;
  padding: 16px;
}

.family-tree-container::-webkit-scrollbar { width: 4px; height: 4px; }
.family-tree-container::-webkit-scrollbar-track { background: transparent; }
.family-tree-container::-webkit-scrollbar-thumb { background: rgba(0, 255, 255, 0.1); border-radius: 2px; }
.family-tree-container::-webkit-scrollbar-thumb:hover { background: rgba(0, 255, 255, 0.2); }

.family-row {
  margin-bottom: 8px;
  overflow-x: auto;
}

.branch-row {
  display: flex;
  align-items: center;
  padding: 10px 8px;
}

/* --- Connectors --- */
.connector {
  flex-shrink: 0;
  display: flex;
  align-items: center;
  margin: 0 4px;
}

.connector-line {
  width: 32px;
  height: 2px;
  background: linear-gradient(to right, rgba(0, 255, 255, 0.25), rgba(0, 255, 255, 0.45));
  position: relative;
}

.connector-arrow {
  position: absolute;
  right: 0;
  top: 50%;
  transform: translateY(-50%);
  width: 0;
  height: 0;
  border-left: 5px solid rgba(0, 255, 255, 0.45);
  border-top: 3px solid transparent;
  border-bottom: 3px solid transparent;
}

/* --- Branching layout --- */
.branch-layout {
  display: flex;
  align-items: center;
}

.shared-prefix {
  display: flex;
  align-items: center;
  padding: 10px 0px 10px 8px;
}

.fork-connector {
  flex-shrink: 0;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  width: 32px;
  position: relative;
  align-self: stretch;
  margin: 0 4px;
}

.fork-line-top,
.fork-line-bottom {
  position: absolute;
  left: 0;
  width: 100%;
  height: 2px;
  background: linear-gradient(to right, rgba(0, 255, 255, 0.25), rgba(0, 255, 255, 0.45));
}

.fork-line-top {
  top: calc(25% + 5px);
}

.fork-line-top::after {
  content: '';
  position: absolute;
  right: 0;
  top: 50%;
  transform: translateY(-50%);
  width: 0;
  height: 0;
  border-left: 5px solid rgba(0, 255, 255, 0.45);
  border-top: 3px solid transparent;
  border-bottom: 3px solid transparent;
}

.fork-line-bottom {
  bottom: calc(25% + 5px);
}

.fork-line-bottom::after {
  content: '';
  position: absolute;
  right: 0;
  top: 50%;
  transform: translateY(-50%);
  width: 0;
  height: 0;
  border-left: 5px solid rgba(0, 255, 255, 0.45);
  border-top: 3px solid transparent;
  border-bottom: 3px solid transparent;
}

/* Vertical stem connecting the two fork lines */
.fork-connector::before {
  content: '';
  position: absolute;
  left: 0;
  top: calc(25% + 5px);
  bottom: calc(25% + 5px);
  width: 2px;
  background: rgba(0, 255, 255, 0.3);
}

.branch-suffixes {
  display: flex;
  flex-direction: column;
}

.branch-suffixes .branch-row {
  padding: 10px 8px 10px 0px;
}

/* --- Separator --- */
.family-separator {
  border-bottom: 1px solid rgba(0, 102, 204, 0.3);
  margin: 12px 0;
}

/* --- Empty state --- */
.empty-state {
  display: flex;
  align-items: center;
  justify-content: center;
  height: 100%;
  color: rgba(0, 255, 255, 0.4);
  font-family: 'Cyber', monospace;
  font-size: 14px;
}
</style>
