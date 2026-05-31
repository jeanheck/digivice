<script setup lang="ts">
import { computed, ref, watch } from "vue";
import Modal from "@/components/modal/Modal.vue";
import Tooltip from "@/components/tooltip/Tooltip.vue";
import EnemyProfile from "@/components/map/enemy-modal/EnemyModalProfile.vue";
import EnemyAttributes from "@/components/map/enemy-modal/EnemyModalAttributes.vue";
import EnemyElements from "@/components/map/enemy-modal/EnemyModalElements.vue";
import EnemyResistances from "@/components/map/enemy-modal/EnemyModalResistances.vue";
import { useLocalization } from "@/composables/useLocalization";
import { useTooltipPosition } from "@/composables/use-tooltip-position";
import { STAT_ICONS } from "@/constants/stat-icons";
import { ImageCatalog } from "@/catalogs/image.catalog.ts";
import { EnemyModalPresenter } from "@/presenters/map/enemy-modal.presenter.ts";
import { Stat } from "@/models/stat";

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

const attributesList = computed(() => {
  if (!props.enemyId) {
    return [];
  }
  return [
    { label: t("stat.strength"), val: enemy.value.strength, icon: STAT_ICONS[Stat.strength] },
    { label: t("stat.defense"), val: enemy.value.defense, icon: STAT_ICONS[Stat.defense] },
    { label: t("stat.spirit"), val: enemy.value.spirit, icon: STAT_ICONS[Stat.spirit] },
    { label: t("stat.wisdom"), val: enemy.value.wisdom, icon: STAT_ICONS[Stat.wisdom] },
    { label: t("stat.speed"), val: enemy.value.speed, icon: STAT_ICONS[Stat.speed] },
  ];
});

const elemTolsList = computed(() => {
  if (!props.enemyId) {
    return [];
  }
  return [
    { label: t("stat.fire"), val: enemy.value.fire, icon: STAT_ICONS[Stat.fire] },
    { label: t("stat.water"), val: enemy.value.water, icon: STAT_ICONS[Stat.water] },
    { label: t("stat.ice"), val: enemy.value.ice, icon: STAT_ICONS[Stat.ice] },
    { label: t("stat.wind"), val: enemy.value.wind, icon: STAT_ICONS[Stat.wind] },
    { label: t("stat.thunder"), val: enemy.value.thunder, icon: STAT_ICONS[Stat.thunder] },
    { label: t("stat.machine"), val: enemy.value.machine, icon: STAT_ICONS[Stat.machine] },
    { label: t("stat.dark"), val: enemy.value.dark, icon: STAT_ICONS[Stat.dark] },
  ];
});

const statusTolsList = computed(() => {
  if (!props.enemyId) {
    return [];
  }
  return [
    { label: enemy.value.canPoison ? t("conditions.resistancePoison") : t("conditions.canPoison"), val: enemy.value.canPoison ? `${enemy.value.poison}%` : "No", icon: "☠️", valColor: enemy.value.canPoison ? "text-white" : "text-red-400" },
    { label: enemy.value.canParalyze ? t("conditions.resistanceParalyze") : t("conditions.canParalyze"), val: enemy.value.canParalyze ? `${enemy.value.paralyze}%` : "No", icon: "⚡", valColor: enemy.value.canParalyze ? "text-white" : "text-red-400" },
    { label: enemy.value.canConfuse ? t("conditions.resistanceConfuse") : t("conditions.canConfuse"), val: enemy.value.canConfuse ? `${enemy.value.confuse}%` : "No", icon: "😵", valColor: enemy.value.canConfuse ? "text-white" : "text-red-400" },
    { label: enemy.value.canSleep ? t("conditions.resistanceSleep") : t("conditions.canSleep"), val: enemy.value.canSleep ? `${enemy.value.sleep}%` : "No", icon: "💤", valColor: enemy.value.canSleep ? "text-white" : "text-red-400" },
    { label: enemy.value.canKO ? t("conditions.resistanceKo") : t("conditions.canKo"), val: enemy.value.canKO ? `${enemy.value.ko ?? 0}%` : "No", icon: "💀", valColor: enemy.value.canKO ? "text-white" : "text-red-400" },
    { label: t("conditions.drain"), val: enemy.value.canDrain ? "Yes" : "No", icon: "🧛", valColor: enemy.value.canDrain ? "text-green-400" : "text-red-400" },
    { label: t("conditions.steal"), val: enemy.value.canSteal ? "Yes" : "No", icon: "🦝", valColor: enemy.value.canSteal ? "text-green-400" : "text-red-400" },
    { label: t("conditions.escape"), val: enemy.value.canEscape ? "Yes" : "No", icon: "🏃", valColor: enemy.value.canEscape ? "text-green-400" : "text-red-400" },
  ];
});

const tooltipPlacement = "below" as const;
const tooltipPosition = useTooltipPosition(150);
const { show: tooltipShow, x: tooltipX, y: tooltipY, showAt, move, hide } = tooltipPosition;
const tooltipTitle = ref("");

const showEnemyStatTooltip = (event: MouseEvent, label: string) => {
  tooltipTitle.value = label;
  showAt(event, { maxWidth: 150, placement: tooltipPlacement });
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
            :items="attributesList"
            @show-stat-tooltip="showEnemyStatTooltip"
            @move-stat-tooltip="moveEnemyStatTooltip"
            @hide-stat-tooltip="hideEnemyStatTooltip"
          />
          <EnemyElements
            :items="elemTolsList"
            @show-stat-tooltip="showEnemyStatTooltip"
            @move-stat-tooltip="moveEnemyStatTooltip"
            @hide-stat-tooltip="hideEnemyStatTooltip"
          />
          <EnemyResistances
            :items="statusTolsList"
            @show-stat-tooltip="showEnemyStatTooltip"
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
