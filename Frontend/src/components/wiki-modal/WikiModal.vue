<script setup lang="ts">
import { computed, ref, watch } from "vue";
import Modal from "@/components/modal/Modal.vue";
import Tooltip from "@/components/tooltip/Tooltip.vue";
import WikiProfilePanel from "@/components/wiki-modal/wiki-profile-panel/WikiProfilePanel.vue";
import WikiDropsPanel from "@/components/wiki-modal/wiki-drops-panel/WikiDropsPanel.vue";
import WikiCardsPanel from "@/components/wiki-modal/wiki-cards-panel/WikiCardsPanel.vue";
import WikiLocationsPanel from "@/components/wiki-modal/wiki-locations-panel/WikiLocationsPanel.vue";
import WikiNpcPanel from "@/components/wiki-modal/wiki-npc-panel/WikiNpcPanel.vue";
import SearchBar from "@/components/search/SearchBar.vue";
import { useI18n } from "vue-i18n";
import { useTooltipPosition } from "@/composables/use-tooltip-position";
import { ImageCatalog } from "@/catalogs/image.catalog.ts";
import { WikiModalPresenter } from "@/presenters/map/wiki-modal.presenter";
import { WikiProfileDropsPresenter } from "@/presenters/map/wiki-modal/wiki-profile-drops.presenter";

const props = defineProps<{
  isOpen: boolean;
  enemyId: string | null;
  locationId?: string | null;
  npcId?: string | null;
}>();

const emit = defineEmits<{
  (e: "close"): void;
}>();

const { t } = useI18n();

type WikiView = "profile" | "drops" | "cards" | "locations" | "npc";

const selectedEnemyId = ref<string | null>(null);
const selectedDropId = ref<string | null>(null);
const selectedCardId = ref<string | null>(null);
const selectedLocationId = ref<string | null>(null);
const selectedNpcId = ref<string | null>(null);
const view = ref<WikiView>("profile");

const isModalOpen = computed(() => {
  return (
    props.isOpen &&
    (selectedEnemyId.value !== null ||
      selectedDropId.value !== null ||
      selectedCardId.value !== null ||
      selectedLocationId.value !== null ||
      selectedNpcId.value !== null)
  );
});

const isProfileView = computed(() => {
  return view.value === "profile";
});

const isLocationsView = computed(() => {
  return view.value === "locations";
});

const isDropsView = computed(() => {
  return view.value === "drops";
});

const isCardsView = computed(() => {
  return view.value === "cards";
});

const isNpcView = computed(() => {
  return view.value === "npc";
});

const canBackToProfile = computed(() => {
  return selectedEnemyId.value !== null && !isProfileView.value;
});

const handleClose = () => {
  emit("close");
};

const allSearchItems = computed(() => {
  return WikiModalPresenter.getAllSearchItems(
    (dropKey) => {
      return t(WikiProfileDropsPresenter.getDropLabelKey(dropKey));
    },
    (cardId) => {
      return t(`cards.${cardId}.name`);
    },
    (locationId) => {
      return t(`location.${locationId}`);
    },
  );
});

const selectedSearchId = computed(() => {
  if (isDropsView.value && selectedDropId.value !== null) {
    return selectedDropId.value;
  }

  if (isCardsView.value && selectedCardId.value !== null) {
    return selectedCardId.value;
  }

  if (isLocationsView.value && selectedLocationId.value !== null) {
    return selectedLocationId.value;
  }

  if (isNpcView.value && selectedNpcId.value !== null) {
    return selectedNpcId.value;
  }

  return selectedEnemyId.value ?? undefined;
});

const clearNonEnemySelections = () => {
  selectedDropId.value = null;
  selectedCardId.value = null;
  selectedLocationId.value = null;
  selectedNpcId.value = null;
};

const openNpcView = (npcId: string) => {
  hide();
  selectedNpcId.value = npcId;
  selectedEnemyId.value = null;
  selectedDropId.value = null;
  selectedCardId.value = null;
  selectedLocationId.value = null;
  view.value = "npc";
};

const handleSearchSelect = (id: string) => {
  const searchItem = allSearchItems.value.find((item) => {
    return item.id === id;
  });
  if (searchItem === undefined) {
    return;
  }

  if (searchItem.kind === "enemy") {
    selectedEnemyId.value = id;
    clearNonEnemySelections();
    view.value = "profile";
    return;
  }

  if (searchItem.kind === "drop") {
    selectedDropId.value = id;
    selectedCardId.value = null;
    selectedLocationId.value = null;
    selectedNpcId.value = null;
    view.value = "drops";
    return;
  }

  if (searchItem.kind === "card") {
    selectedCardId.value = id;
    selectedDropId.value = null;
    selectedLocationId.value = null;
    selectedNpcId.value = null;
    view.value = "cards";
    return;
  }

  if (searchItem.kind === "location") {
    hide();
    selectedLocationId.value = id;
    selectedDropId.value = null;
    selectedCardId.value = null;
    selectedNpcId.value = null;
    view.value = "locations";
    return;
  }

  if (WikiModalPresenter.isNpcSearchKind(searchItem.kind)) {
    openNpcView(id);
  }
};

const openDropsView = (dropId: string) => {
  hide();
  selectedDropId.value = dropId;
  selectedCardId.value = null;
  selectedNpcId.value = null;
  view.value = "drops";
};

const openLocationsView = (locationId: string) => {
  hide();
  selectedLocationId.value = locationId;
  selectedNpcId.value = null;
  view.value = "locations";
};

const backToProfileView = () => {
  if (selectedEnemyId.value === null) {
    return;
  }

  clearNonEnemySelections();
  view.value = "profile";
};

const openEnemyFromDropSource = (enemyId: string) => {
  selectedEnemyId.value = enemyId;
  clearNonEnemySelections();
  view.value = "profile";
};

const openCardFromBooster = (cardId: string) => {
  hide();
  selectedCardId.value = cardId;
  selectedDropId.value = null;
  selectedNpcId.value = null;
  view.value = "cards";
};

const enemy = computed(() => {
  if (selectedEnemyId.value === null) {
    return null;
  }

  return WikiModalPresenter.getEnemyById(selectedEnemyId.value);
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
      if (props.npcId !== null && props.npcId !== undefined) {
        openNpcView(props.npcId);
        return;
      }

      if (props.locationId !== null && props.locationId !== undefined) {
        selectedLocationId.value = props.locationId;
        selectedEnemyId.value = null;
        selectedDropId.value = null;
        selectedCardId.value = null;
        selectedNpcId.value = null;
        view.value = "locations";
        return;
      }

      selectedEnemyId.value = props.enemyId;
      clearNonEnemySelections();
      view.value = "profile";
      return;
    }

    hide();
    selectedEnemyId.value = null;
    clearNonEnemySelections();
    view.value = "profile";
  },
);

const enemyImageUrl = computed(() => {
  if (enemy.value === null) {
    return null;
  }

  return ImageCatalog.getEnemyIconUrl(enemy.value.name);
});
</script>

<template>
  <Modal
    :is-open="isModalOpen"
    max-width="max-w-[1300px]"
    max-height="h-[650px] max-h-[650px]"
    panel-class="w-[1300px]"
    @close="handleClose"
  >
    <template #header>
      <div class="flex items-center gap-6 flex-1 min-w-0">
        <div class="flex items-center gap-2 shrink-0 min-w-0">
          <span
            v-if="isProfileView || !canBackToProfile"
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
            {{ t(`enemy.wiki`) }}
          </h2>
        </div>

        <SearchBar
          :items="allSearchItems"
          :selected-id="selectedSearchId"
          :placeholder="t('enemy.searchPlaceholder')"
          :no-results-label="t('enemy.searchNoResults')"
          @select="handleSearchSelect"
        />
      </div>
    </template>

    <WikiProfilePanel
      v-if="view === 'profile' && enemy !== null"
      :enemy="enemy"
      :enemy-image-url="enemyImageUrl"
      @open-drops="openDropsView"
      @open-locations="openLocationsView"
      @show-stat-key-tooltip="showEnemyStatKeyTooltip"
      @show-condition-tooltip="showEnemyConditionTooltip"
      @move-stat-tooltip="moveEnemyStatTooltip"
      @hide-stat-tooltip="hideEnemyStatTooltip"
    />
    <WikiDropsPanel
      v-else-if="view === 'drops' && selectedDropId !== null"
      :drop-id="selectedDropId"
      @open-enemy="openEnemyFromDropSource"
      @open-card="openCardFromBooster"
    />
    <WikiCardsPanel
      v-else-if="view === 'cards' && selectedCardId !== null"
      :card-id="selectedCardId"
      @open-drop="openDropsView"
    />
    <WikiLocationsPanel
      v-else-if="view === 'locations' && selectedLocationId !== null"
      :location-id="selectedLocationId"
    />
    <WikiNpcPanel
      v-else-if="view === 'npc' && selectedNpcId !== null"
      :npc-id="selectedNpcId"
      @open-locations="openLocationsView"
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
