<script setup lang="ts">
import { computed, watch, nextTick, ref } from "vue";
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

const familyTreeContainer = ref<HTMLElement | null>(null);

const treeViewModel = DigievolutionsModalTreePresenter.getDigievolutionsTree(props.digimonId, props.digimonName);

const scrollSelectedNodeIntoView = (name: string) => {
  const container = familyTreeContainer.value;
  if (!container) {
    return;
  }

  const nodeElement = container.querySelector(`[data-node-name="${CSS.escape(name)}"]`) as HTMLElement | null;
  if (!nodeElement) {
    return;
  }

  const elementTopInContainer = (element: HTMLElement) => {
    return element.getBoundingClientRect().top - container.getBoundingClientRect().top + container.scrollTop;
  };

  const nodeTop = elementTopInContainer(nodeElement);
  const nodeBottom = nodeTop + nodeElement.offsetHeight;
  const viewportTop = container.scrollTop;
  const viewportBottom = viewportTop + container.clientHeight;

  let targetScrollTop = viewportTop;

  if (nodeTop < viewportTop) {
    targetScrollTop = nodeTop;
  } else if (nodeBottom > viewportBottom) {
    targetScrollTop = nodeBottom - container.clientHeight;
  }

  const familyRow = nodeElement.closest(".family-row");
  if (familyRow) {
    const nextSibling = familyRow.nextElementSibling;
    const familySeparator = nextSibling instanceof HTMLElement && nextSibling.classList.contains("family-separator")
      ? nextSibling
      : null;

    if (familySeparator) {
      const separatorBottom = elementTopInContainer(familySeparator) + familySeparator.offsetHeight;
      const scrollForSeparator = separatorBottom - container.clientHeight;
      targetScrollTop = Math.max(targetScrollTop, scrollForSeparator);
    } else {
      targetScrollTop = container.scrollHeight - container.clientHeight;
    }
  }

  targetScrollTop = Math.max(0, Math.min(targetScrollTop, container.scrollHeight - container.clientHeight));

  if (Math.abs(targetScrollTop - container.scrollTop) < 1) {
    return;
  }

  container.scrollTo({ top: targetScrollTop, behavior: "smooth" });
};

watch(() => props.selectedDigievolutionName, (name) => {
  if (!name) {
    return;
  }

  nextTick(() => {
    scrollSelectedNodeIntoView(name);
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
  <div ref="familyTreeContainer" class="family-tree-container custom-scroll">
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
      {{ $t("digievolution.noEvolutionData") }}
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
