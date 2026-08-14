<script setup lang="ts">
import { computed, ref, watch } from "vue";
import Modal from "@/components/modal/Modal.vue";
import Tooltip from "@/components/tooltip/Tooltip.vue";
import BestiaryProfilePanel from "@/components/map/bestiary-modal/bestiary-profile-panel/BestiaryProfilePanel.vue";
import BestiaryDropsPanel from "@/components/map/bestiary-modal/bestiary-drops-panel/BestiaryDropsPanel.vue";
import BestiaryLocationsPanel from "@/components/map/bestiary-modal/bestiary-locations-panel/BestiaryLocationsPanel.vue";
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

type BestiaryView = "profile" | "drops" | "locations";

const selectedEnemyId = ref<string | null>(null);
const selectedDropId = ref<string | null>(null);
const view = ref<BestiaryView>("profile");

const isModalOpen = computed(() => {
  return props.isOpen && selectedEnemyId.value !== null;
});

const modalTitle = computed(() => {
  return t(`enemy.modalTitle.${view.value}`);
});

const isProfileView = computed(() => {
  return view.value === "profile";
});

const isLocationsView = computed(() => {
  return view.value === "locations";
});

const modalMaxWidth = computed(() => {
  if (isLocationsView.value) {
    return "max-w-[1300px]";
  }

  return "max-w-450";
});

const modalPanelClass = computed(() => {
  if (isLocationsView.value) {
    return "w-[1300px]";
  }

  return "w-[98vw]";
});

const handleClose = () => {
  emit("close");
};

const handleSearchSelect = (id: string) => {
  selectedEnemyId.value = id;
  selectedDropId.value = null;
  view.value = "profile";
};

const openDropsView = (dropId: string) => {
  hide();
  selectedDropId.value = dropId;
  view.value = "drops";
};

const openLocationsView = () => {
  hide();
  view.value = "locations";
};

const backToProfileView = () => {
  selectedDropId.value = null;
  view.value = "profile";
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
      selectedDropId.value = null;
      view.value = "profile";
      return;
    }

    hide();
    selectedEnemyId.value = null;
    selectedDropId.value = null;
    view.value = "profile";
  },
);

watch(selectedEnemyId, () => {
  selectedDropId.value = null;
  view.value = "profile";
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
    :max-width="modalMaxWidth"
    max-height="h-[92vh] max-h-250"
    :panel-class="modalPanelClass"
    @close="handleClose"
  >
    <template #header>
      <div class="flex items-center gap-6 flex-1 min-w-0">
        <div class="flex items-center gap-2 shrink-0 min-w-0">
          <span
            v-if="isProfileView"
            class="text-[1.2rem] leading-none grayscale select-none"
            aria-hidden="true"
          >
            <span class="inline-flex leading-none text-[1.2rem] -translate-y-1">🏠</span>
          </span>
          <button
            v-else
            type="button"
            class="text-[1.2rem] leading-none text-gray-400 hover:text-blue-300 transition-colors cursor-pointer"
            :aria-label="t('enemy.back')"
            @click="backToProfileView"
          >
            <span class="inline-flex leading-none text-[1.2rem] -translate-y-0.5">⬅️</span>
          </button>
          <h2 class="text-white font-bold tracking-widest drop-shadow whitespace-nowrap">
            {{ t(`enemy.bestiary`) }}
          </h2>
        </div>

        <SearchBar
          :items="allSearchItems"
          :selected-id="selectedEnemyId ?? undefined"
          :placeholder="t('enemy.searchPlaceholder')"
          :no-results-label="t('enemy.searchNoResults')"
          @select="handleSearchSelect"
        />
      </div>
    </template>

    <BestiaryProfilePanel
      v-if="view === 'profile'"
      :enemy="enemy"
      :enemy-image-url="enemyImageUrl"
      @open-drops="openDropsView"
      @open-locations="openLocationsView"
      @show-stat-key-tooltip="showEnemyStatKeyTooltip"
      @show-condition-tooltip="showEnemyConditionTooltip"
      @move-stat-tooltip="moveEnemyStatTooltip"
      @hide-stat-tooltip="hideEnemyStatTooltip"
    />
    <BestiaryDropsPanel
      v-else-if="view === 'drops'"
      :enemy="enemy"
      :initial-drop-id="selectedDropId"
    />
    <BestiaryLocationsPanel
      v-else
      :enemy="enemy"
    />
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
