<script setup lang="ts">
import type { Digimon } from "@/models/digimon";
import type { DigievolutionTreeFamilyNodeViewModel } from "@/viewmodels/digievolution/digievolution-tree-family-node.viewmodel";
import DigievolutionsTreeFamilyBranch from "./DigievolutionsTreeFamilyBranch.vue";

defineProps<{
  branchs: DigievolutionTreeFamilyNodeViewModel[][];
  digimon: Digimon;
  digimonName: string;
  selectedDigievolutionName?: string;
}>();

const emit = defineEmits<{
  (e: "select-digievolution", name: string): void;
}>();
</script>

<template>
  <div class="family-row">
    <div
      v-for="(branch, branchIndex) in branchs"
      :key="branchIndex"
      class="branch-row"
    >
      <DigievolutionsTreeFamilyBranch
        :nodes="branch"
        :digimon="digimon"
        :digimon-name="digimonName"
        :selected-digievolution-name="selectedDigievolutionName"
        @select-digievolution="emit('select-digievolution', $event)"
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
