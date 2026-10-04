<script setup lang="ts">
import { computed, watch, nextTick, ref } from "vue";
import { useI18n } from "vue-i18n";
import type { Digimon } from "@/models";
import { TreePresenter } from "@/presenters/tree/tree.presenter";
import type { FamilyViewModel } from "@/viewmodels/digievolution/family.viewmodel";
import { usePreferencesStore } from "@/stores/use-preferences-store";
import { useTooltipPosition } from "@/composables/use-tooltip-position";
import Tooltip from "@/components/tooltip/Tooltip.vue";
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

const { t } = useI18n();
const preferencesStore = usePreferencesStore();

const treeViewModel = computed(() => {
  return TreePresenter.getDigievolutionsTree(
    props.digimonId,
    preferencesStore.getPinnedFamilyKey(props.digimonId),
  );
});

const pinTooltipPlacement = "below" as const;
const pinTooltipMaxWidth = 260;
const {
  show: pinTooltipShow,
  x: pinTooltipX,
  y: pinTooltipY,
  maxWidth: pinTooltipMaxWidthValue,
  showAt: showPinTooltipAt,
  move: movePinTooltipTo,
  hide: hidePinTooltip,
} = useTooltipPosition(pinTooltipMaxWidth);
const pinTooltipTitle = ref("");

const getPinTooltipKey = (family: FamilyViewModel): string => {
  return family.isPinned ? "digievolution.unpinFamily" : "digievolution.pinFamily";
};

const showPinTooltip = (event: MouseEvent, family: FamilyViewModel) => {
  pinTooltipTitle.value = t(getPinTooltipKey(family));
  showPinTooltipAt(event, { maxWidth: pinTooltipMaxWidth, placement: pinTooltipPlacement });
};

const movePinTooltip = (event: MouseEvent) => {
  movePinTooltipTo(event, pinTooltipPlacement);
};

const togglePinnedFamily = (family: FamilyViewModel) => {
  hidePinTooltip();
  preferencesStore.togglePinnedFamily(props.digimonId, family.key);
};

const scrollSelectedNodeIntoView = (digievolutionId: number) => {
  const container = familyTreeContainer.value;
  if (!container) {
    return;
  }

  const nodeElement = container.querySelector(
    `[data-node-id="${digievolutionId}"]`,
  ) as HTMLElement | null;
  if (!nodeElement) {
    return;
  }

  const elementTopInContainer = (element: HTMLElement) => {
    return (
      element.getBoundingClientRect().top -
      container.getBoundingClientRect().top +
      container.scrollTop
    );
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

  const familyRow = nodeElement.closest(".family-block");
  if (familyRow) {
    const nextSibling = familyRow.nextElementSibling;
    const familySeparator =
      nextSibling instanceof HTMLElement && nextSibling.classList.contains("family-separator")
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

  targetScrollTop = Math.max(
    0,
    Math.min(targetScrollTop, container.scrollHeight - container.clientHeight),
  );

  if (Math.abs(targetScrollTop - container.scrollTop) < 1) {
    return;
  }

  container.scrollTo({ top: targetScrollTop, behavior: "smooth" });
};

watch(
  () => props.selectedDigievolutionId,
  (digievolutionId) => {
    if (digievolutionId === undefined) {
      return;
    }

    nextTick(() => {
      scrollSelectedNodeIntoView(digievolutionId);
    });
  },
);

const hasBranching = (family: FamilyViewModel): boolean => {
  return family.nodesBeforeFork.length > 0 && family.branchs.length > 1;
};

const families = computed(() => {
  return treeViewModel.value.families;
});
</script>

<template>
  <div ref="familyTreeContainer" class="family-tree-container custom-scroll">
    <template v-for="(family, familyIndex) in families" :key="family.key">
      <div class="family-block flex items-center gap-2">
        <button
          type="button"
          class="pin-icon shrink-0 mb-2 text-sm leading-none cursor-pointer"
          :class="{ 'pin-icon-active': family.isPinned }"
          @click="togglePinnedFamily(family)"
          @mouseenter="showPinTooltip($event, family)"
          @mousemove="movePinTooltip"
          @mouseleave="hidePinTooltip"
        >
          📌
        </button>

        <div class="flex-1 min-w-0">
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
        </div>
      </div>

      <div v-if="familyIndex < families.length - 1" class="family-separator"></div>
    </template>

    <div v-if="families.length === 0" class="empty-state">
      {{ $t("digievolution.noEvolutionData") }}
    </div>

    <Tooltip
      :show="pinTooltipShow"
      :x="pinTooltipX"
      :y="pinTooltipY"
      :title="pinTooltipTitle"
      :max-width="pinTooltipMaxWidthValue"
      placement="below"
    />
  </div>
</template>
