<script setup lang="ts">
import type { Digimon } from "@/models/party/digimon/digimon.ts";
import type { NodeViewModel } from "@/viewmodels/digievolution/node.viewmodel.ts";
import Branch from "./Branch.vue";

defineProps<{
  nodesBeforeFork: NodeViewModel[];
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
  <div class="family-row branching custom-scroll">
    <div class="branch-layout">
      <div class="shared-prefix">
        <Branch
          :nodes="nodesBeforeFork"
          :digimon="digimon"
          :digimon-name="digimonName"
          :selected-digievolution-id="selectedDigievolutionId"
          @select-digievolution-id="emit('select-digievolution-id', $event)"
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
          <Branch
            :nodes="branch"
            :digimon="digimon"
            :digimon-name="digimonName"
            :selected-digievolution-id="selectedDigievolutionId"
            @select-digievolution-id="emit('select-digievolution-id', $event)"
          />
        </div>
      </div>
    </div>
  </div>
</template>
