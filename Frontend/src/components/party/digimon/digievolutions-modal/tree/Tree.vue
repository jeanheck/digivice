<script setup lang="ts">
import { computed, watch, nextTick, ref } from "vue";
import type { Digimon } from "@/models/digimon";
import { TreePresenter } from "@/presenters/tree.presenter.ts";
import type { FamilyViewModel } from "@/viewmodels/digievolution/family.viewmodel.ts";
import SimpleFamily from "./SimpleFamily.vue";
import ForkFamily from "./ForkFamily.vue";

const props = defineProps<{
  digimonName: string;
  digimon: Digimon;
  digimonId: number;
  selectedDigievolutionId?: number;
}>();

const emit = defineEmits<{
  (e: "select-digievolution-id", digievolutionId: number): void;
}>();

const familyTreeContainer = ref<HTMLElement | null>(null);

const treeViewModel = TreePresenter.getDigievolutionsTree(props.digimonId);

const scrollSelectedNodeIntoView = (digievolutionId: number) => {
  const container = familyTreeContainer.value;
  if (!container) {
    return;
  }

  const nodeElement = container.querySelector(`[data-node-id="${digievolutionId}"]`) as HTMLElement | null;
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

watch(() => props.selectedDigievolutionId, (digievolutionId) => {
  if (digievolutionId === undefined) {
    return;
  }

  nextTick(() => {
    scrollSelectedNodeIntoView(digievolutionId);
  });
});

const hasBranching = (family: FamilyViewModel): boolean => {
  return family.nodesBeforeFork.length > 0 && family.branchs.length > 1;
};

const families = computed(() => {
  return treeViewModel.families;
});
</script>

<template>
  <div ref="familyTreeContainer" class="family-tree-container custom-scroll">
    <template v-for="(family, familyIndex) in families" :key="family.key">
      <SimpleFamily
        v-if="!hasBranching(family)"
        :branchs="family.branchs"
        :digimon="digimon"
        :digimon-name="digimonName"
        :selected-digievolution-id="selectedDigievolutionId"
        @select-digievolution-id="emit('select-digievolution-id', $event)"
      />

      <ForkFamily
        v-else
        :nodes-before-fork="family.nodesBeforeFork"
        :branchs="family.branchs"
        :digimon="digimon"
        :digimon-name="digimonName"
        :selected-digievolution-id="selectedDigievolutionId"
        @select-digievolution-id="emit('select-digievolution-id', $event)"
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
  font-size: 14px;
}
</style>
