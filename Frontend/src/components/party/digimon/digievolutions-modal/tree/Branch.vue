<script setup lang="ts">
import type { Digimon } from "@/models/party/digimon/digimon.ts";
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
