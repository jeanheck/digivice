<script setup lang="ts">
import type { Digimon } from "@/models/digimon";
import type { DigievolutionTreeFamilyNodeViewModel } from "@/viewmodels/digievolution/digievolution-tree-family-node.viewmodel";
import DigievolutionsTreeFamilyBranch from "./DigievolutionsTreeFamilyBranch.vue";

defineProps<{
  nodesBeforeFork: DigievolutionTreeFamilyNodeViewModel[];
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
  <div class="family-row branching">
    <div class="branch-layout">
      <div class="shared-prefix">
        <DigievolutionsTreeFamilyBranch
          :nodes="nodesBeforeFork"
          :digimon="digimon"
          :digimon-name="digimonName"
          :selected-digievolution-name="selectedDigievolutionName"
          @select-digievolution="emit('select-digievolution', $event)"
        />
      </div>

      <div class="fork-connector">
        <div class="fork-line-top"></div>
        <div class="fork-line-bottom"></div>
      </div>

      <div class="branch-suffixes">
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
    </div>
  </div>
</template>

<style scoped>
.family-row {
  margin-bottom: 8px;
  overflow-x: auto;
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
  content: "";
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
  content: "";
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
  content: "";
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
  display: flex;
  align-items: center;
  padding: 10px 8px 10px 0px;
}
</style>
