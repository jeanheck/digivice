<script setup lang="ts">
import type { Digimon } from "@/models/digimon";
import type { NodeViewModel } from "@/viewmodels/digievolution/node.viewmodel.ts";
import Branch from "./Branch.vue";

defineProps<{
  branchs: NodeViewModel[][];
  digimon: Digimon;
  digimonName: string;
  selectedDigievolutionId?: number;
}>();

const emit = defineEmits<{
  (e: "select-digievolution-id", digievolutionId: number): void;
}>();
</script>

<template>
  <div class="family-row">
    <div
      v-for="(branch, branchIndex) in branchs"
      :key="branchIndex"
      class="branch-row"
    >
      <Branch
        :nodes="branch"
        :digimon="digimon"
        :digimon-name="digimonName"
        :selected-digievolution-id="selectedDigievolutionId"
        @select-digievolution-id="emit('select-digievolution-id', $event)"
      />
    </div>
  </div>
</template>

<style scoped>
.family-row {
  margin-bottom: 8px;
  overflow-x: auto;
}

.branch-row {
  display: flex;
  align-items: center;
  padding: 10px 8px;
}
</style>
