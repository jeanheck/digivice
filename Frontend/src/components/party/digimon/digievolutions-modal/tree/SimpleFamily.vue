<script setup lang="ts">
import type { Digimon } from "@/models/party/digimon/digimon.ts";
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
  <div class="family-row custom-scroll">
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
