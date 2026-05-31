<script setup lang="ts">
import type { Digimon } from "@/models/digimon";
import type { DigievolutionTreeFamilyNodeViewModel } from "@/viewmodels/digievolution/digievolution-tree-family-node.viewmodel";
import DigievolutionTreeNode from "./DigievolutionTreeNode.vue";

defineProps<{
  nodes: DigievolutionTreeFamilyNodeViewModel[];
  digimon: Digimon;
  digimonName: string;
  selectedDigievolutionName?: string;
}>();

const emit = defineEmits<{
  (e: "select-digievolution", name: string): void;
}>();
</script>

<template>
  <template v-for="(node, nodeIndex) in nodes" :key="node.id">
    <DigievolutionTreeNode
      :node="node"
      :digimon="digimon"
      :digimon-name="digimonName"
      :is-selected="selectedDigievolutionName === node.name"
      class="shrink-0"
      :data-node-name="node.name"
      @select="emit('select-digievolution', node.name)"
    />
    <div v-if="nodeIndex < nodes.length - 1" class="connector">
      <div class="connector-line">
        <div class="connector-arrow"></div>
      </div>
    </div>
  </template>
</template>

<style scoped>
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
</style>
