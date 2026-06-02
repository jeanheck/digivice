<script setup lang="ts">
import type { Digimon } from "@/models/digimon";
import type { NodeViewModel } from "@/viewmodels/digievolution/node.viewmodel.ts";
import Node from "./Node.vue";

defineProps<{
  nodes: NodeViewModel[];
  digimon: Digimon;
  digimonName: string;
  selectedDigievolutionId?: number;
}>();

const emit = defineEmits<{
  (e: "select-digievolution-id", digievolutionId: number): void;
}>();
</script>

<template>
  <template v-for="(node, nodeIndex) in nodes" :key="node.id">
    <Node
      :node="node"
      :digimon="digimon"
      :digimon-name="digimonName"
      :is-selected="selectedDigievolutionId === node.id"
      class="shrink-0"
      :data-node-id="node.id"
      @select="emit('select-digievolution-id', node.id)"
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
