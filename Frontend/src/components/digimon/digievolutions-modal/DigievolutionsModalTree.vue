<script setup lang="ts">
import { computed, watch, ref, nextTick } from "vue";
import type { Digimon } from "@/models/digimon";
import { DigievolutionsModalTreePresenter } from "@/presenters/digievolutions-modal-tree.presenter";
import type { DigievolutionTreeFamilyViewModel } from "@/viewmodels/digievolution/digievolution-tree-family.viewmodel";
import DigievolutionTreeNode from "./DigievolutionTreeNode.vue";

const props = defineProps<{
  digimonName: string;
  digimon: Digimon;
  digimonId: number;
  selectedNodeName?: string;
}>();

const emit = defineEmits<{
  (e: "select-node", name: string): void;
}>();

const treeViewModel = ref(DigievolutionsModalTreePresenter.getDigievolutionsTreeViewModel(props.digimonId, props.digimonName));

const initTreeViewModel = () => {
  treeViewModel.value = DigievolutionsModalTreePresenter.getDigievolutionsTreeViewModel(props.digimonId, props.digimonName);
};

watch(() => props.digimonName, () => initTreeViewModel(), { immediate: true });

watch(() => props.digimonId, () => initTreeViewModel());

watch(() => props.selectedNodeName, (name) => {
  if (!name) {
    return;
  }

  nextTick(() => {
    const element = document.querySelector(`[data-node-name="${name}"]`) as HTMLElement | null;
    element?.scrollIntoView({ behavior: "smooth", block: "nearest", inline: "nearest" });
  });
});

const hasBranching = (family: DigievolutionTreeFamilyViewModel): boolean => {
  return family.nodesBeforeFork.length > 0 && family.branchs.length > 1;
};

const families = computed(() => {
  return treeViewModel.value.families;
});
</script>

<template>
  <div class="family-tree-container">
    <template v-for="(family, familyIndex) in families" :key="family.familyKey">

      <div v-if="!hasBranching(family)" class="family-row">
        <template v-for="(branch, branchIndex) in family.branchs" :key="branchIndex">
          <div class="branch-row">
            <template v-for="(node, nodeIndex) in branch" :key="node.name">
              <DigievolutionTreeNode
                :node="node"
                :digimon="digimon"
                :digimon-name="digimonName"
                :is-selected="selectedNodeName === node.name"
                class="shrink-0"
                :data-node-name="node.name"
                @select="emit('select-node', node.name)"
              />
              <div v-if="nodeIndex < branch.length - 1" class="connector">
                <div class="connector-line">
                  <div class="connector-arrow"></div>
                </div>
              </div>
            </template>
          </div>
        </template>
      </div>

      <div v-else class="family-row branching">
        <div class="branch-layout">
          <div class="shared-prefix">
            <template v-for="(node, nodeIndex) in family.nodesBeforeFork" :key="node.name">
              <DigievolutionTreeNode
                :node="node"
                :digimon="digimon"
                :digimon-name="digimonName"
                :is-selected="selectedNodeName === node.name"
                class="shrink-0"
                :data-node-name="node.name"
                @select="emit('select-node', node.name)"
              />
              <div v-if="nodeIndex < family.nodesBeforeFork.length - 1" class="connector">
                <div class="connector-line">
                  <div class="connector-arrow"></div>
                </div>
              </div>
            </template>
          </div>

          <div class="fork-connector">
            <div class="fork-line-top"></div>
            <div class="fork-line-bottom"></div>
          </div>

          <div class="branch-suffixes">
            <div v-for="(branch, branchIndex) in family.branchs" :key="branchIndex" class="branch-row">
              <template v-for="(node, nodeIndex) in branch" :key="node.name">
                <DigievolutionTreeNode
                  :node="node"
                  :digimon="digimon"
                  :digimon-name="digimonName"
                  :is-selected="selectedNodeName === node.name"
                  :data-node-name="node.name"
                  @select="emit('select-node', node.name)"
                />
                <div v-if="nodeIndex < branch.length - 1" class="connector">
                  <div class="connector-line">
                    <div class="connector-arrow"></div>
                  </div>
                </div>
              </template>
            </div>
          </div>
        </div>
      </div>

      <div v-if="familyIndex < families.length - 1" class="family-separator"></div>
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

.family-separator {
  border-bottom: 1px solid rgba(0, 102, 204, 0.3);
  margin: 12px 0;
}

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
