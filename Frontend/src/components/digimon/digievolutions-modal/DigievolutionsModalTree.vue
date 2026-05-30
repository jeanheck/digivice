<script setup lang="ts">
import { computed, watch, nextTick } from "vue";
import type { Digimon } from "@/models/digimon";
import { DigievolutionsModalTreePresenter } from "@/presenters/digievolutions-modal-tree.presenter";
import type { DigievolutionTreeFamilyViewModel } from "@/viewmodels/digievolution/digievolution-tree-family.viewmodel";
import DigievolutionsTreeSimpleFamily from "./DigievolutionsTreeSimpleFamily.vue";
import DigievolutionsTreeForkFamily from "./DigievolutionsTreeForkFamily.vue";

const props = defineProps<{
  digimonName: string;
  digimon: Digimon;
  digimonId: number;
  selectedDigievolutionName?: string;
}>();

const emit = defineEmits<{
  (e: "select-digievolution", name: string): void;
}>();

const treeViewModel = DigievolutionsModalTreePresenter.getDigievolutionsTree(props.digimonId, props.digimonName);

watch(() => props.selectedDigievolutionName, (name) => {
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
  return treeViewModel.families;
});
</script>

<template>
  <div class="family-tree-container custom-scroll">
    <template v-for="(family, familyIndex) in families" :key="family.key">
      <DigievolutionsTreeSimpleFamily
        v-if="!hasBranching(family)"
        :branchs="family.branchs"
        :digimon="digimon"
        :digimon-name="digimonName"
        :selected-digievolution-name="selectedDigievolutionName"
        @select-digievolution="emit('select-digievolution', $event)"
      />

      <DigievolutionsTreeForkFamily
        v-else
        :nodes-before-fork="family.nodesBeforeFork"
        :branchs="family.branchs"
        :digimon="digimon"
        :digimon-name="digimonName"
        :selected-digievolution-name="selectedDigievolutionName"
        @select-digievolution="emit('select-digievolution', $event)"
      />

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
  font-family: "Cyber", monospace;
  font-size: 14px;
}
</style>
