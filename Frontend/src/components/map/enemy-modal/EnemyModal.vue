<script setup lang="ts">
import { computed, ref, watch } from "vue";
import Modal from "@/components/modal/Modal.vue";
import Tooltip from "@/components/tooltip/Tooltip.vue";
import EnemyProfile from "@/components/map/enemy-modal/EnemyProfile.vue";
import EnemyAttributes from "@/components/map/enemy-modal/EnemyAttributes.vue";
import EnemyElements from "@/components/map/enemy-modal/EnemyElements.vue";
import EnemyConditions from "@/components/map/enemy-modal/EnemyConditions.vue";
import { useLocalization } from "@/composables/useLocalization";
import { useTooltipPosition } from "@/composables/use-tooltip-position";
import { ImageCatalog } from "@/catalogs/image.catalog.ts";
import { EnemyModalPresenter } from "@/presenters/map/enemy-modal.presenter.ts";

const props = defineProps<{
  isOpen: boolean;
  enemyId: string | null;
}>();

const emit = defineEmits<{
  (e: "close"): void;
}>();

const { t } = useLocalization();

const isModalOpen = computed(() => {
  return props.isOpen && props.enemyId !== null;
});

const handleClose = () => {
  emit("close");
};

const enemy = computed(() => {
  return EnemyModalPresenter.getEnemyById(props.enemyId!);
});

const tooltipPlacement = "below" as const;
const tooltipPosition = useTooltipPosition(250);
const { show: tooltipShow, x: tooltipX, y: tooltipY, showAt, move, hide } = tooltipPosition;
const tooltipTitle = ref("");

const showEnemyStatKeyTooltip = (event: MouseEvent, statKey: string) => {
  tooltipTitle.value = t(`stat.${statKey}`);
  showAt(event, { maxWidth: 250, placement: tooltipPlacement });
};

const showEnemyConditionTooltip = (event: MouseEvent, tooltipKey: string) => {
  tooltipTitle.value = t(tooltipKey);
  showAt(event, { maxWidth: 250, placement: tooltipPlacement });
};

const hideEnemyStatTooltip = () => {
  hide();
};

const moveEnemyStatTooltip = (event: MouseEvent) => {
  move(event, tooltipPlacement);
};

watch(() => props.isOpen, (open) => {
  if (!open) {
    hide();
  }
});

const enemyImageUrl = computed(() => {
  if (!props.enemyId) {
    return null;
  }
  return ImageCatalog.getEnemyIconUrl(enemy.value.name);
});
</script>

<template>
  <Modal
    :is-open="isModalOpen"
    max-width="max-w-250"
    @close="handleClose"
  >
    <template #header>
      <h2 class="text-white font-bold tracking-widest drop-shadow flex items-center gap-2">
        {{ enemy.name || $t("enemy.detailsTitle") }}
      </h2>
    </template>

    <div class="p-4 flex flex-col sm:flex-row gap-4 max-h-[70vh] overflow-y-auto custom-scroll">
      <EnemyProfile :enemy="enemy" :enemy-image-url="enemyImageUrl" />

      <div class="flex-1">
        <div class="bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-row justify-around gap-6 h-full items-start">
          <EnemyAttributes
            :attributes="enemy.attributes"
            @show-stat-key-tooltip="showEnemyStatKeyTooltip"
            @move-stat-tooltip="moveEnemyStatTooltip"
            @hide-stat-tooltip="hideEnemyStatTooltip"
          />
          <EnemyElements
            :elements="enemy.elements"
            @show-stat-key-tooltip="showEnemyStatKeyTooltip"
            @move-stat-tooltip="moveEnemyStatTooltip"
            @hide-stat-tooltip="hideEnemyStatTooltip"
          />
          <EnemyConditions
            :conditions="enemy.conditions"
            @show-condition-tooltip="showEnemyConditionTooltip"
            @move-stat-tooltip="moveEnemyStatTooltip"
            @hide-stat-tooltip="hideEnemyStatTooltip"
          />
        </div>
      </div>
    </div>
  </Modal>

  <Tooltip
    :show="tooltipShow"
    :x="tooltipX"
    :y="tooltipY"
    :title="tooltipTitle"
    :max-width="150"
    placement="below"
  />
</template>
