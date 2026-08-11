<script setup lang="ts">
import { computed, ref, watch } from "vue";
import Modal from "@/components/modal/Modal.vue";
import Tooltip from "@/components/tooltip/Tooltip.vue";
import BestiaryInformation from "@/components/map/bestiary-modal/BestiaryInformation.vue";
import BestiaryDropsInformation from "@/components/map/bestiary-modal/BestiaryDropsInformation.vue";
import SearchBar from "@/components/search/SearchBar.vue";
import { useI18n } from "vue-i18n";
import { useTooltipPosition } from "@/composables/use-tooltip-position";
import { ImageCatalog } from "@/catalogs/image.catalog.ts";
import { BestiaryModalPresenter } from "@/presenters/map/bestiary-modal.presenter";

const props = defineProps<{
  isOpen: boolean;
  enemyId: string | null;
}>();

const emit = defineEmits<{
  (e: "close"): void;
}>();

const { t } = useI18n();

type BestiaryView = "information" | "drops";

const selectedEnemyId = ref<string | null>(null);
const view = ref<BestiaryView>("information");

const isModalOpen = computed(() => {
  return props.isOpen && selectedEnemyId.value !== null;
});

const handleClose = () => {
  emit("close");
};

const handleSearchSelect = (id: string) => {
  selectedEnemyId.value = id;
  view.value = "information";
};

const openDropsView = () => {
  hide();
  view.value = "drops";
};

const backToInformationView = () => {
  view.value = "information";
};

const allSearchItems = BestiaryModalPresenter.getAllSearchItems();

const enemy = computed(() => {
  return BestiaryModalPresenter.getEnemyById(selectedEnemyId.value!);
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

watch(
  () => props.isOpen,
  (open) => {
    if (open) {
      selectedEnemyId.value = props.enemyId;
      view.value = "information";
      return;
    }

    hide();
    selectedEnemyId.value = null;
    view.value = "information";
  },
);

watch(selectedEnemyId, () => {
  view.value = "information";
});

const enemyImageUrl = computed(() => {
  if (selectedEnemyId.value === null) {
    return null;
  }
  return ImageCatalog.getEnemyIconUrl(enemy.value.name);
});
</script>

<template>
  <Modal
    :is-open="isModalOpen"
    max-width="max-w-[1200px]"
    panel-class="w-[1200px]"
    @close="handleClose"
  >
    <template #header>
      <div class="flex items-center gap-6 flex-1 min-w-0">
        <h2
          class="text-white font-bold tracking-widest drop-shadow flex items-center gap-2 whitespace-nowrap shrink-0"
        >
          {{ $t("enemy.bestiary") }}
        </h2>

        <SearchBar
          :items="allSearchItems"
          :selected-id="selectedEnemyId ?? undefined"
          :placeholder="t('enemy.searchPlaceholder')"
          :no-results-label="t('enemy.searchNoResults')"
          @select="handleSearchSelect"
        />
      </div>
    </template>

    <BestiaryInformation
      v-if="view === 'information'"
      :enemy="enemy"
      :enemy-image-url="enemyImageUrl"
      @open-drops="openDropsView"
      @show-stat-key-tooltip="showEnemyStatKeyTooltip"
      @show-condition-tooltip="showEnemyConditionTooltip"
      @move-stat-tooltip="moveEnemyStatTooltip"
      @hide-stat-tooltip="hideEnemyStatTooltip"
    />
    <BestiaryDropsInformation v-else @back="backToInformationView" />
  </Modal>

  <Tooltip
    :show="tooltipShow"
    :x="tooltipX"
    :y="tooltipY"
    :title="tooltipTitle"
    :max-width="400"
    placement="below"
  />
</template>
